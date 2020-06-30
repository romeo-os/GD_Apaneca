using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Items
{
    public class ProcedimientoItem
    {
        public int IdProcedimiento { get; set; }
        public string Procedimiento { get; set; }
        public string Duracion { get; set; }
        public string Prioridad { get; set; }
    }
}