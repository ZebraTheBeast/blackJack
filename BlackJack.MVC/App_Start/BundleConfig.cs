using System.Web.Optimization;

namespace BlackJack.MVC
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/login").Include(
					"~/Scripts/site/login.js"));

			bundles.Add(new ScriptBundle("~/bundles/game").Include(
						"~/Scripts/site/game.js",
						"~/Scripts/site/gameHelper.js"));

			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));
		}
	}
}
