using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MediaLibraryDAL.Models
{
    public class BaseModel : IDataModel
    {
        [Key]
        public int Id { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Column("modify_date")]
        public DateTime ModifyDate { get; set; } = DateTime.Now;
    }
}