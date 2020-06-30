using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Models.ViewModels
{
    public class RolesViewModel
    {
        public int Irol { get; set; }
        public string Rol { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public int IdRolUsuario { get; set; }
    }
}