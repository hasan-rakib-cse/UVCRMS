using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UVCRMS.Migrations
{
    /// <inheritdoc />
    public partial class mig02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TeacherRemainingCredit",
                table: "Teachers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherRemainingCredit",
                table: "Teachers");
        }
    }
}
