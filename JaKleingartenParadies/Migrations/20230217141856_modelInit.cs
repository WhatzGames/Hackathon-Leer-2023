using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JaKleingartenParadies.Migrations
{
    /// <inheritdoc />
    public partial class modelInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestTable",
                columns: table => new
                {
                    Uid = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Test1 = table.Column<string>(type: "TEXT", nullable: false),
                    Test2 = table.Column<string>(type: "TEXT", nullable: false),
                    Test3 = table.Column<string>(type: "TEXT", nullable: false),
                    Test4 = table.Column<string>(type: "TEXT", nullable: false),
                    Test5 = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTable", x => x.Uid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestTable");
        }
    }
}
