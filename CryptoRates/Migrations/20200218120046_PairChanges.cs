using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoRates.Migrations
{
    public partial class PairChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotifyOnAbsolute",
                table: "Pairs");

            migrationBuilder.DropColumn(
                name: "IsNotifyOnPercent",
                table: "Pairs");

            migrationBuilder.DropColumn(
                name: "TargetPriceChangeAbsolute",
                table: "Pairs");

            migrationBuilder.DropColumn(
                name: "TargetPriceChangePercent",
                table: "Pairs");

            migrationBuilder.AddColumn<bool>(
                name: "IsNotifyOnAbsoluteChange",
                table: "Pairs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotifyOnPercentChange",
                table: "Pairs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "TargetPriceAbsoluteChange",
                table: "Pairs",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TargetPricePercentChange",
                table: "Pairs",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotifyOnAbsoluteChange",
                table: "Pairs");

            migrationBuilder.DropColumn(
                name: "IsNotifyOnPercentChange",
                table: "Pairs");

            migrationBuilder.DropColumn(
                name: "TargetPriceAbsoluteChange",
                table: "Pairs");

            migrationBuilder.DropColumn(
                name: "TargetPricePercentChange",
                table: "Pairs");

            migrationBuilder.AddColumn<bool>(
                name: "IsNotifyOnAbsolute",
                table: "Pairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotifyOnPercent",
                table: "Pairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "TargetPriceChangeAbsolute",
                table: "Pairs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TargetPriceChangePercent",
                table: "Pairs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
