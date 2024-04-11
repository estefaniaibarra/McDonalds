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
        List<Pregunta> preguntas = new List<Pregunta>();

        public EncuestaService()
        {
            cliente.BaseAddress = new Uri("https://cab1-2806-108e-21-f1d1-d5b2-a1b4-e5b9-201c.ngrok-free.app/votacion/");
        }
        private string pregunta = "";
        public void EstablecerPregunta(Pregunta p)
        {
            //p.Pregunta1 = "califica su experiencia en general";
            pregunta = JsonConvert.SerializeObject(p);
        }

        public async Task<List<Pregunta>> GetPregunta()
        {
            var responseMessage = cliente.GetAsync("pregunta");
            responseMessage.Wait();
            if (responseMessage.Result.IsSuccessStatusCode)
            {
                var response = await responseMessage.Result.Content.ReadAsStringAsync();
                List<Pregunta> p = JsonConvert.DeserializeObject<List<Pregunta>>(response);
                return p;
            }
            else
            {
                return null;
            }
        }


        public async Task Votar(int opcion)
        {
            var response = await cliente.GetAsync("responder/?voto=" + opcion);
            response.EnsureSuccessStatusCode();
        }
        public async Task<bool> VotarPost(List<Pregunta> preguntas)
        {
            var json = JsonConvert.SerializeObject(preguntas);
            var response = await cliente.PostAsync("responder", new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return true;
        }


    }
}


