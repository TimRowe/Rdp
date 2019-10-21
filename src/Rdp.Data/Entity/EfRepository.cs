using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Rdp.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Rdp.Data.Entity
{
    /// <summary>
    /// Entity Framework repository
    /// </summary>
    public partial class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields

        private readonly DbContext _context;
        private DbSet<T> _entities;

        #endregion

        #region Properties

        public DbSet<T> Entities
        {
            get
            {

                if (_entities == null)
                    _entities = _context.Set<T>();

                return _entities;
            }
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return Entities.AsNoTracking();
            }
        }


        #endregion


        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(DbContext context)
        {
            _context = context;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }
        #endregion

        #region PrivateMethods

        private void AttachIfNoAttached(T entity, EntityState e)
        {
            if (_context.Entry(entity).State != e)
            {
                _context.Entry(entity).State = e;
            }
        }


        public bool SwitchDb(SwitchDbExcuteFun fun, EDbType e)
        {
            try
            {
                _context.Database.GetDbConnection().ConnectionString =
                    e == EDbType.UpdateDb ? DbHelperSql.DefaultUpdateConn : DbHelperSql.DefaultQueryConn;
                fun();
            }
            catch (Exception dbEx)
            {
                var fail = new Exception("SwitchDb:" + dbEx.Message, dbEx);
                throw fail;
            }
            finally
            {
                _context.Database.GetDbConnection().ConnectionString =
                    e == EDbType.UpdateDb ? DbHelperSql.DefaultQueryConn : DbHelperSql.DefaultUpdateConn;
            }

            return true;
        }


        private bool SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                var fail = new Exception(dbEx.Message, dbEx);
                throw fail;
            }
            finally
            {
            }

            return true;
        }

        #endregion


        #region PublicMethods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(object id)
        {
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return Entities.Find(id);
        }


        /// <summary>
        /// Find根据主键进行查找
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public T Find(params object[] keyValues)
        {
            return Entities.Find(keyValues);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            AttachIfNoAttached(entity, EntityState.Added);
            SaveChanges();
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
                AttachIfNoAttached(entity, EntityState.Added);

            SaveChanges();
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            AttachIfNoAttached(entity, EntityState.Modified);
            SaveChanges();
           _context.Entry(entity).State = EntityState.Detached;
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
            {
                AttachIfNoAttached(entity, EntityState.Modified);
            }

            SaveChanges();
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(T entity)
        {
            AttachIfNoAttached(entity, EntityState.Deleted);
            Entities.Remove(entity);
            SaveChanges();
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

           foreach (var entity in entities)
            {
                AttachIfNoAttached(entity, EntityState.Deleted);
            }

       //     foreach (var entity in entities)
                //Entities.RemoveRange(entities);

            SaveChanges();
        }

        #endregion


        #region ExtendMethods

        public string GetTableName()
        {
            return typeof(T).GetTypeInfo().GetCustomAttribute<TableAttribute>().Name;
        }

        public DbContext GetDbContext()
        {
            return _context;
        }

        //todo ef core 3.0
        //public List<T> SqlQuery(string sql, params object[] parameters)
        //{
        //    return Table.FromSql(sql, parameters).ToList();
        //}

        public List<T1> SqlQuery<T1>(string sql, params object[] parameters)
        {
            var i = 0;

            foreach (var e in parameters)
            {
                
                if (e == null)
                {
                    i++;
                    continue;
                    
                }

                if (e is string)
                    sql = sql.Replace("@p" + (i++).ToString(), "N'" + e.ToString() + "'");
                else
                    sql = sql.Replace("@p" + (i++).ToString(), e.ToString());

            }


            var dt = DbHelperSql.Query(DbHelperSql.DefaultQueryConn, sql).Tables[0];

            if (dt.Rows.Count <= 0)
                return null;

            return ConvertHelper<T1>.ConvertToModel(dt).ToList();
            
        }


        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="func"></param>
        public void BeginTransaction(Action func)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    func();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
    }





    #endregion


}


public class ConvertHelper<T> 
{

        public static IList<T> ConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = System.Activator.CreateInstance<T>();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

}