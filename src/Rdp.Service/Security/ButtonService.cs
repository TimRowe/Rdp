using System;
using System.Data;
using System.Text;
using Rdp.Core.Security;
using Rdp.Core.Data;
using Rdp.Data.Entity;
using Rdp.Data;
using System.Collections.Generic;
using System.Linq;
using Rdp.Service.Extension;
using Rdp.Service.Dto;

namespace Rdp.Service
{
    ///<summary>
    ///按钮表
    ///</summary>
    public partial class ButtonService : IButtonService
    {
        IRepository<Button> _buttonRepository;

        public IRepository<Button> UseRepository
        {
            get
            {
                return _buttonRepository;
            }

        }

        public ButtonService():this(RepositoryFactory.Create<Button>())
        {
        }

        public ButtonService(IRepository<Button> buttonRepository)
        {
            _buttonRepository = buttonRepository;
        }

        


        #region "ExtensionMethod"
        /// <summary>
        /// 得到用户权限
        /// </summary>
        /// <param name="programID">程序编号</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public  static string GetButton(RoleUser roleUser, string programID)
        {
            var bllButton = new ButtonService();
            StringBuilder strInner = new StringBuilder();
            strInner.Append(" INNER JOIN dbo.tbLOG_Program_Button PB ON PB.Button_ID = tbLOG_Button.Button_ID ");
            strInner.Append(" INNER JOIN dbo.tbLOG_Privilege P ON P.Access_Value = PB.Program_Button_ID ");
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append("Status_Flag = 0 AND PB.Program_ID = ");
            strWhere.Append(DesEncrypt.Decrypt(programID));
            strWhere.Append("  AND P.Access_Master = 2 AND ( ( P.Privilege_Master = 1 AND P.Privilege_Value = '");
            strWhere.Append(roleUser.RoleID);
            strWhere.Append("' ) OR ( P.Privilege_Master = 2 AND P.Privilege_Value = '");
            strWhere.Append(roleUser.UserID.ToUpper());
            strWhere.Append("' ) ) AND GETDATE() >= P.Valid_From AND GETDATE() <= P.Valid_Until group by [Button_Name] ,[Button_Text] ,[Button_Class]  ,[Button_Script] ,[Button_Data_Options] ,[Button_Href],Priority ORDER BY Priority");
            DataTable dt = bllButton.GetList(strWhere.ToString(), "[Button_Name] ,[Button_Text] ,[Button_Class]  ,[Button_Script] ,[Button_Data_Options] ,[Button_Href],min(P.Operation_ID) Operation_ID", strInner.ToString()).Tables[0];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (Convert.ToInt32(dt.Rows[i]["Operation_ID"]) == 3)
                {
                    continue;
                }
                sb.Append("<td><a href=\"");
                sb.Append(dt.Rows[i]["Button_Href"]);
                sb.Append("\" class=\"");
                sb.Append(dt.Rows[i]["Button_Class"]);
                sb.Append("\" id=\"");
                sb.Append(dt.Rows[i]["Button_Name"]);
                sb.Append("\" data-options=\"");
                sb.Append(dt.Rows[i]["Button_Data_Options"]);
                if (Convert.ToInt32(dt.Rows[i]["Operation_ID"]) == 2)
                {
                    sb.Append(",disabled:true\"");
                }
                else
                {
                    sb.Append("\"");
                }
                sb.Append(" onclick=\"");
                sb.Append(dt.Rows[i]["Button_Script"]);
                sb.Append("\" >");
                sb.Append(dt.Rows[i]["Button_Text"]);
                sb.Append("</a></td>");
                sb.Append("<td><div class=\"datagrid-btn-separator\"></div></td>");
            }
            return sb.ToString();
        }

        public List<Button> Search(ButtonSearchRequestDto searchRequest, ref GridParams gridParams)
        {
            var query = from program in _buttonRepository.Table
                        select program;

            query = query.Where(b => !(
                             (searchRequest.ButtonID != 0 && b.ButtonID != searchRequest.ButtonID) ||
                             (!string.IsNullOrEmpty(searchRequest.ButtonName) && b.ButtonName.Trim() != searchRequest.ButtonName) ||
                             (!string.IsNullOrEmpty(searchRequest.ButtonText) && !b.ButtonText.Trim().Contains(searchRequest.ButtonText)) ||
                             (searchRequest.StatusFlag != 0 && b.StatusFlag != searchRequest.StatusFlag)
                         ));

            gridParams.TotalCount = query.Count();
            return QueryExtensions.SortAndPage(query, gridParams).ToList();
        }



        #endregion
    }

    
}


