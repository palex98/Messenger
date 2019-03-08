using System.Web;
using System.Web.Optimization;

namespace Messenger
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/vendor/scripts").Include(
                      "~/Content/vendor/jquery/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendorbootstrap").Include(
                      "~/Content/vendor/bootstrap/js/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/Site").Include(
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/vendorBootstrap").Include(
                      "~/Content/vendor/bootstrap/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/vendor").Include(
                      "~/Content/vendor/fontawesome-free-5.6.3-web/css/all.min.css"));

            bundles.Add(new StyleBundle("~/Content/login_style").Include(
                      "~/Content/Login_style.css"));

            bundles.Add(new StyleBundle("~/Content/admin_style").Include(
                      "~/Content/Admin_style.css"));

            bundles.Add(new ScriptBundle("~/app/scripts").Include(
                        "~/Scripts/scripts.js"));

            bundles.Add(new ScriptBundle("~/app/admin").Include(
                        "~/Scripts/admin.js"));
        }
    }
}
