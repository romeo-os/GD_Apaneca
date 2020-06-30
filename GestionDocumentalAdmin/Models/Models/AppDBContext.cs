namespace GestionDocumentalAdmin.Models.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AppDBContext : DbContext
    {
        public AppDBContext()
            : base("name=AppDBConnection")
        {
        }

        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Carpeta> Carpeta { get; set; }
        public virtual DbSet<Documentos> Documentos { get; set; }
        public virtual DbSet<FlujoDocumento> FlujoDocumento { get; set; }
        public virtual DbSet<HistorialLog> HistorialLog { get; set; }
        public virtual DbSet<Historico> Historico { get; set; }
        public virtual DbSet<PermisoDenegado> PermisoDenegado { get; set; }
        public virtual DbSet<Permisos> Permisos { get; set; }
        public virtual DbSet<Procedimientos> Procedimientos { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.funcion)
                .IsUnicode(false);

            modelBuilder.Entity<Carpeta>()
                .Property(e => e.rutaCarpeta)
                .IsUnicode(false);

            modelBuilder.Entity<Carpeta>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Carpeta>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Documentos>()
                .Property(e => e.nombreDocumentos)
                .IsUnicode(false);

            modelBuilder.Entity<Documentos>()
                .Property(e => e.rutaDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Permisos>()
                .Property(e => e.modulo)
                .IsUnicode(false);

            modelBuilder.Entity<Permisos>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Procedimientos>()
                .Property(e => e.procedimiento)
                .IsUnicode(false);

            modelBuilder.Entity<Procedimientos>()
                .Property(e => e.duracion)
                .IsUnicode(false);

            modelBuilder.Entity<Procedimientos>()
                .Property(e => e.prioridad)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .Property(e => e.rol)
                .IsUnicode(false);

            modelBuilder.Entity<TipoDocumento>()
                .Property(e => e.tipoDocumento1)
                .IsUnicode(false);

            modelBuilder.Entity<TipoDocumento>()
                .Property(e => e.descripcionTipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.clave)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.estado)
                .IsUnicode(false);
        }
    }
}
