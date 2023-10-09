using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tpcly.Lti.Samples.Spa.Migrations
{
    public partial class RemoveToolPlatformRedirectUri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RedirectUri",
                table: "ToolPlatforms",
                newName: "ClientSecret");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientSecret",
                table: "ToolPlatforms",
                newName: "RedirectUri");
        }
    }
}
