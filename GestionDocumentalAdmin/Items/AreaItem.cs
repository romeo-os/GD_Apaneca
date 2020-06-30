using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Items
{
    public class AreaItem
    {
        public int IdArea { get; set; }
        public string Nombre { get; set; }
        public string Funcion { get; set; }
        public string Estado { get; set; }
        //public int? IdUsuario { get; set; }
        public string Encargado { get; set; }
    }
}