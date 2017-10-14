using System.Threading;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using Rdp.Core.Util;

namespace Rdp.Web.Framework.Core
{
    public class Utils
    {
        /// <summary>
        /// 资源文件国际化
        /// </summary>
        /// <remarks></remarks>
        public static void InitializeCulture()
        {
            string currentCulture = CookieManager.GetLanguage();
            if (currentCulture == null)
            {
                //Dim lanStr As String = SessionManager.GetDomain()("Default_Language").ToString()
                var lanStr = "zh-CN";
                //string lanStr = ConfigHelper.GetConfigString("DefaultLanguage");
                CookieManager.AddLanguage(lanStr);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lanStr);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lanStr);
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentCulture);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentCulture);
            }
        }


        /// <summary>
        /// 资源文件国际化
        /// </summary>
        /// <remarks></remarks>
        public static void InitializeCulture(string strDefaultLan)
        {
            string currentCulture = CookieManager.GetLanguage();
            if (currentCulture == null)
            {
                CookieManager.AddLanguage(strDefaultLan);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(strDefaultLan);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(strDefaultLan);
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentCulture);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentCulture);
            }
        }



        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// ZIP解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] zippedData)
        {
            MemoryStream ms = new MemoryStream(zippedData);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                {
                    break; // TODO: might not be correct. Was : Exit While
                }
                else
                {
                    outBuffer.Write(block, 0, bytesRead);
                }
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }



    }
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================

