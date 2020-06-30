using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Items
{
    public class ReporteDUpItem
    {
        public int idDocumento { get; set; }
        public string Documento { get; set; }
        public string Fecha { get; set; }
        public string Usuario { get; set; }
    }
}