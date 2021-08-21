using Microsoft.EntityFrameworkCore.Migrations;

namespace APIMusicPlayLists.Infra.Data.EF.Migrations
{
    public partial class AddFieldsMusicFavoriteAndDeviceUniqueId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Favorite",
                table: "Music",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UniqueID",
                table: "Device",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favorite",
                table: "Music");

            migrationBuilder.DropColumn(
                name: "UniqueID",
                table: "Device");
        }
    }
}
