using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Items
{
    public class DocumentoItem
    {
        public int IdDocumento { get; set; }
        public string NombreDocumentos { get; set; }
        public string RutaDocumento { get; set; }
        public int IdUsuario { get; set; }
        public int? IdTipoDocumento { get; set; }
        public int IdFlujoDocumento { get; set; }
        public int IdCarpeta { get; set; }
        public string TipoDocumento { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        /*public List<HistoricoItem> Historico { get; set; }

        public FlujoDocumentoItem FlujoDocumento { get; set; }

        public TipoDocumentoItem TipoDocumento { get; set; }

        public Usuarios UsuariosItem { get; set; }*/
    }
}
