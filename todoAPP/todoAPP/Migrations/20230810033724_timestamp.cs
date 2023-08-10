using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todoAPP.Migrations
{
    public partial class timestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TodoList",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TodoList",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TodoList");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TodoList");
        }
    }
}
