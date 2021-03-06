﻿using Microsoft.AspNetCore.Mvc;
using Rdp.Web.Framework.Models;
using Rdp.Data.Entity;
using Rdp.Service;
using System;
using Rdp.Web.Framework.Core;
using Rdp.Core;
using Rdp.Resources.Globalization;
using Microsoft.AspNetCore.Diagnostics;
using Rdp.Core.Data;
using System.Linq;

namespace Rdp.Web.Framework.Controllers
{
    public class ErrorController : BaseController
    {
        private IErrorInfoService _errorInfoService;

        public ErrorController(IErrorInfoService errorInfoService)
        {
            _errorInfoService = errorInfoService;
        }

        /// <summary>
        /// 显示错误详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Detail(ErrorModel model)
        {
            var errorMsg = "";
            var redirectUrl = "";
            switch (model.ErrorNo)
            {
                case ErrorTypeEnum.LostSession:
                    errorMsg = ResError.Tooltip1 + ResError.ErrDesc1;
                    redirectUrl = "/Account/Login";
                    break;
                case ErrorTypeEnum.GetIPFailed:
                    errorMsg = ResError.Tooltip1 + ResError.ErrDesc2;
                    break;
                case ErrorTypeEnum.UnassignedBranch:
                    errorMsg = ResError.Tooltip1 + ResError.ErrDesc3;
                    break;
                case ErrorTypeEnum.ServerConnectionFailed:
                    errorMsg = ResError.Tooltip1 + ResError.ErrDesc4;
                    break;
                case ErrorTypeEnum.NotAccessPage:
                    errorMsg = ResError.Tooltip1 + ResError.ErrDesc5;
                    break;
                case ErrorTypeEnum.PageNotFound:
                    errorMsg = ResError.Tooltip1 + ResError.PageNotFound;
                    break;
                default:
                    errorMsg = ResError.Tooltip1 + ResError.ErrDesc;
                    break;
            }
            model.ErrorMsg = string.IsNullOrEmpty(model.ErrorMsg) ? errorMsg : model.ErrorMsg;
            model.RedirectUrl = string.IsNullOrEmpty(model.RedirectUrl) ? redirectUrl : model.RedirectUrl;
            return View("~/Views/RdpTemplate/Error/Detail.cshtml", model);
        }


        public IActionResult Handle()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;

            var errorInfo = new ErrorInfo() {
                ErrorInfoID = 0,
                UserID = SessionManager.GetUserMaster().UserID,
                ErrorCode = 500,
                ErrorMSG = error.Message,
                StackTrace = error.StackTrace,
                Url = HttpContext.Request.Path,
                RunningTime = System.DateTime.Now,
                SolveBy = "0",
                ExecSql = "0"
            };

            if (error.GetType().Name == "SysDbException")
            {
                SysDbException ex = (SysDbException)error;
                errorInfo.ExecSql = ex.SqlInfo;
            }

            _errorInfoService.Add(errorInfo);

            var errorResult = new ErrorModel() { ErrorNo = (ErrorTypeEnum)500, ErrorMsg = "错误ID:" + errorInfo.ErrorInfoID };

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(errorResult);
            else
                return Detail(errorResult);
        }


        /// <summary>
        /// 保存错误
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateInput(false)]
        public JsonResult Add(ErrorInfo model)
        {
            model.Url = Request.Path.ToString();
            model.RunningTime = DateTime.Now;
            if(SessionManager.GetUserMaster() != null)
                model.UserID = SessionManager.GetUserMaster().UserID;
            //_errorInfoService.Add(model);
            return Json(new ResultInfo() { ErrorNo = 0, ErrorMsg = "" });
        }
    }
}
