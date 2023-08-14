using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Divisima.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Name", "Surname", "UserName" },
                values: new object[] { "Kaan", "Sar", "kaan" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admin",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Name", "Surname", "UserName" },
                values: new object[] { "Bekir", "Oturakçı", "bekir" });
        }
    }
}
