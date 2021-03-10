using Microsoft.EntityFrameworkCore.Migrations;

namespace MadPay724.Data.Migrations
{
    public partial class changeBandCardEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiredDateYear",
                table: "BankCards",
                newName: "ExpireDateYear");

            migrationBuilder.RenameColumn(
                name: "ExpiredDateMonth",
                table: "BankCards",
                newName: "ExpireDateMonth");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpireDateYear",
                table: "BankCards",
                newName: "ExpiredDateYear");

            migrationBuilder.RenameColumn(
                name: "ExpireDateMonth",
                table: "BankCards",
                newName: "ExpiredDateMonth");
        }
    }
}
