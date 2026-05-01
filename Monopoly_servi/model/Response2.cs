using System.Text.Json.Serialization;
using System.Diagnostics;

namespace MonopolyService.Models
{
    public class Response2<T>
    {
        [JsonIgnore]
        public bool Status { get; set; }
        public string? UserMssg { get; set; }
        // CAMBIO CLAVE: 'WhenWritingDefault' oculta el campo si es null o false (valor por defecto)
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? Data { get; set; }
        [JsonIgnore]
        public string? ErrorId { get; set; }
        public int StatusCode { get; set; } // En .NET usamos int para el código HTTP

        [JsonIgnore]
        public string? ErrorMssg { get; set; }

        public Response2()
        {
            Status = true;
            UserMssg = "SUCCESS";
            StatusCode = 200;
        }

        public Response2(int statusCode, string userMssg, bool status)
        {
            StatusCode = statusCode;
            UserMssg = userMssg;
            Status = status;
        }

        public Response2(Exception exception) : this(500, "INTERNAL_SERVER_ERROR", false)
        {
            SetError(exception);
        }

        public string SetError(Exception exception)
        {
            ErrorId = Guid.NewGuid().ToString().Substring(0, 8); // Simula DateUtil.generateId()
            ErrorMssg = $" codError: {ErrorId}";

            try
            {
                var frame = new StackTrace(exception, true).GetFrame(0);
                ErrorMssg += $" | class: {frame?.GetMethod()?.DeclaringType?.Name} " +
                             $" | line: {frame?.GetFileLineNumber()} " +
                             $" | method: {frame?.GetMethod()?.Name} " +
                             $" | error: {exception.Message}";
            }
            catch { ErrorMssg += " | json: -"; }

            return ErrorMssg;
        }

        public Response2(T data)
        {
            StatusCode = 200;
            UserMssg = "SUCCESS";
            Data = data;
            // Si data es un bool y es false, Status es false. Si es objeto y es null, Status es false.
            if (data is bool b)
                Status = b;
            else
                Status = data != null;
        }

        public Response2(Exception exception, int errorIdDb)
        {
            ErrorId = Guid.NewGuid().ToString().Substring(0, 8);
            Status = false;
            ErrorMssg = $" | error: {exception.Message}";

            if (errorIdDb == 50000 || errorIdDb == 50001)
            {
                StatusCode = 401;
                UserMssg = exception.InnerException?.Message ?? exception.Message;
            }
            else
            {
                StatusCode = 500;
                UserMssg = "INTERNAL_SERVER_ERROR";
            }
        }
    }
}
