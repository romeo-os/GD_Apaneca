using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestionDocumentalAdmin.Models.ViewModels
{
    public class ProcedimientosViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Procedimiento { get; set; }
        [Required]
        public string Duracion { get; set; }
        [Required]
        public string Prioridad { get; set; }
    }

    public class EditarProcedimientosViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Procedimiento { get; set; }
        [Required]
        public string Duracion { get; set; }
        [Required]
        public string Prioridad { get; set; }
    }
}