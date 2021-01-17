using Benriya.Modules.Inventory.Share.Models.Warehouses;
using Benriya.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
namespace Benriya.Modules.Inventory.Share.Models.Products
{
    [Table("inventory_goods")]
    public class Goods : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Goods_id", IsUnique = true)]
        public Guid id { get; set; }
        [StringLength(255)]
        [IndexColumn("IX_Goods_name", IsUnique = false)]
        public string name { get; set; }
        [IndexColumn("IX_Goods_code", IsUnique = false)]
        [StringLength(100)]
        public string code { get; set; }
        [StringLength(500)]
        public string description { get; set; }
        [ForeignKey("Category")]
        [IndexColumn("IX_Goods_Goods_Category_id", IsUnique = false)]
        public int category_id { get; set; }
        [ForeignKey("Unit")]
        [IndexColumn("IX_Goods_Goods_Unit_id", IsUnique = false)]
        public int unit_id { get; set; }
        [IndexColumn("IX_Goods_isActive", IsUnique = false)]
        public bool is_active { get; set; } = true;
        [IndexColumn("IX_Goods_isType_colorr", IsUnique = false)]
        public bool is_type_color { get; set; } = true;
        [IndexColumn("IX_Goods_quantity", IsUnique = false)]
        public int quantity { get; set; }
        public int quantity_origin { get; set; }
        public bool is_color { get; set; }
        public bool is_weight { get; set; }
        [Required]
        public virtual Goods_Category Category { get; set; }
        [Required]
        public virtual Goods_Unit Unit { get; set; }
        public virtual ICollection<Goods_Color> Colors { get; set; }
        public virtual ICollection<Goods_Image> Images { get; set; }
        public virtual ICollection<Goods_Tags> Tags { get; set; }
        public virtual ICollection<Warehouse_Store> Warehouse_Store { get; set; }
    }
}
