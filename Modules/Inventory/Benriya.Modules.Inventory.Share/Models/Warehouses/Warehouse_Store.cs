using Benriya.Modules.Inventory.Share.Models.Products;
using Benriya.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.Inventory.Share.Models.Warehouses
{
    [Table("inventory_warehouse_store")]
    public class Warehouse_Store : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_IVW_Store_id", IsUnique = true)]
        public Guid id { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; } = true;
        [ForeignKey("Goods")]
        [IndexColumn("IX_IVW_Store_Goods_id", IsUnique = false)]
        public Guid goods_id { get; set; }
        public int quantity { get; set; }
        [ForeignKey("Warehouse")]
        [IndexColumn("IX_IVW_Store_Warehouse_id", IsUnique = false)]
        public int warehouse_id { get; set; }
        [ForeignKey("Area")]
        [IndexColumn("IX_IVW_Store_Area_id", IsUnique = false)]
        public Guid area_id { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Warehouse_Area Area { get; set; }
        public virtual Goods Goods { get; set; }
    }
}