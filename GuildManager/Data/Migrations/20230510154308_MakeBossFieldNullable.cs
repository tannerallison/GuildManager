#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace GuildManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeBossFieldNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Minions_Players_BossId",
                table: "Minions");

            migrationBuilder.AlterColumn<int>(
                name: "BossId",
                table: "Minions",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Minions_Players_BossId",
                table: "Minions",
                column: "BossId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Minions_Players_BossId",
                table: "Minions");

            migrationBuilder.AlterColumn<int>(
                name: "BossId",
                table: "Minions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Minions_Players_BossId",
                table: "Minions",
                column: "BossId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
