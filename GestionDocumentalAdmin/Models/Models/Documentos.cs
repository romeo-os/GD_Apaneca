namespace GestionDocumentalAdmin.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Documentos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Documentos()
        {
            Historico = new HashSet<Historico>();
        }

        [Key]
        public int idDocumento { get; set; }

        [StringLength(100)]
        public string nombreDocumentos { get; set; }

        [StringLength(100)]
        public string rutaDocumento { get; set; }

        //[Column(TypeName = "date")]
        public DateTime? created_at { get; set; }

        //[Column(TypeName = "date")]
        public DateTime? updated_at { get; set; }

        //[Column(TypeName = "date")]
        public DateTime? deleted_at { get; set; }

        public int? idUsuario { get; set; }

        public int? idTipoDocumento { get; set; }

        public int? idFlujoDocumento { get; set; }

        public int? idCarpeta { get; set; }

        public virtual Carpeta Carpeta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Historico> Historico { get; set; }

        public virtual FlujoDocumento FlujoDocumento { get; set; }

        public virtual TipoDocumento TipoDocumento { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
