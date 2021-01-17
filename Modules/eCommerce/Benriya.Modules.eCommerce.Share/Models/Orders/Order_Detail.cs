using Benriya.Modules.Inventory.Share.Models.Products;
using Benriya.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.eCommerce.Share.Models.Orders
{
    [Table("ecommerce_order_detail")]
    public class Order_Detail : Common_Model_Min
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Order_Detail_id", IsUnique = true)]
        public Guid id { get; set; }
        [IndexColumn("IX_Order_Detail_Goods_id", IsUnique = false)]
        [ForeignKey("Goods")]
        public Guid goods_id { get; set; }        
        public decimal price { get; set; }
        public decimal original_price { get; set; }
        public Guid promo_id { get; set; }
        public bool is_active { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Goods Goods { get; set; }

    }
}
