using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ouvidoria_social.Models;
using ouvidoria_social.Models.Adapter;
using ouvidoria_social.Models.Enum;
using ouvidoria_social.Models.Utils;
using Ouvidoria_Social_DataAccess;

namespace ouvidoria_social.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Perfil(int Id)
        {
            var model = ConnectionDbClass.DataModel;

            var pessoa = model.Pessoas.SingleOrDefault(s => s.Id == Id);

            return View(pessoa);
        }

        public ActionResult PerfilInstituicao(int Id)
        {
            var model = ConnectionDbClass.DataModel;

            var pessoa = model.Pessoas.SingleOrDefault(s => s.Id == Id);
            
            return View("Perfil", pessoa);
        }

        public ActionResult PerfilNotificacao(int Id)
        {
            var model = ConnectionDbClass.DataModel;

            var pessoa = model.Pessoas.SingleOrDefault(s => s.Id == Id);

            return View(pessoa);
        }

        public ActionResult ReativarPerfil(int Id)
        {
            var model = ConnectionDbClass.DataModel;

            var pessoa = model.Pessoas.FirstOrDefault(f => f.Id == Id);

            pessoa.IsAtivo = 'S';

            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            UsuarioRepository.Deslogar();
            return RedirectToAction("Index", "Autenticacao");
        }

        public ActionResult DesativarPerfil(int Id)
        {
            var model = ConnectionDbClass.DataModel;

            var pessoa = model.Pessoas.FirstOrDefault(f => f.Id == Id);

            pessoa.IsAtivo = 'N';

            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            UsuarioRepository.Deslogar();
            return RedirectToAction("Logout", "Autenticacao");
        }

        [HttpPost]
        public ActionResult AlterarImagem(HttpPostedFileBase file)
        {
            var usuario = (Usuario) Session["Usuario"];

            if (file.ContentLength > 1048576)
            {
                ViewBag.Erro = "O Arquivo de imagem ultrapassa o limite de 1Mb.";
                return View("Perfil", usuario.Pessoa);
            }

            if (file.ContentLength > 0)
            {
            
                var fileName = usuario.Pessoa.Avatar;
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = usuario.Login.ToLower() + "_" + usuario.Pessoa.Id + "_" + DateTime.Now.ToString("yyyyMd") + ".jpg";

                    if (string.IsNullOrEmpty(usuario.Pessoa.Avatar))
                    {
                        usuario.Pessoa.Avatar = fileName;
                    }
                    
                    var model = ConnectionDbClass.DataModel;

                    if (model.HasChanges)
                    {
                        model.SaveChanges();
                    }
                }

                var path = Path.Combine(Server.MapPath("~/Images/Uploads/Avatar"), fileName);
                
                file.SaveAs(path);
            }

            return View("Perfil", usuario.Pessoa);
        }

        public ActionResult AlterarPerfil(int Id)
        {
            var model = ConnectionDbClass.DataModel;

            var pessoa = model.Pessoas.SingleOrDefault(s => s.Id == Id);

            return View(pessoa);
        }

        public ActionResult GravarPerfil(Pessoa pessoa)
        {
            var usuario = (Usuario)Session["Usuario"];
            
            var model = ConnectionDbClass.DataModel;

            usuario.Pessoa.Nome = pessoa.Nome;
            usuario.Pessoa.Telefone = pessoa.Telefone;
            usuario.Pessoa.Email = pessoa.Email;
            usuario.Pessoa.Descricao = pessoa.Descricao;
            usuario.Pessoa.Id_cidade = pessoa.Id_cidade;

            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            return View("Perfil", usuario.Pessoa);
        }

        public ActionResult AlterarUsuario(UsuarioAdapter user)
        {
            var usuario = (Usuario)Session["Usuario"];

            usuario.Login = user.Login;
            usuario.Senha = user.Senha;

            var model = ConnectionDbClass.DataModel;

            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            return View("Perfil", usuario.Pessoa);
        }

        public ActionResult IncluirInstituicao(string nome)
        {
            var model = ConnectionDbClass.DataModel;

            var newInst = new Instituicao()
            {
                Nome = nome,
                Nome_fantasia = nome
            };
            
            model.Add(newInst);

            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            var intituicoes = model.Instituicaos.OrderBy(o => o.Nome).ToList();

            var inst = InstituicaoAdapter.Adaptar(intituicoes);

            return Json(inst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult _DesafiosPessoa(int IdPessoa)
        {
            var pessoa = ConnectionDbClass.DataModel.Pessoas.FirstOrDefault(f => f.Id == IdPessoa);
            if (pessoa == null)
            {
                return PartialView("Error");
            }

            if (Session["Usuario"] != null)
            {
                var usuario = (Usuario) Session["Usuario"];

                if (pessoa.Id == usuario.Pessoa.Id)
                {
                    PontuacaoUtils.CalcularDesafios(usuario.Pessoa);
                }
            }

            return PartialView("_DesafiosPessoa", pessoa);
        }

        [HttpPost]
        public PartialViewResult _ReclamacoesUsuario(bool isThisUser)
        {
            if (Session["Usuario"] != null)
            {
                var usuario = (Usuario) Session["Usuario"];

                ViewData["isThisUser"] = isThisUser;
                if (usuario.Pessoa.Instituicao != null)
                {
                    return PartialView("_ReclamacaoInstituicaoView", usuario.Pessoa.Reclamacaos.ToList());
                }
                else
                {
                    return PartialView("_ReclamacoesUsuarioView", usuario.Pessoa.Reclamacaos.ToList());
                }
            }

            return PartialView("Error");
        }

        public ActionResult Ranking()
        {
            ViewData["idTipoRanking"] = 1;

            return View();
        }

        public PartialViewResult AlterarRanking(int id)
        {
            ViewData["idTipoRanking"] = id;
            ViewData["fontSize"] = 0;

            return PartialView("_RankingView");
        }
    }
}