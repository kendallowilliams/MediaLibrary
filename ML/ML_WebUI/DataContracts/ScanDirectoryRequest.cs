using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.DataContracts
{
    public class ScanDirectoryRequest
    {
        public string Path { get; set; }
        public bool Recursive { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(Path) && Directory.Exists(Path);
    }
}