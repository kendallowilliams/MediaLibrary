using System.Web;
using System.Web.Optimization;

namespace MediaLibraryWebUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/polyfill").Include(
                        "~/lib/js-polyfills/polyfill.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Content/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/lib/jquery/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/lib/jqueryui/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/marquee").Include(
                        "~/lib/jQuery.Marquee/jquery.marquee.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/lib/modernizr/modernizr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/lib/bootstrap/dist/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/popperjs").Include(
                      "~/lib/popper.js/umd/popper.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fontawesome").Include(
                      "~/lib/font-awesome/js/all.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/requirejs").Include(
                      "~/lib/require.js/require.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/lib/bootstrap/dist/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/lib/font-awesome/css/all.min.css",
                      "~/lib/bootstrap/dist/css/bootstrap.min.css",
                      "~/lib/jqueryui/jquery-ui.min.css"));
        }
    }
}