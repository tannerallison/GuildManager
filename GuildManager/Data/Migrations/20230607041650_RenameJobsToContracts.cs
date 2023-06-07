using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuildManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameJobsToContracts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobMinion");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    PatronId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Players_PatronId",
                        column: x => x.PatronId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContractMinion",
                columns: table => new
                {
                    AssignedMinionsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContractsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractMinion", x => new { x.AssignedMinionsId, x.ContractsId });
                    table.ForeignKey(
                        name: "FK_ContractMinion_Contracts_ContractsId",
                        column: x => x.ContractsId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractMinion_Minions_AssignedMinionsId",
                        column: x => x.AssignedMinionsId,
                        principalTable: "Minions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractMinion_ContractsId",
                table: "ContractMinion",
                column: "ContractsId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PatronId",
                table: "Contracts",
                column: "PatronId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractMinion");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PatronId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Players_PatronId",
                        column: x => x.PatronId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobMinion",
                columns: table => new
                {
                    AssignedMinionsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    JobsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobMinion", x => new { x.AssignedMinionsId, x.JobsId });
                    table.ForeignKey(
                        name: "FK_JobMinion_Jobs_JobsId",
                        column: x => x.JobsId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobMinion_Minions_AssignedMinionsId",
                        column: x => x.AssignedMinionsId,
                        principalTable: "Minions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobMinion_JobsId",
                table: "JobMinion",
                column: "JobsId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_PatronId",
                table: "Jobs",
                column: "PatronId");
        }
    }
}
