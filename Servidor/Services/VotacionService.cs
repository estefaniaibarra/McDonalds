using Newtonsoft.Json;
using Servidor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Servidor.Services
{
    public class VotacionService
    {
        public event Action<List<Pregunta>>? VotoRecibido;
        HttpListener listener = new();
        List<Pregunta> preguntas = new();

        public VotacionService()
        {
            listener.Prefixes.Add("http://127.0.0.1:4322/votacion/");
            EstablecerPreguntas();
        }

        private void EstablecerPreguntas()
        {
            Pregunta pregunta1 = new Pregunta() 
            { 
                 Id = 1,
                _Pregunta = "¿Estás conforme con la atención del personal?"
            };

            Pregunta pregunta2 = new Pregunta()
            {
                Id = 2,
                _Pregunta = "¿Nuestro local cumple con los estándares de higiene?"
            };

            Pregunta pregunta3 = new Pregunta()
            {
                Id = 3,
                _Pregunta = "Califique la calidad en general del servicio que recibió"
            };

            preguntas.Add(pregunta1);
            preguntas.Add(pregunta2);
            preguntas.Add(pregunta3);
        }

        public void Iniciar()
        {
            if (!listener.IsListening)
            {
                listener.Start();
                listener.BeginGetContext(ContextoRecibido, null);
            }
        }
        private string pregunta = "";
        private void ContextoRecibido(IAsyncResult ar)
        {
            var context = listener.EndGetContext(ar);
            listener.BeginGetContext(ContextoRecibido, null);

            if (context.Request.Url != null)
            {
                if (context.Request.Url.LocalPath == "/votacion/pregunta")
                {
                    var json = JsonConvert.SerializeObject(preguntas);
                    byte[] buffer = Encoding.UTF8.GetBytes(json);
                    context.Response.ContentType = "application/json"; 
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    context.Response.StatusCode = 200;
                    context.Response.Close();
                }
                else if (context.Request.HttpMethod == "POST" && (context.Request.Url.LocalPath == "/votacion/responder"))
                {
                    var stream = new StreamReader(context.Request.InputStream);
                    var json = stream.ReadToEnd();
                    List<Pregunta>? preguntas = JsonConvert.DeserializeObject<List<Pregunta>>(json);
                    //Voto? voto = JsonConvert.DeserializeObject<Voto>(json);
                    context.Response.StatusCode = 200;
                    if (preguntas != null)
                    {
                        VotoRecibido?.Invoke(preguntas);
                        //Lanzamos al viewmodel
                    }

                    context.Response.Close();
                }
                //else if (context.Request.Url.LocalPath == "/votacion/responder")
                //{
                //    if (context.Request.QueryString["voto"] != null)
                //    {

                //        int voto = int.Parse(context.Request.QueryString["voto"]);
                //        context.Response.StatusCode = 200;
                //        VotoRecibido?.Invoke(voto);
                //        context.Response.Close();
                //    }
                //    else
                //    {
                //        context.Response.StatusCode = 400;
                //        context.Response.Close();
                //    }
                //}
                //else
                //{
                //    context.Response.StatusCode = 404;
                //    context.Response.Close();
                //}

            }
        }
    }
}
