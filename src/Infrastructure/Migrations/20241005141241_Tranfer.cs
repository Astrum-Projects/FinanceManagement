using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Tranfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncome",
                table: "Transfers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 5, 14, 12, 40, 270, DateTimeKind.Utc).AddTicks(7693),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 10, 3, 14, 30, 4, 705, DateTimeKind.Utc).AddTicks(3887));

            migrationBuilder.AddColumn<bool>(
                name: "IsIncome",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncome",
                table: "Categories");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 3, 14, 30, 4, 705, DateTimeKind.Utc).AddTicks(3887),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 10, 5, 14, 12, 40, 270, DateTimeKind.Utc).AddTicks(7693));

            migrationBuilder.AddColumn<bool>(
                name: "IsIncome",
                table: "Transfers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
