using Benriya.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.Inventory.Share.Models.Warehouses
{
    [Table("inventory_warehouse_area")]
    public class Warehouse_Area : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_IVW_Area_id", IsUnique = true)]
        public Guid id { get; set; }
        [StringLength(255)]
        [IndexColumn("IX_IVW_Area_name", IsUnique = false)]
        public string name { get; set; }
        [StringLength(500)]
        public string description { get; set; }
        public bool is_active { get; set; } = true;
        [ForeignKey("Warehouse")]
        [IndexColumn("IX_IVW_Area_Warehouse_id", IsUnique = false)]
        public int warehouse_id { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}