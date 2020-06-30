using GestionDocumentalAdmin.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Items
{
    public class UsuarioItem
    {
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public string email { get; set; }
        public string estado { get; set; }
        public string rol { get; set; }

        /*public virtual ICollection<Area> Area { get; set; }
        public virtual ICollection<Documentos> Documentos { get; set; }
        public virtual ICollection<HistorialLog> HistorialLog { get; set; }
        public virtual ICollection<Historico> Historico { get; set; }*/
    }
}