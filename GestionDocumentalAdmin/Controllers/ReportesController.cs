using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionDocumentalAdmin.Helpers;
using GestionDocumentalAdmin.Items;
using GestionDocumentalAdmin.Models.Models;
using Rotativa;

namespace GestionDocumentalAdmin.Controllers
{
    public class ReportesController : Controller
    {
        // GET: Reportes
        public ActionResult ReportesP()
        {
            return View();
        }

        public ActionResult ValorReporteDs()
        {
            var db = new AppDBContext();
            //var doc = db.Documentos.ToList();
            var doc = from d in db.Documentos
                      where d.idFlujoDocumento != null select d;

            var lstDocs = new SelectList(doc, "idDocumento", "nombreDocumentos");
            ViewData["documentos"] = lstDocs;

            return PartialView();
        }

        
        public ActionResult reporteDS(int Id)
        {
            List<ReporteDSItem> lst = null;
            using (AppDBContext db = new AppDBContext())
            {
                lst = (from d in db.Documentos
                       from f in db.FlujoDocumento
                       from p in db.Procedimientos
                       from u in db.Usuarios
                       from h in db.Historico
                       from a in db.Area
                       where d.idDocumento == Id
                       && f.idDocumento == Id
                       && p.idProcedimiento == f.idProcedimiento
                       && p.idProcedimiento == h.idProcedimiento
                       && a.idArea == f.idArea
                       //&& a.idUsuario == h.idUsuario     // 
                       && h.idDocumento == d.idDocumento
                       && u.idUsuario == h.idUsuario
                       select new ReporteDSItem
                       {
                           IdD = d.idDocumento,
                           NombreD = d.nombreDocumentos,
                           Area = a.nombre,
                           Proceso = p.procedimiento,
                           FechaC = SqlFunctions.DateName("day", h.fechaInicio) + "/" + SqlFunctions.DateName("month", h.fechaInicio) + "/" + SqlFunctions.DateName("year", h.fechaInicio)
                       }).ToList();
            }

            //Creamos el PDF de la vista html
            var reporte = new ViewAsPdf("reporteDS", lst) {
                PageSize = Rotativa.Options.Size.Letter,
                PageMargins = new Rotativa.Options.Margins(10, 20, 20, 20)
            };
            
            return reporte;
        }

        //-------------Reporte documentos subidos por usuario--------------

        public ActionResult ValorReporteDUp()
        {
            var db = new AppDBContext();
            //var doc = db.Documentos.ToList();
            var user = from u in db.Usuarios
                      where u.deleted_at == null
                      select u;

            var lstUser = new SelectList(user, "idUsuario", "nombre");
            ViewData["usuarios"] = lstUser;

            return PartialView();
        }

        public ActionResult reporteDUp(int Id)
        {
            List<ReporteDUpItem> lst = null;
            using (AppDBContext db = new AppDBContext())
            {
                lst = (from d in db.Documentos
                       from u in db.Usuarios
                       where d.idFlujoDocumento == null 
                       && d.idUsuario ==  u.idUsuario
                       && d.idUsuario == Id
                       select new ReporteDUpItem
                       {
                           Documento = d.nombreDocumentos,
                           Fecha = SqlFunctions.DateName("day", d.created_at) + "/" + SqlFunctions.DateName("month", d.created_at) + "/" + SqlFunctions.DateName("year", d.created_at),
                           Usuario = u.nombre +" "+ u.apellido
                       }).ToList();
            }

            //Creamos el PDF de la vista html
            var reporte = new ViewAsPdf("reporteDUp", lst)
            {
                PageSize = Rotativa.Options.Size.Letter,
                PageMargins = new Rotativa.Options.Margins(10, 20, 20, 20)
            };

            return reporte;
        }


    }
}