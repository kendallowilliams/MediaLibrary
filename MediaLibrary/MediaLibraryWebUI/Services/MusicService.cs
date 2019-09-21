using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Services
{
    [Export(typeof(IMusicService))]
    public class MusicService : IMusicService
    {
        [ImportingConstructor]
        public MusicService() { }

        public IEnumerable<IGrouping<string, Track>> GetSongGroups(IEnumerable<Track> songs, SongSort sort)
        {
            IEnumerable < IGrouping<string, Track>> groups = null;

            switch(sort)
            {
                case SongSort.AtoZ:
                    groups = GetSongsAtoZ(songs.OrderBy(song => song.Title));
                    break;
            }

            return groups;
        }

        private IEnumerable<IGrouping<string, Track>> GetSongsAtoZ(IEnumerable<Track> songs)
        {
            Func<string, string> getLabel = title =>
             {
                 char first = title.ToUpper().First();
                 string label = string.Empty;

                 if (Char.IsLetter(first)) { label = first.ToString(); }
                 else if (Char.IsDigit(first)) { label = first.ToString(); }
                 else label = "#";

                 return label;
             };

            return songs.GroupBy(track => getLabel(track.Title)).OrderBy(group => group.Key);
        }
    }
}