namespace GestionDocumentalAdmin.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FlujoDocumento")]
    public partial class FlujoDocumento
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FlujoDocumento()
        {
            Documentos = new HashSet<Documentos>();
        }

        [Key]
        public int idFlujoDocumento { get; set; }

        public int idDocumento { get; set; }

        public int? idProcedimiento { get; set; }

        public int? idArea { get; set; }

        public virtual Area Area { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentos> Documentos { get; set; }

        public virtual Procedimientos Procedimientos { get; set; }
    }
}
