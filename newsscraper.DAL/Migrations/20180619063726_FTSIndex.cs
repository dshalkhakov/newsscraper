using Microsoft.EntityFrameworkCore.Migrations;

namespace newsscraper.DAL.Migrations
{
    public partial class FTSIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE INDEX Articles_TitleContent_FTS ON ""Articles"" USING GIN (to_tsvector('russian', ""Title"" || ' ' || ""Contents""));");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX Articles_TitleContent_FTS;");
        }
    }
}
