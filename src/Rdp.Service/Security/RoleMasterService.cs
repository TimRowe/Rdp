using Rdp.Core.Data;
using Rdp.Data;
using Rdp.Data.Entity;

namespace Rdp.Service
{
    ///<summary>
    ///角色表
    ///</summary>
    public partial class RoleMasterService : IRoleMasterService
    {

        IRepository<RoleMaster> _roleMasterRepository;

        public IRepository<RoleMaster> UseRepository
        {
            get
            {
                return _roleMasterRepository;
            }

        }

        public RoleMasterService() : this(RepositoryFactory.Create<RoleMaster>())
        {
        }

        public RoleMasterService(IRepository<RoleMaster> roleMasterRepository)
        {
            _roleMasterRepository = roleMasterRepository;
        }
    }
}
