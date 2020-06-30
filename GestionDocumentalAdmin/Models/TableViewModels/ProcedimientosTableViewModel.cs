using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Models.TableViewModels
{
    public class ProcedimientosTableViewModel
    {
        public int Id { get; set; }
        public string Procedimiento { get; set; }
        public string Duracion { get; set; }
        public string Prioridad { get; set; }
    }
}