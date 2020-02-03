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
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/lib/jqueryui/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/marquee").Include(
                        "~/lib/jQuery.Marquee/jquery.marquee.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/lib/bootstrap/dist/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/popperjs").Include(
                      "~/Scripts/umd/popper.js"));

            bundles.Add(new ScriptBundle("~/bundles/fontawesome").Include(
                      "~/lib/font-awesome/js/all.js"));

            bundles.Add(new ScriptBundle("~/bundles/jplayer").Include(
                      "~/lib/jplayer/jplayer/jquery.jplayer.min.js",
                      "~/lib/jplayer/add-on/jplayer.playlist.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/lib/font-awesome/css/all.css",
                      "~/lib/bootstrap/dist/css/bootstrap.min.css",
                      "~/lib/jqueryui/jquery-ui.min.css"));
        }
    }
}