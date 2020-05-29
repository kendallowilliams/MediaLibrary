using System;
using System.Collections.Generic;
using System.Text;

namespace MediaLibraryMobile.Services.Interfaces
{
    public interface ILogService
    {
        void Info(string source, string message);
        void Warn(string source, string message);
        void Error(string source, string message);
    }
}
