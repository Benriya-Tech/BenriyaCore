using Benriya.Modules.eCommerce.Share.Models.Orders;
using Benriya.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.eCommerce.Share.Models.Promotion
{
    [Table("ecommerce_promotion")]
    public class Pomotion_Master : Common_Model_Min
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Pomotion_id", IsUnique = true)]
        public Guid id { get; set; }
        [IndexColumn("IX_Pomotion_id", IsUnique = false)]
        [ForeignKey("Goods")]
        public Guid goods_id { get; set; }
        public decimal price { get; set; }
        public decimal original_price { get; set; }
        public Guid promo_id { get; set; }
        public virtual ICollection<Order> Orders { get; set; }


    }
}
