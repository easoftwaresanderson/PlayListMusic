using Microsoft.EntityFrameworkCore.Migrations;

namespace APIMusicPlayLists.Infra.Data.EF.Migrations
{
    public partial class ForenkeyPlayListOnDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Device_PlayList_PlayListId",
                table: "Device");

            migrationBuilder.DropIndex(
                name: "IX_Device_PlayListId",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "PlayListId",
                table: "Device");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayListId",
                table: "Device",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Device_PlayListId",
                table: "Device",
                column: "PlayListId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Device_PlayList_PlayListId",
                table: "Device",
                column: "PlayListId",
                principalTable: "PlayList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
