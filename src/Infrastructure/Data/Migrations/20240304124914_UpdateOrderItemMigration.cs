using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TropicFeel.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderItemMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemReference",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemReference",
                table: "Items");
        }
    }
}
