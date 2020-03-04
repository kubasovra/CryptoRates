using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoRates.Migrations
{
    public partial class PairModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNotifyOnAbsoluteChange",
                table: "Pairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotifyOnPercentChange",
                table: "Pairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "TargetPriceAbsoluteChange",
                table: "Pairs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TargetPricePercentChange",
                table: "Pairs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
