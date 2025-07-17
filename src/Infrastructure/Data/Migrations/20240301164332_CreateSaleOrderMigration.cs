using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TropicFeel.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateSaleOrderMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderReferenceJlp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrderDateJlp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderNetSuite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderSprint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrder", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesOrder");
        }
    }
}
