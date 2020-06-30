using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionDocumentalAdmin.Items;
using GestionDocumentalAdmin.Models.Models;
using GestionDocumentalAdmin.Models.TableViewModels;
using GestionDocumentalAdmin.Models.ViewModels;
using GestionDocumentalAdmin.Responses;
using GestionDocumentalAdmin.Helpers;
using GestionDocumentalAdmin.Commons;

namespace GestionDocumentalAdmin.Controllers
{
    public class MantenimientosController : Controller
    {
        // GET: Mantenimientos
        public ActionResult MantenimientosP()
        {
            return View();
        }

        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_procedimientos)]
        public ActionResult Procedimientos()
        {

            return View();
        }

        public ActionResult GetProcedimientos()
        {
            //List<ProcedimientosTableViewModel> lst = null;
            List<ProcedimientoItem> lst;
            Response<List<ProcedimientoItem>> response;
            string mensaje = "Error al obtener datos";
            bool correcto = false;

            using (AppDBContext db = new AppDBContext())
            {
                var obj = (from p in db.Procedimientos
                           select new ProcedimientoItem
                           {
                               IdProcedimiento = p.idProcedimiento,
                               Procedimiento = p.procedimiento,
                               Duracion = p.duracion,
                               Prioridad = p.prioridad
                           }).ToList();

                lst = obj;

                mensaje = "Success";
                correcto = true;
            }
            response = new Response<List<ProcedimientoItem>>(correcto, mensaje, lst);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_procedimientos)]
        public ActionResult NuevoProceso()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoProceso(Procedimientos model)
        {
            Response<Procedimientos> response;
            string mensaje = "Error al intentar guardar el registro";
            bool correcto = false;

            if (!ModelState.IsValid)
            {

                return View(model);
            }

            using (var db = new AppDBContext())
            {
                Procedimientos procedimiento = new Procedimientos();

                procedimiento.idProcedimiento = 0;
                procedimiento.procedimiento = model.procedimiento;
                procedimiento.duracion = model.duracion;
                procedimiento.prioridad = model.prioridad;

                db.Entry(procedimiento).State = System.Data.Entity.EntityState.Added;

                db.SaveChanges();

                mensaje = "Se guardo el registro";
                correcto = true;
            }

            response = new Response<Procedimientos>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        //GET
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_procedimientos)]
        public ActionResult EditarProceso(int Id)
        {
            Procedimientos model = new Procedimientos();

            using (var db = new AppDBContext())
            {

                //traemos el objeto con el Id
                var obj = db.Procedimientos.Find(Id);

                //Llenamos el model
                model.idProcedimiento = obj.idProcedimiento;
                model.procedimiento = obj.procedimiento;
                model.duracion = obj.duracion;
                model.prioridad = obj.prioridad;

            }

            return PartialView(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProceso(Procedimientos model)
        {
            Response<Procedimientos> response;
            string mensaje = "Error al intentar guardar el registro";
            bool correcto = false;

            if (!ModelState.IsValid)
            {

                return View(model);
            }

            using (var db = new AppDBContext())
            {
                var oProcedimientos = db.Procedimientos.Find(model.idProcedimiento);

                // oProcedimientos.idProcedimiento = model.Id;
                oProcedimientos.procedimiento = model.procedimiento;
                oProcedimientos.duracion = model.duracion;
                oProcedimientos.prioridad = model.prioridad;

                //Indicamos el Entity que se edito
                db.Entry(oProcedimientos).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                mensaje = "Se guardo el registro";
                correcto = true;

            }

            response = new Response<Procedimientos>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpDelete]
        public ActionResult EliminarProceso(int Id)
        {
            Response<Procedimientos> response;
            string mensaje = "Error al eliminar el registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                var oProcedimiento = db.Procedimientos.Find(Id);

                //Indicamos el Entity que se elimino
                db.Entry(oProcedimiento).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                mensaje = "Procedimiento eliminado";
                correcto = true;
            }
            response = new Response<Procedimientos>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        //-----------------------FUNCIONALIDAD TIPOS DE DOCUMENTOS-----------------------
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_tipos_de_documentos)]
        public ActionResult TiposDoc()
        {
            return View();
        }

        public ActionResult GetTiposDoc()
        {
            List<TiposDocItem> lst;
            Response<List<TiposDocItem>> response;
            string mensaje = "Error al obtener datos";
            bool correcto = false;

            //List<TiposDocTableViewModel> lst = null;
            using (AppDBContext db = new AppDBContext())
            {
                var obj = (from t in db.TipoDocumento
                           select new TiposDocItem
                           {
                               IdTipodDoc = t.idTipoDocumento,
                               TipoDoc = t.tipoDocumento1,
                               DescripcionT = t.descripcionTipoDocumento
                           }).ToList();

                lst = obj;

                mensaje = "Success";
                correcto = true;
            }

            response = new Response<List<TiposDocItem>>(correcto, mensaje, lst);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_tipos_de_documentos)]
        public ActionResult AgregarTiposDoc()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarTiposDoc(TipoDocumento model)
        {
            Response<TipoDocumento> response;
            string mensaje = "Error al intentar guardar el registro";
            bool correcto = false;

            using (AppDBContext db = new AppDBContext())
            {

                TipoDocumento oTipoDoc = new TipoDocumento();

                oTipoDoc.idTipoDocumento = model.idTipoDocumento;
                oTipoDoc.tipoDocumento1 = model.tipoDocumento1;
                oTipoDoc.descripcionTipoDocumento = model.descripcionTipoDocumento;

                db.Entry(oTipoDoc).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                mensaje = "Se guardo el registro";
                correcto = true;

            }

            response = new Response<TipoDocumento>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        //GET
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_tipos_de_documentos)]
        public ActionResult EditarTipoDoc(int Id)
        {

            TipoDocumento model = new TipoDocumento();

            using (var db = new AppDBContext())
            {

                //traer el objeto por su id
                var oTiposDoc = db.TipoDocumento.Find(Id);

                //llenar el model
                model.idTipoDocumento = oTiposDoc.idTipoDocumento;
                model.tipoDocumento1 = oTiposDoc.tipoDocumento1;
                model.descripcionTipoDocumento = oTiposDoc.descripcionTipoDocumento;
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarTipoDoc(TipoDocumento model)
        {
            Response<TipoDocumento> response;
            string mensaje = "Error al intentar guardar el registro";
            bool correcto = false;

            if (!ModelState.IsValid)
            {

                return View(model);
            }

            using (var db = new AppDBContext())
            {
                var oTipoDoc = db.TipoDocumento.Find(model.idTipoDocumento);

                oTipoDoc.tipoDocumento1 = model.tipoDocumento1;
                oTipoDoc.descripcionTipoDocumento = model.descripcionTipoDocumento;

                db.Entry(oTipoDoc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                mensaje = "Se guardo el registro";
                correcto = true;
            }

            response = new Response<TipoDocumento>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public ActionResult EliminarTipoDoc(int Id)
        {
            Response<TipoDocumento> response;
            string mensaje = "Error al eliminar el registro";
            bool correcto = false;
            bool e = false;

            using (var db = new AppDBContext())
            {

                var oTiposDoc = db.TipoDocumento.Find(Id);


                try
                {

                    db.Entry(oTiposDoc).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();

                }
                catch (Exception)
                {

                    //Si esta asociado a un documento no se podra eliminar
                    e = true;
                }

                if (e == true)
                {
                    mensaje = "Tipo documento asociado a un documento";
                    correcto = false;
                }
                else
                {
                    mensaje = "Tipo documento eliminado";
                    correcto = true;
                }


            }

            response = new Response<TipoDocumento>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);

        }
    }
}