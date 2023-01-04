using Client.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class EncuestaService
    {
        HttpClient cliente = new HttpClient();
        public EncuestaService()
        {
            cliente.BaseAddress = new Uri("https://2be8-189-159-255-77.ngrok.io/votacion");
        }
        private string pregunta = "";
        public void EstablecerPregunta(Pregunta p)
        {
            p.Pregunta1 = "califica su experiencia en general";
            pregunta = JsonConvert.SerializeObject(p);
        }
        public async Task Votar(int opcion)
        {
            var response = await cliente.GetAsync("responder/?voto=" + opcion);
            response.EnsureSuccessStatusCode();
        }
        public async Task VotarPost(Voto v)
        {
            var json = JsonConvert.SerializeObject(v);
            var response = await cliente.PostAsync("responder", new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

    }
}


