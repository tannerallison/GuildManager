#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace GuildManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelateMinionToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BossId",
                table: "Minions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Minions_BossId",
                table: "Minions",
                column: "BossId");

            migrationBuilder.AddForeignKey(
                name: "FK_Minions_Players_BossId",
                table: "Minions",
                column: "BossId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Minions_Players_BossId",
                table: "Minions");

            migrationBuilder.DropIndex(
                name: "IX_Minions_BossId",
                table: "Minions");

            migrationBuilder.DropColumn(
                name: "BossId",
                table: "Minions");
        }
    }
}
