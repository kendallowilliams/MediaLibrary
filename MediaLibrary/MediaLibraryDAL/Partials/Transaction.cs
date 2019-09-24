using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums.TransactionEnums;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Transaction : IDataModel
    {
        public Transaction()
        {
            Status = (int)TransactionStatus.NotStarted;
            Type = (int)TransactionTypes.None;
        }

        public Transaction(TransactionTypes transactionType)
        {
            Status = (int)TransactionStatus.NotStarted;
            Type = (int)transactionType;
        }
    }
}
