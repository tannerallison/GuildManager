using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuildManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class SwitchPrivilegeToCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivilegeRole_Privileges_PrivilegesId",
                table: "PrivilegeRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Privileges",
                table: "Privileges");

            migrationBuilder.CreateIndex(name: "IX_Privileges_Code", table: "Privileges", column: "Code", unique: true);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Privileges");

            migrationBuilder.RenameColumn(
                name: "PrivilegesId",
                table: "PrivilegeRole",
                newName: "PrivilegesCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Privileges",
                table: "Privileges",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivilegeRole_Privileges_PrivilegesCode",
                table: "PrivilegeRole",
                column: "PrivilegesCode",
                principalTable: "Privileges",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivilegeRole_Privileges_PrivilegesCode",
                table: "PrivilegeRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Privileges",
                table: "Privileges");

            migrationBuilder.RenameColumn(
                name: "PrivilegesCode",
                table: "PrivilegeRole",
                newName: "PrivilegesId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Privileges",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(name: "IX_Privileges_Code", table: "Privileges", column: "Id", unique: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Privileges",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Privileges",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Privileges",
                table: "Privileges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivilegeRole_Privileges_PrivilegesId",
                table: "PrivilegeRole",
                column: "PrivilegesId",
                principalTable: "Privileges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
