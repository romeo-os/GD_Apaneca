using GestionDocumentalAdmin.Helpers;
using GestionDocumentalAdmin.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Commons
{
    public class FrontUser
    {
        public static bool TienePermiso(RolesPermisos valor)
        {
            AppDBContext db = new AppDBContext();

            var usuarioRol = SessionHelper.GetUser().rol;


            var objRol = (from r in db.Roles
                         where r.rol == usuarioRol
                         select r.idRol).ToList();

            var idRol = objRol[0];

            //var permiso = (int)valor; //Obtener el int del enum

            var obj = (from pd in db.PermisoDenegado
                       where pd.idRol == idRol // idRol del usuario logeado
                       && pd.idPermiso == (int)valor // idPermiso al que desea acceder el usuario
                       select pd).ToList();
            if(obj.Count() > 0)
            {
                //Si existe es falso (No tiene permiso)
                return false;
            } 
            else
            {
                return true;
            }
        } 
       
    }
}