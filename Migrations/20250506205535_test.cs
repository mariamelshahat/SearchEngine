using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchEngine.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Urlswithranks",
                columns: table => new
                {
                    URL = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<int>(type: "int", nullable: false),
                    PageRank = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urlswithranks", x => x.URL);
                    table.UniqueConstraint("AK_Urlswithranks_FileName", x => x.FileName);
                });

            migrationBuilder.CreateTable(
                name: "inverted_index",
                columns: table => new
                {
                    Word = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inverted_index", x => new { x.Word, x.FileId });
                    table.ForeignKey(
                        name: "FK_inverted_index_Urlswithranks_FileId",
                        column: x => x.FileId,
                        principalTable: "Urlswithranks",
                        principalColumn: "FileName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inverted_index_FileId",
                table: "inverted_index",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inverted_index");

            migrationBuilder.DropTable(
                name: "Urlswithranks");
        }
    }
}
