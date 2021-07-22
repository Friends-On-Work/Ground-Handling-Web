using Microsoft.EntityFrameworkCore.Migrations;

namespace Ground_Handlng.DataObjects.Migrations
{
    public partial class groupId_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookSections_BookChapters_GroupId",
                table: "BookSections");

            migrationBuilder.AlterColumn<string>(
                name: "BookSectionTitle",
                table: "BookSections",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "BookSectionNumber",
                table: "BookSections",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_BookSections_BookSections_GroupId",
                table: "BookSections",
                column: "GroupId",
                principalTable: "BookSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookSections_BookSections_GroupId",
                table: "BookSections");

            migrationBuilder.AlterColumn<string>(
                name: "BookSectionTitle",
                table: "BookSections",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BookSectionNumber",
                table: "BookSections",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookSections_BookChapters_GroupId",
                table: "BookSections",
                column: "GroupId",
                principalTable: "BookChapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
