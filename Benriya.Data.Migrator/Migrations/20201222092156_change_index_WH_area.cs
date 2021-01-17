using Microsoft.EntityFrameworkCore.Migrations;

namespace Benriya.Data.Migrator.Migrations
{
    public partial class change_index_WH_area : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IVW_Area_name",
                table: "inventory_warehouse_area");

            migrationBuilder.CreateIndex(
                name: "IX_IVW_Area_name",
                table: "inventory_warehouse_area",
                column: "name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IVW_Area_name",
                table: "inventory_warehouse_area");

            migrationBuilder.CreateIndex(
                name: "IX_IVW_Area_name",
                table: "inventory_warehouse_area",
                column: "name",
                unique: true);
        }
    }
}
