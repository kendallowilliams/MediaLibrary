using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace MediaLibraryWebUI.ControllerFactory
{
    public class MefControllerFactory : IControllerFactory
    {
        private string pluginPath;
        private DirectoryCatalog catalog;
        private CompositionContainer container;

        private DefaultControllerFactory defaultControllerFactory;

        public MefControllerFactory(string pluginPath)
        {
            this.pluginPath = pluginPath;
            this.catalog = new DirectoryCatalog(pluginPath);
            this.container = new CompositionContainer(catalog);

            this.defaultControllerFactory = new DefaultControllerFactory();
        }

        #region IControllerFactory Members

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            IController controller = null;

            if (controllerName != null)
            {
                Lazy<IController> export = this.container.GetExports<IController>(controllerName).FirstOrDefault();

                if (export != null)
                {
                    controller = export.Value;
                }
            }

            if (controller == null)
            {
                return this.defaultControllerFactory.CreateController(requestContext, controllerName);
            }

            return controller;
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller)
        {
            IDisposable disposable = controller as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        #endregion
    }
}