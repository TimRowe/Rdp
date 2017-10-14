using Rdp.Data.Entity;
using System.Collections.Generic;

namespace Rdp.Service.Dto
{
    public class MenuTreeDto
    {
        /// <summary>
        /// 当前菜单
        /// </summary>
        public Program CurrentMenu { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<MenuTreeDto> ChildMenus { get; set; }
    }
}



