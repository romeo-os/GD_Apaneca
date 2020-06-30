namespace GestionDocumentalAdmin.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Procedimientos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Procedimientos()
        {
            FlujoDocumento = new HashSet<FlujoDocumento>();
            Historico = new HashSet<Historico>();
        }

        [Key]
        public int idProcedimiento { get; set; }

        [StringLength(50)]
        public string procedimiento { get; set; }

        [StringLength(50)]
        public string duracion { get; set; }

        [StringLength(50)]
        public string prioridad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FlujoDocumento> FlujoDocumento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Historico> Historico { get; set; }
    }
}
