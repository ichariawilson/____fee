using Microsoft.EntityFrameworkCore.Migrations;

namespace Scholarship.API.Infrastructure.ScholarshipMigrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "scholarship_course_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "scholarship_currency_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "scholarship_duration_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "scholarship_educationlevel_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "scholarship_feestructure_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "scholarship_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "scholarship_interest_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "scholarship_location_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "scholarship_school_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "ScholarshipCurrency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipCurrency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScholarshipDuration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipDuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScholarshipEducationLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EducationLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipEducationLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScholarshipInterest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Interest = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipInterest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScholarshipLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScholarshipCourse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ScholarshipDurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScholarshipCourse_ScholarshipDuration_ScholarshipDurationId",
                        column: x => x.ScholarshipDurationId,
                        principalTable: "ScholarshipDuration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scholarship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PictureFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScholarshipCurrencyId = table.Column<int>(type: "int", nullable: false),
                    ScholarshipDurationId = table.Column<int>(type: "int", nullable: false),
                    ScholarshipEducationLevelId = table.Column<int>(type: "int", nullable: false),
                    ScholarshipInterestId = table.Column<int>(type: "int", nullable: false),
                    ScholarshipLocationId = table.Column<int>(type: "int", nullable: false),
                    AvailableSlots = table.Column<int>(type: "int", nullable: false),
                    ReslotThreshold = table.Column<int>(type: "int", nullable: false),
                    MaxSlotThreshold = table.Column<int>(type: "int", nullable: false),
                    OnReapply = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scholarship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scholarship_ScholarshipCurrency_ScholarshipCurrencyId",
                        column: x => x.ScholarshipCurrencyId,
                        principalTable: "ScholarshipCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scholarship_ScholarshipDuration_ScholarshipDurationId",
                        column: x => x.ScholarshipDurationId,
                        principalTable: "ScholarshipDuration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scholarship_ScholarshipEducationLevel_ScholarshipEducationLevelId",
                        column: x => x.ScholarshipEducationLevelId,
                        principalTable: "ScholarshipEducationLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scholarship_ScholarshipInterest_ScholarshipInterestId",
                        column: x => x.ScholarshipInterestId,
                        principalTable: "ScholarshipInterest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scholarship_ScholarshipLocation_ScholarshipLocationId",
                        column: x => x.ScholarshipLocationId,
                        principalTable: "ScholarshipLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScholarshipSchool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ScholarshipLocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipSchool", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScholarshipSchool_ScholarshipLocation_ScholarshipLocationId",
                        column: x => x.ScholarshipLocationId,
                        principalTable: "ScholarshipLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScholarshipFeeStructure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ScholarshipCurrencyId = table.Column<int>(type: "int", nullable: false),
                    ScholarshipDurationId = table.Column<int>(type: "int", nullable: false),
                    ScholarshipEducationLevelId = table.Column<int>(type: "int", nullable: false),
                    ScholarshipCourseId = table.Column<int>(type: "int", nullable: false),
                    ScholarshipSchoolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipFeeStructure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScholarshipFeeStructure_ScholarshipCourse_ScholarshipCourseId",
                        column: x => x.ScholarshipCourseId,
                        principalTable: "ScholarshipCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScholarshipFeeStructure_ScholarshipCurrency_ScholarshipCurrencyId",
                        column: x => x.ScholarshipCurrencyId,
                        principalTable: "ScholarshipCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScholarshipFeeStructure_ScholarshipDuration_ScholarshipDurationId",
                        column: x => x.ScholarshipDurationId,
                        principalTable: "ScholarshipDuration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScholarshipFeeStructure_ScholarshipEducationLevel_ScholarshipEducationLevelId",
                        column: x => x.ScholarshipEducationLevelId,
                        principalTable: "ScholarshipEducationLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScholarshipFeeStructure_ScholarshipSchool_ScholarshipSchoolId",
                        column: x => x.ScholarshipSchoolId,
                        principalTable: "ScholarshipSchool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scholarship_ScholarshipCurrencyId",
                table: "Scholarship",
                column: "ScholarshipCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Scholarship_ScholarshipDurationId",
                table: "Scholarship",
                column: "ScholarshipDurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Scholarship_ScholarshipEducationLevelId",
                table: "Scholarship",
                column: "ScholarshipEducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Scholarship_ScholarshipInterestId",
                table: "Scholarship",
                column: "ScholarshipInterestId");

            migrationBuilder.CreateIndex(
                name: "IX_Scholarship_ScholarshipLocationId",
                table: "Scholarship",
                column: "ScholarshipLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipCourse_ScholarshipDurationId",
                table: "ScholarshipCourse",
                column: "ScholarshipDurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipFeeStructure_ScholarshipCourseId",
                table: "ScholarshipFeeStructure",
                column: "ScholarshipCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipFeeStructure_ScholarshipCurrencyId",
                table: "ScholarshipFeeStructure",
                column: "ScholarshipCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipFeeStructure_ScholarshipDurationId",
                table: "ScholarshipFeeStructure",
                column: "ScholarshipDurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipFeeStructure_ScholarshipEducationLevelId",
                table: "ScholarshipFeeStructure",
                column: "ScholarshipEducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipFeeStructure_ScholarshipSchoolId",
                table: "ScholarshipFeeStructure",
                column: "ScholarshipSchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipSchool_ScholarshipLocationId",
                table: "ScholarshipSchool",
                column: "ScholarshipLocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scholarship");

            migrationBuilder.DropTable(
                name: "ScholarshipFeeStructure");

            migrationBuilder.DropTable(
                name: "ScholarshipInterest");

            migrationBuilder.DropTable(
                name: "ScholarshipCourse");

            migrationBuilder.DropTable(
                name: "ScholarshipCurrency");

            migrationBuilder.DropTable(
                name: "ScholarshipEducationLevel");

            migrationBuilder.DropTable(
                name: "ScholarshipSchool");

            migrationBuilder.DropTable(
                name: "ScholarshipDuration");

            migrationBuilder.DropTable(
                name: "ScholarshipLocation");

            migrationBuilder.DropSequence(
                name: "scholarship_course_hilo");

            migrationBuilder.DropSequence(
                name: "scholarship_currency_hilo");

            migrationBuilder.DropSequence(
                name: "scholarship_duration_hilo");

            migrationBuilder.DropSequence(
                name: "scholarship_educationlevel_hilo");

            migrationBuilder.DropSequence(
                name: "scholarship_feestructure_hilo");

            migrationBuilder.DropSequence(
                name: "scholarship_hilo");

            migrationBuilder.DropSequence(
                name: "scholarship_interest_hilo");

            migrationBuilder.DropSequence(
                name: "scholarship_location_hilo");

            migrationBuilder.DropSequence(
                name: "scholarship_school_hilo");
        }
    }
}
