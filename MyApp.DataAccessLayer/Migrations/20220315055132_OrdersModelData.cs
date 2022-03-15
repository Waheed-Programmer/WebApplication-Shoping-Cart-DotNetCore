using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.DataAccessLayer.Migrations
{
    public partial class OrdersModelData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderHeaders_AspNetUsers_ApplicationuserId1",
                table: "orderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_orderHeaders_ApplicationuserId1",
                table: "orderHeaders");

            migrationBuilder.DropColumn(
                name: "ApplicationuserId1",
                table: "orderHeaders");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationuserId",
                table: "orderHeaders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_orderHeaders_ApplicationuserId",
                table: "orderHeaders",
                column: "ApplicationuserId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeaders_AspNetUsers_ApplicationuserId",
                table: "orderHeaders",
                column: "ApplicationuserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderHeaders_AspNetUsers_ApplicationuserId",
                table: "orderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_orderHeaders_ApplicationuserId",
                table: "orderHeaders");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationuserId",
                table: "orderHeaders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationuserId1",
                table: "orderHeaders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orderHeaders_ApplicationuserId1",
                table: "orderHeaders",
                column: "ApplicationuserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeaders_AspNetUsers_ApplicationuserId1",
                table: "orderHeaders",
                column: "ApplicationuserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
