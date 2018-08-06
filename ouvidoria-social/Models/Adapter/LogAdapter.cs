using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ouvidoria_Social_DataAccess;

namespace ouvidoria_social.Models.Adapter
{
    public class LogAdapter
    {
        public static RouteData CurrentRoute(HttpContextWrapper httpContext)
        {
            return RouteTable.Routes.GetRouteData(httpContext);
        }

        public static string Mensagem(Log_participapao log, bool isPopup)
        {
            var str = "";

            if (isPopup)
            {
                str = Mensagem(log);
            }
            else
            {
                var imagem = "";
                var httpContext = new HttpContextWrapper(HttpContext.Current);
                var url = new UrlHelper( new RequestContext(httpContext, CurrentRoute(httpContext)));
                

                if (!string.IsNullOrEmpty(log.Pessoa.Avatar))
                {
                    var urlPessoa = url.Content("~/Images/Uploads/Avatar/" + log.Pessoa.Avatar);

                    imagem = "<a href='\\Usuario\\Perfil?Id=" + log.Pessoa.Id + "'>" +
                             "<img class='img-circle' src='" + urlPessoa + "' width='32' height='32'>" +
                             "</a>";
                }

                str = "<div class='row'><div class='col-md-2'>" + imagem + "</div>" +
                      "<div class='col-md-9'>"+ Mensagem(log) +"</div></div>";
            }

            return str;
        }

        public static string Mensagem(Log_participapao log)
        {
            var str = "";

            var nome = log.Pessoa.Nome;
            if (nome.Length > 5)
            {
                nome = nome.Substring(0, 5) + "...";
            }

            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var url = new UrlHelper(new RequestContext(httpContext, CurrentRoute(httpContext)));

            var icon = "";

            if (log.Tipo.Contains("Descur"))
            {
                icon = "<i class='glyphicon glyphicon-thumbs-down'></i> ";
            }
            else if (log.Tipo.Contains("Curt"))
            {
                icon = "<i class='glyphicon glyphicon-thumbs-up'></i> ";
            }
            else if (log.Tipo.ToLower().Contains("soluc"))
            {
                icon = "<i class='glyphicon glyphicon-check'></i> ";
            }
            else if (log.Tipo.Contains("Acess"))
            {
                icon = "<i class='glyphicon glyphicon-share-alt'></i> ";
            }
            else if (log.Tipo.Contains("Coment"))
            {
                icon = "<i class='glyphicon glyphicon-comment'></i> ";
            }

            if ((log.Tipo == "Descurtiu") || (log.Tipo == "Acessou") || (log.Tipo == "Curtiu") || (log.Tipo == "Solucionou"))
            {
                str = icon +
                      "<a href='" + url.Action("Perfil", "Usuario", new {Id = log.Pessoa.Id }) + "'>" + nome + "</a> " + log.Tipo +
                      " a " +
                      "<a href='" + url.Action("AcessarReclamacao", "Ouvidoria", new {Id = log.Reclamacao.Id }) + "'>Reclamação</a>";
            }
            else
            {
                str = icon +
                      "<a href='" + url.Action("Perfil", "Usuario", new { Id = log.Pessoa.Id }) + "'>" + nome + "</a> " + log.Tipo +
                      " na " +
                      "<a href='" + url.Action("AcessarReclamacao", "Ouvidoria", new { Id = log.Reclamacao.Id }) + "'>Reclamação</a>";
            }

            return str;
        }

    }
}