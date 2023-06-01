using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuildManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingPatronToJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PatronId",
                table: "Jobs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_PatronId",
                table: "Jobs",
                column: "PatronId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Players_PatronId",
                table: "Jobs",
                column: "PatronId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Players_PatronId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_PatronId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "PatronId",
                table: "Jobs");
        }
    }
}
