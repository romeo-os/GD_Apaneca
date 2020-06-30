namespace GestionDocumentalAdmin.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HistorialLog")]
    public partial class HistorialLog
    {
        [Key]
        public int idHistorialLog { get; set; }

        public TimeSpan? hora { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }

        public int? idUsuario { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
