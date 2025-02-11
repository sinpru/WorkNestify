using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkNestify.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsersJobApplicationsCompanyReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobStatuses_JobStatusID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobTypes_JobTypeID",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyID1",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobStatusID1",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobTypeID1",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyReviews",
                columns: table => new
                {
                    CompanyReviewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyReviewContent = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<double>(type: "FLOAT", nullable: false),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    CompanyID1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyReviews", x => x.CompanyReviewID);
                    table.ForeignKey(
                        name: "FK_CompanyReviews_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyReviews_Companies_CompanyID1",
                        column: x => x.CompanyID1,
                        principalTable: "Companies",
                        principalColumn: "CompanyID");
                });

            migrationBuilder.CreateTable(
                name: "JobApplicationStatuses",
                columns: table => new
                {
                    JobApplicationStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplicationStatuses", x => x.JobApplicationStatusID);
                });

            migrationBuilder.CreateTable(
                name: "Employers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employers_ApplicationUsers_Id",
                        column: x => x.Id,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employers_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobSeekers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResumeUrl = table.Column<string>(type: "NVARCHAR(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSeekers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobSeekers_ApplicationUsers_Id",
                        column: x => x.Id,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    JobApplicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoverLetter = table.Column<string>(type: "TEXT", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    JobSeekerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobID = table.Column<int>(type: "int", nullable: false),
                    JobApplicationStatusID = table.Column<int>(type: "int", nullable: false),
                    JobApplicationStatusID1 = table.Column<int>(type: "int", nullable: true),
                    JobID1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.JobApplicationID);
                    table.ForeignKey(
                        name: "FK_JobApplications_JobApplicationStatuses_JobApplicationStatusID",
                        column: x => x.JobApplicationStatusID,
                        principalTable: "JobApplicationStatuses",
                        principalColumn: "JobApplicationStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobApplications_JobApplicationStatuses_JobApplicationStatusID1",
                        column: x => x.JobApplicationStatusID1,
                        principalTable: "JobApplicationStatuses",
                        principalColumn: "JobApplicationStatusID");
                    table.ForeignKey(
                        name: "FK_JobApplications_JobSeekers_JobSeekerID",
                        column: x => x.JobSeekerID,
                        principalTable: "JobSeekers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobApplications_Jobs_JobID",
                        column: x => x.JobID,
                        principalTable: "Jobs",
                        principalColumn: "JobID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobApplications_Jobs_JobID1",
                        column: x => x.JobID1,
                        principalTable: "Jobs",
                        principalColumn: "JobID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyID",
                table: "Jobs",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyID1",
                table: "Jobs",
                column: "CompanyID1");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobStatusID1",
                table: "Jobs",
                column: "JobStatusID1");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobTypeID1",
                table: "Jobs",
                column: "JobTypeID1");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyReviews_CompanyID",
                table: "CompanyReviews",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyReviews_CompanyID1",
                table: "CompanyReviews",
                column: "CompanyID1");

            migrationBuilder.CreateIndex(
                name: "IX_Employers_CompanyID",
                table: "Employers",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobApplicationStatusID",
                table: "JobApplications",
                column: "JobApplicationStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobApplicationStatusID1",
                table: "JobApplications",
                column: "JobApplicationStatusID1");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobID",
                table: "JobApplications",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobID1",
                table: "JobApplications",
                column: "JobID1");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobSeekerID",
                table: "JobApplications",
                column: "JobSeekerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Companies_CompanyID",
                table: "Jobs",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Companies_CompanyID1",
                table: "Jobs",
                column: "CompanyID1",
                principalTable: "Companies",
                principalColumn: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobStatuses_JobStatusID",
                table: "Jobs",
                column: "JobStatusID",
                principalTable: "JobStatuses",
                principalColumn: "JobStatusID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobStatuses_JobStatusID1",
                table: "Jobs",
                column: "JobStatusID1",
                principalTable: "JobStatuses",
                principalColumn: "JobStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobTypes_JobTypeID",
                table: "Jobs",
                column: "JobTypeID",
                principalTable: "JobTypes",
                principalColumn: "JobTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobTypes_JobTypeID1",
                table: "Jobs",
                column: "JobTypeID1",
                principalTable: "JobTypes",
                principalColumn: "JobTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Companies_CompanyID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Companies_CompanyID1",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobStatuses_JobStatusID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobStatuses_JobStatusID1",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobTypes_JobTypeID",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobTypes_JobTypeID1",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "CompanyReviews");

            migrationBuilder.DropTable(
                name: "Employers");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropTable(
                name: "JobApplicationStatuses");

            migrationBuilder.DropTable(
                name: "JobSeekers");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_CompanyID",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_CompanyID1",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobStatusID1",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobTypeID1",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CompanyID1",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobStatusID1",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobTypeID1",
                table: "Jobs");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobStatuses_JobStatusID",
                table: "Jobs",
                column: "JobStatusID",
                principalTable: "JobStatuses",
                principalColumn: "JobStatusID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobTypes_JobTypeID",
                table: "Jobs",
                column: "JobTypeID",
                principalTable: "JobTypes",
                principalColumn: "JobTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
