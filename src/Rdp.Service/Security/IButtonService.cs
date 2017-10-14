using Rdp.Core.Data;
using Rdp.Data.Entity;
using Rdp.Service;
using Rdp.Service.Dto;
using System.Collections.Generic;

namespace Rdp.Service
{
    public interface IButtonService : IService<Button>
    {
        List<Button> Search(ButtonSearchRequestDto searchRequest, ref GridParams gridParams);
    }
}
