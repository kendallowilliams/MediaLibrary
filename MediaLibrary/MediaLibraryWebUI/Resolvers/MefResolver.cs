using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace MediaLibraryWebUI.Resolvers
{
    public class MefResolver : IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private readonly CompositionContainer container;

        public MefResolver(CompositionContainer container)
        {
            this.container = container;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            var name = AttributedModelServices.GetContractName(serviceType);
            var export = container.GetExportedValueOrDefault<object>(name);
            return export;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            var exports = container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
            return exports;
        }
    }
}