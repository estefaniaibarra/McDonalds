using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Model
{
    public class Pregunta
    {
        public int Id { get; set; }
        public string _Pregunta { get; set; } = "";
        public int Respuesta { get; set; } // Maximo 5 

        //public string Pregunta1 { get; set; } 

        //public int Voto1 { get; set; }
        //public int Voto2 { get; set; }
        //public int Voto3 { get; set; }
        //public int Voto4 { get; set; }
        //public int Voto5 { get; set; }
    }
}
