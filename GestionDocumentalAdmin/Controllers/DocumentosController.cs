using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using GestionDocumentalAdmin.Models.ViewModels;
using GestionDocumentalAdmin.Models.TableViewModels;
using GestionDocumentalAdmin.Models.Models;
using GestionDocumentalAdmin.Responses;
using GestionDocumentalAdmin.Items;
using static GestionDocumentalAdmin.Filters.VerificarSesion;
using GestionDocumentalAdmin.Helpers;
using GestionDocumentalAdmin.Commons;
using System.Globalization;
using System.Data.Entity.SqlServer;

namespace GestionDocumentalAdmin.Controllers
{
    [Autenticado]
    public class DocumentosController : Controller
    {
        // GET: /Documentos/Documentos_Principal
        public ActionResult Documentos_Principal()
        {
            return View();
        }

        // GET: /Documentos/CrearCarpeta
        [PermisoAttribute(Permiso = RolesPermisos.Agregar_documentos)]
        public ActionResult CrearCarpeta()
        {
            return PartialView();                               //Devolver vista parcial para mostrarlo en el modal
        }

        // POST: /Documentos/CrearCarpeta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearCarpeta(Carpeta model)
        {
            Response<Carpeta> response;
            string mensaje = "Error al intentar guardar el registro";
            bool correcto = false;

            if (ModelState.IsValid)
            {
                using (var db = new AppDBContext())
                {
                    Carpeta carpeta = new Carpeta();

                    //Accedemos a la ruta donde se crearan todas las carpetas.
                    //la Carpeta "Global" fue creada manualmente para crear ahi dentro otras
                    string rutaGlobal = Server.MapPath("~/Global/" + model.nombre);
                    DateTime fhActual = DateTime.Now;
                    
                    //Llenamos la Tabla Carpetas de la BD
                    carpeta.idCarpeta = 0;
                    carpeta.rutaCarpeta = "~/Global/" + model.nombre;
                    carpeta.nombre = model.nombre;
                    carpeta.descripcion = model.descripcion;
                    carpeta.created_at = fhActual;
                    carpeta.updated_at = fhActual;
                    carpeta.idUsuario = SessionHelper.GetUserId();

                    //Indicamos el Entity que se agrego
                    db.Entry(carpeta).State = System.Data.Entity.EntityState.Added;

                    //db.Carpetas.Add(carpeta);
                    db.SaveChanges();

                    //Preguntamos si la carpeta a crear ya existe
                    if (!Directory.Exists(rutaGlobal))
                    {
                        //Creamos la carpeta con su nombre dentro de la carpeta "Global"
                        Directory.CreateDirectory(rutaGlobal);
                    }

                    mensaje = "Se guardo el registro";
                    correcto = true;
                }
            }

            response = new Response<Carpeta>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // GET: /Documentos/ActualizarCarpeta/5
        [PermisoAttribute(Permiso = RolesPermisos.Editar_documentos)]
        public ActionResult ActualizarCarpeta(int id)
        {
            Carpeta model = new Carpeta();

            using (var db = new AppDBContext())
            {

                //traemos el objeto con el Id
                var obj = db.Carpeta.Find(id);

                //Llenamos el model
                model.idCarpeta = obj.idCarpeta;
                model.nombre = obj.nombre;
                model.descripcion = obj.descripcion;
            }
            return PartialView(model);                               //Devolver vista parcial para mostrarlo en el modal
        }

        // POST: /Documentos/ActualizarCarpeta/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarCarpeta(Carpeta model)
        {
            Response<Carpeta> response;
            string mensaje = "Error al intentar guardar el registro";
            bool correcto = false;

            if (ModelState.IsValid)
            {
                using (var db = new AppDBContext())
                {
                    var oCarpeta = db.Carpeta.Find(model.idCarpeta);
                    DateTime fhActual = DateTime.Today;

                    //Se edita primero el nombre de la carpeta
                    var rutaActual = $@"{Server.MapPath("~/Global/" + oCarpeta.nombre)}";
                    var rutaDestino = $@"{Server.MapPath("~/Global/" + model.nombre)}";

                    if (oCarpeta.nombre != model.nombre)
                    {
                        //Preguntamos si la carpeta a modificar ya existe
                        if (Directory.Exists(rutaActual))
                        {
                            Directory.Move(rutaActual, rutaDestino);
                        }
                        else
                        {
                            Directory.CreateDirectory(rutaDestino);
                        }

                        oCarpeta.rutaCarpeta = "~/Global/" + model.nombre;
                        oCarpeta.nombre = model.nombre;
                    }

                    //Se actualizan en la base de datos
                    oCarpeta.descripcion = model.descripcion;
                    oCarpeta.updated_at = fhActual;

                    //Indicamos el Entity que se edito
                    db.Entry(oCarpeta).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    mensaje = "Se guardo el registro";
                    correcto = true;
                }
            }

            response = new Response<Carpeta>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // DELETE: /Documentos/BorrarCarpeta/5
        [HttpDelete]
        public ActionResult BorrarCarpeta(int id)
        {
            Response<Carpeta> response;
            string mensaje = "Error al intentar eliminar el registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                DateTime fhActual = DateTime.Now;

                var oDocs = (from d in db.Documentos
                             where d.idCarpeta == id
                             select d).ToList();

                //si no esta vacio
                if (oDocs != null)
                {
                    //oDocs.deleted_at = fhActual;
                    foreach (var d in oDocs)
                    {

                        d.deleted_at = fhActual;

                    }
                }

                var oCarpeta = db.Carpeta.Find(id);

                //var rutaActual = $@"{Server.MapPath("~/Global/" + oCarpeta.nombre)}";
                //var rutaDestino = $@"{Server.MapPath("~/Papelera/" + oCarpeta.nombre)}";

                //if (!Directory.Exists(rutaDestino))
                //{
                //    //Movemos la carpeta fisica a la ubicacion: "Papelera"
                //    Directory.Move(rutaActual, rutaDestino);
                //    oCarpeta.rutaCarpeta = "~/Papelera/" + oCarpeta.nombre;
                //}

                oCarpeta.deleted_at = fhActual;

                //Indicamos el Entity que se elimino
                db.Entry(oCarpeta).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                mensaje = "Se elimino el registro, Los documentos ahora esta en la papelera";
                correcto = true;
            }

            response = new Response<Carpeta>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

            // GET: /Documentos/GetData 
            //Carpetas
            public ActionResult GetData()
        {
            List<CarpetaItem> lst;
            Response<List<CarpetaItem>> response;
            string mensaje = "Error al obtener datos";
            bool correcto = false;
            int idU = SessionHelper.GetUserId();
            
            using (AppDBContext db = new AppDBContext())
            {
                var obj = db.Carpeta
                    .Where(c => c.deleted_at == null && c.idUsuario == idU)
                    .Select(c => new CarpetaItem
                    {
                        IdCarpeta = c.idCarpeta,
                        RutaCarpeta = c.rutaCarpeta,
                        Nombre = c.nombre,
                        Descripcion = c.descripcion,
                        Fecha = c.created_at.ToString()
                    }).ToList();

                

                lst = obj;

                mensaje = "Success";
                correcto = true;
            }

            response = new Response<List<CarpetaItem>>(correcto, mensaje, lst);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ContenidoCarpetas(int Id)
        {
            using (AppDBContext db = new AppDBContext())
            {
                var oCarpeta = db.Carpeta.Find(Id);
                string carpeta = oCarpeta.nombre;

                ViewData["NombreC"] = carpeta;
                ViewData["Id_Carpeta"] = oCarpeta.idCarpeta;
            }
            
            return View();
        }

        // GET: /Documentos/GetDocumentosData
        //Contenido de carpetas por Id
        public ActionResult GetDocumentosData(int Id)
        {
            List<DocumentoItem> lst;
            Response<List<DocumentoItem>> response;
            string mensaje = "Error al obtener datos";
            bool correcto = false;

            using (AppDBContext db = new AppDBContext())
            {
                var obj = db.Documentos
                    .Where(d => d.deleted_at == null && d.idFlujoDocumento == null && d.idCarpeta == Id) //d.idFlujoDocumento == null
                    .Select(d => new DocumentoItem                                  //Para solo mostrar los que estan en carpetas
                    {
                        IdDocumento = d.idDocumento,
                        NombreDocumentos = d.nombreDocumentos,
                        RutaDocumento = d.rutaDocumento,
                        TipoDocumento = d.TipoDocumento.tipoDocumento1,
                        IdTipoDocumento = d.idTipoDocumento,
                        IdCarpeta = d.Carpeta.idCarpeta,
                        created_at = d.created_at.ToString(), //== null?"": d.created_at.Value.ToString("dd MMM HH:mm:ss"),
                        updated_at = d.updated_at.ToString()// == null?"":d.updated_at.Value.ToString("dd MMM HH:mm:ss")
                    }).ToList();

                lst = obj;

                mensaje = "Success";
                correcto = true;
            }

            response = new Response<List<DocumentoItem>>(correcto, mensaje, lst);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // GET: /Documentos/SubirDoc/5
        [PermisoAttribute(Permiso = RolesPermisos.Agregar_documentos)]
        public ActionResult SubirDoc(int Id)
        {
            var db = new AppDBContext();
            var tipoDoc = db.TipoDocumento.ToList();
            var lstTipos = new SelectList(tipoDoc, "idTipoDocumento", "tipoDocumento1");
            ViewData["tipoDoc"] = lstTipos;
            ViewData["id"] = Id;

            return PartialView();
        }

        // POST: /Documentos/SubirDoc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubirDoc(Documentos model, HttpPostedFileBase file)
        {
            //var db = new DBRemoteConnection();
            Response<Carpeta> response;
            string mensaje = "Error al intentar guardar el registro";
            bool correcto = false;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            DateTime fhActual = DateTime.Now;

            using (var db = new AppDBContext())
            {
                var oCarpeta = db.Carpeta.Find(model.idCarpeta);
                var docs = new Documentos();

                if (file != null)
                {
                    string rutaDoc = Server.MapPath("~/Global/"+oCarpeta.nombre+"/");
                    string ext = Path.GetExtension(file.FileName);
                    rutaDoc += model.nombreDocumentos+ext;
                    try
                    {
                        file.SaveAs(rutaDoc);

                        docs.idDocumento = 1;
                        docs.nombreDocumentos = model.nombreDocumentos;
                        docs.rutaDocumento = "~/Global/" + oCarpeta.nombre + "/" + model.nombreDocumentos+ext;
                        docs.created_at = fhActual;
                        docs.updated_at = fhActual;
                        docs.idUsuario = SessionHelper.GetUserId();
                        //var tipoDoc = db.TipoDocumentoes.ToList();
                        docs.idTipoDocumento = model.idTipoDocumento;
                       // docs.idTipoDocumento = model.idTipoDocumento;
                        docs.idCarpeta = model.idCarpeta;
                        docs.created_at = DateTime.Today;

                        db.Documentos.Add(docs);

                        //Indicamos el Entity que se agrego
                        db.Entry(docs).State = System.Data.Entity.EntityState.Added;

                        db.SaveChanges();
                        
                        mensaje = "Se guardo el registro";
                        correcto = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }

            }

            response = new Response<Carpeta>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
           
        }

        //Eliminar un documento(Enviar a papelera)
        // DELETE: /Documentos/EliminarArchivo/5
        [HttpDelete]
        public ActionResult EliminarArchivo(int Id)
        {
            Response<Carpeta> response;
            string mensaje = "Error al eliminar el registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                DateTime fhActual = DateTime.Now;
                var oDocs = db.Documentos.Find(Id);
                
                //var rutaActual = $@"{Server.MapPath(oDocs.rutaDocumento)}";
                //string ext = Path.GetExtension(rutaActual);
                //var rutaDestino = $@"{Server.MapPath("~/Papelera/"+oDocs.nombreDocumentos+ext)}";

                
                //Movemos el archivo fisico a la ubicacion: "Papelera"
                //System.IO.File.Move(rutaActual, rutaDestino);

                //oDocs.rutaDocumento = "~/Papelera/"+oDocs.nombreDocumentos+ext;
                oDocs.deleted_at = fhActual;

                //Indicamos el Entity que se elimino
                db.Entry(oDocs).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                mensaje = "Se elimino el registro, El archivo ahora esta en la papelera";
                correcto = true;
            }

            response = new Response<Carpeta>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // GET: /Documentos/EditarDocumento/5
        [PermisoAttribute(Permiso = RolesPermisos.Editar_documentos)]
        public ActionResult EditarDocumento(int Id)
        {
            Documentos model = new Documentos();
           
            using (var db = new AppDBContext())
            {
                //traemos el objeto con el Id
                var oDoc = db.Documentos.Find(Id);
                //Llenamos el DropDownList con los tipos de documentos
                var tipoDoc = db.TipoDocumento.ToList();
                var lstTipos = new SelectList(tipoDoc, "idTipoDocumento", "tipoDocumento1");
                ViewData["tipoDoc"] = lstTipos;

                //Llenamos el model
                model.idDocumento = oDoc.idDocumento;
                model.nombreDocumentos = oDoc.nombreDocumentos;
                model.idTipoDocumento = oDoc.idTipoDocumento;
                
            }

            return PartialView(model);
        }
        
        // POST: /Documentos/EditarDocumento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarDocumento(Documentos model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Response<Documentos> response;
            string mensaje = "Error al editar el registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                var oDoc = db.Documentos.Find(model.idDocumento);
                var oCarpeta = db.Carpeta.Find(oDoc.idCarpeta);
                DateTime fhActual = DateTime.Today;

                oDoc.nombreDocumentos = model.nombreDocumentos;
                oDoc.idTipoDocumento = model.idTipoDocumento;

                string rutaActual = Server.MapPath(oDoc.rutaDocumento);
                string ext = Path.GetExtension(rutaActual);
                string nuevoNombre = Server.MapPath("~/Global/"+oCarpeta.nombre+"/"+ oDoc.nombreDocumentos+ext);
                System.IO.File.Move(rutaActual, nuevoNombre);

                oDoc.rutaDocumento = "~/Global/" + oCarpeta.nombre + "/" + oDoc.nombreDocumentos + ext;
                oDoc.updated_at = fhActual;

                //Indicamos el Entity que se edito
                db.Entry(oDoc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                mensaje = "Se guardo el registro";
                correcto = true;

                //return Redirect(Url.Content("~/Documentos/ContenidoCarpetas/"));//+ oDoc.idCarpeta));
            }

            response = new Response<Documentos>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        // GET: /Documentos/Descargar/5
        [PermisoAttribute(Permiso = RolesPermisos.Descargar_documentos)]
        public FileResult Descargar(int Id)
        {
            var db = new AppDBContext();
            var oDoc = db.Documentos.Find(Id);
            var ext = Path.GetExtension(oDoc.rutaDocumento);

            var ruta = Server.MapPath(oDoc.rutaDocumento);
            string mine = "";

            //tipos de MIME que se esperan usar en el sistema
            //Funcionando con Firefox
            if (ext == ".pdf")
            {
                mine = "application/pdf";
            }
            else if (ext == ".jpg" || ext == ".jpeg")
            {
                mine = "image/jpeg";
            }
            else if (ext == ".doc" || ext == ".docx")
            {
                mine = "Application/msword";
            }else if (ext == ".xls" || ext == ".xlsx")
            {
                mine = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }else if(ext == ".png")
            {
                mine = "image/png";
            }
            else if(ext == ".txt")
            {
                mine = "text/plain";
            }

            return File(ruta,mine, oDoc.nombreDocumentos);

        }

        // GET: /Documentos/ScannerView
        public ActionResult ScannerView()
        {
            var db = new AppDBContext();
            var tipoList = db.TipoDocumento.ToList();
            var carpetaList = db.Carpeta.ToList();

            var lstTipos = new SelectList(tipoList, "idTipoDocumento", "tipoDocumento1");
            var lstCarpetas = new SelectList(carpetaList, "idCarpeta", "nombre");
            ViewData["docType"] = lstTipos;
            ViewData["folder"] = lstCarpetas;

            return View();
        }

        // POST: /Documentos/ScannerView
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScannerView(Documentos model, HttpPostedFileBase scanedfile)
        {
            Response<Carpeta> response;
            string mensaje = "Error al intentar guardar el registro";
            bool correcto = false;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            DateTime fhActual = DateTime.Today;

            using (var db = new AppDBContext())
            {
                var oCarpeta = db.Carpeta.Find(model.idCarpeta);
                var docs = new Documentos();

                if (scanedfile != null)
                {
                    try
                    {
                        string rutaDoc = Server.MapPath("~/Global/" + oCarpeta.nombre + "/");
                        string ext = Path.GetExtension(scanedfile.FileName);
                        rutaDoc += model.nombreDocumentos + ext;

                        scanedfile.SaveAs(rutaDoc);

                        docs.idDocumento = 1;
                        docs.nombreDocumentos = model.nombreDocumentos;
                        docs.rutaDocumento = "~/Global/" + oCarpeta.nombre + "/" + model.nombreDocumentos + ext;
                        docs.created_at = fhActual;
                        docs.updated_at = fhActual;
                        docs.idUsuario = SessionHelper.GetUserId();
                        //var tipoDoc = db.TipoDocumentoes.ToList();
                        docs.idTipoDocumento = model.idTipoDocumento;
                        docs.idTipoDocumento = model.idTipoDocumento;
                        docs.idCarpeta = model.idCarpeta;

                        db.Documentos.Add(docs);

                        //Indicamos el Entity que se agrego
                        db.Entry(docs).State = System.Data.Entity.EntityState.Added;

                        db.SaveChanges();

                        mensaje = "Se guardo el registro";
                        correcto = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }

            }

            response = new Response<Carpeta>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
