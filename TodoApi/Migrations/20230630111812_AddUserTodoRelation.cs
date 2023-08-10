using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTodoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Secret",
                table: "TodoItems");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TodoItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_UserId",
                table: "TodoItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Users_UserId",
                table: "TodoItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Users_UserId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_UserId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TodoItems");

            migrationBuilder.AddColumn<string>(
                name: "Secret",
                table: "TodoItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
