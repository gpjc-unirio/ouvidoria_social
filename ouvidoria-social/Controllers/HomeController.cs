using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ouvidoria_social.Models;
using ouvidoria_social.Models.Utils;
using Ouvidoria_Social_DataAccess;
using Ouvidoria_Social_DataAccess.Control;
using Telerik.OpenAccess;
using Action = Antlr.Runtime.Misc.Action;

namespace ouvidoria_social.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error(string msg)
        {
            ViewBag.Erro = msg;

            return View();
        }

        public ActionResult NotificacoesPerfil(int idPessoa)
        {
            return RedirectToAction("PerfilNotificacao", "Usuario", new {id = idPessoa});
        }

        public int MarcarNotificacoesLidasAjax(int idPessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logs = model.Log_participapaos.Where(w => w.Id_pessoa == idPessoa).ToList();

            logs.ForEach(f => f.Visto = true);

            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            return _QtdeNotificacoes();
        }

        public ActionResult MarcarNotificacoesLidas(int idPessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logs = model.Log_participapaos.Where(w => w.Id_pessoa == idPessoa).ToList();

            logs.ForEach(f => f.Visto = true);

            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            return RedirectToAction("Perfil", "Usuario", new {Id = idPessoa});
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            try
            {
                if (string.IsNullOrEmpty(viewName))
                    viewName = ControllerContext.RouteData.GetRequiredString("action");

                if (model != null)
                    ViewData.Model = model;

                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return string.Empty;
        }

        public ActionResult ListaNotificacoes(int idPessoa)
        {
            var model = ConnectionDbClass.DataModel;

            model.Refresh(RefreshMode.OverwriteChangesFromStore, model.Log_participapaos);
            model.Refresh(RefreshMode.OverwriteChangesFromStore, model.Pessoas);

            var logs = model.Log_participapaos.Where(w => w.Id_pessoa == idPessoa && w.Visto == false).ToList();
            logs = logs.OrderByDescending(o => o.Datahora).Take(5).ToList();

            var content = RenderPartialViewToString("_NotificacoesPopup", logs).Trim().Replace('\n', ' ').Replace('\r', ' ').Replace("&lt;", "<").
                                                                                       Replace("&gt;", ">").Replace("&#39;", "'");

            return Content(content, "text/html");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public int _TimePartialViewAjax()
        {
            return ++ContadorModel.Cont;
        }

        public string _VerificarNivel()
        {
            if (!TipoAmbienteCtrl.ControlarGamificacao)
            {
                return "";
            }

            var model = ConnectionDbClass.DataModel;
            Usuario usuario = null;

            if (Session["NivelUsuarioAtual"] == null)
            {
                usuario = (Usuario) Session["Usuario"];
                if (usuario != null)
                {
                    var nivel = model.Pessoa_nivel_pontos.First(f => f.Id_pessoa == usuario.Id_pessoa);
                    Session["NivelUsuarioAtual"] = nivel.Nivel;
                }
            }

            if (Session["Usuario"] != null)
            {
                var nivel = (int?) Session["NivelUsuarioAtual"];
                usuario = (Usuario) Session["Usuario"];

                ConnectionDbClass.DataModel.Refresh(RefreshMode.OverwriteChangesFromStore, ConnectionDbClass.DataModel.Pessoas);
                ConnectionDbClass.DataModel.Refresh(RefreshMode.OverwriteChangesFromStore, ConnectionDbClass.DataModel.Pessoa_nivel_pontos);

                if (PontuacaoUtils.AvancarNivel(usuario.Pessoa))
                {
                    var nivelAtual = model.Pessoa_nivel_pontos.First(f => f.Id_pessoa == usuario.Id_pessoa);

                    nivelAtual.Nivel++;

                    if (model.HasChanges)
                    {
                        model.SaveChanges();
                    }

                    var proxNivel = PontuacaoUtils.PontosProximoNivel(usuario.Pessoa);
                    var difPontos = proxNivel - nivelAtual.Pontos;

                    var str = "<h2>Parabéns " + usuario.Pessoa.Nome + "</h2><br>" +
                                "<p>Você avanço do nível " + nivel.Value + " para o nível " + nivelAtual.Nivel +
                                " com " + nivelAtual.Pontos + "</p>" +
                                "<p>Para o o nível " + (nivelAtual.Nivel) + " você irá precisar de " + difPontos +
                                " pontos!</p>" +
                                "<p><strong>BOA SORTE!</strong></p>";

                    return str;
                }
            }


            return "";
        }

        public int _QtdeNotificacoes()
        {
            

            int qtde = 0;

            if (Session["Usuario"] != null)
            {
                var usuario = (Usuario) Session["Usuario"];

                qtde = this.Notiticacoes(usuario.Id_pessoa).Count;
            }

            return qtde;
        }

        private List<Log_participapao> Notiticacoes(int? idPessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var qtde = model.Log_participapaos.Where(w => w.Id_pessoa == idPessoa && w.Visto.Value == false).ToList();

            return qtde;
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/Uploads"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }
    }
}