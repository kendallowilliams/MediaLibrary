using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface IProcessorService
    {
        Task RefreshPodcasts();
        Task RefreshMusic();
        Task PerformCleanup();
    }
}
