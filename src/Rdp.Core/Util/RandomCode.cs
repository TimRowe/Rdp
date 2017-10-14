using System;

namespace Rdp.Core.Util
{
    public class RandomCode
    {
        #region "从字符串里随机得到，规定个数的字符串."
        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串
        /// </summary>
        /// <param name="allChar">随机码内容</param>
        /// <param name="codeCount">随机码个数</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetRandomCode(string allChar, int codeCount)
        {
            //string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"; 
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i <= codeCount - 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(Guid.NewGuid().GetHashCode());
                }
                int t = rand.Next(allCharArray.Length);
                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
        #endregion
    }
}

