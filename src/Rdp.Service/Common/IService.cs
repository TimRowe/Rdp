using Rdp.Core.Data;
using Rdp.Core.Dependency;

namespace Rdp.Service
{
   
    
    
    public interface IService<T> : IDependency where T : BaseEntity
    {

        IRepository<T> UseRepository
        {
            get;
        }



    }
}