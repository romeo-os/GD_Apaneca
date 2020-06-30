namespace GestionDocumentalAdmin.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Area")]
    public partial class Area
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Area()
        {
            FlujoDocumento = new HashSet<FlujoDocumento>();
        }

        [Key]
        public int idArea { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(100)]
        public string funcion { get; set; }

        public string estado { get; set; }

        public int? idUsuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FlujoDocumento> FlujoDocumento { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
