using System.Web;
using System.Web.Optimization;

namespace MediaLibraryWebUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Content/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/fontawesome").Include(
                      "~/lib/font-awesome/js/all.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/requirejs").Include(
                      "~/lib/require.js/require.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/lib/jquery-ui-1.12.1.custom/jquery-ui.min.css",
                      "~/Content/site.css",
                      "~/lib/font-awesome/css/all.min.css",
                      "~/lib/bootstrap/dist/css/bootstrap.min.css"));
        }
    }
}