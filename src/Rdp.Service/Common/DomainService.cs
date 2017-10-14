using Rdp.Core.Data;
using Rdp.Data;
using Rdp.Data.Entity;

namespace Rdp.Service
{
    ///<summary>
    ///COU范围表
    ///</summary>
    public partial class DomainService : IDomainService
    {
        IRepository<Domain> _domainRepository;

        public IRepository<Domain> UseRepository
        {
            get
            {
                return _domainRepository;
            }

        }

        public DomainService()
            : this(RepositoryFactory.Create<Domain>())
        {
        }

        public DomainService(IRepository<Domain> domainRepository)
        {
            _domainRepository = domainRepository;
        }

    }
}


//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
