using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cardiac_Patient_Monitoring_System.Migrations
{
    /// <inheritdoc />
    public partial class AddAtributeToDiagnosis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConditionStartDate",
                table: "Diagnoses",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConditionStartDate",
                table: "Diagnoses");
        }
    }
}
