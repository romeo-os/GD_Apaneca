using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace GestionDocumentalAdmin.Models.ViewModels
{
    public class CrearCarpetasViewModel
    {
       public int IdCarpeta { get; set; }
        //[Required]
       public string RutaCarpeta { get; set; }
        [Required]
       public string Nombre { get; set; }
        [Required]
       public string Descripcion { get; set; }
       public string Create_at { get; set; }
       public string Updated_at { get; set; }
       public string Deleted_at { get; set; }
    }

    public class EditarCarpetasViewModel
    {
        public int IdCarpeta { get; set; }
        //[Required]
        public string RutaCarpeta { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public string Create_at { get; set; }
        public string Updated_at { get; set; }
        public string Deleted_at { get; set; }
    }
}