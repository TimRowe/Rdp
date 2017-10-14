
using Rdp.Data.Entity;
using Rdp.Service.Dto;
using System.Collections.Generic;
using System.Data;

namespace Rdp.Service
{
    public interface ICodeTableService : IService<CodeTable>
    {
        /// <summary>
        ///返回下拉框数据 
        /// </summary>
        /// <param name="tableName">码表名称</param>
        /// <returns></returns>
        /// <remarks></remarks>
        DataTable GetCombobox(string tableName);

        /// <summary>
        /// 返回Table的所有行
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        DataTable GetCodeTable(string tableName);

        /// <summary>
        /// 返回CodeTable下拉数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<CodeTableItemDto> GetCodeTable(CodeTableDto model);

        /// <summary>
        /// 返回DB下拉数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<CodeTableItemDto> GetGeneralTable(CodeTableDto model);
    }
}