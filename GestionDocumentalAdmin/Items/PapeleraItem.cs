using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Items
{
    public class PapeleraItem
    {
        public int? IdCarpeta { get; set; }
        public int IdDocumento { get; set; }
        public string RutaCarpeta { get; set; }
        public string RutaDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public string Descripcion { get; set; }
        public string FechaE { get; set; }
    }
}