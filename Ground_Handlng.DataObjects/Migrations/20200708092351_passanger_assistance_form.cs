using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ground_Handlng.DataObjects.Migrations
{
    public partial class passanger_assistance_form : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ambulance",
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
                    Address = table.Column<string>(nullable: true),
                    AmbulanceCompanyContact = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ambulance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FrequentTravelerMedicalCard",
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
                    FrequentTravelerCardNumber = table.Column<string>(nullable: true),
                    IssuedBy = table.Column<string>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrequentTravelerMedicalCard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntendedEscorts",
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
                    Title = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PNR = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    LanguageSpoken = table.Column<string>(nullable: true),
                    MedicalQualification = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntendedEscorts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeetAndAssist",
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
                    Contact = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetAndAssist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherGroundArrangment",
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
                    Remark = table.Column<string>(nullable: true),
                    DepatureAirport = table.Column<string>(nullable: true),
                    TransitAirport = table.Column<string>(nullable: true),
                    ArrivalAirport = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherGroundArrangment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProposedIternary",
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
                    Airlines = table.Column<string>(nullable: true),
                    FlightNumber = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    FlightDate = table.Column<DateTime>(nullable: false),
                    Orgin = table.Column<string>(nullable: true),
                    Destination = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposedIternary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialInflightNeed",
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
                    TypeOfArrangment = table.Column<int>(nullable: false),
                    Equipment = table.Column<int>(nullable: false),
                    ArrangingCompanyName = table.Column<string>(nullable: true),
                    ArrivalAirport = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialInflightNeed", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WheelChairNeeded",
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
                    WheelChairType = table.Column<int>(nullable: false),
                    WheelChairCatagory = table.Column<int>(nullable: false),
                    CollapsaibleWCOB = table.Column<bool>(nullable: false),
                    OwnWheelChair = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WheelChairNeeded", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PassangerAssistanceForm",
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
                    Title = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(nullable: false),
                    PNR = table.Column<string>(nullable: true),
                    NatureOfDisablity = table.Column<string>(nullable: true),
                    StreacherNeeded = table.Column<bool>(nullable: false),
                    SpecialInflightNeedId = table.Column<long>(nullable: true),
                    ProposedIternaryId = table.Column<long>(nullable: true),
                    AmbulanceId = table.Column<long>(nullable: true),
                    MeetAndAssistId = table.Column<long>(nullable: true),
                    OtherGroundArrangmentId = table.Column<long>(nullable: true),
                    WheelChairNeededsId = table.Column<long>(nullable: true),
                    IntendedEscortsId = table.Column<long>(nullable: true),
                    FrequentTravelerMedicalCardId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassangerAssistanceForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassangerAssistanceForm_Ambulance_AmbulanceId",
                        column: x => x.AmbulanceId,
                        principalTable: "Ambulance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PassangerAssistanceForm_FrequentTravelerMedicalCard_FrequentTravelerMedicalCardId",
                        column: x => x.FrequentTravelerMedicalCardId,
                        principalTable: "FrequentTravelerMedicalCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PassangerAssistanceForm_IntendedEscorts_IntendedEscortsId",
                        column: x => x.IntendedEscortsId,
                        principalTable: "IntendedEscorts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PassangerAssistanceForm_MeetAndAssist_MeetAndAssistId",
                        column: x => x.MeetAndAssistId,
                        principalTable: "MeetAndAssist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PassangerAssistanceForm_OtherGroundArrangment_OtherGroundArrangmentId",
                        column: x => x.OtherGroundArrangmentId,
                        principalTable: "OtherGroundArrangment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PassangerAssistanceForm_ProposedIternary_ProposedIternaryId",
                        column: x => x.ProposedIternaryId,
                        principalTable: "ProposedIternary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PassangerAssistanceForm_SpecialInflightNeed_SpecialInflightNeedId",
                        column: x => x.SpecialInflightNeedId,
                        principalTable: "SpecialInflightNeed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PassangerAssistanceForm_WheelChairNeeded_WheelChairNeededsId",
                        column: x => x.WheelChairNeededsId,
                        principalTable: "WheelChairNeeded",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PassangerAssistanceForm_AmbulanceId",
                table: "PassangerAssistanceForm",
                column: "AmbulanceId");

            migrationBuilder.CreateIndex(
                name: "IX_PassangerAssistanceForm_FrequentTravelerMedicalCardId",
                table: "PassangerAssistanceForm",
                column: "FrequentTravelerMedicalCardId");

            migrationBuilder.CreateIndex(
                name: "IX_PassangerAssistanceForm_IntendedEscortsId",
                table: "PassangerAssistanceForm",
                column: "IntendedEscortsId");

            migrationBuilder.CreateIndex(
                name: "IX_PassangerAssistanceForm_MeetAndAssistId",
                table: "PassangerAssistanceForm",
                column: "MeetAndAssistId");

            migrationBuilder.CreateIndex(
                name: "IX_PassangerAssistanceForm_OtherGroundArrangmentId",
                table: "PassangerAssistanceForm",
                column: "OtherGroundArrangmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PassangerAssistanceForm_ProposedIternaryId",
                table: "PassangerAssistanceForm",
                column: "ProposedIternaryId");

            migrationBuilder.CreateIndex(
                name: "IX_PassangerAssistanceForm_SpecialInflightNeedId",
                table: "PassangerAssistanceForm",
                column: "SpecialInflightNeedId");

            migrationBuilder.CreateIndex(
                name: "IX_PassangerAssistanceForm_WheelChairNeededsId",
                table: "PassangerAssistanceForm",
                column: "WheelChairNeededsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PassangerAssistanceForm");

            migrationBuilder.DropTable(
                name: "Ambulance");

            migrationBuilder.DropTable(
                name: "FrequentTravelerMedicalCard");

            migrationBuilder.DropTable(
                name: "IntendedEscorts");

            migrationBuilder.DropTable(
                name: "MeetAndAssist");

            migrationBuilder.DropTable(
                name: "OtherGroundArrangment");

            migrationBuilder.DropTable(
                name: "ProposedIternary");

            migrationBuilder.DropTable(
                name: "SpecialInflightNeed");

            migrationBuilder.DropTable(
                name: "WheelChairNeeded");
        }
    }
}
