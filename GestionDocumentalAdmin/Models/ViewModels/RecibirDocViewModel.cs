using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Models.ViewModels
{
    //Los documentos que se van a gestionar no seran alamacenados FISICAMENTE al sistema
    //Solo se ingresara informacion referente a ellos. para gestionarlos

    public class RecibirDocViewModel
    {
        public int IdDocumento { get; set; }
        public string NombreDoc { get; set; }
        public string RutaDoc { get; set; }     //no se ingresara porque no es doc físico
        public string Create_at { get; set; }   
        public string Updated_at { get; set; }  //no se ingresara porque no es doc físico
        public string Deleted_at { get; set; }  //no se ingresara porque no es doc físico
        public int IdUsuario { get; set; }
        public int IdTipoDoc { get; set; }
        public int IdProceso { get; set; }
        public int IdFlujoDoc { get; set; }   //Se asignara a un flujo
        public int Idcarpeta { get; set; }    //no se ingresara porque no es doc físico

        /*---------- Asignar a un flujo------------*/
        public int IdFlujo { get; set; }
        public int IdProcedimiento { get; set; }
        public int IdArea { get; set; }

    }
}