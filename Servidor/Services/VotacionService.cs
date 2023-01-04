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
        public event Action<int>? VotoRecibido;
        HttpListener listener = new();
        public VotacionService()
        {
            listener.Prefixes.Add("http://*:4321/votacion/");
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
                    byte[] buffer = Encoding.UTF8.GetBytes(pregunta);
                    context.Response.ContentType = "application/json"; 
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    context.Response.StatusCode = 200;
                    context.Response.Close();
                }
                else if (context.Request.HttpMethod == "POST" && (context.Request.Url.LocalPath == "/votacion/responder"))
                {
                    var stream = new StreamReader(context.Request.InputStream);
                    var json = stream.ReadToEnd();
                    Voto? voto = JsonConvert.DeserializeObject<Voto>(json);
                    context.Response.StatusCode = 200;
                    if (voto != null)
                    {
                        VotoRecibido?.Invoke(voto.Opcion);
                    }

                    context.Response.Close();
                }
                else if (context.Request.Url.LocalPath == "/votacion/responder")
                {
                    if (context.Request.QueryString["voto"] != null)
                    {

                        int voto = int.Parse(context.Request.QueryString["voto"]);
                        context.Response.StatusCode = 200;
                        VotoRecibido?.Invoke(voto);
                        context.Response.Close();
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.Close();
                }

            }
        }
    }
}
