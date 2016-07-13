using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWebServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            WebServer ws = new WebServer(SendResponse, "http://localhost:8080/");
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
        }

        public static HttpListenerResponse SendResponse(HttpListenerContext ctx)
        {
            HttpListenerRequest request = ctx.Request;
            HttpListenerResponse response = ctx.Response;

            String endpoint = request.RawUrl;

            if (endpoint.EndsWith("assignment_callback"))
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "application/json";
                response.StatusDescription = "{}";
                return response;
            }
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }
    }
}

