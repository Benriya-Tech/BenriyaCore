using Benriya.Modules.Inventory.Share.Models.Products;
using Benriya.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.eCommerce.Share.Models.Orders
{
    [Table("ecommerce_order")]
    public class Order : Common_Model_Min
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Order_id", IsUnique = true)]
        public Guid id { get; set; }
        [StringLength(16)]
        public string code { get; set; }
        public bool is_temp { get; set; }
        public decimal total { get; set; }
        public bool is_active { get; set; }
        public bool is_confirm { get; set; }
        public bool is_completed { get; set; }
        [StringLength(32)]
        public string status { get; set; }
        public Guid session_id { get; set; }
        public virtual ICollection<Order_Detail> Order_Details { get; set; }
    }
}
