using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Models.TableViewModels
{
    public class AreasTableViewModel
    {
            public int Id { get; set; }
            [Required]
            public string Nombre { get; set; }
            [Required]
            public string Funcion { get; set; }
            public int? IdUsuario { get; set; }
            public string Encargado { get; set; }
    }
}