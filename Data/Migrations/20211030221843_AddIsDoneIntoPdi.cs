using Microsoft.EntityFrameworkCore.Migrations;

namespace PdiManager.Data.Migrations
{
    public partial class AddIsDoneIntoPdi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "Pdis",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "Pdis");
        }
    }
}
