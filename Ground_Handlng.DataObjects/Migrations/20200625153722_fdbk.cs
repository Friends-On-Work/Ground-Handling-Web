using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ground_Handlng.DataObjects.Migrations
{
    public partial class fdbk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "FeedbackRequest",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "FeedbackRequest",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "FeedbackRequest",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RevisedBy",
                table: "FeedbackRequest",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RevisionDate",
                table: "FeedbackRequest",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "FeedbackRequest",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FeedbackRequest",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "FeedbackRequest");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "FeedbackRequest");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "FeedbackRequest");

            migrationBuilder.DropColumn(
                name: "RevisedBy",
                table: "FeedbackRequest");

            migrationBuilder.DropColumn(
                name: "RevisionDate",
                table: "FeedbackRequest");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "FeedbackRequest");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FeedbackRequest");
        }
    }
}
