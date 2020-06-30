using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Commons
{
    public enum RolesPermisos
    {
        #region Administrador
        Gestionar_Usuarios = 1,
        Gestionar_Roles = 2,
        Gestionar_Areas = 3,
        Gestionar_Papelera = 4,
        #endregion

        #region Mantenimientos
        Gestionar_tipos_de_documentos = 5,
        Gestionar_procedimientos = 6,
        #endregion

        #region Reportes
        Gestión_de_reportes = 7,
        #endregion

        #region TimeLine
        Ubicar_documentos = 8,
        Recibir_documentos = 9,
        #endregion

        #region Documentos
        Agregar_documentos = 10,
        Editar_documentos = 11,
        Eliminar_documentos = 12,
        Descargar_documentos = 13
        #endregion
    }
}