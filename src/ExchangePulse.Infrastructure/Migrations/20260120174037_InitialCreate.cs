using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangePulse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DailyVariation = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    LogReturn = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    MovingAverage7d = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    MovingAverage30d = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Volatility30d = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SharpeDaily = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SharpeAnnual = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Drawdown = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Beta = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    VaREmpirical95 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    VaRCornishFisher95 = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Inflation = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeMetrics_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuyPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Volume = table.Column<long>(type: "bigint", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MacroEvents = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code",
                table: "Currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeMetrics_CurrencyId",
                table: "ExchangeMetrics",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyId",
                table: "ExchangeRates",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeMetrics");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
