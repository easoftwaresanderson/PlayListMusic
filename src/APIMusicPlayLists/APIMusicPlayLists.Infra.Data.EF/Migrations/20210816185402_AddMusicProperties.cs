using Microsoft.EntityFrameworkCore.Migrations;

namespace APIMusicPlayLists.Infra.Data.EF.Migrations
{
    public partial class AddMusicProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlbumName",
                table: "Music",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlbumNotes",
                table: "Music",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AlbumYear",
                table: "Music",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlbumName",
                table: "Music");

            migrationBuilder.DropColumn(
                name: "AlbumNotes",
                table: "Music");

            migrationBuilder.DropColumn(
                name: "AlbumYear",
                table: "Music");
        }
    }
}
