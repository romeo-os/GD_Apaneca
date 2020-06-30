using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Models.TableViewModels
{
    public class CarpetasTableViewModel
    {
        public int IdCarpeta { get; set; }
        public string RutaCarpeta { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Create_at { get; set; }
        public string Updated_at { get; set; }
        public string Deleted_at { get; set; }
    }
}