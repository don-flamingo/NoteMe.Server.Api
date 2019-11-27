using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteMe.Server.Infrastructure.Sql.Migrations
{
    public partial class note_tags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Notes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Notes");
        }
    }
}
