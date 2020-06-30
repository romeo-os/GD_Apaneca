using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestionDocumentalAdmin.Models.ViewModels
{
    public class AreasViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Funcion { get; set; }
        public int Encargado { get; set; } //Es el idUsuario 
    }

    public class EditarAreasViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Funcion { get; set; }
        public int? Encargado { get; set; } //Es el idUsuario 
    }
}