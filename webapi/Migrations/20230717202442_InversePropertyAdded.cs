using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class InversePropertyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "role",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_role_CompanyId",
                table: "role",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_role_company_CompanyId",
                table: "role",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_company_CompanyId",
                table: "role");

            migrationBuilder.DropIndex(
                name: "IX_role_CompanyId",
                table: "role");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "role");
        }
    }
}
