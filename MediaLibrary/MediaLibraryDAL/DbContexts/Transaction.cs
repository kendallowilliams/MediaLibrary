using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }
        public int Type { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
