 using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionDocumentalAdmin.Commons;
using GestionDocumentalAdmin.Helpers;
using GestionDocumentalAdmin.Items;
using GestionDocumentalAdmin.Models.Models;
using GestionDocumentalAdmin.Models.TableViewModels;
using GestionDocumentalAdmin.Models.ViewModels;
using GestionDocumentalAdmin.Responses;

namespace GestionDocumentalAdmin.Controllers
{
    public class AdministradorController : Controller
    {

        // GET: Administrador
        public ActionResult AdministradorP()
        {
            return View();
        }

        /////////////////Administracion de usuarios/////////////////////
        // GET: /Administrador/GetData
        public ActionResult GetData()
        {
            List<UsuarioItem> list;
            Response<List<UsuarioItem>> response;
            string mensaje = "Error al obtener datos";
            bool correcto = false;

            using (AppDBContext db = new AppDBContext())
            {
                var obj = db.Usuarios
                    .Where(u => u.deleted_at == null)
                    .Select(u => new UsuarioItem
                    {
                        idUsuario = u.idUsuario,
                        nombre = u.nombre + " " + u.apellido,
                        usuario = u.usuario,
                        email = u.email,
                        estado = u.estado
                    }).ToList();

                list = obj;

                mensaje = "Success";
                correcto = true;
            }

            response = new Response<List<UsuarioItem>>(correcto, mensaje, list);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // GET: /Administrador/AdminUsuario
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_Usuarios)]
        public ActionResult AdminUsuario()
        {
            return View();
        }

        // GET: /Administrador/NuevoUsuario
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_Usuarios)]
        public ActionResult NuevoUsuario()
        {
            var db = new AppDBContext();
            var roles = db.Roles.ToList();
            var lstRoles = new SelectList(roles, "idRol", "rol");
            ViewData["roles"] = lstRoles;

            return PartialView();
        }

        // POST: /Administrador/NuevoUsuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoUsuario(Usuarios model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }

            Response<Carpeta> response;
            string mensaje = "Error al guardar el registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                Usuarios usuario = new Usuarios();

                usuario.idUsuario = 0;
                usuario.nombre = model.nombre;
                usuario.apellido = model.apellido;
                usuario.usuario = model.usuario;
                usuario.clave = model.clave;
                usuario.email = model.email;
                //usuario.estado = model.estado;
                usuario.idRol = model.idRol;
                usuario.created_at = DateTime.Today;

                db.Usuarios.Add(usuario);

                db.SaveChanges();

                mensaje = "Se guardo el registro";
                correcto = true;
            }

            response = new Response<Carpeta>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // GET: /Administrador/EditarUsuario/5
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_Usuarios)]
        public ActionResult EditarUsuario(int Id)
        {
            Usuarios model = new Usuarios();

            using (var db = new AppDBContext()) {

                //traemos el objeto con el Id
                var oUsuario = db.Usuarios.Find(Id);

                //Llenamos el model
                model.idUsuario = oUsuario.idUsuario;
                model.nombre = oUsuario.nombre;
                model.apellido = oUsuario.apellido;
                model.usuario = oUsuario.usuario;
                model.clave = oUsuario.clave;
                model.email = oUsuario.email;
                //model.estado = oUsuario.estado;

                var roles = db.Roles.ToList();
                var lstRoles = new SelectList(roles, "idRol", "rol");

                ViewData["roles"] = lstRoles;
                model.idRol = oUsuario.idRol;
             
            }

            return PartialView(model);
        }


        //Recibimos el modelo
        // POST: /Administrador/EditarUsuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(Usuarios model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }

            Response<Carpeta> response;
            string mensaje = "Error al editar el registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                var oUsuario = db.Usuarios.Find(model.idUsuario);

                oUsuario.nombre = model.nombre;
                oUsuario.apellido = model.apellido;
                oUsuario.usuario = model.usuario;
                oUsuario.email = model.email;
                //oUsuario.estado = model.estado;
                oUsuario.idRol = model.idRol;
                oUsuario.created_at = DateTime.Today;

                if(model.clave != null && model.clave.Trim() != "")
                {
                    oUsuario.clave = model.clave;
                }

                //Indicamos el Entity que se edito
                db.Entry(oUsuario).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                mensaje = "Se guardo el registro";
                correcto = true;
            }

            response = new Response<Carpeta>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //Validar eliminacion con usuarios asosciados a otras tablas
        // DELETE: /Administrador/EliminarUsuario/5
        [HttpDelete]
        public ActionResult EliminarUsuario(int Id)
        {
            DateTime fhActual = DateTime.Now;
            Response<Carpeta> response;
            string mensaje = "Error al editar el registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                var oUsuario = db.Usuarios.Find(Id);

                //Se realizara borrado logico
                //Actualizando el campo deleted_at del usuario

                //validamos si tiene un área asignda
                string estado ="";

                var oArea = (from a in db.Area
                            where a.idUsuario == Id
                            select a).ToList();
                
                foreach(var a in oArea)
                {
                    estado = a.estado;
                }

                if (estado == "true")
                {
                    mensaje = "El usuario está a cargo de un área";
                }
                else
                {

                    oUsuario.deleted_at = fhActual;
                    db.Entry(oUsuario).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    mensaje = "Se elimino el registro";
                    correcto = true;
                }
            }

            response = new Response<Carpeta>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //----------------FUNCIONALIDAD AREAS---------------------------
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_Areas)]
        public ActionResult Areas()
        {
            return View();
        }

        public ActionResult GetAreas()
        {
            List<AreaItem> lst;
            Response<List<AreaItem>> response;
            string mensaje = "Error al obtener datos";
            bool correcto = false;

            using (AppDBContext db = new AppDBContext())
            {   
                         var obj = from a in db.Area.ToList()
                           select new AreaItem
                          {
                              IdArea = a.idArea,
                              Nombre = a.nombre,
                              Funcion = a.funcion,
                              Encargado = (new Func<string>(() => {
                                  try
                                  {
                                      
                                      if (a.estado == "false")
                                      {
                                          //Mostramos sin asignar
                                          return "Sin Asignar";
                                      }
                                      else
                                      {
                                          //Obtener el nombre del Usuario
                                          var oUsuario = db.Usuarios.Find(a.idUsuario);
                                          return oUsuario.nombre +" "+ oUsuario.apellido;
                                        
                                      }

                                  }
                                  catch
                                  {
                                      //si ocurre un error
                                      return "Error";
                                  }
                              }))()
                          };

               lst = obj.ToList();
                
                
                mensaje = "Success";
                correcto = true;
            }

            response = new Response<List<AreaItem>>(correcto, mensaje, lst);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //GET
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_Areas)]
        public ActionResult NuevaArea()
        {
            var db = new AppDBContext();
            //Llenamos el DropDownList con los usuarios que existen
            var encargado = from u in db.Usuarios
                             where u.deleted_at == null
                             select u;

            var encargadoA = encargado.ToList();
            // mostrar Nombre + Apellido ?
            var lstTipos = new SelectList(encargadoA, "idUsuario", "Nombre");
            ViewData["encargadoA"] = lstTipos;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevaArea(Area model)
        {
            Response<Area> response;
            string mensaje = "Error al intentar guardar el registro";
            bool correcto = false;

            if (!ModelState.IsValid)
            {

                return View(model);
            }

            using (var db = new AppDBContext())
            {

                Area areas = new Area();

                areas.idArea = 0;
                areas.nombre = model.nombre;
                areas.funcion = model.funcion;
                areas.estado = "true";
                areas.idUsuario = model.idUsuario;
                db.Entry(areas).State = System.Data.Entity.EntityState.Added;

                db.SaveChanges();

                mensaje = "Se guardo el registro";
                correcto = true;
            }

            response = new Response<Area>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //GET
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_Areas)]
        public ActionResult EditarArea(int Id)
        {
            Area model = new Area();
            //EditarAreasViewModel model = new EditarAreasViewModel();

            using (var db = new AppDBContext())
            {

                ////traemos el objeto con el Id
                var obj = db.Area.Find(Id);

                //Llenamos el DropDownList con los usuarios que existen
                var encargado = from u in db.Usuarios
                                where u.deleted_at == null
                                select u;

                var encargadoA = encargado.ToList();
                // mostrar Nombre + Apellido ?
                var lstTipos = new SelectList(encargadoA, "idUsuario", "Nombre");
                ViewData["encargadoA"] = lstTipos;

                ////Llenamos el model
                model.idArea = obj.idArea;
                model.nombre = obj.nombre;
                model.funcion = obj.funcion;

                model.idUsuario = obj.idUsuario;

            }

            return PartialView(model);
        }


        //Recibimos el modelo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarArea(Area model)
        {
            Response<Area> response;
            string mensaje = "Error al intentar guardar el registro";
            bool correcto = false;

            if (!ModelState.IsValid)
            {

                return View(model);
            }

            using (var db = new AppDBContext())
            {
                var oAreas = db.Area.Find(model.idArea);

                //oAreas.idArea = model.idArea;
                oAreas.nombre = model.nombre;
                oAreas.funcion = model.funcion;

                //si idUsuario es null, estoy desasignando usuario

                if(model.idUsuario == null)
                {
                    //no modifico el idUsuario
                    oAreas.estado = "false";
                }
                else
                {
                    //si modifico el id de usuario
                    oAreas.estado = "true";
                    oAreas.idUsuario = model.idUsuario;
                }

                

                //Indicamos el Entity que se edito
                db.Entry(oAreas).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                mensaje = "Se guardo el registro";
                correcto = true;

            }

            response = new Response<Area>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpDelete]
        public ActionResult EliminarArea(int Id)
        {
            Response<Area> response;
            string mensaje = "Error al eliminar el registro";
            bool correcto = false;
            bool e = false;

            using (var db = new AppDBContext())
            {
                var oArea = db.Area.Find(Id);

                try
                {
                    //Indicamos el Entity que se elimino
                    db.Entry(oArea).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    //Si esta asociada a un usuario
                    e = true;
                }

                if (e == true)
                {
                    mensaje = "Área asociada a un usuario";
                    correcto = false;
                }
                else
                {
                    mensaje = "Área eliminada";
                    correcto = true;
                }


            }

            response = new Response<Area>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        ////////////////Papelera////////////////
        //GET /Administrador/GetPapeleraData
        public ActionResult GetPapeleraData()
        {
            List<PapeleraItem> list;
            Response<List<PapeleraItem>> response;
            string mensaje = "Error al obtener datos";
            bool correcto = false;
            using (AppDBContext db = new AppDBContext())
            {
                list = (from d in db.Documentos
                        //from d in db.Documentos
                       where d.deleted_at != null
                       select new PapeleraItem
                       {
                           IdCarpeta = d.idCarpeta,
                           IdDocumento = d.idDocumento,
                           NombreArchivo = d.nombreDocumentos,
                           FechaE = SqlFunctions.DateName("day", d.deleted_at) + "/" + SqlFunctions.DateName("month", d.deleted_at) + "/" + SqlFunctions.DateName("year", d.deleted_at)
                           //FechaED = d.deleted_at.ToString() 

                       }).ToList();

                //var obj = from c in db.Carpeta.ToList()
                //          from d in db.Documentos.ToList()
                //          where c.deleted_at != null
                //          && d.deleted_at != null
                //           select new PapeleraItem
                //            {
                //            IdCarpeta = c.idCarpeta,
                //            IdDocumento = d.idDocumento,
                //            RutaCarpeta = c.rutaCarpeta,
                //            RutaDocumento = d.rutaDocumento,
                //            NombreArchivo = (new Func<string>(() => {
                //                try
                //                {
                //                    if (c.deleted_at.Equals(d.deleted_at))
                //                    {
                //                        return c.nombre;
                //                    }
                //                    else
                //                    {
                //                        return d.nombreDocumentos;
                //                    }
                //                }
                //                catch
                //                {
                //                    //si ocurre un error
                //                    return "Error";
                //                }
                //            }))(),
                //            Descripcion = c.descripcion,
                //            //FechaE = SqlFunctions.DateName("day", d.deleted_at) + "/" + SqlFunctions.DateName("month", d.deleted_at) + "/" + SqlFunctions.DateName("year", d.deleted_at)
                //            FechaE = d.deleted_at.ToString()
                //        };

                //list = obj.ToList();

                mensaje = "Success";
                correcto = true;
                
            }

            response = new Response<List<PapeleraItem>>(correcto, mensaje, list);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //GET /Administrador/Papelera
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_Papelera)]
        public ActionResult Papelera()
        {
            return View();
        }

        public ActionResult resetArchivo(int Id)
        {
            Response<Carpeta> response;
            string mensaje = "Error al intentar recuperar el registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                DateTime fhActual = DateTime.Now;

                var oDocs = (from d in db.Documentos
                             where d.idCarpeta == Id
                             select d).ToList();

                //var oDoc = db.Documentos.Find(oDoc)



                //si no esta vacio
                if (oDocs != null)
                {
                    //oDocs.deleted_at = fhActual;
                    foreach (var d in oDocs)
                    {

                        d.deleted_at = null;

                    }
                }

                var oCarpeta = db.Carpeta.Find(Id);

                //var rutaActual = $@"{Server.MapPath("~/Global/" + oCarpeta.nombre)}";
                //var rutaDestino = $@"{Server.MapPath("~/Papelera/" + oCarpeta.nombre)}";

                /* if (!Directory.Exists(rutaDestino))
                 {
                     //Movemos la carpeta fisica a la ubicacion: "Papelera"
                     Directory.Move(rutaActual, rutaDestino);
                     oCarpeta.rutaCarpeta = "~/Papelera/" + oCarpeta.nombre;
                 }*/
                if (oCarpeta != null)
                {
                    oCarpeta.deleted_at = null;
                }

                //Indicamos el Entity que se elimino
                db.Entry(oCarpeta).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                mensaje = "Se recuperó el registro";
                correcto = true;
            }

            response = new Response<Carpeta>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //Eliminar definitivamente de la papelera
        // DELETE: /Administrador/EliminarArchivo/5
        [HttpDelete]
        public ActionResult EliminarArchivo(int Id)
        {
            Response<Documentos> response;
            string mensaje = "Error al eliminar el archivo";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
               
                var oDoc = db.Documentos.Find(Id);
                
                string rutaD = $@"{Server.MapPath(oDoc.rutaDocumento)}";

                //Eliminar documento
                try
                {
                    //Directory.Delete(rutaC, true);
                    System.IO.File.Delete(rutaD);
                    
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
                

                //removemos de la bd
                db.Documentos.Remove(oDoc);
                db.SaveChanges();
                
              
               mensaje = "Se elimino el archivo";
               correcto = true;
             
            }

            response = new Response<Documentos>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        //////////////////Roles//////////////////////
        //GET /Administrador/Roles
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_Roles)]
        public ActionResult Roles()
        {
            return View();
        }

        //GET /Administrador/GetRolesData
        public ActionResult GetRolesData()
        {
            List<RolItem> list;
            Response<List<RolItem>> response;
            string mensaje = "Error al obtener datos";
            bool correcto = false;

            using (AppDBContext db = new AppDBContext())
            {
                var obj = db.Roles
                    .Select(r => new RolItem
                    {
                        idRol = r.idRol,
                        rol = r.rol
                    }).ToList();

                list = obj;

                mensaje = "Success";
                correcto = true;
            }

            response = new Response<List<RolItem>>(correcto, mensaje, list);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //GET /Administrador/NuevoRol
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_Roles)]
        public ActionResult NuevoRol()
        {
            return PartialView();
        }

        //POST /Administrador/NuevoRol
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoRol(Roles model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }

            Response<Roles> response;
            string mensaje = "Error al guardar el registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                Roles rol = new Roles();
                rol.rol = model.rol;

                db.Roles.Add(rol);
                db.SaveChanges();

                mensaje = "Se guardo el registro";
                correcto = true;
            }

            response = new Response<Roles>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //GET /Administrador/EditarRol/5
        [PermisoAttribute(Permiso = RolesPermisos.Gestionar_Roles)]
        public ActionResult EditarRol(int Id)
        {
            Roles model = new Roles();

            using (var db = new AppDBContext())
            {
                //traemos el objeto con el Id
                var rol = db.Roles.Find(Id);

                //Llenamos el model
                model.idRol = rol.idRol;
                model.rol = rol.rol;
            }

            return PartialView(model);
        }

        // POST: /Administrador/EditarRol
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarRol(Roles model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }

            Response<Roles> response;
            string mensaje = "Error al editar el registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                var rol = db.Roles.Find(model.idRol);
                if (rol != null)
                {
                    rol.rol = model.rol;

                    //Indicamos el Entity que se edito
                    db.Entry(rol).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    mensaje = "Se guardo el registro";
                    correcto = true;
                }
            }

            response = new Response<Roles>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // DELETE: /Administrador/EliminarRol/5
        [HttpDelete]
        public ActionResult EliminarRol(int Id)
        {
            Response<Roles> response;
            string mensaje = "Error al eliminar el registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                var rol = db.Roles.Find(Id);

                if (rol != null)
                {
                    db.Roles.Remove(rol);
                    db.Entry(rol).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();

                    mensaje = "Se elimino el archivo";
                    correcto = true;
                }
            }

            response = new Response<Roles>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Permisos(int Id)
        {
            
            using (AppDBContext db = new AppDBContext())
            {
                var oRol = db.Roles.Find(Id);
                string rol = oRol.rol;

                

                ViewData["idRol"] = Id;
                ViewData["Rol"] = rol;
            }

            return View();
        }

        [HttpPost]
        public ActionResult SavePermisos(int idRol, int permisoId)
        {
            Response<PermisoDenegado> response;
            string mensaje = "Error al actualizar registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {

                PermisoDenegado pd = new PermisoDenegado();
                //var opd = db.PermisoDenegado.Find(permisoId);

                //si ya existe no hacemos nada
                var oPd = (from _pd in db.PermisoDenegado
                           where _pd.idRol == idRol && _pd.idPermiso == permisoId
                           select _pd).FirstOrDefault();

                if (oPd == null)
                {
                    pd.idRol = idRol;
                    pd.idPermiso = permisoId;

                    db.PermisoDenegado.Add(pd);
                    db.SaveChanges();
                }
                

                //db.Entry(pd).State = System.Data.Entity.EntityState.Modified;
                
                mensaje = "Permisos actualizados";
                correcto = true;

            }

            response = new Response<PermisoDenegado>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public ActionResult RemovePermiso(int idRol, int permisoId)
        {
            Response<PermisoDenegado> response;
            string mensaje = "Error al actualizar registro";
            bool correcto = false;

            using (var db = new AppDBContext())
            {
                //Comprobamos si esta el registro
                var oPd = (from pd in db.PermisoDenegado
                           where pd.idRol == idRol
                           && pd.idPermiso == permisoId
                           select pd).FirstOrDefault();

                if (oPd != null)
                {
                    db.PermisoDenegado.Remove(oPd);
                    db.Entry(oPd).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                
                    mensaje = "Permisos actualizados";
                    correcto = true;
            }

            response = new Response<PermisoDenegado>(correcto, mensaje);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPermisos()
        {
            List<PermisosItem> list;
            Response<List<PermisosItem>> response;
            string mensaje = "Error al obtener datos";
            bool correcto = false;

            using (AppDBContext db = new AppDBContext())
            {
                

                var obj = db.Permisos.
                    Select(p => new PermisosItem
                    {
                        idPermiso = p.idPermiso,
                        modulo = p.modulo,
                        permiso = p.descripcion
                    }).ToList();

                list = obj;

                mensaje = "Success";
                correcto = true;
            }

            response = new Response<List<PermisosItem>>(correcto, mensaje, list);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //Obtener los permisos denegados por Rol
        [HttpPost]
        public ActionResult GetPermisosD(int Id)
        {
           
            AppDBContext db = new AppDBContext();

                var opd = (from pd in db.PermisoDenegado
                           where pd.idRol == Id
                           select pd.idPermiso).ToList();

            return Json(opd, JsonRequestBehavior.AllowGet);
        }

    }
}