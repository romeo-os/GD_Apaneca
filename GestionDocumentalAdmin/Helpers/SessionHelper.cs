using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using GestionDocumentalAdmin.Models.Models;
using GestionDocumentalAdmin.Items;

namespace GestionDocumentalAdmin.Helpers
{
    public class SessionHelper
    {
        public static bool ExistUserInSession()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public static void DestroyUserSession()
        {
            FormsAuthentication.SignOut();
        }

        public static int GetUserId()
        {
            int user_id = 0;
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                if (ticket != null)
                {
                    user_id = Convert.ToInt32(ticket.UserData);
                }
            }
            return user_id;
        }

        public static UsuarioItem GetUser()
        {
            int user_id = 0;
            UsuarioItem usuario = null;

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                if (ticket != null)
                {
                    user_id = Convert.ToInt32(ticket.UserData);
                }
            }

            using (AppDBContext db = new AppDBContext())
            {
                try
                {
                    var objUser = db.Usuarios
                    .Where(u => u.deleted_at == null && u.idUsuario.Equals(user_id))
                    .Select(u => new UsuarioItem
                    {
                        idUsuario = u.idUsuario,
                        nombre = u.nombre,
                        apellido = u.apellido,
                        usuario = u.usuario,
                        email = u.email,
                        rol = u.Roles.rol
                    }).FirstOrDefault();

                    usuario = objUser;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            return usuario;
        }

        public static void AddUserToSession(string id)
        {
            bool persist = true;
            HttpCookie cookie = FormsAuthentication.GetAuthCookie("usuario", persist);

            cookie.Name = FormsAuthentication.FormsCookieName;
            cookie.Expires = DateTime.Now.AddMonths(3);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, id);

            cookie.Value = FormsAuthentication.Encrypt(newTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}