using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionDocumentalAdmin.Helpers;
using GestionDocumentalAdmin.Models.Models;
using static GestionDocumentalAdmin.Filters.VerificarSesion;

namespace GestionDocumentalAdmin.Controllers
{
    public class HomeController : Controller
    {
        [NoCache]
        [NoLogin]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Title = "Login";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Usuarios objUser)
        {
            if (ModelState.IsValid)
            {
                using (AppDBContext db = new AppDBContext())
                {
                    try
                    {   //a.deleted_at.Equals(null) para no dejar pasar a los eliminados
                        var obj = db.Usuarios
                            .Where(a => a.usuario.Equals(objUser.usuario) && a.clave.Equals(objUser.clave) && a.deleted_at.Equals(null))
                            .FirstOrDefault();
                        if (obj != null)
                        {
                            SessionHelper.AddUserToSession(obj.idUsuario.ToString());
                            return RedirectToAction("UserDashBoard");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }

                }
            }
            ViewBag.Message = "Usuario o clave incoreccto";
            return View(objUser);
        }

        [Autenticado]
        public ActionResult UserDashBoard()
        {
            if (SessionHelper.ExistUserInSession())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        
        public ActionResult LogOut() {
            SessionHelper.DestroyUserSession();

            return RedirectToAction("Index");
        }
    }
}
