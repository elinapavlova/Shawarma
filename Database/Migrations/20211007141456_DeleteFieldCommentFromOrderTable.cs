using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class DeleteFieldCommentFromOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Orders",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);
        }
    }
}
