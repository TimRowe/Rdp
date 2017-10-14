using Rdp.Core.Data;
using Rdp.Data;
using Rdp.Data.Entity;

namespace Rdp.Service
{
    ///<summary>
    ///分店权限表
    ///</summary>
    public partial class BatchGeneratorService : IBatchGeneratorService
    {
        private IRepository<BatchGenerator> _batchGeneratorRepository;

        public IRepository<BatchGenerator> UseRepository
        {
            get
            {
                return _batchGeneratorRepository;
            }
        }

        public BatchGeneratorService()
            : this(RepositoryFactory.Create<BatchGenerator>())
        {
        }

        public BatchGeneratorService(IRepository<BatchGenerator> batchGeneratorRepository)
        {
            _batchGeneratorRepository = batchGeneratorRepository;
        }

        #region "ExtensionMethod"
        /// <summary>
        /// 获取最大数字
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public long GetBatch(string code)
        {
            //return _dal.GetBatch(code);
            return 0;
            //try {
            //    SP_CRM_BATCH_NO_GENERATOR s = new SP_CRM_BATCH_NO_GENERATOR();

            //}
            //catch { }

            // Dim batch As Long
            //Try
            //    Dim parameters As SqlParameter() = {
            //        New SqlParameter("@Code", SqlDbType.Char, 20),
            //        New SqlParameter("@Count", SqlDbType.Int, 4),
            //        New SqlParameter("@ErrorNo", SqlDbType.Int, 4),
            //        New SqlParameter("@Batch", SqlDbType.Int, 4)
            //        }
            //    parameters(0).Value = code
            //    parameters(1).Value = 1
            //    parameters(2).Direction = ParameterDirection.Output
            //    parameters(3).Direction = ParameterDirection.Output
            //    DbHelperSql.RunProcedure(DbHelperSql.DefaultUpdateConn, "SP_CRM_BATCH_NO_GENERATOR", parameters)
            //    batch = parameters(3).Value
            //Catch ex As Exception
            //    batch = 0
            //End Try
            //Return batch



        }
        #endregion
    }
}

