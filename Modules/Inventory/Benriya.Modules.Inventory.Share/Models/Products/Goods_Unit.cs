using Benriya.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.Inventory.Share.Models.Products
{
    [Table("inventory_goods_unit")]
    public class Goods_Unit : Common_Model_Min
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Goods_Unit_id", IsUnique = true)]
        public int id { get; set; }
        [StringLength(200)]
        [IndexColumn("IX_Goods_Unit_name", IsUnique = true)]
        public string name { get; set; }
        [IndexColumn("IX_Goods_Unit_isActive", IsUnique = false)]
        public bool is_active { get; set; } = true;
        public string description { get; set; }     
    }
}
