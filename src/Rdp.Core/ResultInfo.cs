﻿using System.Data;
namespace Rdp.Core
{
    public class ResultInfo
    {
        /// <summary>
        /// 错误码
        /// </summary>
        /// <remarks></remarks>

        public int ErrorNo { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <remarks></remarks>

        public string ErrorMsg { get; set; }
        /// <summary>
        /// 错误信息反馈表
        /// </summary>
        /// <remarks></remarks>

        public DataTable ErrorTable { get; set; }
    }
    public class OperateResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }
}


