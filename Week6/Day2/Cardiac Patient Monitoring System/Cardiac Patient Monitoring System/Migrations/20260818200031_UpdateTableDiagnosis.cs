using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cardiac_Patient_Monitoring_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableDiagnosis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordedByDoctorName",
                table: "Diagnoses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Diagnoses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecordedByDoctorName",
                table: "Diagnoses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Diagnoses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
