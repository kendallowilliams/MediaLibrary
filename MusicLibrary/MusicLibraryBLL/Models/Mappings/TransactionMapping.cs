using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperExtensions.Mapper;

namespace MusicLibraryBLL.Models.Mappings
{
    public class TransactionMapping : ClassMapper<Transaction>
    {
        public TransactionMapping()
        {
            Table("_transaction");

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ErrorMessage).Column("error_message");
            Map(x => x.StatusMessage).Column("status_message");
            Map(x => x.ModifyDate).Column("modify_date");
            Map(x => x.CreateDate).Column("create_date");

            AutoMap();
        }
    }
}