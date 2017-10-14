using Rdp.Core.Data;
using Rdp.Data.Entity;
using Rdp.Service.Dto;
using System;
using System.Collections.Generic;
using System.Data;

namespace Rdp.Service
{
    public interface IProgramButtonService : IService<ProgramButton>
    {
        int Add(int programID, String buttonID);

        int Add(int programID, String buttonID, String Url);

        bool Update(int programID, string buttonID);

        bool Update(int programID, string buttonID, string Url);

        bool Update(int programButtonID, int programID, string buttonID, string Url);
        DataSet GetList(int programID);

        DataSet GetList(string strWhere, string fieldList, int privilegeMaster, string privilegeValue);

        List<ProgramButtonSearchResultDto> Search(ProgramButtonSearchRequestDto searchRequest, ref GridParams gridParams);
    }
}
