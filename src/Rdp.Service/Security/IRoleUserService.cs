using Rdp.Core.Data;
using Rdp.Data.Entity;
using Rdp.Service.Dto;
using System;
using System.Collections.Generic;
using System.Data;

namespace Rdp.Service
{
    public interface IRoleUserService : IService<RoleUser>
    {
        RoleUser GetModel(string userID);

        int Add(String userID, String roleID);

        bool Update(String userID, String roleID);

        DataSet GetList(String strWhere, String filedList);

        List<RoleUserSearchResultDto> Search(RoleUserSearchRequestDto searchRequest, ref GridParams gridParams);
    }
}
