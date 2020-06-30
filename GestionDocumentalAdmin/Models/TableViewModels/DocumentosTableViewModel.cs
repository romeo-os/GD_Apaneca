using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Models.TableViewModels
{
    public class DocumentosTableViewModel
    {
        public int IdDocumento { get; set; }
        public string NombreDoc { get; set; }
        public string RutaDoc { get; set; }
        public string Create_at { get; set; }
        public string Updated_at { get; set; }
        public string Deleted_at { get; set; }
        public int IdUsuario { get; set; }
        public string TipoDoc { get; set; }  //IdTipoDoc
        public int IdFlujoDoc { get; set; }
        public int Idcarpeta { get; set; }
    }
}