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
    
    public partial class Procedimiento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Procedimiento()
        {
            this.FlujoDocumentoes = new HashSet<FlujoDocumento>();
            this.Historicoes = new HashSet<Historico>();
        }
    
        public int idProcedimiento { get; set; }
        public string procedimiento1 { get; set; }
        public string duracion { get; set; }
        public string prioridad { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FlujoDocumento> FlujoDocumentoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Historico> Historicoes { get; set; }
    }
}
