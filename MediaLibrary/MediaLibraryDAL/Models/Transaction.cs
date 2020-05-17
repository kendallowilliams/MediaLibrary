using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums;

namespace MediaLibraryDAL.Models
{
    public class Transaction: BaseModel
    {
        public Transaction()
        {
            Status = TransactionStatus.NotStarted;
            Type = TransactionTypes.None;
        }

        public Transaction(TransactionTypes transactionType)
        {
            Status = TransactionStatus.NotStarted;
            Type = transactionType;
        }

        public TransactionTypes Type { get; set; }

        public string Message { get; set; }

        public TransactionStatus Status { get; set; }

        public string StatusMessage { get; set; }

        public string ErrorMessage { get; set; }
    }
}
