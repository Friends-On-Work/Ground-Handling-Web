using Microsoft.EntityFrameworkCore.Migrations;

namespace Ground_Handlng.DataObjects.Migrations
{
    public partial class update_waiver_and_pregnancy_email_add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberAndType",
                table: "WaiverFormForAcceptance",
                newName: "TypePETAVI");

            migrationBuilder.AddColumn<string>(
                name: "NumberPETAVI",
                table: "WaiverFormForAcceptance",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailOfCaptain",
                table: "PregnancyCertificate",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailOfPassenger",
                table: "PregnancyCertificate",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberPETAVI",
                table: "WaiverFormForAcceptance");

            migrationBuilder.DropColumn(
                name: "EmailOfCaptain",
                table: "PregnancyCertificate");

            migrationBuilder.DropColumn(
                name: "EmailOfPassenger",
                table: "PregnancyCertificate");

            migrationBuilder.RenameColumn(
                name: "TypePETAVI",
                table: "WaiverFormForAcceptance",
                newName: "NumberAndType");
        }
    }
}
