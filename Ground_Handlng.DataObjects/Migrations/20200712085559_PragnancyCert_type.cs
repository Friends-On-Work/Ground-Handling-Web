using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ground_Handlng.DataObjects.Migrations
{
    public partial class PragnancyCert_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PregnancyCertificate",
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
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    DateOfExamination = table.Column<DateTime>(nullable: false),
                    DateOfTravel = table.Column<DateTime>(nullable: false),
                    DateOfBirthEstimated = table.Column<DateTime>(nullable: false),
                    DateCertificateIssued = table.Column<DateTime>(nullable: false),
                    SignatureOfRelative = table.Column<string>(nullable: true),
                    SignatureOfPhysician = table.Column<string>(nullable: true),
                    SignatureOfPassenger = table.Column<string>(nullable: true),
                    PregnancyCertificateType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PregnancyCertificate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaiverFormForAcceptance",
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
                    BagTagNumber = table.Column<string>(nullable: true),
                    NumberAndType = table.Column<string>(nullable: true),
                    DateOfTravel = table.Column<DateTime>(nullable: false),
                    SignatureOfPassenger = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaiverFormForAcceptance", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PregnancyCertificate");

            migrationBuilder.DropTable(
                name: "WaiverFormForAcceptance");
        }
    }
}
