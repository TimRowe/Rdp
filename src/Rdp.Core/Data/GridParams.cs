using System.Collections.Generic;

namespace Rdp.Core.Data
{

    public class GridParams
    {

        /// <summary>
        /// 当前页面
        /// </summary>
        /// <remarks></remarks>

        public int PageIndex;
        /// <summary>
        /// 当前页面
        /// </summary>
        /// <remarks></remarks>

        public int PageSize;
        /// <summary>
        /// 排序方向（正序，反序）
        /// </summary>
        /// <remarks></remarks>

        public string SortDirection;
        /// <summary>
        /// 排序字段
        /// </summary>
        /// <remarks></remarks>

        public string SortField;

        public int TotalCount;

        public int TotalPage;


        public List<GridColumn> Columns;

        public GridParams()
        {
            PageIndex = 1;
            PageSize = 100;
        }

    }

}


