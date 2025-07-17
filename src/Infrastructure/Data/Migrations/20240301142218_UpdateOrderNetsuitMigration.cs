using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TropicFeel.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderNetsuitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustbodyPwkTipoSoId",
                table: "CustbodyPwkTipoSos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustbodyPwkTipoSoId",
                table: "CustbodyPwkTipoSos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
