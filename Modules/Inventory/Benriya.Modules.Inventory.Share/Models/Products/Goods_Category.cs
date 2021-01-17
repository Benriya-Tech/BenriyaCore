using Benriya.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.Inventory.Share.Models.Products
{
    [Table("inventory_goods_category")]
    public class Goods_Category:Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Goods_Category_id", IsUnique = true)]
        public int id { get; set; }
        [StringLength(255)]
        [IndexColumn("IX_Goods_Category_name", IsUnique = true)]
        public string name { get; set; }
        [StringLength(500)]
        public string description { get; set; }
        [IndexColumn("IX_Goods_Category_isActive", IsUnique = false)]
        public bool is_active { get; set; } = true;
        public virtual ICollection<Goods> Products { get; set; }
    }
}
