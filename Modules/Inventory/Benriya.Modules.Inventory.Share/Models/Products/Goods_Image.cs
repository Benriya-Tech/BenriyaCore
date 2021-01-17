using Benriya.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.Inventory.Share.Models.Products
{
    [Table("inventory_goods_images")]
    public class Goods_Image : Common_Model_Min
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Goods_Image_id", IsUnique = true)]
        public Guid id { get; set; }
        [StringLength(128)]
        [IndexColumn("IX_Goods_Image_img_name", IsUnique = false)]
        public string image_name { get; set; }
        [ForeignKey("Product")]
        [IndexColumn("IX_Goods_Image_Goods_id", IsUnique = false)]
        public Guid product_id { get; set; }
        [ForeignKey("Product")]
        [IndexColumn("IX_Goods_Image_Color_id", IsUnique = false)]
        public Guid color_id { get; set; }
        [IndexColumn("IX_Image_quantity", IsUnique = false)]
        public int quantity { get; set; }
        public virtual Goods Goods { get; set; }
        public virtual Goods_Color Color { get; set; }
    }
}
