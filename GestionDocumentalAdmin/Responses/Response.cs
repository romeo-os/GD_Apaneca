using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionDocumentalAdmin.Responses
{
    /**
     * Clase generica para devolver en un solo formato la data, el JSON que se devuelve siempre 
     * tomará la estructura de esta clase, eso sirve para manejar de una sola forma las validaciones en el frontend
     */
    public class Response<T>
    {
        public bool success;
        public String message;
        public T data;

        public Response(bool success, String message, T data)
        {
            this.success = success;
            this.message = message;
            this.data = data;
        }

        public Response(bool success, String message)
        {
            this.success = success;
            this.message = message;
        }
    }
}