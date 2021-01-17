using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Benriya.Data.Migrator.Migrations
{
    public partial class add_color_store : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Goods_Color_name",
                table: "inventory_goods_color");

            migrationBuilder.DropColumn(
                name: "color",
                table: "inventory_goods_color");

            migrationBuilder.DropColumn(
                name: "name",
                table: "inventory_goods_color");

            migrationBuilder.RenameIndex(
                name: "IX_cms_category_tags_tag_id",
                table: "cms_category_tags",
                newName: "IX_CMS_Tags_tag_id");

            migrationBuilder.RenameIndex(
                name: "IX_cms_category_tags_category_id",
                table: "cms_category_tags",
                newName: "IX_CMS_Category_Tags_cate_id");

            migrationBuilder.AddColumn<int>(
                name: "color_id",
                table: "inventory_goods_color",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_color",
                table: "inventory_goods",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_weight",
                table: "inventory_goods",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "color_store",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    hex_code = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_color_store", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "weight_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    short_name = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weight_type", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Colors_Store_id",
                table: "inventory_goods_color",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "IX_Color_Store_hexcode",
                table: "color_store",
                column: "hex_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Color_Store_id",
                table: "color_store",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Color_Store_name",
                table: "color_store",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weight_Type_id",
                table: "weight_type",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weight_Type_name",
                table: "weight_type",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weight_Type_short_name",
                table: "weight_type",
                column: "short_name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_goods_color_color_store_color_id",
                table: "inventory_goods_color",
                column: "color_id",
                principalTable: "color_store",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_goods_color_color_store_color_id",
                table: "inventory_goods_color");

            migrationBuilder.DropTable(
                name: "color_store");

            migrationBuilder.DropTable(
                name: "weight_type");

            migrationBuilder.DropIndex(
                name: "IX_Goods_Colors_Store_id",
                table: "inventory_goods_color");

            migrationBuilder.DropColumn(
                name: "color_id",
                table: "inventory_goods_color");

            migrationBuilder.DropColumn(
                name: "is_color",
                table: "inventory_goods");

            migrationBuilder.DropColumn(
                name: "is_weight",
                table: "inventory_goods");

            migrationBuilder.RenameIndex(
                name: "IX_CMS_Tags_tag_id",
                table: "cms_category_tags",
                newName: "IX_cms_category_tags_tag_id");

            migrationBuilder.RenameIndex(
                name: "IX_CMS_Category_Tags_cate_id",
                table: "cms_category_tags",
                newName: "IX_cms_category_tags_category_id");

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "inventory_goods_color",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "inventory_goods_color",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Color_name",
                table: "inventory_goods_color",
                column: "name");
        }
    }
}
