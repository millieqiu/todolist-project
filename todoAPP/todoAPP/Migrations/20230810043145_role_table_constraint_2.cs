using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todoAPP.Migrations
{
    public partial class role_table_constraint_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoList_Users_UserID",
                table: "TodoList");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "TodoList",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoList_Users_UserID",
                table: "TodoList",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoList_Users_UserID",
                table: "TodoList");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "TodoList",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoList_Users_UserID",
                table: "TodoList",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
