using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WifiAPIExam.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShipIds",
                columns: table => new
                {
                    ShipId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipIds", x => x.ShipId);
                });

            migrationBuilder.CreateTable(
                name: "WifiDatabase",
                columns: table => new
                {
                    VoucherId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShipId = table.Column<int>(type: "integer", nullable: false),
                    SellTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActivationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Billing = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    DataSentKB = table.Column<int>(type: "integer", nullable: false),
                    DataReceivedKB = table.Column<int>(type: "integer", nullable: false),
                    CreditCardCountry = table.Column<string>(type: "text", nullable: false),
                    Devices = table.Column<int>(type: "integer", nullable: false),
                    VolumeQuotaMB = table.Column<int>(type: "integer", nullable: false),
                    TimeQuotaMinutes = table.Column<int>(type: "integer", nullable: false),
                    Fees = table.Column<decimal>(type: "numeric", nullable: false),
                    PriceNok = table.Column<decimal>(type: "numeric", nullable: false),
                    RefundNok = table.Column<int>(type: "integer", nullable: false),
                    RefundTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VatCountry = table.Column<string>(type: "text", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DepartureCc = table.Column<string>(type: "text", nullable: false),
                    DestinationCc = table.Column<string>(type: "text", nullable: false),
                    ActivationCc = table.Column<string>(type: "text", nullable: false),
                    UniqueToday = table.Column<int>(type: "integer", nullable: false),
                    UniqueThisMonth = table.Column<int>(type: "integer", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WifiDatabase", x => x.VoucherId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WifiDatabase_ShipId_SellTime",
                table: "WifiDatabase",
                columns: new[] { "ShipId", "SellTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipIds");

            migrationBuilder.DropTable(
                name: "WifiDatabase");
        }
    }
}
