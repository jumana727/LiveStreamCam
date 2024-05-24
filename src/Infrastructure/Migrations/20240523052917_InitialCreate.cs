using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stream",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stream", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnalyticsSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AnalyticsType = table.Column<int>(type: "integer", nullable: false),
                    ProcessorType = table.Column<int>(type: "integer", nullable: false),
                    SkipFrames = table.Column<long>(type: "bigint", nullable: false),
                    VideoStreamId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyticsSettings_Stream_VideoStreamId",
                        column: x => x.VideoStreamId,
                        principalTable: "Stream",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsSettings_VideoStreamId",
                table: "AnalyticsSettings",
                column: "VideoStreamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalyticsSettings");

            migrationBuilder.DropTable(
                name: "Stream");
        }
    }
}
