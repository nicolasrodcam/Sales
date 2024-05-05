using System.Net;

namespace Sales.WEB.Repositories
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set;}
        public T? Response { get; set;}

        public async Task<string?> GetErrorMessage()
        {
            if (!Error)
            {
                return null;
            }

            var statusCode = HttpResponseMessage.StatusCode;
            if (statusCode == HttpStatusCode.NotFound)
            {
                return "Recurso no Encontrado";
            }
            else if (statusCode == HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            else if (statusCode == HttpStatusCode.Unauthorized)
            {
                return "Tienes que logearte para realizar esta operacion";
            }
            else if (statusCode == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para realizar esta operacion";
            }
            return "Ha ocurrido un error inesperado";
        }
    }
}
