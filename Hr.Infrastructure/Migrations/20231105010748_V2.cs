using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicHolidays_GeneralSettings_GeneralSettingsId",
                table: "PublicHolidays");

            migrationBuilder.DropIndex(
                name: "IX_PublicHolidays_GeneralSettingsId",
                table: "PublicHolidays");

            migrationBuilder.DropColumn(
                name: "GeneralSettingsId",
                table: "PublicHolidays");

            migrationBuilder.DropColumn(
                name: "Absent",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "BonusHour",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "DiscountHour",
                table: "Attendances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneralSettingsId",
                table: "PublicHolidays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Absent",
                table: "Attendances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "BonusHour",
                table: "Attendances",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiscountHour",
                table: "Attendances",
                type: "float",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PublicHolidays_GeneralSettingsId",
                table: "PublicHolidays",
                column: "GeneralSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicHolidays_GeneralSettings_GeneralSettingsId",
                table: "PublicHolidays",
                column: "GeneralSettingsId",
                principalTable: "GeneralSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
