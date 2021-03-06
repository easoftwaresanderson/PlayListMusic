using Microsoft.EntityFrameworkCore.Migrations;

namespace APIMusicPlayLists.Infra.Data.EF.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayListName = table.Column<string>(type: "TEXT", nullable: true),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UniqueID = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Model = table.Column<string>(type: "TEXT", nullable: true),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: true),
                    VersionString = table.Column<string>(type: "TEXT", nullable: true),
                    Platform = table.Column<string>(type: "TEXT", nullable: true),
                    Idiom = table.Column<string>(type: "TEXT", nullable: true),
                    DeviceType = table.Column<string>(type: "TEXT", nullable: true),
                    PlayListID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Device_PlayList_PlayListID",
                        column: x => x.PlayListID,
                        principalTable: "PlayList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Music",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MusicName = table.Column<string>(type: "TEXT", nullable: true),
                    ArtistName = table.Column<string>(type: "TEXT", nullable: true),
                    AlbumImage = table.Column<string>(type: "TEXT", nullable: true),
                    AlbumName = table.Column<string>(type: "TEXT", nullable: true),
                    AlbumYear = table.Column<int>(type: "INTEGER", nullable: false),
                    AlbumNotes = table.Column<string>(type: "TEXT", nullable: true),
                    Favorite = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayListID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Music", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Music_PlayList_PlayListID",
                        column: x => x.PlayListID,
                        principalTable: "PlayList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Device_PlayListID",
                table: "Device",
                column: "PlayListID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Music_PlayListID",
                table: "Music",
                column: "PlayListID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "Music");

            migrationBuilder.DropTable(
                name: "PlayList");
        }
    }
}
