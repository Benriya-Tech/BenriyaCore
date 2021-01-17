using Benriya.Share.Models;
using Benriya.Share.Models.Colors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.Inventory.Share.Models.Products
{
    [Table("inventory_goods_color")]
    public class Goods_Color : Common_Model_Min
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Goods_Color_id", IsUnique = true)]
        public Guid id { get; set; }
        [ForeignKey("Color")]
        [IndexColumn("IX_Goods_Colors_Store_id", IsUnique = false)]
        public int color_id { get; set; }
        [IndexColumn("IX_Goods_Color_quantity", IsUnique = false)]
        public string quantity { get; set; }
        [ForeignKey("Product")]
        [IndexColumn("IX_Goods_Color_Goods_id", IsUnique = false)]
        public Guid product_id { get; set; }
        public virtual Goods Product { get; set; }
        public virtual Color_Store Color { get; set; }
        public virtual ICollection<Goods_Image> Images { get; set; }
    }
}
