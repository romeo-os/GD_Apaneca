using System.Web;
using System.Web.Optimization;

namespace GestionDocumentalAdmin
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.mCustomScrollbar.js",
                        "~/Scripts/validate.messages.es.js",
                        "~/Scripts/toastr.js",
                        "~/Scripts/base64.js",
                        "~/Scripts/Js/layout.view.js",
                        "~/Scripts/DatosUsuario/datosUsuario.js"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/umd/popper.js",
                      "~/Scripts/umd/popper-utils.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/flat-ui.js",               //http://designmodo.github.io/Flat-UI/
                      "~/Scripts/bootbox.js",
                      "~/Scripts/datatables.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/flat-ui.css",              //http://designmodo.github.io/Flat-UI/
                      "~/Content/css/animation.css",
                      "~/Content/css/fontello.css",
                      "~/Content/jquery.mCustomScrollbar.css",
                      "~/Content/fontawesome-all.css",      //https://fontawesome.com/start
                      "~/Content/css/style.css",
                      "~/Content/css/StyleLayout.css",
                      "~/Content/toastr.css",
                      "~/Content/datatables.css"));
        }
    }
}
