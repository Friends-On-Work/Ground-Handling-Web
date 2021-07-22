using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ground_Handlng.DataObjects.Models.Master;
using Ground_Handlng.DataObjects.Models.Others;
using Ground_Handlng.DataObjects.Models.UserManagment.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ground_Handlng.DataObjects.Models.Operational.Manual;
using Ground_Handlng.DataObjects.Models.Master.BulletinMaster;
using Ground_Handlng.DataObjects.Models.Operational.Bulletin;
using Ground_Handlng.DataObjects.Models.Operational;
using Ground_Handlng.DataObjects.Models.Operational.Forms.PassangerAssistanceForm;
using Ground_Handlng.DataObjects.Models.Operational.Forms.IndemnityCertificate;
using Ground_Handlng.DataObjects.Models.Operational.Forms.PregnancyCertificate;
using Ground_Handlng.DataObjects.Models.Operational.Forms.WaiverFormForAcceptance;
using Ground_Handlng.DataObjects.Models.Operational.Forms.Illustration;
using Ground_Handlng.DataObjects.Models.Operational.Forms.HandoverForm;

namespace Ground_Handlng.DataObjects.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext)
           : base(dbContext)
        {

        }
        /// <summary>
        /// Overridden On Model Creating method
        /// </summary>
        /// <param name="modelBuilder">Database Model Builder</param>
        ///          
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException("ModelBuilder is NULL");

            base.OnModelCreating(modelBuilder);

            //Rename the default table names
            modelBuilder.Entity<IdentityRole<string>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            //Name the new tables; define keys and relations
            modelBuilder.Entity<ApplicationRole>().HasKey(r => r.Id);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").HasMany((ApplicationUser u) => u.UserRoles);
            modelBuilder.Entity<ApplicationUserRole>()/*.HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId })*/;
            modelBuilder.Entity<ApplicationPrivilege>().ToTable("Privileges").HasKey(p => p.Id);
            modelBuilder.Entity<ApplicationRole>().HasMany((ApplicationRole r) => r.RolePrivileges);
            modelBuilder.Entity<ApplicationRolePrivilege>().ToTable("RolePrivileges").HasKey(p => new { p.RoleId, p.PrivilegeId });
            //modelBuilder.Entity<SalesOrder>().HasIndex(u => u.OrderCode).IsUnique(true);

            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        public DbSet<ApplicationPrivilege> ApplicationPrivileges { get; set; }
        public DbSet<ApplicationRolePrivilege> ApplicationRolePrivileges { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<PasswordStore> PasswordStore { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<BookChapter> BookChapter { get; set; }
        public DbSet<BookChapterSection> BookChapterSection { get; set; }
        public DbSet<ManualAccessLog> ManualAccessLog { get; set; }
        public DbSet<BulletinTypes> BulletinTypes { get; set; }
        public DbSet<BulletinNoticeType> BulletinNoticeType { get; set; }
        public DbSet<BulletinTo> BulletinTo { get; set; }
        public DbSet<BulletinFrom> BulletinFrom { get; set; }
        public DbSet<Bulletin> Bulletin { get; set; }
        public DbSet<FeedbackRequest> FeedbackRequests { get; set; }
        public DbSet<CostSavingIdeaRequest> CostSavingIdeaRequests { get; set; }
        public DbSet<FrequentlyAskedQuestion> FrequentlyAskedQuestions { get; set; }
        public DbSet<ManagementContact> ManagementContacts { get; set; }
        public DbSet<SliderViewItem> SliderViewItems { get; set; }

        //Important Forms
        public DbSet<PassangerAssistanceForm> PassangerAssistanceForm { get; set; }
        public DbSet<Ambulance> Ambulance { get; set; }
        public DbSet<FrequentTravelerMedicalCard> FrequentTravelerMedicalCard { get; set; }
        public DbSet<IntendedEscorts> IntendedEscorts { get; set; }
        public DbSet<MeetAndAssist> MeetAndAssist { get; set; }
        public DbSet<OtherGroundArrangment> OtherGroundArrangment { get; set; }
        public DbSet<ProposedIternary> ProposedIternary { get; set; }
        public DbSet<SpecialInflightNeed> SpecialInflightNeed { get; set; }
        public DbSet<WheelChairNeeded> WheelChairNeeded { get; set; }
        //IndemnityCertificate
        public DbSet<IndemnityCertificate> IndemnityCertificate { get; set; }
        public DbSet<PregnancyCertificate> PregnancyCertificate { get; set; }
        public DbSet<WaiverFormForAcceptance> WaiverFormForAcceptance { get; set; }
        public DbSet<Illustration> Illustration { get; set; }
        public DbSet<HandoverForm> HandoverForm { get; set; }
        public DbSet<HandOverStaff> HandOverStaff { get; set; }
    }
}
