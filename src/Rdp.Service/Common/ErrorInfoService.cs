using Rdp.Core.Data;
using Rdp.Data;
using Rdp.Data.Entity;

namespace Rdp.Service
{
    ///<summary>
    ///操作错误表
    ///</summary>
    public partial class ErrorInfoService : IErrorInfoService
    {
        IRepository<ErrorInfo> _errorInfoRepository;

        public IRepository<ErrorInfo> UseRepository
        {
            get
            {
                return _errorInfoRepository;
            }

        }

        public ErrorInfoService()
            : this(RepositoryFactory.Create<ErrorInfo>())
        {
        }

        public ErrorInfoService(IRepository<ErrorInfo> errorInfoRepository)
        {
            _errorInfoRepository = errorInfoRepository;
        }
    }
}


