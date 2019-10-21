using Rdp.Core.Data;
using Rdp.Data.Entity;
using Rdp.Service.Dto;
using System.Collections.Generic;

namespace Rdp.Service
{
    public interface IProgramService : IService<Program>
    {
        MenuTreeDto GetNavigationItem(RoleUser user);
        MenuTreeDto GetNavigationItemV3(RoleUser user);
        MenuTreeDto GetNavigationItemV3(List<RoleUser> roleUsers);
        List<ProgramSearchResultDto> Search(ProgramSearchRequestDto searchRequest, ref GridParams gridParams);
        bool Add(Program model);
    }
}
