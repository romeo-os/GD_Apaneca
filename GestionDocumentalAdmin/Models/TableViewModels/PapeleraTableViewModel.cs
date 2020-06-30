using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Models.TableViewModels
{
    public class PapeleraTableViewModel
    {
        public int IdCarp { get; set; }
        public int IdDoc { get; set; }
        public string Carpeta { get; set; }
        public string Documento { get; set; }
        public string FechaEC { get; set; }
        public string FechaED { get; set; }

        //public List<PapeleraTableViewModelC> papeleraC { get; set; }
        //public List<PapeleraTableViewModelD> papeleraD { get; set; }
    }

    //public class PapeleraTableViewModelC
    //{
    //    public int IdCarp { get; set; }
    //    public string Carpeta { get; set; }
    //    public string FechaEC { get; set; }
    //}

    //public class PapeleraTableViewModelD
    //{
    //    public int IdDoc { get; set; }
    //    public string Documento { get; set; }
    //    public string FechaED { get; set; }
    //}
}