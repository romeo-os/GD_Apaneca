namespace GestionDocumentalAdmin.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Historico")]
    public partial class Historico
    {
        [Key]
        public int idHistorico { get; set; }

        public int? idDocumento { get; set; }

        public int? idProcedimiento { get; set; }

        public DateTime? fechaInicio { get; set; }

        public DateTime? fechaFin { get; set; }

        public int? idUsuario { get; set; }

        public virtual Documentos Documentos { get; set; }

        public virtual Procedimientos Procedimientos { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
