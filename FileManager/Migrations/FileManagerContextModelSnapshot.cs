﻿// <auto-generated />
using System;
using FileManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FileManager.Migrations
{
    [DbContext(typeof(FileManagerContext))]
    partial class FileManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FileManager.Models.Database.DepartmentsDocuments.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("FileManager.Models.Database.DepartmentsDocuments.DepartmentsDocument", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("DepartmentId");

                    b.Property<Guid>("ReportingYearDocumentTitleId");

                    b.HasKey("Id");

                    b.HasAlternateKey("DepartmentId", "ReportingYearDocumentTitleId");

                    b.HasIndex("ReportingYearDocumentTitleId");

                    b.ToTable("DepartmentsDocument");
                });

            modelBuilder.Entity("FileManager.Models.Database.DepartmentsDocuments.DepartmentsDocumentsVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("DepartmentDocumentId");

                    b.Property<string>("FileName");

                    b.Property<string>("Path");

                    b.Property<DateTime>("UploadedDateTime");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentDocumentId");

                    b.ToTable("DepartmentsDocumentsVersion");
                });

            modelBuilder.Entity("FileManager.Models.Database.DocumentStatus.DocumentStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("DocumentStatus");
                });

            modelBuilder.Entity("FileManager.Models.Database.DocumentStatus.DocumentStatusHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CommentEdits");

                    b.Property<Guid>("DepartmentsDocumentId");

                    b.Property<Guid>("DocumentStatusId");

                    b.Property<DateTime>("SettingDateTime");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentsDocumentId");

                    b.HasIndex("DocumentStatusId");

                    b.HasIndex("UserId");

                    b.ToTable("DocumentStatusHistory");
                });

            modelBuilder.Entity("FileManager.Models.Database.DocumentStatus.RoleStatus", b =>
                {
                    b.Property<Guid>("RoleId");

                    b.Property<Guid>("DocumentStatusId");

                    b.Property<Guid>("Id");

                    b.HasKey("RoleId", "DocumentStatusId");

                    b.HasIndex("DocumentStatusId");

                    b.ToTable("RoleStatus");
                });

            modelBuilder.Entity("FileManager.Models.Database.ReportingYearDocumentTitles.DocumentTitle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<Guid>("DocumentTypeId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("DocumentTypeId");

                    b.ToTable("DocumentTitle");
                });

            modelBuilder.Entity("FileManager.Models.Database.ReportingYearDocumentTitles.DocumentType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("DocumentType");
                });

            modelBuilder.Entity("FileManager.Models.Database.ReportingYearDocumentTitles.ReportingYear", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.ToTable("ReportingYear");
                });

            modelBuilder.Entity("FileManager.Models.Database.ReportingYearDocumentTitles.ReportingYearDocumentTitle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("DocumentTitleId");

                    b.Property<Guid>("ReportingYearId");

                    b.HasKey("Id");

                    b.HasAlternateKey("DocumentTitleId", "ReportingYearId");

                    b.HasIndex("ReportingYearId");

                    b.ToTable("ReportingYearDocumentTitle");
                });

            modelBuilder.Entity("FileManager.Models.Database.UserDepartmentRoles.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.HasKey("Id");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("FileManager.Models.Database.UserDepartmentRoles.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FistName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

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

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("FileManager.Models.Database.UserDepartmentRoles.UserDepartmentRole", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.Property<Guid>("DepartmentId");

                    b.HasKey("UserId", "RoleId", "DepartmentId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserDepartmentRoles");
                });

            modelBuilder.Entity("FileManager.Models.Database.UserSystemRoles.SystemRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetSystemRoles");
                });

            modelBuilder.Entity("FileManager.Models.Database.UserSystemRoles.UserSystemRole", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserSystemRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FileManager.Models.Database.DepartmentsDocuments.DepartmentsDocument", b =>
                {
                    b.HasOne("FileManager.Models.Database.DepartmentsDocuments.Department", "Department")
                        .WithMany("DepartmentsDocuments")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FileManager.Models.Database.ReportingYearDocumentTitles.ReportingYearDocumentTitle", "ReportingYearDocumentTitle")
                        .WithMany("DepartmentsDocuments")
                        .HasForeignKey("ReportingYearDocumentTitleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FileManager.Models.Database.DepartmentsDocuments.DepartmentsDocumentsVersion", b =>
                {
                    b.HasOne("FileManager.Models.Database.DepartmentsDocuments.DepartmentsDocument", "DepartmentsDocument")
                        .WithMany("DepartmentsDocumentsVersions")
                        .HasForeignKey("DepartmentDocumentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FileManager.Models.Database.DocumentStatus.DocumentStatusHistory", b =>
                {
                    b.HasOne("FileManager.Models.Database.DepartmentsDocuments.DepartmentsDocument", "DepartmentsDocument")
                        .WithMany("DocumentStatusHistories")
                        .HasForeignKey("DepartmentsDocumentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FileManager.Models.Database.DocumentStatus.DocumentStatus", "DocumentStatus")
                        .WithMany("DocumentStatusHistories")
                        .HasForeignKey("DocumentStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FileManager.Models.Database.UserDepartmentRoles.User", "User")
                        .WithMany("DocumentStatusHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FileManager.Models.Database.DocumentStatus.RoleStatus", b =>
                {
                    b.HasOne("FileManager.Models.Database.DocumentStatus.DocumentStatus", "DocumentStatus")
                        .WithMany("RoleStatuses")
                        .HasForeignKey("DocumentStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FileManager.Models.Database.UserDepartmentRoles.Role", "Role")
                        .WithMany("RoleStatuses")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FileManager.Models.Database.ReportingYearDocumentTitles.DocumentTitle", b =>
                {
                    b.HasOne("FileManager.Models.Database.ReportingYearDocumentTitles.DocumentType", "DocumentType")
                        .WithMany("DocumentTitles")
                        .HasForeignKey("DocumentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FileManager.Models.Database.ReportingYearDocumentTitles.ReportingYearDocumentTitle", b =>
                {
                    b.HasOne("FileManager.Models.Database.ReportingYearDocumentTitles.DocumentTitle", "DocumentTitle")
                        .WithMany("ReportingYearDocumentTitles")
                        .HasForeignKey("DocumentTitleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FileManager.Models.Database.ReportingYearDocumentTitles.ReportingYear", "ReportingYear")
                        .WithMany("ReportingYearDocumentTitles")
                        .HasForeignKey("ReportingYearId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FileManager.Models.Database.UserDepartmentRoles.UserDepartmentRole", b =>
                {
                    b.HasOne("FileManager.Models.Database.DepartmentsDocuments.Department", "Department")
                        .WithMany("UserDepartmentRoles")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FileManager.Models.Database.UserDepartmentRoles.Role", "Role")
                        .WithMany("UserDepartmentRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FileManager.Models.Database.UserDepartmentRoles.User", "User")
                        .WithMany("UserDepartmentRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FileManager.Models.Database.UserSystemRoles.UserSystemRole", b =>
                {
                    b.HasOne("FileManager.Models.Database.UserSystemRoles.SystemRole", "SystemRole")
                        .WithMany("UserSystemRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FileManager.Models.Database.UserDepartmentRoles.User", "User")
                        .WithMany("UserSystemRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("FileManager.Models.Database.UserSystemRoles.SystemRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("FileManager.Models.Database.UserDepartmentRoles.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("FileManager.Models.Database.UserDepartmentRoles.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("FileManager.Models.Database.UserDepartmentRoles.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
