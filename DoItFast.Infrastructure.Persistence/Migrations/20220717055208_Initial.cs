using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoItFast.Infrastructure.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DoItFast");

            migrationBuilder.CreateTable(
                name: "Gateway",
                schema: "DoItFast",
                columns: table => new
                {
                    SerialNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ReadableName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SerialNumber", x => x.SerialNumber);
                });

            migrationBuilder.CreateTable(
                name: "PeripheralDeviceStatus",
                schema: "DoItFast",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeripheralDeviceStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeripheralDevice",
                schema: "DoItFast",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Vendor = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PeripheralDeviceStatusId = table.Column<int>(type: "int", nullable: false),
                    GatewayId = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeripheralDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeripheralDevice_Gateway_GatewayId",
                        column: x => x.GatewayId,
                        principalSchema: "DoItFast",
                        principalTable: "Gateway",
                        principalColumn: "SerialNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeripheralDevice_PeripheralDeviceStatus_PeripheralDeviceStatusId",
                        column: x => x.PeripheralDeviceStatusId,
                        principalSchema: "DoItFast",
                        principalTable: "PeripheralDeviceStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeripheralDevice_GatewayId",
                schema: "DoItFast",
                table: "PeripheralDevice",
                column: "GatewayId");

            migrationBuilder.CreateIndex(
                name: "IX_PeripheralDevice_PeripheralDeviceStatusId",
                schema: "DoItFast",
                table: "PeripheralDevice",
                column: "PeripheralDeviceStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeripheralDevice",
                schema: "DoItFast");

            migrationBuilder.DropTable(
                name: "Gateway",
                schema: "DoItFast");

            migrationBuilder.DropTable(
                name: "PeripheralDeviceStatus",
                schema: "DoItFast");
        }
    }
}
