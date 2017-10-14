using Rdp.Core;
using Rdp.Data.Entity;

namespace Rdp.Service
{
    public interface IInformationHistoryService : IService<InformationHistory>
    {
        /// <summary>
        /// 发送短信消息
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="content">短信内容</param>
        /// <returns></returns>
        ResultInfo SendSms(string phoneNumber, string content);

        /// <summary>
        /// 外网发送短信
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="content">短信内容</param>
        /// <returns></returns>
        ResultInfo SendSmsOutside(string phoneNumber, string content);
    }
}
