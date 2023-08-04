using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todoAPP.Migrations
{
    public partial class fix_constraint_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TodoList",
                newName: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TodoList_UserID",
                table: "TodoList",
                column: "UserID");

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

            migrationBuilder.DropIndex(
                name: "IX_TodoList_UserID",
                table: "TodoList");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "TodoList",
                newName: "UserId");
        }
    }
}
