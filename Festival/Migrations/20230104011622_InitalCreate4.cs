using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Festival.Migrations
{
    /// <inheritdoc />
    public partial class InitalCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Categories_CategoriesCategory_ID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Categories_CategoriesCategory_ID",
                table: "News");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribes_Events_EventsEvent_ID",
                table: "Subscribes");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribes_Users_UsersUser_ID",
                table: "Subscribes");

            migrationBuilder.DropIndex(
                name: "IX_Subscribes_EventsEvent_ID",
                table: "Subscribes");

            migrationBuilder.DropIndex(
                name: "IX_Subscribes_UsersUser_ID",
                table: "Subscribes");

            migrationBuilder.DropIndex(
                name: "IX_News_CategoriesCategory_ID",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_Events_CategoriesCategory_ID",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventsEvent_ID",
                table: "Subscribes");

            migrationBuilder.DropColumn(
                name: "UsersUser_ID",
                table: "Subscribes");

            migrationBuilder.DropColumn(
                name: "CategoriesCategory_ID",
                table: "News");

            migrationBuilder.DropColumn(
                name: "CategoriesCategory_ID",
                table: "Events");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventsEvent_ID",
                table: "Subscribes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsersUser_ID",
                table: "Subscribes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoriesCategory_ID",
                table: "News",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoriesCategory_ID",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Subscribes_EventsEvent_ID",
                table: "Subscribes",
                column: "EventsEvent_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribes_UsersUser_ID",
                table: "Subscribes",
                column: "UsersUser_ID");

            migrationBuilder.CreateIndex(
                name: "IX_News_CategoriesCategory_ID",
                table: "News",
                column: "CategoriesCategory_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CategoriesCategory_ID",
                table: "Events",
                column: "CategoriesCategory_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Categories_CategoriesCategory_ID",
                table: "Events",
                column: "CategoriesCategory_ID",
                principalTable: "Categories",
                principalColumn: "Category_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Categories_CategoriesCategory_ID",
                table: "News",
                column: "CategoriesCategory_ID",
                principalTable: "Categories",
                principalColumn: "Category_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribes_Events_EventsEvent_ID",
                table: "Subscribes",
                column: "EventsEvent_ID",
                principalTable: "Events",
                principalColumn: "Event_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribes_Users_UsersUser_ID",
                table: "Subscribes",
                column: "UsersUser_ID",
                principalTable: "Users",
                principalColumn: "User_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
