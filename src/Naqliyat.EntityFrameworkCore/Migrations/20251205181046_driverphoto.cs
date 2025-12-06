using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naqliyat.Migrations
{
    /// <inheritdoc />
    public partial class driverphoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Attachments_PhotoId",
                table: "Drivers");

            migrationBuilder.AlterColumn<Guid>(
                name: "PhotoId",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Attachments_PhotoId",
                table: "Drivers",
                column: "PhotoId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Attachments_PhotoId",
                table: "Drivers");

            migrationBuilder.AlterColumn<Guid>(
                name: "PhotoId",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Attachments_PhotoId",
                table: "Drivers",
                column: "PhotoId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
