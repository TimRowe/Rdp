using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.Util;
using System.Text.RegularExpressions;

namespace Rdp.Core.Util
{
    public class NPOIHelper
    {
        /// <summary>
        /// NPOI简单Demo，快速入门代码
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="strFileName"></param>
        /// <remarks>NPOI认为Excel的第一个单元格是：(0，0)</remarks>
        /// <Author>卜永济www.cnblogs.com/BuBu/ 2013-6-7 22:21:41</Author>
        public static void ExportEasy(DataTable dtSource, string strFileName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

            //填充表头
            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(0);
            foreach (DataColumn column in dtSource.Columns)
            {
                dataRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
            }

            if ((dtSource.Rows.Count >= 65536))
            {
                throw new Exception("不能汇出超过65536条记录!!!");
            }

            //填充内容
            for (int i = 0; i <= dtSource.Rows.Count - 1; i++)
            {
                dataRow = (HSSFRow)sheet.CreateRow(i + 1);
                for (int j = 0; j <= dtSource.Columns.Count - 1; j++)
                {
                    dataRow.CreateCell(j).SetCellValue(dtSource.Rows[i][j].ToString());
                }
            }


            //保存
            using (MemoryStream ms = new MemoryStream())
            {
                CreateFolder(strFileName.Substring(0, strFileName.LastIndexOf("\\")));
                using (FileStream fs = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }


        /// <summary>
        /// 按条数分批汇出数据，减轻服务器压力
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="strFileName"></param>
        /// <remarks> </remarks>
        /// <Author>吴伟兴 2014-12-03 16:25:41</Author>
        /// Tim 2016-09-18  表列头改用Caption 
        public static void ExportBatch(DataTable dtSource, string strFileName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            CreateFolder(strFileName.Substring(0, strFileName.LastIndexOf("\\")));
            int iCount = 0;

            //每批汇出行数
            int sheetRows = 50000;
            for (int c = 0; c <= dtSource.Rows.Count - 1; c += sheetRows)
            {
                //For c As Integer = 0 To 10
                if (c == iCount * sheetRows)
                {
                    iCount = iCount + 1;
                    HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

                    //填充表头
                    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(0);
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
                    }

                    //填充内容
                    int rowCounts = (dtSource.Rows.Count > iCount * sheetRows ? iCount * 10 : dtSource.Rows.Count);
                    int rowIndex = 1;
                    for (int i = c; i <= rowCounts - 1; i++)
                    {
                        dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                        for (int j = 0; j <= dtSource.Columns.Count - 1; j++)
                        {
                            dataRow.CreateCell(j).SetCellValue(dtSource.Rows[i][j].ToString());
                        }
                        rowIndex += 1;
                    }

                    //保存
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (FileStream fs = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            workbook.Write(ms);
                            ms.Flush();
                            ms.Position = 0;
                            byte[] data = ms.ToArray();
                            fs.Write(data, 0, data.Length);
                            fs.Flush();
                        }
                    }

                }
            }
        }



        /// <summary>
        /// 多个datatable的汇出
        /// </summary>
        /// <param name="dtSource">DataSet</param>
        /// <param name="strFileName">路径</param>
        /// <param name="dtTitle">每个datatable的title</param>
        /// <param name="startTotal">开始合计列序号</param>
        /// <param name="totalName">合计名称</param>
        public static void ExportEasy(DataSet dtSource, string strFileName, string dtTitle, bool isTotal, int startTotal, string totalName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();
            HSSFRow dataRow = default(HSSFRow);
            int row = 0;
            string[] title = dtTitle.Split(',');
            if (dtSource.Tables.Count != title.Length)
            {
                return;
            }
            for (int k = 0; k <= dtSource.Tables.Count - 1; k++)
            {
                //填充表头
                dataRow = (HSSFRow)sheet.CreateRow(row);
                dataRow.CreateCell(0).SetCellValue(title[k]);
                //填充表标题title
                //填充列头
                row += 1;
                dataRow = (HSSFRow)sheet.CreateRow(row);
                foreach (DataColumn column in dtSource.Tables[k].Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
                }

                //要做合计
                if (isTotal == true)
                {
                    //填充内容，i为行j为列
                    row += 1;
                    string[] sum = {
                        "A",
                        "B",
                        "C",
                        "D",
                        "E",
                        "F",
                        "G",
                        "H",
                        "I",
                        "J",
                        "K",
                        "L",
                        "M",
                        "N",
                        "O",
                        "P",
                        "Q",
                        "R",
                        "S",
                        "T",
                        "U",
                        "V",
                        "W",
                        "X",
                        "Y",
                        "Z"
                    };
                    for (int i = 0; i <= dtSource.Tables[k].Rows.Count; i++)
                    {
                        dataRow = (HSSFRow)sheet.CreateRow(row + i);
                        for (int j = 0; j <= dtSource.Tables[k].Columns.Count - 1; j++)
                        {
                            //最后一行做合计
                            if (i == dtSource.Tables[k].Rows.Count)
                            {
                                if (i != 0)
                                {
                                    //做合计
                                    if (j >= startTotal)
                                    {
                                        dataRow.CreateCell(j).CellFormula = "SUM(" + sum[j] + (row + 1).ToString() + ":" + sum[j] + (row + i).ToString() + ")";
                                        sheet.ForceFormulaRecalculation = true;
                                        //合计标题
                                    }
                                    else if (j == 0)
                                    {
                                        dataRow.CreateCell(j).SetCellValue(totalName);
                                    }
                                    else
                                    {
                                        dataRow.CreateCell(j).SetCellValue("");
                                    }
                                    //如果0行数据
                                }
                                else
                                {
                                    //做合计
                                    if (j >= startTotal)
                                    {
                                        dataRow.CreateCell(j).CellFormula = "0";
                                        //合计标题
                                    }
                                    else if (j == 0)
                                    {
                                        dataRow.CreateCell(j).SetCellValue(totalName);
                                    }
                                    else
                                    {
                                        dataRow.CreateCell(j).SetCellValue("");
                                    }
                                }
                                //不是最后一行填充数据
                            }
                            else
                            {
                                //设置数字类型
                                if (Regex.IsMatch(dtSource.Tables[k].Rows[i][j].ToString(), @"^[+-]?\d*[.]?\d*$"))
                                {
                                    dataRow.CreateCell(j).SetCellValue(Convert.ToInt32(dtSource.Tables[k].Rows[i][j]));
                                }
                                else
                                {
                                    dataRow.CreateCell(j).SetCellValue(dtSource.Tables[k].Rows[i][j].ToString());
                                }
                            }
                        }
                    }
                    //不做合计
                }
                else
                {
                    //填充内容，i为行j为列
                    if (dtSource.Tables[k].Rows.Count == 0)
                    {
                        dataRow = (HSSFRow)sheet.CreateRow(row + 1);
                        for (int j = 0; j <= dtSource.Tables[k].Columns.Count - 1; j++)
                        {
                            //做合计
                            if (j >= startTotal)
                            {
                                dataRow.CreateCell(j).CellFormula = "0";
                                //合计标题
                            }
                            else if (j == 0)
                            {
                                dataRow.CreateCell(j).SetCellValue(totalName);
                            }
                            else
                            {
                                dataRow.CreateCell(j).SetCellValue("");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= dtSource.Tables[k].Rows.Count - 1; i++)
                        {
                            dataRow = (HSSFRow)sheet.CreateRow(row + i + 1);
                            for (int j = 0; j <= dtSource.Tables[k].Columns.Count - 1; j++)
                            {
                                dataRow.CreateCell(j).SetCellValue(dtSource.Tables[k].Rows[i][j].ToString());
                            }
                        }
                    }
                }
                row += dtSource.Tables[k].Rows.Count + 3;
                //表与表之间隔两行
            }

            //保存
            using (MemoryStream ms = new MemoryStream())
            {
                CreateFolder(strFileName.Substring(0, strFileName.LastIndexOf("\\")));
                using (FileStream fs = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }


        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <Author>卜永济www.cnblogs.com/BuBu/ 2013-6-7 22:21:41</Author>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

            //#Region "右击文件 属性信息"
            if (true)
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "Your Company";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "卜永济";
                //填加xls文件作者信息
                si.ApplicationName = "NPOI程序导出";
                //填加xls文件创建程序信息
                si.LastAuthor = "卜永济";
                //填加xls文件最后保存者信息
                si.Comments = "说明信息";
                //填加xls文件作者信息
                si.Title = "CRM";
                //填加xls文件标题信息
                si.Subject = "NPOI测试Demo";
                //填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
            //#End Region

            HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
            //dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd")

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i <= dtSource.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dtSource.Columns.Count - 1; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }



            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                //#Region "新建表，填充表头，填充列头，样式"
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = (HSSFSheet)workbook.CreateSheet();
                    }

                    //#Region "表头及样式"
                    if (!string.IsNullOrEmpty(strHeaderText))
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        //headStyle.Alignment = CellHorizontalAlignment.CENTER
                        HSSFFont font = (HSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;

                        //sheet.AddMergedRegion(New Region(0, 0, 0, dtSource.Columns.Count - 1))
                        //headerRow.Dispose()
                    }
                    //#End Region


                    if (string.IsNullOrEmpty(strHeaderText))
                    {
                        rowIndex = 0;
                    }
                    else
                    {
                        rowIndex = 1;
                    }

                    //#Region "列头及样式"
                    if (true)
                    {
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(rowIndex);


                        HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        //headStyle.Alignment = CellHorizontalAlignment.CENTER
                        HSSFFont font = (HSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);


                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽

                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        //headerRow.Dispose()
                    }
                    //#End Region
                    if (string.IsNullOrEmpty(strHeaderText))
                    {
                        rowIndex = 1;
                    }
                    else
                    {
                        rowIndex = 2;
                    }

                }
                //#End Region


                //#Region "填充内容"
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String":
                            //字符串类型
                            newCell.SetCellValue(drValue);
                            break; // TODO: might not be correct. Was : Exit Select

                        case "System.DateTime":
                            //日期类型
                            DateTime dateV = default(DateTime);
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;
                            //格式化显示
                            break; // TODO: might not be correct. Was : Exit Select

                        case "System.Boolean":
                            //布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break; // TODO: might not be correct. Was : Exit Select

                        //整型
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break; // TODO: might not be correct. Was : Exit Select

                        //浮点型
                        case "System.Decimal":
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break; // TODO: might not be correct. Was : Exit Select

                        case "System.DBNull":
                            //空值处理
                            newCell.SetCellValue("");
                            break; // TODO: might not be correct. Was : Exit Select

                        default:
                            newCell.SetCellValue("");
                            break; // TODO: might not be correct. Was : Exit Select

                    }
                }
                //#End Region

                rowIndex += 1;
            }


            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                //sheet.Dispose()
                //workbook.Dispose()

                return ms;
            }



        }


        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        /// <Author>卜永济www.cnblogs.com/BuBu/ 2013-6-7 22:21:41</Author>
        public static void Export(DataTable dtSource, string strHeaderText, string strFileName)
        {
            using (MemoryStream ms = Export(dtSource, strHeaderText))
            {
                CreateFolder(strFileName.Substring(0, strFileName.LastIndexOf("\\")));
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        public static void CreateFolder(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 用于Web导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        /// <Author>卜永济www.cnblogs.com/BuBu/ 2013-6-7 22:21:41</Author>
        //todo暂时屏蔽，建议放在Rdp.Web.Framework中扩展
        /*public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName)
        {

            HttpContext curContext = HttpContext.Current;

            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));

            curContext.Response.BinaryWrite(Export(dtSource, strHeaderText).GetBuffer());
            curContext.Response.End();

        }*/


        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook = default(HSSFWorkbook);
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j <= cellCount - 1; j++)
            {
                HSSFCell cell = (HSSFCell)headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j <= cellCount - 1; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        //dataRow(j) = row.GetCell(j).ToString()
                        HSSFCell cell = (HSSFCell)row.GetCell(j);
                        if (cell.CellType == NPOI.SS.UserModel.CellType.Formula)
                        {
                            switch (cell.CachedFormulaResultType)
                            {
                                case NPOI.SS.UserModel.CellType.String:
                                    dataRow[j] = row.GetCell(j).StringCellValue;
                                    break;
                                case NPOI.SS.UserModel.CellType.Numeric:
                                    dataRow[j] = row.GetCell(j).NumericCellValue;
                                    break;
                                default:
                                    dataRow[j] = row.GetCell(j).ToString();
                                    break;
                            }

                        }
                        else
                        {
                            dataRow[j] = row.GetCell(j).ToString();
                        }
                    }
                }
                dt.Rows.Add(dataRow);
            }

            //while (rows.MoveNext())
            //{
            //    HSSFRow row = (HSSFRow)rows.Current;
            //    DataRow dr = dt.NewRow();

            //    for (int i = 0; i < row.LastCellNum; i++)
            //    {
            //        HSSFCell cell = row.GetCell(i);


            //        if (cell == null)
            //        {
            //            dr[i] = null;
            //        }
            //        else
            //        {
            //            dr[i] = cell.ToString();
            //        }
            //    }
            //    dt.Rows.Add(dr);
            //}

            return dt;
        }
        /// <summary>
        /// 将Excel里的数据分页显示
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <param name="pageSize">每页显示行数</param>
        /// <param name="currentPage">当前页数</param>
        /// <param name="total">总行数</param>
        /// <returns></returns>
        /// <remarks></remarks>
        /// <author>卜永济</author>
        public static DataTable ImportByPage(string strFileName, int pageSize, int currentPage, ref int total)
        {
            DataTable dt = new DataTable();
            HSSFWorkbook hssfworkbook = default(HSSFWorkbook);
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            total = sheet.LastRowNum;

            HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j <= cellCount - 1; j++)
            {
                HSSFCell cell = (HSSFCell)headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            int firstRow = sheet.FirstRowNum + 1 + (currentPage - 1) * pageSize;
            for (int i = firstRow; i <= firstRow + pageSize - 1; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                if (i <= sheet.LastRowNum)
                {
                    for (int j = row.FirstCellNum; j <= cellCount - 1; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            HSSFCell cell = (HSSFCell)row.GetCell(j);
                            if (cell.CellType == NPOI.SS.UserModel.CellType.Formula)
                            {
                                switch (cell.CachedFormulaResultType)
                                {
                                    case NPOI.SS.UserModel.CellType.String:
                                        dataRow[j] = row.GetCell(j).StringCellValue;
                                        break;
                                    case NPOI.SS.UserModel.CellType.Numeric:
                                        dataRow[j] = row.GetCell(j).NumericCellValue;
                                        break;
                                    default:
                                        dataRow[j] = row.GetCell(j).ToString();
                                        break;
                                }

                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                        }
                    }

                    dt.Rows.Add(dataRow);
                }
            }

            return dt;

            //dt.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExcelFileStream"></param>
        /// <param name="SheetName"></param>
        /// <param name="HeaderRowIndex"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable Import(Stream ExcelFileStream, string SheetName, int HeaderRowIndex)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            HSSFSheet sheet = (HSSFSheet)workbook.GetSheet(SheetName);

            DataTable table = new DataTable();

            HSSFRow headerRow = (HSSFRow)sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i <= cellCount - 1; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum - 1; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j <= cellCount - 1; j++)
                {
                    dataRow[j] = row.GetCell(j).ToString();
                }
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExcelFileStream"></param>
        /// <param name="SheetIndex"></param>
        /// <param name="HeaderRowIndex"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable Import(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(SheetIndex);

            DataTable table = new DataTable();

            HSSFRow headerRow = (HSSFRow)sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i <= cellCount - 1; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            //Dim rowCount As Integer = sheet.LastRowNum

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);

                if ((row == null))
                {
                    continue;
                }

                DataRow dataRow = table.NewRow();
                dynamic bAllNull = true;
                for (int j = row.FirstCellNum; j <= cellCount - 1; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        dataRow[j] = row.GetCell(j).ToString();
                        if (row.GetCell(j).ToString().Trim() != string.Empty)
                        {
                            bAllNull = false;
                        }
                    }
                }

                if (!bAllNull)
                {
                    table.Rows.Add(dataRow);
                }

            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }
    }
}





