﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace MadPay724.Data.Migrations
{
    public partial class photopublicid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Photos");

           
        }
    }
}
