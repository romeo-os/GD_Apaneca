using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestionDocumentalAdmin.Models.ViewModels
{
    public class TiposDocViewModel
    {
        public int Id { get; set; }
        [Required]
        public string TipoDoc { get; set; }
        [Required]
        public string DescripcionT { get; set; }
    }

    public class EditarTiposDocViewModel
    {
        public int Id { get; set; }
        [Required]
        public string TipoDoc { get; set; }
        [Required]
        public string DescripcionT { get; set; }
    }
}