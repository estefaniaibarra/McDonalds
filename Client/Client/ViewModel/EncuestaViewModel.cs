using Client.Model;
using Client.Services;
using Client.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Client.ViewModel
{
    public class EncuestaViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        EncuestaService client = new EncuestaService();
        //errores
        public string Error = "";
        //vistas
        EncuestaView encuestaview;
        FinalizarView pantallafinalview;
        InicioView InicioView;
        //objeto
        public Pregunta Pregunta { get; set; }
        //comandos
        public ICommand VotarCommand { get; set; }
        public ICommand IniciarCommand { get; set; }
        public ICommand PantallaFinalCommand { get; set; }
        public ICommand RegresarCommand { get; set; }
        public bool Votado { get; set; }
        //actualizar
        public void Lanzar(string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        //constructor
        public EncuestaViewModel()
        {
            VotarCommand = new Command<int>(Votar);
            IniciarCommand = new Command(VistaInicial);
            PantallaFinalCommand = new Command(PantallaFinal);
            RegresarCommand = new Command(Regresar);
        }
        //metodos
        
        private async void Votar(int obj)
        {
            try
            {
                if (Pregunta != null)
                {
                    Voto v = new Voto();
                    v.Opcion = obj;
                    await client.Votar(obj);
                    Votado = true;
                    Lanzar(nameof(Votado));
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Lanzar(nameof(Error));
            }
        }
       // SOLO NAVEGACIÓN //
        public void VistaInicial()
        {
            encuestaview = new EncuestaView() { BindingContext=this};
            Application.Current.MainPage.Navigation.PushAsync(encuestaview);
        }
        public void Regresar()
        {
            InicioView = new InicioView() { BindingContext = this };
            Application.Current.MainPage.Navigation.PushAsync(InicioView);
        }
        public void PantallaFinal()
        {
            pantallafinalview = new FinalizarView() { BindingContext = this };
            Application.Current.MainPage.Navigation.PushAsync(pantallafinalview);
        }


    }
}
