using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rdp.Core.Data;
using Rdp.Data.Entity;
using Rdp.Service.Dto;
using Rdp.Data;
using Rdp.Service.Extension;

namespace Rdp.Service
{
    /// <summary>
    /// 程序表
    /// </summary>
    public partial class ProgramService : IProgramService
    {
        IRepository<Program> _programRepository;
        IRepository<Privilege> _privilegeRepository;

        public IRepository<Program> UseRepository
        {
            get
            {
                return _programRepository;
            }

        }

        public ProgramService()
            : this(RepositoryFactory.Create<Program>(), RepositoryFactory.Create<Privilege>())
        {
        }

        public ProgramService(IRepository<Program> programRepository, IRepository<Privilege> privilegeRepository)
        {
            _programRepository = programRepository;
            _privilegeRepository = privilegeRepository;
        }


        public bool Add(Program model)
        {
            var programId = (from u in _programRepository.Table
                             where u.ParentID == model.ParentID
                             select u.ProgramID).DefaultIfEmpty().Max();
            model.ProgramID = programId == 0 ? 1000 * model.ParentID + 1 : programId + 1;
            return ServiceExtensions.Add(this, model);

        }

        #region "ExtensionMethod"
        
        class ProgramDto
        {
            public int ProgramID { get; set; }
            public string ProgramName { get; set; }
            public string Icon { get; set; }
            public int ParentID { get; set; }
            public Int16 Priority { get; set; }
            public string Url { get; set; }
        }

        class ProgramExDto
        {
            public int ProgramID { get; set; }
            public string ProgramName { get; set; }
            public string Icon { get; set; }
            public int ParentID { get; set; }
            public Int16 Priority { get; set; }
            public string Url { get; set; }
            public int Levels { get; set; }
        }


        public MenuTreeDto GetNavigationItemV3(RoleUser user)
        {
                var list = _programRepository.SqlQuery<ProgramExDto>(@"WITH Tmp
                  AS(SELECT DISTINCT
                                PR.Access_Value AS Program_ID
                       FROM     dbo.tbLOG_Privilege PR
                       WHERE    Access_Master = 1
                                AND((PR.Privilege_Master = 1
                                        AND PR.Privilege_Value = @p0
                                      )
                                      OR(PR.Privilege_Master = 2
                                           AND PR.Privilege_Value = @p1
                                         )
                                    )
                                AND PR.Operation_ID <> 3
                     ),
                Sd
                  AS(SELECT   Program_ID AS ProgramID,
                                Program_Name AS ProgramName,
                                Icon,
                                Parent_ID  AS ParentID,
                                Priority,
                                Url,
                                3 AS Levels
                       FROM     dbo.tbLOG_Program
                       WHERE    Status_Flag = 0
                                AND Is_Visible = 0
                                AND Parent_ID <> 0
                                AND Program_ID IN(SELECT  Program_ID
                                                    FROM    Tmp)
                     ),
                S2
                  AS(SELECT   Program_ID AS ProgramID,
                                Program_Name AS ProgramName,
                                Icon,
                                Parent_ID AS ParentID,
                                Priority,
                                Url,
                                2 AS Levels
                       FROM     dbo.tbLOG_Program
                       WHERE    Status_Flag = 0
                                AND Is_Visible = 0
                                AND Program_ID IN(SELECT  ParentID
                                                    FROM    Sd)
                     ),
                S3 AS(SELECT   Program_ID AS ProgramID,
                                Program_Name AS ProgramName,
                                Icon,
                                Parent_ID AS ParentID,
                                Priority,
                                Url,
                                1 AS Levels
                       FROM     dbo.tbLOG_Program
                       WHERE    Status_Flag = 0
                                AND Is_Visible = 0
                                AND Program_ID IN(SELECT  ParentID
                                                    FROM    S2)
                     )
                SELECT ProgramID,
                        ProgramName,
                        Icon,
                        ParentID,
                        Priority,
                        Url,
                        Levels
                FROM S3
                UNION
                SELECT  ProgramID ,
                        ProgramName ,
                        Icon ,
                        ParentID ,
                        Priority ,
                        Url ,
                        Levels
                FROM    S2
                UNION
                SELECT ProgramID,
                        ProgramName,
                        Icon,
                        ParentID,
                        Priority,
                        Url,
                        Levels
                FROM Sd", user.RoleID.ToString(), user.UserID);

            var menu = new MenuTreeDto() { CurrentMenu = null, ChildMenus = new List<MenuTreeDto>() };

            foreach (var e in list)
            {
                if (e.Levels != 1) continue;

                var secondMenuTree = new MenuTreeDto()
                {
                    CurrentMenu = new Program()
                    {
                        ProgramName = e.ProgramName,
                        ProgramID = e.ProgramID,
                        Icon = e.Icon,
                        ParentID = e.ParentID,
                        Url = e.Url
                    },
                    ChildMenus = new List<MenuTreeDto>()
                };

                foreach (var e1 in list)
                {
                    if (e1.Levels != 2 || e1.ParentID != e.ProgramID) continue;

                
                        var threesecondMenuTree = new MenuTreeDto()
                        {
                            CurrentMenu = new Program()
                            {
                                ProgramName = e1.ProgramName,
                                ProgramID = e1.ProgramID,
                                Icon = e1.Icon,
                                ParentID = e1.ParentID,
                                Url = e1.Url
                            },
                            ChildMenus = new List<MenuTreeDto>()
                        };

                        foreach (var e2 in list)
                        {
                            if (e2.Levels != 3 || e2.ParentID != e1.ProgramID) continue;

                            threesecondMenuTree.ChildMenus.Add(new MenuTreeDto()
                            {
                                CurrentMenu = new Program()
                                {
                                    ProgramName = e2.ProgramName,
                                    ProgramID = e2.ProgramID,
                                    Icon = e2.Icon,
                                    ParentID = e2.ParentID,
                                    Url = e2.Url
                                },
                                ChildMenus = null
                            });
                        }

                        secondMenuTree.ChildMenus.Add(threesecondMenuTree);
                    
                }

                menu.ChildMenus.Add(secondMenuTree);
            }

            return menu;

        }


        public MenuTreeDto GetNavigationItem(RoleUser user)
        {
            var list  = _programRepository.SqlQuery<ProgramDto>(@"

    WITH    Tmp AS
            (SELECT DISTINCT
                        PR.Access_Value AS Program_ID
               FROM     dbo.tbLOG_Privilege PR
               WHERE    Access_Master = 1
                        AND((PR.Privilege_Master = 1 AND PR.Privilege_Value = @p0)
                              OR(PR.Privilege_Master = 2 AND PR.Privilege_Value = @p1))
                        AND PR.Operation_ID <> 3
             ), Sd AS
            (SELECT   Program_ID,
                        Program_Name,
                        Icon,
                        Parent_ID,
                        Priority,
                        Url
               FROM     dbo.tbLOG_Program
               WHERE    Status_Flag = 0
                        AND Is_Visible = 0
                        AND Parent_ID <> 0 AND Program_ID IN ( SELECT  Program_ID FROM    Tmp)
             )
            SELECT  Program_ID as ProgramID,
                    Program_Name as ProgramName,
                    Icon,
                    Parent_ID as ParentID,
                    Priority,
                    Url
            FROM dbo.tbLOG_Program
            WHERE   Status_Flag = 0
                    AND Is_Visible = 0
                    AND Parent_ID = 0
                    AND Program_ID IN(SELECT  Parent_ID FROM    Sd)
            UNION
            SELECT  Program_ID as ProgramID,
                    Program_Name as ProgramName,
                    Icon ,
                    Parent_ID as ParentID,
                    Priority,
                    Url
            FROM    Sd ORDER BY Priority; ", user.RoleID.ToString(), user.UserID);


            var menu = new MenuTreeDto() { CurrentMenu = null, ChildMenus = new List<MenuTreeDto>() };

            foreach (var e in list)
            {
                if(e.ParentID != 0)continue;

                var secondMenuTree = new MenuTreeDto()
                {
                    CurrentMenu = new Program()
                    {
                        ProgramName = e.ProgramName,
                        ProgramID = e.ProgramID,
                        Icon = e.Icon,
                        ParentID = e.ParentID,
                        Url = e.Url
                    },
                    ChildMenus = new List<MenuTreeDto>()
                };
               
                foreach(var e1 in list)
                {
                    if(e1.ParentID == e.ProgramID && e1.ParentID != 0)
                    {
                        secondMenuTree.ChildMenus.Add(new MenuTreeDto()
                        {
                            CurrentMenu = new Program()
                            {
                            ProgramName = e1.ProgramName,
                            ProgramID = e1.ProgramID,
                            Icon = e1.Icon,
                            ParentID = e1.ParentID,
                            Url = e1.Url
                            },
                            ChildMenus = null
                        });
                    }
                }

                menu.ChildMenus.Add(secondMenuTree);
            }

            return menu;
        }

        

        public List<ProgramSearchResultDto> Search(ProgramSearchRequestDto searchRequest, ref GridParams gridParams)
        {
            var query = from program in _programRepository.Table
                        join parent in _programRepository.Table
                             on program.ParentID equals parent.ProgramID
                        select new ProgramSearchResultDto()
                        {
                            ProgramID = program.ProgramID,
                            ProgramName = program.ProgramName,
                            ParentID = program.ParentID,
                            ParentName = parent.ProgramName,
                            Url = program.Url,
                            Icon = program.Icon,
                            IsVisible = program.IsVisible,
                            Priority = program.Priority,
                            StatusFlag = program.StatusFlag
                        };

            query= query.Where(b=>!(
                            (searchRequest.ProgramID != 0 && b.ProgramID != searchRequest.ProgramID) ||
                            (!string.IsNullOrEmpty(searchRequest.ProgramName) && b.ProgramName.Trim() != searchRequest.ProgramName) ||
                            (searchRequest.ParentID != 0 && b.ParentID != searchRequest.ParentID)
                        ));

            gridParams.TotalCount = query.Count();
            return QueryExtensions.SortAndPage(query, gridParams).ToList();
        }

        


        #endregion
    }




    
}


