﻿// <auto-generated />
using System;
using Ground_Handlng.DataObjects.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ground_Handlng.DataObjects.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200625064015_user_For_Login_Response")]
    partial class user_For_Login_Response
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Master.BulletinMaster.BulletinFrom", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("RevisedBy");

                    b.Property<DateTime>("RevisionDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("BulletinFrom");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Master.BulletinMaster.BulletinNoticeType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("RevisedBy");

                    b.Property<DateTime>("RevisionDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.Property<string>("TypeName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("BulletinNoticeType");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Master.BulletinMaster.BulletinTo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("RevisedBy");

                    b.Property<DateTime>("RevisionDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("BulletinTo");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Master.BulletinMaster.BulletinTypes", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("RevisedBy");

                    b.Property<DateTime>("RevisionDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.Property<string>("TypeName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("BulletinTypes");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Master.Menus", b =>
                {
                    b.Property<long>("MenuId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Icon");

                    b.Property<string>("Name");

                    b.Property<long?>("ParentId");

                    b.Property<string>("Privilages");

                    b.Property<string>("RevisedBy");

                    b.Property<DateTime>("RevisionDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.Property<string>("Url");

                    b.HasKey("MenuId");

                    b.HasIndex("ParentId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Master.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int>("NotificationState");

                    b.Property<int>("NotificationType");

                    b.Property<string>("ObjectURL");

                    b.Property<string>("ReceiverId");

                    b.Property<string>("SenderId");

                    b.Property<string>("Subject");

                    b.Property<DateTime>("TimeSent");

                    b.HasKey("NotificationId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Master.PasswordStore", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("PasswordStore");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Operational.Bulletin.Bulletin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BulletinUrl");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("Date");

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<string>("NoticeNumber")
                        .IsRequired();

                    b.Property<string>("NoticeOrBulletin")
                        .IsRequired();

                    b.Property<string>("NoticeTo")
                        .IsRequired();

                    b.Property<string>("Remark");

                    b.Property<string>("RevisedBy");

                    b.Property<DateTime>("RevisionDate");

                    b.Property<string>("SentBy")
                        .IsRequired();

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Bulletin");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Operational.Manual.Book", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BookTitle")
                        .IsRequired();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("RevisedBy");

                    b.Property<DateTime>("RevisionDate");

                    b.Property<int>("SequenceNo");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Operational.Manual.BookChapter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BookChapterNumber")
                        .IsRequired();

                    b.Property<string>("BookChapterTitle")
                        .IsRequired();

                    b.Property<long>("BookId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("RevisedBy");

                    b.Property<DateTime>("RevisionDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("BookChapters");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Operational.Manual.BookChapterSection", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BookChapterID");

                    b.Property<string>("BookSectionNumber")
                        .IsRequired();

                    b.Property<string>("BookSectionTitle")
                        .IsRequired();

                    b.Property<string>("BookUrl");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("EndDate");

                    b.Property<long?>("GroupId");

                    b.Property<bool>("HasUpdate");

                    b.Property<string>("RevisedBy");

                    b.Property<DateTime>("RevisionDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("BookChapterID");

                    b.HasIndex("GroupId");

                    b.ToTable("BookSections");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Operational.Manual.ManualAccessLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BookChapterId");

                    b.Property<long>("BookId");

                    b.Property<long>("BookSectionId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("DownloadTime");

                    b.Property<bool>("Downloaded");

                    b.Property<string>("EmployeeId")
                        .IsRequired();

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("RevisedBy");

                    b.Property<DateTime>("RevisionDate");

                    b.Property<bool>("Seen");

                    b.Property<DateTime>("SeenTime");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.Property<DateTime>("SyncTime");

                    b.Property<int>("UpdateVersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("BookChapterId");

                    b.HasIndex("BookId");

                    b.HasIndex("BookSectionId");

                    b.ToTable("ManualAccessLog");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Others.AccessLog", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ActionDate");

                    b.Property<string>("ActionDid");

                    b.Property<string>("FromWhichComputer");

                    b.Property<string>("FullName");

                    b.Property<string>("FunctionalityType");

                    b.Property<string>("StatementTaken");

                    b.Property<string>("UserGroup");

                    b.Property<string>("UserName");

                    b.HasKey("id");

                    b.ToTable("AccessLogs");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationPrivilege", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action");

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("Privileges");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationRolePrivilege", b =>
                {
                    b.Property<string>("RoleId");

                    b.Property<string>("PrivilegeId");

                    b.Property<string>("ApplicationRoleId");

                    b.Property<string>("PrivilageId");

                    b.HasKey("RoleId", "PrivilegeId");

                    b.HasIndex("ApplicationRoleId");

                    b.HasIndex("PrivilageId");

                    b.ToTable("RolePrivileges");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("FirstLogin");

                    b.Property<string>("FullName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Position");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<string>", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUserRole<string>");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationUserRole", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUserRole<string>");

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("RoleId1");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("RoleId1");

                    b.HasDiscriminator().HasValue("ApplicationUserRole");
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Master.Menus", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.Master.Menus", "ParentMenuId")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Master.Notification", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationUser", "ReceiverUser")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationUser", "SenderUser")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Operational.Manual.BookChapter", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.Operational.Manual.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Operational.Manual.BookChapterSection", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.Operational.Manual.BookChapter", "BookChapters")
                        .WithMany()
                        .HasForeignKey("BookChapterID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Ground_Handlng.DataObjects.Models.Operational.Manual.BookChapter", "BookChapterSections")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.Operational.Manual.ManualAccessLog", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.Operational.Manual.BookChapter", "BookChapters")
                        .WithMany()
                        .HasForeignKey("BookChapterId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Ground_Handlng.DataObjects.Models.Operational.Manual.Book", "Books")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Ground_Handlng.DataObjects.Models.Operational.Manual.BookChapterSection", "BookSections")
                        .WithMany()
                        .HasForeignKey("BookSectionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationRolePrivilege", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationRole")
                        .WithMany("RolePrivileges")
                        .HasForeignKey("ApplicationRoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationPrivilege", "Privilage")
                        .WithMany()
                        .HasForeignKey("PrivilageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationUserRole", b =>
                {
                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationUser")
                        .WithMany("UserRoles")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Ground_Handlng.DataObjects.Models.UserManagment.Identity.ApplicationRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId1")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
