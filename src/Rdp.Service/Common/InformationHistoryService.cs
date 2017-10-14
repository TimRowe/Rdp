using Rdp.Core;
using Rdp.Core.Data;
using Rdp.Data;
using Rdp.Data.Entity;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Rdp.Service
{
    class InformationHistoryService : IInformationHistoryService
    {
        IRepository<InformationHistory> _informationHistoryRepository;

        public InformationHistoryService()
            : this(RepositoryFactory.Create<InformationHistory>())
        {

        }

        public InformationHistoryService(IRepository<InformationHistory> informationHistoryRepository)
        {
            _informationHistoryRepository = informationHistoryRepository;
        }
        

        public IRepository<InformationHistory> UseRepository
        {
            get { return _informationHistoryRepository; }
        }


        public ResultInfo SendSms(string phoneNumber, string content)
        {
            var information = new InformationHistory();
            information.ContactWay = phoneNumber;
            information.ChannelID = 4;
            information.SendContent = content;
            information.CreateBy = "WEB";
            information.SendDate = DateTime.Parse("1900-1-1");
            information.SentDate = DateTime.Parse("1900-1-1");
            this.Add(information);
            return new ResultInfo() { ErrorNo = 0, ErrorMsg = "" };
        }

        public ResultInfo SendSmsOutside(string phoneNumber, string content)
        {
             
            var strSqlInsert = "INSERT INTO COU_CN_MD.dbo.tbCOM_Information_History(Channel_ID,Send_Content,Contact_Way,Create_By,Send_Date,Sent_Date,Status_ID)VALUES ( 4 , @SendContent ,@ContactWay ,'WEB' ,'1900-1-1' ,'1900-1-1' , 0 )";
            
            SqlParameter[] parametersInserts = {
                   new SqlParameter("@SendContent", SqlDbType.NVarChar),
                   new SqlParameter("@ContactWay", SqlDbType.VarChar, 100),
                   };
            parametersInserts[0].Value = content;
            parametersInserts[1].Value = phoneNumber;

            var row = DbHelperSql.ExecuteSql(DbHelperSql.DefaultUpdateConn, strSqlInsert.ToString(), parametersInserts);
            if (row <= 0)
            {
                throw new SysDbException("SendSmsOutside未能插入数据库", strSqlInsert, parametersInserts);
            }

            return new ResultInfo() { ErrorNo = 0, ErrorMsg = "" };
        }
    }
}
