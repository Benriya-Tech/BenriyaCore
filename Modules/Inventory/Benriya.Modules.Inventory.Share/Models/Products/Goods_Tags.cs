using Benriya.Share.Models;
using Benriya.Share.Models.CoreTags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.Inventory.Share.Models.Products
{
    [Table("inventory_goods_tags")]
    public class Goods_Tags : Common_Model_Min
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Goods_Tags_id", IsUnique = true)]
        public Guid id { get; set; }
        [ForeignKey("Product")]
        [IndexColumn("IX_Goods_Tags_Goods_id", IsUnique = false)]
        public Guid product_id { get; set; }
        [ForeignKey("Tags")]
        [IndexColumn("IX_Goods_Tags__id",IsUnique = false)]
        public Guid tag_id { get; set; }
        public virtual Tags Tags { get; set; }
        public virtual Goods Product { get; set; }
    }
}
