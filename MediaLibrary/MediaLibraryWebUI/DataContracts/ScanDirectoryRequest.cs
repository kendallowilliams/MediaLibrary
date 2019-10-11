using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.DataContracts
{
    public class ScanDirectoryRequest
    {
        public string Directory { get; set; }
        public bool Recursive { get; set; }
        public bool Copy { get; set; }
    }
}