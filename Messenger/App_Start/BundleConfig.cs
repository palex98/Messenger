using System.Web.Optimization;

namespace Messenger
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/vendor/scripts").Include(
                      "~/Scripts/jquery-3.3.1.min.js"));

            bundles.Add(new ScriptBundle("~/scripts/bootstrap").Include(
                "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/Site").Include(
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/Bootstrap").Include(
                      "~/Content/bootstrap.min.css"));

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
