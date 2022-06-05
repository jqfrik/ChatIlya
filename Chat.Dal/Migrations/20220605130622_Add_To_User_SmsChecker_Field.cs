using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Dal.Migrations
{
    public partial class Add_To_User_SmsChecker_Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SmsChecker",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmsChecker",
                table: "Users");
        }
    }
}
