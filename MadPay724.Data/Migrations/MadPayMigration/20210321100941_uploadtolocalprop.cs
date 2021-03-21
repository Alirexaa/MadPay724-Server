using Microsoft.EntityFrameworkCore.Migrations;

namespace MadPay724.Data.Migrations.MadPayMigration
{
    public partial class uploadtolocalprop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UploadToLocal",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadToLocal",
                table: "Settings");
        }
    }
}
