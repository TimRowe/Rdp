using Rdp.Core.Data;
using Rdp.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rdp.Service.Implement
{
    public class DefaultService<T> : IService<T> where T : BaseEntity
    {
        IRepository<T> _baseRepository;

        public DefaultService(IRepository<T> baseRepository)
        {
            this._baseRepository = baseRepository;
        }

        IRepository<T> IService<T>.UseRepository {
            get
            {
                return _baseRepository;
            }
        }
    }
}
