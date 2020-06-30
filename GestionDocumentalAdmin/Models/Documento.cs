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
    
    public partial class Documento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Documento()
        {
            this.Historicoes = new HashSet<Historico>();
        }
    
        public int idDocumento { get; set; }
        public string nombreDocumentos { get; set; }
        public string rutaDocumento { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
        public Nullable<int> idUsuario { get; set; }
        public Nullable<int> idTipoDocumento { get; set; }
        public Nullable<int> idFlujoDocumento { get; set; }
        public Nullable<int> idCarpeta { get; set; }
    
        public virtual Carpeta Carpeta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Historico> Historicoes { get; set; }
        public virtual FlujoDocumento FlujoDocumento { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}