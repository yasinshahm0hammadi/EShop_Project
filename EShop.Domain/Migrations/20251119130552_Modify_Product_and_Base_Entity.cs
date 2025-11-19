using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Modify_Product_and_Base_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "TicketMessages");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "TicketMessages");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "SiteBanners");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "SiteBanners");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ProductSelectedCategories");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "ProductSelectedCategories");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ProductFeatures");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "ProductFeatures");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "AboutUs");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "Users",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Users",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "Tickets",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Tickets",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "TicketMessages",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "TicketMessages",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "Sliders",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Sliders",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "SiteSettings",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "SiteSettings",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "SiteBanners",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "SiteBanners",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "Roles",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Roles",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "Questions",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Questions",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "ProductSelectedCategories",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "ProductSelectedCategories",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "Products",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Products",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "ProductFeatures",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "ProductFeatures",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "ProductColors",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "ProductColors",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "ProductCategories",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "ProductCategories",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "Features",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Features",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "Contacts",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Contacts",
                newName: "IsPublished");

            migrationBuilder.RenameColumn(
                name: "editorName",
                table: "AboutUs",
                newName: "Modifiedby");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "AboutUs",
                newName: "IsPublished");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Tickets",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "Tickets",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "TicketMessages",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TicketMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "TicketMessages",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Sliders",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "Sliders",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "SiteSettings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SiteSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "SiteSettings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "SiteBanners",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SiteBanners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "SiteBanners",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Roles",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "Roles",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Questions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "Questions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ProductSelectedCategories",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProductSelectedCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "ProductSelectedCategories",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Products",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "Products",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ProductFeatures",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProductFeatures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "ProductFeatures",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ProductColors",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProductColors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "ProductColors",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ProductCategories",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "ProductCategories",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Features",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Features",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "Features",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Contacts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "Contacts",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "AboutUs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AboutUs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "AboutUs",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TicketMessages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TicketMessages");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "TicketMessages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SiteBanners");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SiteBanners");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "SiteBanners");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductSelectedCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductSelectedCategories");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "ProductSelectedCategories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductFeatures");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductFeatures");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "ProductFeatures");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "AboutUs");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "Users",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Users",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "Tickets",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Tickets",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "TicketMessages",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "TicketMessages",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "Sliders",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Sliders",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "SiteSettings",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "SiteSettings",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "SiteBanners",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "SiteBanners",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "Roles",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Roles",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "Questions",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Questions",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "ProductSelectedCategories",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "ProductSelectedCategories",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "Products",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Products",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "ProductFeatures",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "ProductFeatures",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "ProductColors",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "ProductColors",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "ProductCategories",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "ProductCategories",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "Features",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Features",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "Contacts",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Contacts",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "Modifiedby",
                table: "AboutUs",
                newName: "editorName");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "AboutUs",
                newName: "IsDelete");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "TicketMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "TicketMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Sliders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Sliders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "SiteSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "SiteSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "SiteBanners",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "SiteBanners",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Questions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Questions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ProductSelectedCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "ProductSelectedCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ProductFeatures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "ProductFeatures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ProductColors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "ProductColors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ProductCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "ProductCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Features",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Features",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Contacts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Contacts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "AboutUs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "AboutUs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
