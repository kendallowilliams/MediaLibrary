using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Enums
{
    public static class TransactionEnums
    {
        public enum TransactionTypes
        {
            GetTracks,
            GetAlbums,
            GetArtists,
            Read,
            None = -1
        }

        public enum TransactionStatus
        {
            NotStarted,
            Started,
            InProgres,
            Errored,
            Cancelled,
            Done
        }
    }
}
