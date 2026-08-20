using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cardiac_Patient_Monitoring_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePatientRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Diagnoses_Doctors_DoctorId",
                table: "Diagnoses");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Doctors_DoctorId",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_VitalSigns_Doctors_DoctorId",
                table: "VitalSigns");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_VitalSigns_DoctorId",
                table: "VitalSigns");

            migrationBuilder.DropIndex(
                name: "IX_Medications_DoctorId",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Diagnoses_DoctorId",
                table: "Diagnoses");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "VitalSigns");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "RecordedByName",
                table: "VitalSigns",
                newName: "RecordedByDoctorName");

            migrationBuilder.RenameColumn(
                name: "PrescribedByName",
                table: "Medications",
                newName: "PrescribedByDoctorName");

            migrationBuilder.AddColumn<string>(
                name: "RecordedByDoctorName",
                table: "Diagnoses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecordedByDoctorName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordedByDoctorName",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "RecordedByDoctorName",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "RecordedByDoctorName",
                table: "VitalSigns",
                newName: "RecordedByName");

            migrationBuilder.RenameColumn(
                name: "PrescribedByDoctorName",
                table: "Medications",
                newName: "PrescribedByName");

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "VitalSigns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Medications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Diagnoses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.DoctorId);
                    table.ForeignKey(
                        name: "FK_Doctors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VitalSigns_DoctorId",
                table: "VitalSigns",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_DoctorId",
                table: "Medications",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_DoctorId",
                table: "Diagnoses",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UserId",
                table: "Doctors",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnoses_Doctors_DoctorId",
                table: "Diagnoses",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Doctors_DoctorId",
                table: "Medications",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSigns_Doctors_DoctorId",
                table: "VitalSigns",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
