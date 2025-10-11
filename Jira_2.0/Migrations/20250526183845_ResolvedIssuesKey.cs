using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jira_2._0.Migrations
{
    /// <inheritdoc />
    public partial class ResolvedIssuesKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedIssues_ApplicationUser_AssignedUserId",
                table: "AssignedIssues");

            migrationBuilder.DropForeignKey(
                name: "FK_ResolvedIssues_ApplicationUser_ResolvedUserId",
                table: "ResolvedIssues");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_ResolvedIssues_ResolvedUserId",
                table: "ResolvedIssues");

            migrationBuilder.DropIndex(
                name: "IX_AssignedIssues_AssignedUserId",
                table: "AssignedIssues");

            migrationBuilder.AlterColumn<string>(
                name: "ResolvedUserId",
                table: "ResolvedIssues",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedUserId",
                table: "AssignedIssues",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResolvedUserId",
                table: "ResolvedIssues",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedUserId",
                table: "AssignedIssues",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    IsClaimVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsRejected = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    RequestedClaim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResolvedIssues_ResolvedUserId",
                table: "ResolvedIssues",
                column: "ResolvedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedIssues_AssignedUserId",
                table: "AssignedIssues",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedIssues_ApplicationUser_AssignedUserId",
                table: "AssignedIssues",
                column: "AssignedUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResolvedIssues_ApplicationUser_ResolvedUserId",
                table: "ResolvedIssues",
                column: "ResolvedUserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
