using MediaLibraryBLL.Services.Interfaces;
using MediaLibraryDAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryConsole
{
    class Program
    {
        private CompositionContainer _container;

        static async Task Main(string[] args)
        {
            Program program = new Program();

            program.ConfigureMEF();
            await program.Run();
        }

        public void ConfigureMEF()
        {
            var catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IProcessorService).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IDataService).Assembly));
            _container = new CompositionContainer(catalog);
            _container.ComposeParts(this);
        }

        public async Task Run()
        {
            IProcessorService processorService = _container.GetExportedValue<IProcessorService>();

            await Task.WhenAll(processorService.RefreshMusic(), processorService.RefreshPodcasts(), processorService.PerformCleanup());
        }
    }
}
