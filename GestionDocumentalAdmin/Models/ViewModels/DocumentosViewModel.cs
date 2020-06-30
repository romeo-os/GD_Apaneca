using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestionDocumentalAdmin.Models.ViewModels
{
    public class DocumentosViewModel
    {
        public int IdDocumento { get; set; }
        [Required]
        public string NombreDoc { get; set; }
        //[Required] no se puede validar ya que no se trae en el modelo
        public string RutaDoc { get; set; }
        public string Create_at { get; set; }
        public string Updated_at { get; set; }
        public string Deleted_at { get; set; }
        public int IdUsuario { get; set; }
        [Required]
        public int IdTipoDoc { get; set; }
        public int IdFlujoDoc { get; set; }
        public int Idcarpeta { get; set; }
    }
    
    public class EditarDocumentosViewModel
    {
        public int IdDocumento { get; set; }
        public string NombreDoc { get; set; }
        public string RutaDoc { get; set; }
        public string Create_at { get; set; }
        public string Updated_at { get; set; }
        public string Deleted_at { get; set; }
        public int IdUsuario { get; set; }
        public int? IdTipoDoc { get; set; }
        public int IdFlujoDoc { get; set; }
        public int Idcarpeta { get; set; }
    }
}