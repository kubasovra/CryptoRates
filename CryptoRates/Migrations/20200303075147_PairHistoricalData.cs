using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoRates.Migrations
{
    public partial class PairHistoricalData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_Currencies_FirstCurrencyCurrencyId",
                table: "Pairs");

            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_Currencies_SecondCurrencyCurrencyId",
                table: "Pairs");

            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_AspNetUsers_UserId",
                table: "Pairs");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pairs",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SecondCurrencyCurrencyId",
                table: "Pairs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FirstCurrencyCurrencyId",
                table: "Pairs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HistoricalData",
                table: "Pairs",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_Currencies_FirstCurrencyCurrencyId",
                table: "Pairs",
                column: "FirstCurrencyCurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_Currencies_SecondCurrencyCurrencyId",
                table: "Pairs",
                column: "SecondCurrencyCurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_AspNetUsers_UserId",
                table: "Pairs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_Currencies_FirstCurrencyCurrencyId",
                table: "Pairs");

            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_Currencies_SecondCurrencyCurrencyId",
                table: "Pairs");

            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_AspNetUsers_UserId",
                table: "Pairs");

            migrationBuilder.DropColumn(
                name: "HistoricalData",
                table: "Pairs");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pairs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "SecondCurrencyCurrencyId",
                table: "Pairs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FirstCurrencyCurrencyId",
                table: "Pairs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_Currencies_FirstCurrencyCurrencyId",
                table: "Pairs",
                column: "FirstCurrencyCurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_Currencies_SecondCurrencyCurrencyId",
                table: "Pairs",
                column: "SecondCurrencyCurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_AspNetUsers_UserId",
                table: "Pairs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
