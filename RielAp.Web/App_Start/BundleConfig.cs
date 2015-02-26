using System.Web;
using System.Web.Optimization;

namespace RielAp.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/mainjs").Include("~/Scripts/jquery-{version}.js").Include("~/Scripts/bootstrap.js").Include("~/Scripts/Custom/CookiesHelper.js"));
            bundles.Add(new ScriptBundle("~/bundles/js").Include("~/Scripts/jquery-ui-{version}.js").IncludeDirectory("~/Scripts/Custom/Common", "*.js").Include("~/Scripts/Custom/AddressFilter.js"));

            bundles.Add(new ScriptBundle("~/bundles/edit").Include("~/Scripts/Custom/Map.js", "~/Scripts/Custom/EditAnnouncement.js"));
            bundles.Add(new ScriptBundle("~/bundles/details").Include("~/Scripts/Custom/Map.js", "~/Scripts/Custom/Details.js", "~/Scripts/Custom/Common/jquery.custom.carouselDraggable.js", "~/Scripts/Custom/Common/slimbox.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/reset.css", "~/Content/Site.css", "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/photos").Include("~/Content/slimbox2.css", "~/Content/jquery.custom.carouselDraggable.css"));
        }
    }
}