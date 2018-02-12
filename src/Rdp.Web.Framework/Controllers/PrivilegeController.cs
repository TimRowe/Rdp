using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Web.Framework.Core;
using Rdp.Core.Util;
using Rdp.Web.Framework.Filters;
using Rdp.Core;
using Rdp.Resources.Globalization;
using System.Collections.Generic;
using Rdp.Data.Entity;
using Rdp.Service.Dto;
using Rdp.Core.Security;

namespace Rdp.Web.Framework.Controllers
{
    /// <summary>
    /// 账户管理控制器
    /// </summary>
    [LoginAuthorize]
    public class PrivilegeController : BaseController
    {
        private IPrivilegeService _privilegeService;
        private IProgramService _programService;

        public PrivilegeController(
                IPrivilegeService privilegeService,
                IProgramService programService
            )
        {
            _privilegeService = privilegeService;
            _programService = programService;
        }

        /// <summary>
        /// 获取当前程序的按钮
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult ProgramButton(string style)
        {
            ViewBag.style = style;
            var path = HttpContext.Request.Path.ToString();
            int programId = _programService.GetCachedModel(p => p.Url == path).ProgramID;
            var buttonList = _privilegeService.GetProgramButton(SessionManager.GetRoleUser(), programId);
            return PartialView(buttonList);
        }

        /// <summary>
        /// 分配系统权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Assign(string privilegeMaster, string privilegeValue)
        {
            ViewData["privilegeMaster"] = privilegeMaster;
            ViewData["privilegeValue"] = privilegeValue;
            return View();
        }


        /// <summary>
        /// 分配系统权限
        /// </summary>
        /// <param name="strSql">需更新的Sql</param>
        /// <param name="privilegeIdList">当前的privilegeIdList</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Assign(List<PrivilegeAssignDto> privilegeList)
        {
            var resultInfo = _privilegeService.AssignPrivilege(privilegeList);

            if(resultInfo.ErrorNo == 0)
                resultInfo.ErrorMsg = ResSuggest.OperateSuccess;

            return Json(resultInfo);
        }

        /// <summary>
        /// 获取权限目录树
        /// </summary>
        /// <param name="privilegeMaster"></param>
        /// <param name="privilegeValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetTreeGrid(string parentId, int privilegeMaster, string privilegeValue)
        {

            var gridParams = this.GetGridParams();
            var dt = _privilegeService.GetTreeGrid(parentId, privilegeMaster, privilegeValue, ref gridParams);
            var result =  JSONHelper.ToJson(dt, gridParams.TotalCount);

            //因为Easyui父节点没有_parentId选项，所以进行替代
            result = result.Replace("\"Access_Master\":1,\"_parentId\":0", "\"Access_Master\":1");
            return result;
        }

        /// <summary>
        /// 判断某用户是否具有某程序权限。
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CheckRight(string userId, string programId)
        {
            if(_privilegeService.CheckRight(userId, int.Parse(DesEncrypt.Decrypt(programId))))
            {
                return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = "" });
            }
            else
            {
                return Json(new ResultInfo() { ErrorNo = -1, ErrorMsg = ResMessage.ValidNotThrough });
            }
        }

    }
}
