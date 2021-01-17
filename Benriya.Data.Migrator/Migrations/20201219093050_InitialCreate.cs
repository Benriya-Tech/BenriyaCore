using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Benriya.Data.Migrator.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cms_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cms_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ecommerce_promotion",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    goods_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    original_price = table.Column<decimal>(type: "numeric", nullable: false),
                    promo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ecommerce_promotion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "filestore_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_extension = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    file_type = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filestore_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inventory_goods_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_goods_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inventory_goods_unit",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_goods_unit", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inventory_warehouse",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    remark = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_warehouse", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "menu_system",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_redirect = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    parent_menu_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_system", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permission_access",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_access", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
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
                    table.PrimaryKey("PK_tags_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    role_level = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cms_contents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    path = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    body = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cms_contents", x => x.id);
                    table.ForeignKey(
                        name: "FK_cms_contents_cms_category_category_id",
                        column: x => x.category_id,
                        principalTable: "cms_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ecommerce_order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    is_temp = table.Column<bool>(type: "boolean", nullable: false),
                    total = table.Column<decimal>(type: "numeric", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_confirm = table.Column<bool>(type: "boolean", nullable: false),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Pomotion_Masterid = table.Column<Guid>(type: "uuid", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ecommerce_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_ecommerce_order_ecommerce_promotion_Pomotion_Masterid",
                        column: x => x.Pomotion_Masterid,
                        principalTable: "ecommerce_promotion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "filestore_documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    module = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false),
                    url = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    file_type_id = table.Column<int>(type: "integer", nullable: false),
                    model_id = table.Column<int>(type: "integer", nullable: false),
                    model_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    check_sum = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filestore_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_filestore_documents_filestore_types_file_type_id",
                        column: x => x.file_type_id,
                        principalTable: "filestore_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "filestore_files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    module = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false),
                    url = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    file_type_id = table.Column<int>(type: "integer", nullable: false),
                    model_id = table.Column<int>(type: "integer", nullable: false),
                    model_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    check_sum = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filestore_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_filestore_files_filestore_types_file_type_id",
                        column: x => x.file_type_id,
                        principalTable: "filestore_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "filestore_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    module = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false),
                    url = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    file_type_id = table.Column<int>(type: "integer", nullable: false),
                    model_id = table.Column<int>(type: "integer", nullable: false),
                    model_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    check_sum = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filestore_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_filestore_images_filestore_types_file_type_id",
                        column: x => x.file_type_id,
                        principalTable: "filestore_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_goods",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    unit_id = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_type_color = table.Column<bool>(type: "boolean", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    quantity_origin = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_goods", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_goods_inventory_goods_category_category_id",
                        column: x => x.category_id,
                        principalTable: "inventory_goods_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_goods_inventory_goods_unit_unit_id",
                        column: x => x.unit_id,
                        principalTable: "inventory_goods_unit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_warehouse_area",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    warehouse_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_warehouse_area", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_warehouse_area_inventory_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalTable: "inventory_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "menu_quick_access",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    system_menu_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_quick_access", x => x.id);
                    table.ForeignKey(
                        name: "FK_menu_quick_access_menu_system_system_menu_id",
                        column: x => x.system_menu_id,
                        principalTable: "menu_system",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tags_data",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    group_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_tags_data_tags_group_group_id",
                        column: x => x.group_id,
                        principalTable: "tags_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_policy_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    module_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    module_code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    permission_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_policy_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_policy_roles_permission_access_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permission_access",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_policy_roles_user_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "user_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    alias_name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    firstname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    lastname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_user_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "user_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cms_comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cms_comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_cms_comments_cms_contents_content_id",
                        column: x => x.content_id,
                        principalTable: "cms_contents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cms_content_likes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cms_content_likes", x => x.id);
                    table.ForeignKey(
                        name: "FK_cms_content_likes_cms_contents_content_id",
                        column: x => x.content_id,
                        principalTable: "cms_contents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ecommerce_order_detail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    goods_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    original_price = table.Column<decimal>(type: "numeric", nullable: false),
                    promo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ecommerce_order_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_ecommerce_order_detail_inventory_goods_goods_id",
                        column: x => x.goods_id,
                        principalTable: "inventory_goods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_goods_color",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    quantity = table.Column<string>(type: "text", nullable: true),
                    color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_goods_color", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_goods_color_inventory_goods_product_id",
                        column: x => x.product_id,
                        principalTable: "inventory_goods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_warehouse_store",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    goods_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    warehouse_id = table.Column<int>(type: "integer", nullable: false),
                    area_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_warehouse_store", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_warehouse_store_inventory_goods_goods_id",
                        column: x => x.goods_id,
                        principalTable: "inventory_goods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_warehouse_store_inventory_warehouse_area_area_id",
                        column: x => x.area_id,
                        principalTable: "inventory_warehouse_area",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_warehouse_store_inventory_warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalTable: "inventory_warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cms_category_tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cms_category_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_cms_category_tags_cms_category_category_id",
                        column: x => x.category_id,
                        principalTable: "cms_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cms_category_tags_tags_data_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cms_content_tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cms_content_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_cms_content_tags_cms_contents_content_id",
                        column: x => x.content_id,
                        principalTable: "cms_contents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cms_content_tags_tags_data_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_goods_tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_goods_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_goods_tags_inventory_goods_product_id",
                        column: x => x.product_id,
                        principalTable: "inventory_goods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_inventory_goods_tags_tags_data_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_credential",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    password_hash = table.Column<byte[]>(type: "bytea", nullable: true),
                    password_salt = table.Column<byte[]>(type: "bytea", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_credential", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_credential_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_direct_auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    expiry_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_direct_auth", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_direct_auth_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_login_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    logout_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    logout_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_loggedIn = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_login_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_login_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "text", nullable: true),
                    expiry = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cms_comment_likes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    comment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cms_comment_likes", x => x.id);
                    table.ForeignKey(
                        name: "FK_cms_comment_likes_cms_comments_comment_id",
                        column: x => x.comment_id,
                        principalTable: "cms_comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cms_comment_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    updated_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    alias_name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    firstname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    lastname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cms_comment_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_cms_comment_users_cms_comments_comment_id",
                        column: x => x.comment_id,
                        principalTable: "cms_comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderOrder_Detail",
                columns: table => new
                {
                    Order_Detailsid = table.Column<Guid>(type: "uuid", nullable: false),
                    Ordersid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOrder_Detail", x => new { x.Order_Detailsid, x.Ordersid });
                    table.ForeignKey(
                        name: "FK_OrderOrder_Detail_ecommerce_order_detail_Order_Detailsid",
                        column: x => x.Order_Detailsid,
                        principalTable: "ecommerce_order_detail",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderOrder_Detail_ecommerce_order_Ordersid",
                        column: x => x.Ordersid,
                        principalTable: "ecommerce_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_goods_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    Goodsid = table.Column<Guid>(type: "uuid", nullable: true),
                    Colorid = table.Column<Guid>(type: "uuid", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_goods_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_goods_images_inventory_goods_color_Colorid",
                        column: x => x.Colorid,
                        principalTable: "inventory_goods_color",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_goods_images_inventory_goods_Goodsid",
                        column: x => x.Goodsid,
                        principalTable: "inventory_goods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cms_category_id",
                table: "cms_category",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cms_category_name",
                table: "cms_category",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_cms_category_tags_category_id",
                table: "cms_category_tags",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_cms_category_tags_id",
                table: "cms_category_tags",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cms_category_tags_tag_id",
                table: "cms_category_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_cms_comment_likes_comment_id",
                table: "cms_comment_likes",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_cms_comment_likes_id",
                table: "cms_comment_likes",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cms_comment_users_alias_name",
                table: "cms_comment_users",
                column: "alias_name");

            migrationBuilder.CreateIndex(
                name: "IX_cms_comment_users_comment_id",
                table: "cms_comment_users",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_cms_comment_users_email",
                table: "cms_comment_users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_cms_comment_users_firstname",
                table: "cms_comment_users",
                column: "firstname");

            migrationBuilder.CreateIndex(
                name: "IX_cms_comment_users_id",
                table: "cms_comment_users",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cms_comment_users_lastname",
                table: "cms_comment_users",
                column: "lastname");

            migrationBuilder.CreateIndex(
                name: "IX_cms_comments_content_id",
                table: "cms_comments",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "IX_cms_comments_id",
                table: "cms_comments",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cms_content_likes_content_id",
                table: "cms_content_likes",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "IX_cms_content_likes_id",
                table: "cms_content_likes",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cms_content_tags_content_id",
                table: "cms_content_tags",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "IX_cms_content_tags_id",
                table: "cms_content_tags",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cms_content_tags_tag_id",
                table: "cms_content_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_cms_contents_category_id",
                table: "cms_contents",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_cms_contents_id",
                table: "cms_contents",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cms_contents_name",
                table: "cms_contents",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_cms_contents_path",
                table: "cms_contents",
                column: "path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ecommerce_order_Pomotion_Masterid",
                table: "ecommerce_order",
                column: "Pomotion_Masterid");

            migrationBuilder.CreateIndex(
                name: "IX_Order_id",
                table: "ecommerce_order",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Detail_Goods_id",
                table: "ecommerce_order_detail",
                column: "goods_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Detail_id",
                table: "ecommerce_order_detail",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pomotion_id",
                table: "ecommerce_promotion",
                columns: new[] { "id", "goods_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Docs_is_active",
                table: "filestore_documents",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Documents_Docs_model_id",
                table: "filestore_documents",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Documents_Docs_model_uuid",
                table: "filestore_documents",
                column: "model_uuid");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Documents_id",
                table: "filestore_documents",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Documents_module",
                table: "filestore_documents",
                column: "module");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Documents_name",
                table: "filestore_documents",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Documents_Type_id",
                table: "filestore_documents",
                column: "file_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Documents_Files_model_id",
                table: "filestore_files",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Documents_Files_model_uuid",
                table: "filestore_files",
                column: "model_uuid");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Files_id",
                table: "filestore_files",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Files_is_active",
                table: "filestore_files",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Files_module",
                table: "filestore_files",
                column: "module");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Files_name",
                table: "filestore_files",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Files_Type_id",
                table: "filestore_files",
                column: "file_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Documents_Images_model_id",
                table: "filestore_images",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Documents_Images_model_uuid",
                table: "filestore_images",
                columns: new[] { "model_uuid", "check_sum" });

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Images_id",
                table: "filestore_images",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Images_is_active",
                table: "filestore_images",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Images_module",
                table: "filestore_images",
                column: "module");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Images_name",
                table: "filestore_images",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Images_Type_id",
                table: "filestore_images",
                column: "file_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Type_id",
                table: "filestore_types",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Type_name",
                table: "filestore_types",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Type_name_extension",
                table: "filestore_types",
                column: "file_extension",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileStore_Type_x",
                table: "filestore_types",
                column: "file_type");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_code",
                table: "inventory_goods",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Goods_Category_id",
                table: "inventory_goods",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Goods_Unit_id",
                table: "inventory_goods",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_id",
                table: "inventory_goods",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_isActive",
                table: "inventory_goods",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_isType_colorr",
                table: "inventory_goods",
                column: "is_type_color");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_name",
                table: "inventory_goods",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_quantity",
                table: "inventory_goods",
                column: "quantity");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Category_id",
                table: "inventory_goods_category",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Category_isActive",
                table: "inventory_goods_category",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Category_name",
                table: "inventory_goods_category",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Color_Goods_id",
                table: "inventory_goods_color",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Color_id",
                table: "inventory_goods_color",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Color_name",
                table: "inventory_goods_color",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Color_quantity",
                table: "inventory_goods_color",
                column: "quantity");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Image_Color_id",
                table: "inventory_goods_images",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Image_Goods_id",
                table: "inventory_goods_images",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Image_id",
                table: "inventory_goods_images",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Image_img_name",
                table: "inventory_goods_images",
                column: "image_name");

            migrationBuilder.CreateIndex(
                name: "IX_Image_quantity",
                table: "inventory_goods_images",
                column: "quantity");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_goods_images_Colorid",
                table: "inventory_goods_images",
                column: "Colorid");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_goods_images_Goodsid",
                table: "inventory_goods_images",
                column: "Goodsid");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Tags__id",
                table: "inventory_goods_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Tags_Goods_id",
                table: "inventory_goods_tags",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Tags_id",
                table: "inventory_goods_tags",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Unit_id",
                table: "inventory_goods_unit",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Unit_isActive",
                table: "inventory_goods_unit",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Unit_name",
                table: "inventory_goods_unit",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Warehouse_id",
                table: "inventory_warehouse",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Warehouse_name",
                table: "inventory_warehouse",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IVW_Area_id",
                table: "inventory_warehouse_area",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IVW_Area_name",
                table: "inventory_warehouse_area",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IVW_Area_Warehouse_id",
                table: "inventory_warehouse_area",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_IVW_Store_Area_id",
                table: "inventory_warehouse_store",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_IVW_Store_Goods_id",
                table: "inventory_warehouse_store",
                column: "goods_id");

            migrationBuilder.CreateIndex(
                name: "IX_IVW_Store_id",
                table: "inventory_warehouse_store",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IVW_Store_Warehouse_id",
                table: "inventory_warehouse_store",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_quick_access_id",
                table: "menu_quick_access",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuickMenu_on_SystemMenu_id",
                table: "menu_quick_access",
                column: "system_menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Code",
                table: "menu_system",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menu_Name",
                table: "menu_system",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_ParentMenu_id",
                table: "menu_system",
                column: "parent_menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_system_id",
                table: "menu_system",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderOrder_Detail_Ordersid",
                table: "OrderOrder_Detail",
                column: "Ordersid");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Access_Code",
                table: "permission_access",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permission_access_id",
                table: "permission_access",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tags_data_group_id",
                table: "tags_data",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tags_data_id",
                table: "tags_data",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tags_data_name",
                table: "tags_data",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_tags_group_id",
                table: "tags_group",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tags_group_name",
                table: "tags_group",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_credential_id",
                table: "user_credential",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_credential_user_id",
                table: "user_credential",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_direct_auth_id",
                table: "user_direct_auth",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_direct_auth_key",
                table: "user_direct_auth",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_direct_auth_user_id",
                table: "user_direct_auth",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_login_logs_id",
                table: "user_login_logs",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_login_logs_user_id",
                table: "user_login_logs",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policy_Permission_id",
                table: "user_policy_roles",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_Policy_Roles_id",
                table: "user_policy_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_policy_roles_id",
                table: "user_policy_roles",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rolecode",
                table: "user_roles",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleLevel",
                table: "user_roles",
                column: "role_level");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_id",
                table: "user_roles",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_tokens_id",
                table: "user_tokens",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_tokens_user_id",
                table: "user_tokens",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_id",
                table: "users",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cms_category_tags");

            migrationBuilder.DropTable(
                name: "cms_comment_likes");

            migrationBuilder.DropTable(
                name: "cms_comment_users");

            migrationBuilder.DropTable(
                name: "cms_content_likes");

            migrationBuilder.DropTable(
                name: "cms_content_tags");

            migrationBuilder.DropTable(
                name: "filestore_documents");

            migrationBuilder.DropTable(
                name: "filestore_files");

            migrationBuilder.DropTable(
                name: "filestore_images");

            migrationBuilder.DropTable(
                name: "inventory_goods_images");

            migrationBuilder.DropTable(
                name: "inventory_goods_tags");

            migrationBuilder.DropTable(
                name: "inventory_warehouse_store");

            migrationBuilder.DropTable(
                name: "menu_quick_access");

            migrationBuilder.DropTable(
                name: "OrderOrder_Detail");

            migrationBuilder.DropTable(
                name: "user_credential");

            migrationBuilder.DropTable(
                name: "user_direct_auth");

            migrationBuilder.DropTable(
                name: "user_login_logs");

            migrationBuilder.DropTable(
                name: "user_policy_roles");

            migrationBuilder.DropTable(
                name: "user_tokens");

            migrationBuilder.DropTable(
                name: "cms_comments");

            migrationBuilder.DropTable(
                name: "filestore_types");

            migrationBuilder.DropTable(
                name: "inventory_goods_color");

            migrationBuilder.DropTable(
                name: "tags_data");

            migrationBuilder.DropTable(
                name: "inventory_warehouse_area");

            migrationBuilder.DropTable(
                name: "menu_system");

            migrationBuilder.DropTable(
                name: "ecommerce_order_detail");

            migrationBuilder.DropTable(
                name: "ecommerce_order");

            migrationBuilder.DropTable(
                name: "permission_access");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "cms_contents");

            migrationBuilder.DropTable(
                name: "tags_group");

            migrationBuilder.DropTable(
                name: "inventory_warehouse");

            migrationBuilder.DropTable(
                name: "inventory_goods");

            migrationBuilder.DropTable(
                name: "ecommerce_promotion");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "cms_category");

            migrationBuilder.DropTable(
                name: "inventory_goods_category");

            migrationBuilder.DropTable(
                name: "inventory_goods_unit");
        }
    }
}
