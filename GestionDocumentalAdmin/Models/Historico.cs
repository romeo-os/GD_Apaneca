//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GestionDocumentalAdmin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Historico
    {
        public int idHistorico { get; set; }
        public Nullable<int> idDocumento { get; set; }
        public Nullable<int> idProcedimiento { get; set; }
        public Nullable<System.DateTime> fechaInicio { get; set; }
        public Nullable<System.DateTime> fechaFin { get; set; }
        public Nullable<int> idUsuario { get; set; }
    
        public virtual Documento Documento { get; set; }
        public virtual Procedimiento Procedimiento { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
