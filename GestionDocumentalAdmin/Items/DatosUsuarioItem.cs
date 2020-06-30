using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Items
{
    public class DatosUsuarioItem
    {

        public int idUsuario { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public string nuevaClave { get; set; }
        public string confirClave { get; set; }
    }
}