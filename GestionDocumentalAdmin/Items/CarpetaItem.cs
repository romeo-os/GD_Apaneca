using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Items
{
    public class CarpetaItem
    {
        public int IdCarpeta { get; set; }
        public string RutaCarpeta { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Fecha { get; set; }
        public List<DocumentoItem> Documentos { get; set; }
    }
}