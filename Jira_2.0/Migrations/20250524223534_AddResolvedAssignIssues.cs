using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jira_2._0.Migrations
{
    /// <inheritdoc />
    public partial class AddResolvedAssignIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsClaimVerified = table.Column<bool>(type: "bit", nullable: false),
                    RequestedClaim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRejected = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignedIssues",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    IssueID = table.Column<int>(type: "int", nullable: false),
                    AssignedUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedIssues", x => new { x.IssueID, x.ProjectID });
                    table.ForeignKey(
                        name: "FK_AssignedIssues_ApplicationUser_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignedIssues_Issues_IssueID",
                        column: x => x.IssueID,
                        principalTable: "Issues",
                        principalColumn: "IssueID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignedIssues_ProjectsData_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "ProjectsData",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResolvedIssues",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    IssueID = table.Column<int>(type: "int", nullable: false),
                    ResolvedUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolvedIssues", x => new { x.IssueID, x.ProjectID });
                    table.ForeignKey(
                        name: "FK_ResolvedIssues_ApplicationUser_ResolvedUserId",
                        column: x => x.ResolvedUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResolvedIssues_Issues_IssueID",
                        column: x => x.IssueID,
                        principalTable: "Issues",
                        principalColumn: "IssueID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResolvedIssues_ProjectsData_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "ProjectsData",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedIssues_AssignedUserId",
                table: "AssignedIssues",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedIssues_ProjectID",
                table: "AssignedIssues",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ResolvedIssues_ProjectID",
                table: "ResolvedIssues",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ResolvedIssues_ResolvedUserId",
                table: "ResolvedIssues",
                column: "ResolvedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedIssues");

            migrationBuilder.DropTable(
                name: "ResolvedIssues");

            migrationBuilder.DropTable(
                name: "ApplicationUser");
        }
    }
}
