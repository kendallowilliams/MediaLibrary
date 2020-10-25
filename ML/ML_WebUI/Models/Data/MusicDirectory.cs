using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IO_Path = System.IO.Path;

namespace MediaLibraryWebUI.Models.Data
{
    public class MusicDirectory
    {
        public MusicDirectory(string path)
        {
            Path = path;
            SubDirectories = Enumerable.Empty<MusicDirectory>();
        }

        public MusicDirectory(string path, IEnumerable<string> subDirectoryPaths) : this(path)
        {
            SubDirectories = subDirectoryPaths.OrderBy(item => item)
                                              .Select(item => new MusicDirectory(item))
                                              .ToList();
        }

        public MusicDirectory(string path, IEnumerable<string> subDirectoryPaths, IEnumerable<TrackPath> includedTrackPaths) : this(path)
        {
            SubDirectories = subDirectoryPaths.OrderBy(item => item)
                                              .Select(item => new MusicDirectory(item)
                                              {
                                                  Id = includedTrackPaths.FirstOrDefault(_path => _path.Location.Equals(item, StringComparison.OrdinalIgnoreCase))?.Id
                                              })
                                              .ToList();
        }

        public string Path { get; set; }

        public int? Id { get; set; }

        public IEnumerable<MusicDirectory> SubDirectories { get; set; }

        public int FileCount { get; set; }
        public bool IsLoading { get; set; }
    }
}