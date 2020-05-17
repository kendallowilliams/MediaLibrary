using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryDAL.Enums;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Transaction : IDataModel
    {
        public Transaction() : base()
        {
            Status = (int)TransactionStatus.NotStarted;
            Type = (int)TransactionTypes.None;
        }

        public Transaction(TransactionTypes transactionType) : base()
        {
            Status = (int)TransactionStatus.NotStarted;
            Type = (int)transactionType;
        }
    }
}
