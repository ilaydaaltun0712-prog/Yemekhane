using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Yemekhane.Migrations
{
    /// <inheritdoc />
    public partial class GunYemekIliskisi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.CreateTable(
                name: "Gunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gunler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Yemekler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yemekler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GunYemekleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GunId = table.Column<int>(type: "int", nullable: false),
                    YemekId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GunYemekleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GunYemekleri_Gunler_GunId",
                        column: x => x.GunId,
                        principalTable: "Gunler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GunYemekleri_Yemekler_YemekId",
                        column: x => x.YemekId,
                        principalTable: "Yemekler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Gunler",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 1, "Pazartesi" },
                    { 2, "Salı" },
                    { 3, "Çarşamba" },
                    { 4, "Perşembe" },
                    { 5, "Cuma" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GunYemekleri_GunId_YemekId",
                table: "GunYemekleri",
                columns: new[] { "GunId", "YemekId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GunYemekleri_YemekId",
                table: "GunYemekleri",
                column: "YemekId");

            migrationBuilder.CreateIndex(
                name: "IX_Yemekler_Ad",
                table: "Yemekler",
                column: "Ad",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GunYemekleri");

            migrationBuilder.DropTable(
                name: "Gunler");

            migrationBuilder.DropTable(
                name: "Yemekler");

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gun = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "Gun", "Icerik" },
                values: new object[,]
                {
                    { 1, "Pazartesi", "" },
                    { 2, "Salı", "" },
                    { 3, "Çarşamba", "" },
                    { 4, "Perşembe", "" },
                    { 5, "Cuma", "" }
                });
        }
    }
}
