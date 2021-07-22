using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ground_Handlng.DataObjects.Migrations
{
    public partial class Illustartion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Illustration",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    RevisionDate = table.Column<DateTime>(nullable: false),
                    RevisedBy = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    RequiredForm = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    FlightNumber = table.Column<string>(nullable: true),
                    Witness = table.Column<string>(nullable: true),
                    TelephoneNo = table.Column<int>(nullable: false),
                    PermanentAddress = table.Column<string>(nullable: true),
                    KindOfDocument = table.Column<string>(nullable: true),
                    Signed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Illustration", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Illustration");
        }
    }
}
