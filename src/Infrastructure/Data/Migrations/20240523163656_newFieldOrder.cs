using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TropicFeel.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class newFieldOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustbodyPwkOrderType",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustbodyPwkOrderType",
                table: "Orders");
        }
    }
}
