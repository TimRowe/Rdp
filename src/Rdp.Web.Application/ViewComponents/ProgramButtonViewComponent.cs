using Microsoft.AspNetCore.Mvc;
using Rdp.Service;
using Rdp.Web.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tmq.Web1.ViewComponents
{
    public class ProgramButtonViewComponent : ViewComponent
    {
        private IProgramService _programService;
        private IPrivilegeService _privilegeService;

        public ProgramButtonViewComponent(IProgramService programService, IPrivilegeService privilegeService)
        {
            _programService = programService;
            _privilegeService = privilegeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string style)
        {
            ViewBag.style = style;
            var path = HttpContext.Request.Path.ToString();
            int programId = _programService.GetCachedModel(p => p.Url == path).ProgramID;
            var buttonList = await _privilegeService.GetProgramButtonAsync(SessionManager.GetRoleUser(), programId);
            return View("/Views/RdpTemplate/Privilege/ProgramButton.cshtml",buttonList);
        }
    }
}
