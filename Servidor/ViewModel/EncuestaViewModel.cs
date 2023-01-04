using Servidor.Models;
using Servidor.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Servidor.ViewModel
{
    public class EncuestaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Pregunta? Pregunta { get; set; }
        public string Error { get; set; } = "";
        //comandos
        public ICommand IniciarCommand { get; set; }
        //votos
        public int Voto1 { get; set; }
        public int Voto2 { get; set; }
        public int Voto3 { get; set; }
        public int Voto4 { get; set; }
        public int Voto5 { get; set; }
        public int Total => Voto1 + Voto2 + Voto3+Voto4+Voto5;
        VotacionService server = new ();
        //metodos
        public void Actualizar(string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void Server_VotoRecibido(int obj)
        {
            switch (obj)
            {
                case 1: Voto1++; break;
                case 2: Voto2++; break;
                case 3: Voto3++; break;
                case 4: Voto4++; break;
                case 5: Voto5++; break;
            }
            Actualizar();
        }


    }
}
