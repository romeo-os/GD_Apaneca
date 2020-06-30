using GestionDocumentalAdmin.Helpers;
using GestionDocumentalAdmin.Items;
using GestionDocumentalAdmin.Models.Models;
using GestionDocumentalAdmin.Responses;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.Mvc;

namespace GestionDocumentalAdmin.Controllers
{
    public class DatosUsuarioController : Controller
    {
 
        public ActionResult UpdateDatos()
        {
            DatosUsuarioItem user = new DatosUsuarioItem();

            using (var db = new AppDBContext())
            {
                int Id = SessionHelper.GetUser().idUsuario;

                var userD = db.Usuarios.Find(Id);

                user.idUsuario = userD.idUsuario;
                user.usuario = userD.usuario;
                  
            }
            return PartialView(user);
        }

        [HttpPost]
        public ActionResult UpdateDatos(DatosUsuarioItem model)
        {

            Response<DatosUsuarioItem> response;
            string mensaje = "Error al intentar guardar el registro";
            bool correcto = false;

            if (!ModelState.IsValid)
            {

                return View(model);
            }

            using (var db = new AppDBContext())
            {
                var oUsuario = db.Usuarios.Find(model.idUsuario);
                
                oUsuario.usuario = model.usuario;

                if(model.clave == oUsuario.clave && model.nuevaClave == model.confirClave)
                {
                    oUsuario.clave = model.nuevaClave;

                    db.Entry(oUsuario).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    mensaje = "Se actualizaron sus datos";
                    correcto = true;
                }
                else
                {
                    mensaje = "Se produjo un error al actualizar";
                    correcto = false;
                }
                
            }

            response = new Response<DatosUsuarioItem>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}