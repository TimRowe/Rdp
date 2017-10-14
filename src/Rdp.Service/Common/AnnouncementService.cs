using Rdp.Core.Data;
using Rdp.Data;
using Rdp.Data.Entity;


namespace Rdp.Service
{
    ///<summary>
    ///系统通告
    ///</summary>
    public partial class AnnouncementService : IAnnouncementService
    {
        IRepository<Announcement> _announcementRepository;

        public IRepository<Announcement> UseRepository
        {
            get
            {
                return _announcementRepository;
            }
        }

        public AnnouncementService()
            : this(RepositoryFactory.Create<Announcement>())
        {
        }

        public AnnouncementService(IRepository<Announcement> announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }


        #region "ExtensionMethod"
        public string GetAnnouncement()
        {
            return "";
              /*StringBuilder strWhere = new StringBuilder();
                strWhere.Append("Status_Flag = 0 ");
                BLLBranch bllBranch = new BLLBranch();
                string district = bllBranch.GetDistrictBySessionBranchCode();
                if (!string.IsNullOrEmpty(district))
                {
                    strWhere.Append("AND District+',' LIKE '%");
                    strWhere.Append(district);
                    strWhere.Append(",%'");
                }
                strWhere.Append(" ORDER BY Announcement_ID DESC");
                DataSet ds = GetList(strWhere.ToString(), "TOP 10 Announcement_Name,Announcement_Url");
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    sb.Append(" <a href=\"javascript:void(0);\" onclick=\"goToAnnouncement('");
                    sb.Append(ds.Tables[0].Rows[i]["Announcement_Url"]);
                    sb.Append("');\">");
                    sb.Append(ds.Tables[0].Rows[i]["Announcement_Name"]);
                    sb.Append("</a>");
                    if (i == 9)
                    {
                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"moreAnnouncement();\">More...</a>");
                    }
                }
                return sb.ToString();*/
        }
        #endregion
    }
}


