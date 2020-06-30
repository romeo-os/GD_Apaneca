using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GestionDocumentalAdmin.Controllers;
using GestionDocumentalAdmin.Helpers;
using GestionDocumentalAdmin.Models.Models;

namespace GestionDocumentalAdmin.Filters
{
    public class VerificarSesion : ActionFilterAttribute
    {
        /*public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var oUsuario = (Usuarios)HttpContext.Current.Session["usuario"];

            if (oUsuario == null) {
                //Si la sesion es null. 
                //Y no es el HomeController
                if (filterContext.Controller is HomeController == false) {

                    //Lo enviamos al login(la vista Index)
                    filterContext.HttpContext.Response.Redirect("~/Home/Index");
                }
            }

            base.OnActionExecuting(filterContext);
        }*/

        // Si no estamos logeado, regresamos al login
        public class AutenticadoAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                base.OnActionExecuting(filterContext);

                if (!SessionHelper.ExistUserInSession())
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Home",
                        action = "Index"
                    }));
                }
            }
        }

        // Si estamos logeado ya no podemos acceder a la página de Login
        public class NoLoginAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                base.OnActionExecuting(filterContext);

                if (SessionHelper.ExistUserInSession())
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Home",
                        action = "UserDashBoard"
                    }));
                }
            }
        }

        // Prevenir que se almacene la pagina en cache
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public sealed class NoCacheAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();

                base.OnResultExecuting(filterContext);
            }
        }
    }
}