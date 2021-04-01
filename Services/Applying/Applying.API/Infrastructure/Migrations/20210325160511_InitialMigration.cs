using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Applying.API.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "applying");

            migrationBuilder.CreateSequence(
                name: "applicationitemseq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "applicationseq",
                schema: "applying",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "paymentseq",
                schema: "applying",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "studentseq",
                schema: "applying",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "applicationstatus",
                schema: "applying",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicationstatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "paymenttypes",
                schema: "applying",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymenttypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "requests",
                schema: "applying",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "students",
                schema: "applying",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdentityGuid = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "paymentmethods",
                schema: "applying",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentmethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_paymentmethods_paymenttypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalSchema: "applying",
                        principalTable: "paymenttypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_paymentmethods_students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "applying",
                        principalTable: "students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                schema: "applying",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Profile_IDNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profile_Request = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Profile_GenderId = table.Column<int>(type: "int", nullable: true),
                    ApplicationStatusId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_applications_applicationstatus_ApplicationStatusId",
                        column: x => x.ApplicationStatusId,
                        principalSchema: "applying",
                        principalTable: "applicationstatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applications_paymentmethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "applying",
                        principalTable: "paymentmethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_applications_students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "applying",
                        principalTable: "students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "applicationitems",
                schema: "applying",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ScholarshipItemId = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScholarshipItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SlotAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Slots = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicationitems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_applicationitems_applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "applying",
                        principalTable: "applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_applicationitems_ApplicationId",
                schema: "applying",
                table: "applicationitems",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_applications_ApplicationStatusId",
                schema: "applying",
                table: "applications",
                column: "ApplicationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_applications_PaymentMethodId",
                schema: "applying",
                table: "applications",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_applications_StudentId",
                schema: "applying",
                table: "applications",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_paymentmethods_PaymentTypeId",
                schema: "applying",
                table: "paymentmethods",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_paymentmethods_StudentId",
                schema: "applying",
                table: "paymentmethods",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_students_IdentityGuid",
                schema: "applying",
                table: "students",
                column: "IdentityGuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applicationitems",
                schema: "applying");

            migrationBuilder.DropTable(
                name: "requests",
                schema: "applying");

            migrationBuilder.DropTable(
                name: "applications",
                schema: "applying");

            migrationBuilder.DropTable(
                name: "applicationstatus",
                schema: "applying");

            migrationBuilder.DropTable(
                name: "paymentmethods",
                schema: "applying");

            migrationBuilder.DropTable(
                name: "paymenttypes",
                schema: "applying");

            migrationBuilder.DropTable(
                name: "students",
                schema: "applying");

            migrationBuilder.DropSequence(
                name: "applicationitemseq");

            migrationBuilder.DropSequence(
                name: "applicationseq",
                schema: "applying");

            migrationBuilder.DropSequence(
                name: "paymentseq",
                schema: "applying");

            migrationBuilder.DropSequence(
                name: "studentseq",
                schema: "applying");
        }
    }
}
