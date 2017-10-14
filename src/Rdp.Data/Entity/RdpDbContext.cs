using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rdp.Core.Data;

namespace Rdp.Data.Entity
{
    public partial class RdpDbContext : DbContext
    {
		
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			//Name=MyDbContext"
			optionsBuilder.UseSqlServer(DbHelperSql.DefaultQueryConn);
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
             modelBuilder.Entity<Announcement>().HasKey(x => x.AnnouncementID);

             modelBuilder.Entity<Area>().HasKey(x => x.AreaID);

             modelBuilder.Entity<BatchGenerator>().HasKey(x => x.Code);

             modelBuilder.Entity<BranchMember>().HasKey(x => x.BranchMember_);

             modelBuilder.Entity<Button>().HasKey(x => x.ButtonID);

             modelBuilder.Entity<CodeTable>().HasKey(x => x.TableID);

             modelBuilder.Entity<Country>().HasKey(x => x.CountryCode);

             modelBuilder.Entity<Domain>().HasKey(x => x.DomainID);

             modelBuilder.Entity<ErrorInfo>().HasKey(x => x.ErrorInfoID);

             modelBuilder.Entity<GlobalResources>().HasKey(x => x.Id);

             modelBuilder.Entity<InformationHistory>().HasKey(x => x.Seq);

             modelBuilder.Entity<Job>().HasKey(x => x.JobID);

             modelBuilder.Entity<Operation>().HasKey(x => x.OperationID);

             modelBuilder.Entity<Privilege>().HasKey(x => x.PrivilegeID);

             modelBuilder.Entity<Program>().HasKey(x => x.ProgramID);

             modelBuilder.Entity<ProgramButton>().HasKey(x => x.ProgramButtonID);

             modelBuilder.Entity<RoleMaster>().HasKey(x => x.RoleID);

             modelBuilder.Entity<RoleUser>().HasKey(x => x.UserRoleID);

             modelBuilder.Entity<Site>().HasKey(x => x.SiteID);

             modelBuilder.Entity<UserIP>().HasKey(x => x.UserIPID);

             modelBuilder.Entity<UserMaster>().HasKey(x => x.UserID);

             modelBuilder.Entity<UserSite>().HasKey(x => new { x.UserSiteID, x.UserID, x.SiteID });

             modelBuilder.Entity<VersionControl>().HasKey(x => x.Key);

        }

   }
}
// </auto-generated>
