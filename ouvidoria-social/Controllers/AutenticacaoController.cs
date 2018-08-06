using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using ouvidoria_social.Models;
using ouvidoria_social.Models.Adapter;
using ouvidoria_social.Models.Enum;
using ouvidoria_social.Models.Utils;
using Ouvidoria_Social_DataAccess;

namespace ouvidoria_social.Controllers
{
    public class AutenticacaoController : Controller
    {
        // GET: Autenticacao
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logar(Usuario usuario)
        {
            var user = UsuarioRepository.AutenticaUsuario(usuario);

            if (user == null)
            {
                ViewBag.Error = "Nome do usuário ou senha inválidos";
                return View("Index");
            }

            if (user.Pessoa.IsAtivo == 'S')
            {
                return RedirectToAction("Perfil", "Usuario", new {Id = user.Pessoa.Id});
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(Usuario usuario)
        {
            UsuarioRepository.CadastrarUsuario(usuario);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            UsuarioRepository.Deslogar();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public PartialViewResult _AddPessoaView()
        {
            return PartialView("_AddPessoaView");
        }

        [HttpPost]
        public PartialViewResult AddPessoaAjax(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            ViewBag.Tipo = 3;

            if (pessoa.Tipo == 'I')
            {
                var inst = model.Instituicaos.FirstOrDefault(w => (w.Nome.ToLower() == pessoa.Nome.ToLower()) || (w.Nome_fantasia.ToLower() == pessoa.Nome.ToLower()));

                if (inst != null)
                {
                    if (inst.Pessoas.Any())
                    {
                        if (inst.Pessoas[0].Usuarios.Count > 0)
                        {
                            ViewBag.Mensagem =
                                "Esta instituição já existe, e possui um usuário associado. Entre em contato com a mesma para obter mais detalhes.";
                            ViewBag.Tipo = 0;
                        }
                        else
                        {
                            ViewBag.Mensagem =
                                "Esta instituição já existe, entretanto não existe usuário associado a mesma, você irá cadastrá-lo.";
                            ViewBag.Tipo = 1;
                        }

                        Session["CadastroPessoa"] = inst.Pessoas[0];
                        return PartialView("_AddUsuario", null);
                    }
                    else
                    {
                        ViewBag.Mensagem =
                            "Esta instituição já existe, entretanto não existe usuário associado a mesma, você irá cadastrá-lo.";
                        ViewBag.Tipo = 1;
                    }

                    inst.Email = pessoa.Email;
                    inst.Telefone = pessoa.Telefone;

                    pessoa.Instituicao = inst;
                }
                else
                {
                    pessoa.Instituicao = new Instituicao()
                    {
                        Nome = pessoa.Nome,
                        Email = pessoa.Email,
                        Telefone = pessoa.Telefone
                    };
                }
            }
            
            Session["CadastroPessoa"] = pessoa;
            return PartialView("_AddUsuario", null);
        }

        [HttpPost]
        public PartialViewResult AddUsuarioAjax(UsuarioAdapter adapter)
        {

            var p = (Pessoa) Session["CadastroPessoa"];
            var model = ConnectionDbClass.DataModel;

            var usuario = adapter.ConvertToUsuario();
            usuario.Gamify = true;
            
            usuario.Pessoa = p;
            p.Usuarios.Add(usuario);

            ConnectionDbClass.DataModel.Add(p);
            
            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            Session["CadastroPessoa"] = null;
            Session.Remove("CadastroPessoa");

            ViewData["Titulo"] = p.Nome;
            ViewData["Msg"] = "Seu Usuário foi criado com sucesso!";
            
            PontuacaoUtils.Pontuar(usuario.Pessoa, PontuacaoEnum.CadastroUsuario);

            return PartialView("_MensagemAlerta");
        }

        public ActionResult VerificaUsuario(string login)
        {
            if (Session["Usuario"] != null)
            {
                var user = (Usuario) Session["Usuario"];

                if (user.Login == login)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }

            var model = ConnectionDbClass.DataModel;

            var usuario = model.Usuarios.Where(w => w.Login == login).ToList();

            if (usuario.Count == 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult VerificaSenhaAntiga(string OldPassword)
        {
            var usuario = (Usuario)Session["Usuario"];

            if (usuario.Senha == OldPassword)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListaCidade(string uf)
        {
            var model = ConnectionDbClass.DataModel;
            
            var cidades = model.Cidades.Where(w => w.Uf == uf).ToList();

            var cities = CidadeAdapter.Adaptar(cidades);

            return Json(cities, JsonRequestBehavior.AllowGet);
        }
    }
}