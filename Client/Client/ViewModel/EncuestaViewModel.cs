using Client.Model;
using Client.Services;
using Client.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Client.ViewModel
{
    public class EncuestaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        EncuestaService client = new EncuestaService();
        public List<Pregunta> preguntas { get; set; } = new List<Pregunta>();

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
        public ICommand ChangeValorCommand { get; set; }
        public ICommand IniciarCommand { get; set; }
        public ICommand PantallaFinalCommand { get; set; }
        public ICommand RegresarCommand { get; set; }
        public bool Votado1Enabled { get; set; } = true;
        public bool Votado2Enabled { get; set; } = true;
        public bool Votado3Enabled { get; set; } = true;
        public bool VotadoEnabled { get; set; } = true;

        public string Pregunta1 { get; set; }
        public string Pregunta2 { get; set; }
        public string Pregunta3 { get; set; }

        //constructor
        public EncuestaViewModel()
        {
            VotarCommand = new Command<Respuesta>(Votar);

            IniciarCommand = new Command(VistaInicial);
            PantallaFinalCommand = new Command(PantallaFinal);
            RegresarCommand = new Command(Regresar);
            ChangeValorCommand = new Command<Respuesta>(ChangeValor);
        }

        private void ChangeValor(Respuesta res)
        {
            try
            {
                if (res != null && preguntas != null)
                {
                    foreach (var p in preguntas)
                    {
                        if (p.Id == res.Id)
                        {
                            p.Respuesta = res.Valor;

                            switch (p.Id)
                            {
                                case 1: Votado1Enabled = false; break;
                                case 2: Votado2Enabled = false; break;
                                case 3: Votado3Enabled = false; break;
                                default:
                                    break;
                            }
                        }
                    }

                    Lanzar();
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Lanzar(nameof(Error));
            }
        }

        //metodos

        private async void Votar(Respuesta res)
        {
            try
            {
                if (res != null && !preguntas.Any(x => x.Respuesta == 0))
                {
                    List<Pregunta> p = new List<Pregunta>(preguntas);
                    if (await client.VotarPost(p))
                        VotadoEnabled = false;

                    Lanzar(nameof(VotadoEnabled));
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Lanzar(nameof(Error));
            }
        }
        // SOLO NAVEGACIÓN //
        public async void VistaInicial()
        {
            encuestaview = new EncuestaView() { BindingContext = this };
            preguntas = await client.GetPregunta();
            foreach (var p in preguntas)
            {
                switch (p.Id)
                {
                    case 1: Pregunta1 = p._Pregunta; break;
                    case 2: Pregunta2 = p._Pregunta; break;
                    case 3: Pregunta3 = p._Pregunta; break;
                    default:
                        break;
                }
            }
           await Application.Current.MainPage.Navigation.PushAsync(encuestaview);
            Lanzar();
        }
        public void Regresar()
        {
            InicioView = new InicioView() { BindingContext = this };
            Application.Current.MainPage.Navigation.PushAsync(InicioView);
        }
        public async void PantallaFinal()
        {
            pantallafinalview = new FinalizarView() { BindingContext = this };
            //await client.VotarPost(preguntas);
            if (await client.VotarPost(preguntas))
                VotadoEnabled = false;
            Lanzar();
            await Application.Current.MainPage.Navigation.PushAsync(pantallafinalview);
        }

        //actualizar
        public void Lanzar(string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
