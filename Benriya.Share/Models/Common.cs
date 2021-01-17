using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Benriya.Share.Models
{
    public class Common_Model : IEntity
    {
        public Common_Model()
        {
            created_date = DateTime.Now;
            updated_date = DateTime.Now;
        }
        private DateTime? created_date;
        [ReadOnly(true)]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่สร้าง")]
        public DateTime? created
        {
            get { return created_date ?? DateTime.Now; }
            set { this.created_date = value; }
        }
        private DateTime? updated_date;
        [ReadOnly(true)]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "แก้ไขล่าสุด")]
        public DateTime? updated
        {
            get { return updated_date ?? DateTime.Now; }
            set { this.updated_date = DateTime.Now; }
        }

        //[IndexColumn("IX_Created_by_User_idx", IsUnique = false)]
        //[ForeignKey("Users")]
        public Guid created_by { get; set; }
        //[IndexColumn("IX_Updated_by_User_idx",IsUnique = false)]
        //[ForeignKey("Users")]
        public Guid updated_by { get; set; }
        //public string created_by_name { get; set; }
        //public string updated_by_name { get; set; }
        [StringLength(45)]
        [Browsable(false)]
        public string created_ip { get; set; }
        [StringLength(45)]
        [Browsable(false)]
        public string updated_ip { get; set; }

    }

    public class Common_Model_Force : IEntity
    {
        public Common_Model_Force()
        {
            created_date = DateTime.Now;
            updated_date = DateTime.Now;
        }
        private DateTime? created_date;
        [ReadOnly(true)]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "วันที่สร้าง")]
        public DateTime? created
        {
            get { return created_date ?? DateTime.Now; }
            set { this.created_date = value; }
        }
        private DateTime? updated_date;
        [ReadOnly(true)]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "แก้ไขล่าสุด")]
        public DateTime? updated
        {
            get { return updated_date ?? DateTime.Now; }
            set { this.updated_date = DateTime.Now; }
        }

        //[IndexColumn("IX_FCreated_by_User_id", IsUnique = false)]
        //[ForeignKey("Users")]
        public Guid? created_by { get; set; } = Guid.Empty;
        //[IndexColumn("IX_FUpdated_by_User_id", IsUnique = false)]
        //[ForeignKey("Users")]
        public Guid? updated_by { get; set; } = Guid.Empty;
        [StringLength(45)]
        [Browsable(false)]
        public string created_ip { get; set; }
        [StringLength(45)]
        [Browsable(false)]
        public string updated_ip { get; set; }
    }
    public class Common_Model_Min : IEntity
    {
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }
        public Guid? created_by { get; set; }
        public Guid? updated_by { get; set; }
    }

    public class Common_Model_View
    {
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }
        public Guid? created_by { get; set; }
        public Guid? updated_by { get; set; }
        public string created_by_name { get; set; }
        public string updated_by_name { get; set; }
    }
}
