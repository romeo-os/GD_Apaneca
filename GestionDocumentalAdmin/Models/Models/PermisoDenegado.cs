namespace GestionDocumentalAdmin.Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermisoDenegado")]
    public partial class PermisoDenegado
    {
        [Key]
        public int idPermisoD { get; set; }

        public int? idRol { get; set; }

        public int? idPermiso { get; set; }

        public virtual Roles Roles { get; set; }

        public virtual Permisos Permisos { get; set; }
    }
}
