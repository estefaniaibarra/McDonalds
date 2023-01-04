using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Pregunta
    {
        public string Pregunta1 { get; set; } = "";

        public string Opcion1 { get; set; } = "";
        public string Opcion2 { get; set; } = "";
        public string Opcion3 { get; set; } = "";
        public string Opcion4 { get; set; } = "";
        public string Opcion5 { get; set; } = "";

        public int Voto1 { get; set; }
        public int Voto2 { get; set; }
        public int Voto3 { get; set; }
    }
}
