namespace GestionDocumentalAdmin.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Carpeta")]
    public partial class Carpeta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Carpeta()
        {
            Documentos = new HashSet<Documentos>();
        }

        [Key]
        public int idCarpeta { get; set; }

        [StringLength(100)]
        public string rutaCarpeta { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(100)]
        public string descripcion { get; set; }

        //[Column(TypeName = "date")]
        public DateTime? created_at { get; set; }

        //[Column(TypeName = "date")]
        public DateTime? updated_at { get; set; }

        //[Column(TypeName = "date")]
        public DateTime? deleted_at { get; set; }

        public int idUsuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documentos> Documentos { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
