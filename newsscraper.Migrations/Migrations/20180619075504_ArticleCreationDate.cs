using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace newsscraper.Migrations
{
    public partial class ArticleCreationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Articles",
                nullable: true);

            // Articles URI uniquness constraint
            migrationBuilder.CreateIndex(
                name: "IX_Articles_Uri",
                table: "Articles",
                column: "Uri",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Uri",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Articles");
        }
    }
}
