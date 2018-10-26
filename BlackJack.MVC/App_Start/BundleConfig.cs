using System.Web.Optimization;

namespace BlackJack.MVC
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/main").Include(
						"~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.cookie.js",
                        "~/Scripts/modernizr-*",
                        "~/Scripts/bootstrap.js"));

			bundles.Add(new ScriptBundle("~/bundles/login").Include(
                    "~/Scripts/kendo.all.min.js",
                    "~/Scripts/site/logs.js"));

            bundles.Add(new ScriptBundle("~/bundles/logs").Include(
                    "~/Scripts/site/login.js"));

            bundles.Add(new ScriptBundle("~/bundles/game").Include(
						"~/Scripts/site/game.js",
						"~/Scripts/site/gameHelper.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/kendo").Include(
                  "~/Content/kendo.common.min.css",
                  "~/Content/kendo.bootstrap.min.css"));

        }
	}
}
