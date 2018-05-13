using MusicLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryBLL.Models
{
    public class BaseModel : IDataModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ModifyDate { get; set; } = DateTime.Now;
    }
}