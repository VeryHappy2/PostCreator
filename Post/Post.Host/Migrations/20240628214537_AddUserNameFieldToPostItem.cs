using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Post.Host.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameFieldToPostItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Catagory",
                table: "PostCategoryEntity",
                newName: "Category");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "PostItem",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "PostItem",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "PostItem");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "PostCategoryEntity",
                newName: "Catagory");

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "PostItem",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
