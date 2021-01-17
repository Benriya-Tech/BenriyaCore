using Benriya.Share.Models;
using Benriya.Share.Models.FileStore;
using Benriya.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.Inventory.Share.Models.Warehouses
{
    [Table("inventory_warehouse")]
    public class Warehouse : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Inventory_Warehouse_id", IsUnique = true)]
        public int id { get; set; }
        [StringLength(255)]
        [Required(ErrorMessage = "Please enter name")]
        [IndexColumn("IX_Inventory_Warehouse_name", IsUnique = true)]
        public string name { get; set; }
        [StringLength(500)]
        public string description { get; set; }
        [StringLength(500)]
        public string remark { get; set; }
        public bool is_active { get; set; } = true;
        public bool is_main { get; set; }
        public virtual ICollection<Warehouse_Area> Areas { get; set; }
        public virtual ICollection<Warehouse_Store> Stores { get; set; }
        [NotMapped]
        public virtual List<FileStore_Images> Images { get; set; }
    }
}