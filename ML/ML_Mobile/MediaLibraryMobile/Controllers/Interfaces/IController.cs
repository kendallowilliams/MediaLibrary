using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryMobile.Controllers.Interfaces
{
    public interface IController
    {
        void Startup();
        void Shutdown();
    }
}
