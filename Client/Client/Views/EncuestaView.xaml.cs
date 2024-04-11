using Client.Model;
using Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EncuestaView : ContentPage
    {
        public EncuestaView()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            EncuestaViewModel vm = (EncuestaViewModel)this.BindingContext;
            Button button = sender as Button;
            object[] parametros = button.CommandParameter as object[];
            if (vm != null)
            {
                Respuesta respuesta = new Respuesta()
                {
                    Id = (int)parametros[0],
                    Valor = (int)parametros[1]
                };

                vm.ChangeValorCommand?.Execute(respuesta);
            }
        }
    }
}