using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Models.TableViewModels
{
    public class UbicarTableViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Area { get; set; }
        public string Encargado { get; set; }
        public string Proceso { get; set; }
        public string FechaI { get; set; }
        public string FechaS { get; set; }
        public int? Tiempo { get; set; }
    }
}