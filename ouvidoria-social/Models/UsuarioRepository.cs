using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ouvidoria_social.Models.Utils;
using Ouvidoria_Social_DataAccess;

namespace ouvidoria_social.Models
{
    public class UsuarioRepository
    {

        public static Usuario AutenticaUsuario(Usuario usarioLogado)
        {
            var model = ConnectionDbClass.DataModel;

            var usuarios = model.Usuarios.Where(s => (s.Login == usarioLogado.Login || s.Pessoa.Email == usarioLogado.Login)).ToList();

            var usuario = usuarios.FirstOrDefault(user => Criptografia.Compara(usarioLogado.Senha, user.Senha));

            if (usuario == null)
                return null;

            //Criando um objeto cookie
            HttpCookie UserCookie = new HttpCookie("UserCookieAuthentication");

            //Setando o ID do usuário no cookie
            UserCookie.Value = usuario.Login;

            //Definindo o prazo de vida do cookie
            UserCookie.Expires = DateTime.Now.AddDays(1);

            //Adicionando o cookie no contexto da aplicação
            HttpContext.Current.Response.Cookies.Add(UserCookie);

            HttpContext.Current.Session["Usuario"] = usuario;
            HttpContext.Current.Session["RankingAcesso"] = true;

            var acesso = new Log_acesso()
            {
                Datahora = DateTime.Now,
                Usuario = usuario
            };

            model.Add(acesso);
            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            DesafiosRepository.GerarDesafiosIniciais(usuario.Pessoa);

            HttpContext.Current.Session["ControlarGamificacao"] = usuario.Gamify.Value;

            return usuario;
        }

        public static void CadastrarUsuario(Usuario usuario)
        {
            var model = ConnectionDbClass.DataModel;

            try
            {
                model.Add(usuario);
                if (model.HasChanges)
                {
                    model.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        public static Usuario GetUsuarioLogado()
        {
            try
            {
                var login = HttpContext.Current.Request.Cookies["UserCookieAuthentication"];

                if (login == null)
                {
                    return null;
                }
                else
                {
                    Usuario usuario = null;

                    if (HttpContext.Current.Session["Usuario"] == null)
                    {
                        var model = ConnectionDbClass.DataModel;

                        usuario =
                            model.Usuarios.SingleOrDefault(
                                s => s.Login == login.Value.ToString() || s.Pessoa.Email == login.Value.ToString());

                        if (usuario != null)
                        {
                            HttpContext.Current.Session["Usuario"] = usuario;
                        }
                    }
                    else
                    {
                        usuario = (Usuario) HttpContext.Current.Session["Usuario"];
                    }

                    HttpContext.Current.Session["ControlarGamificacao"] = usuario.Gamify.Value;

                    return usuario;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void Deslogar()
        {
            var cookie = HttpContext.Current.Request.Cookies["UserCookieAuthentication"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            HttpContext.Current.Session["Usuario"] = null;
            HttpContext.Current.Session.Remove("Usuario");
            HttpContext.Current.Session["NivelUsuarioAtual"] = null;
            HttpContext.Current.Session.Remove("NivelUsuarioAtual");
            HttpContext.Current.Session["ControlarGamificacao"] = null;
            HttpContext.Current.Session.Remove("ControlarGamificacao");
            HttpContext.Current.Session["ReclamacaoFiltro"] = null;
            HttpContext.Current.Session.Remove("ReclamacaoFiltro");
            HttpContext.Current.Session["RankingAcesso"] = null;
            HttpContext.Current.Session.Remove("RankingAcesso");
        }
    }
}