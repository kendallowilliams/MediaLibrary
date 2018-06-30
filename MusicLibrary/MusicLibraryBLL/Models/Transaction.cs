using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicLibraryBLL.Enums.TransactionEnums;

namespace MusicLibraryBLL.Models
{
    public class Transaction: BaseModel
    {
        public Transaction()
        {
            Status = TransactionStatus.NotStarted;
            Type = TransactionTypes.None;
        }

        public TransactionTypes Type { get; set; }

        public TransactionStatus Status { get; set; }

        public string StatusMessage { get; set; }

        public string ErrorMessage { get; set; }
    }
}
