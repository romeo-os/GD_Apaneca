using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionDocumentalAdmin.Helpers;
using GestionDocumentalAdmin.Items;
using GestionDocumentalAdmin.Models.Models;
using GestionDocumentalAdmin.Models.TableViewModels;
using GestionDocumentalAdmin.Models.ViewModels;
using GestionDocumentalAdmin.Responses;
using GestionDocumentalAdmin.Commons;

namespace GestionDocumentalAdmin.Controllers
{
    public class TimeLineController : Controller
    {
        // GET: TimeLine
        public ActionResult TimeLP()
        {
            return View();
        }


        //GET
        [PermisoAttribute(Permiso = RolesPermisos.Recibir_documentos)]
        public ActionResult RecibirDoc()
        {
            var db = new AppDBContext();

            var tiposDocs = db.TipoDocumento.ToList();
            var lstTiposDoc = new SelectList(tiposDocs, "idTipoDocumento", "tipoDocumento1");
            ViewData["tiposDocs"] = lstTiposDoc;

            var procedimientos = db.Procedimientos.ToList();
            var lstProcedimientos = new SelectList(procedimientos, "idProcedimiento", "procedimiento");
            ViewData["procedimientos"] = lstProcedimientos;

            return PartialView();
        }

        [HttpPost]
        public ActionResult RecibirDoc(RecibirDocViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = new AppDBContext())
            {
                //Consultar si esxite o no el documento
                var buscarDoc = (from d in db.Documentos
                                 where d.nombreDocumentos == model.NombreDoc
                                 select d).ToList();

                if (buscarDoc.Count >= 1)
                {
                    //Existe
                    DocumentoExiste(model.NombreDoc, model.IdProceso);
                }
                else
                {
                    //No Existe
                    DocumentoNoExiste(model.NombreDoc, model.IdTipoDoc, model.IdProceso);
                }
            }
            return Redirect(Url.Content("~/TimeLine/UbicarDocumentosP"));
        }


        //-----------------------Metodos para recibir un documento-----------------------

        public void DocumentoExiste(string _nombreDoc, int _idProc)
        {
            int idUsuarioActual = SessionHelper.GetUser().idUsuario;
            using (var db = new AppDBContext())
            {
                DateTime fhActual = DateTime.Now;
                Documentos oDocs = new Documentos();
                FlujoDocumento fDocs = new FlujoDocumento();
                Historico oHistorico = new Historico();

                //Editamos solo la info del mismo
                var idDoc = (from dId in db.Documentos
                             where dId.nombreDocumentos == _nombreDoc
                             select dId).FirstOrDefault();

                idDoc.updated_at = fhActual;

                db.Entry(idDoc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //obtenemos el area del usuario
                var area = (from a in db.Area
                            where a.idUsuario == idUsuarioActual
                            select a).FirstOrDefault();

                //Obtenemos datos del doc Guardado/Editado
                var docs = (from d in db.Documentos
                            where d.nombreDocumentos == _nombreDoc
                            select d).FirstOrDefault();


                //Obtener historico de donde sale 
                var oHistoricoA = (from h in db.Historico
                                   where h.idDocumento == idDoc.idDocumento &&
                                   h.fechaFin == null
                                   select h).FirstOrDefault();
                //Asignamos la fecha de salida
                oHistoricoA.fechaFin = fhActual;
                db.Entry(oHistoricoA).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();


                //Creamos el flujo
                fDocs.idDocumento = docs.idDocumento;
                fDocs.idProcedimiento = _idProc;
                fDocs.idArea = area.idArea;


                //----------Creamos el Historico----------------
                oHistorico.idDocumento = docs.idDocumento;
                oHistorico.idProcedimiento = _idProc;
                oHistorico.fechaInicio = fhActual;
                //oHistorico.fechaFin = ; Queda NULL

                oHistorico.idUsuario = idUsuarioActual;
                db.Historico.Add(oHistorico);

                //Asignamos el flujo al documento
                db.FlujoDocumento.Add(fDocs);
                db.SaveChanges();

            }

        }

        public void DocumentoNoExiste(string _nombreDoc, int _idTipoD, int _idProc)
        {
            int idUsuarioActual = SessionHelper.GetUser().idUsuario;
            using (var db = new AppDBContext())
            {
                DateTime fhActual = DateTime.Now;
                Documentos oDocs = new Documentos();
                FlujoDocumento fDocs = new FlujoDocumento();
                Historico oHistorico = new Historico();

                //Agregamos info del doc
                oDocs.nombreDocumentos = _nombreDoc;
                oDocs.created_at = fhActual;
                oDocs.idUsuario = idUsuarioActual;
                oDocs.idTipoDocumento = _idTipoD;

                db.Documentos.Add(oDocs);
                db.SaveChanges();
                //obtenemos el area del usuario
                var area = (from a in db.Area
                                //where a.idUsuario == oDocs.idUsuario
                            where a.idUsuario == idUsuarioActual
                            select a).FirstOrDefault();

                //Obtenemos datos del doc Guardado/Editado
                var docs = (from d in db.Documentos
                            where d.nombreDocumentos == _nombreDoc
                            select d).FirstOrDefault();

                //Creamos el flujo
                fDocs.idDocumento = docs.idDocumento;
                fDocs.idProcedimiento = _idProc;
                fDocs.idArea = area.idArea;

                //----------Creamos el Historico----------------
                //(VALIDAR ENTRADA Y SALIDA DE AREA)

                oHistorico.idDocumento = docs.idDocumento;
                oHistorico.idProcedimiento = _idProc;
                oHistorico.fechaInicio = fhActual;

                //No se guardara fecha de salida
                //oHistorico.fechaFin = fhActual;


                oHistorico.idUsuario = idUsuarioActual;
                db.Historico.Add(oHistorico);


                //Asignamos el flujo al documento
                db.FlujoDocumento.Add(fDocs);
                db.SaveChanges();

                //Ontenemos el ID del flujo para luego asignar al doc
                var flujo = (from f in db.FlujoDocumento
                             where f.idDocumento == docs.idDocumento && f.idArea == area.idArea
                             select f).FirstOrDefault();

                //Asignamos el flujo al doc nuevo
                oDocs.idFlujoDocumento = flujo.idFlujoDocumento;

                db.Entry(oDocs).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

        }
        //----------------------Fin metodos----------------------------
        [PermisoAttribute(Permiso = RolesPermisos.Ubicar_documentos)]
        public ActionResult UbicarDocumentosP()
        {
            return View();
        }

        public ActionResult GetDocumentos()
        {

            List<UbicarDocItem> lst;
            Response<List<UbicarDocItem>> response;
            string mensaje = "Error al obtener datos";
            bool correcto = false;

            //List<DocumentosTableViewModel> lst = null;

            using (AppDBContext db = new AppDBContext())
            {
                var obj = (from d in db.Documentos
                           join td in db.TipoDocumento on
                           d.idTipoDocumento equals td.idTipoDocumento
                           where d.deleted_at == null && d.idFlujoDocumento != null
                           select new UbicarDocItem
                           {
                               IdDocumento = d.idDocumento,
                               NombreDoc = d.nombreDocumentos,
                               TipoDocumento = td.tipoDocumento1
                           }).ToList();

                lst = obj;

                mensaje = "Success";
                correcto = true;
            }

            response = new Response<List<UbicarDocItem>>(correcto, mensaje, lst);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //Recibe el ID del documento en movimiento
        [PermisoAttribute(Permiso = RolesPermisos.Ubicar_documentos)]
        public ActionResult UbicarDocumentosR(int Id)
        {
            List<UbicarTableViewModel> lst = null;
            using (AppDBContext db = new AppDBContext())
            {
                int idU = SessionHelper.GetUser().idUsuario;

                //obtenemos el area del usuario
                var area = (from a in db.Area
                            where a.idUsuario == idU
                            select a).FirstOrDefault();

                var oDoc = db.Documentos.Find(Id);
                ViewData["nombreDoc"] = oDoc.nombreDocumentos;
                ViewData["encargado"] = oDoc.Carpeta;

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
                       select new UbicarTableViewModel
                       {
                           Area = a.nombre,
                           Proceso = p.procedimiento,
                           Encargado = u.nombre + " " + u.apellido,
                           FechaI = h.fechaInicio.ToString(),
                           FechaS = h.fechaFin.ToString(),
                           Tiempo = SqlFunctions.DateDiff("mi", h.fechaInicio, h.fechaFin)
                       }).ToList();
            }

            return View(lst);
        }

    }
}