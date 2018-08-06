using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ouvidoria_social.Models.Adapter;
using ouvidoria_social.Models.Enum;
using ouvidoria_social.Models.Utils;
using Ouvidoria_Social_DataAccess;
using WebGrease.Css.Extensions;

namespace ouvidoria_social.Controllers
{
    public class OuvidoriaController : Controller
    {
        // GET: Ouvidoria
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult FiltrarReclamacoes(string stringBusca, bool isSolucionado, int tipoBusca)
        {
            var model = ConnectionDbClass.DataModel;

            var reclamacoes = model.Reclamacaos.Where(w => w.IsAtivo.Value).ToList();

            if (isSolucionado)
            {
                reclamacoes = reclamacoes.Where(w => w.IsSolucionado.Value == isSolucionado || 
                    w.Comentarios.Any(a => a.IsSolucao.Value == isSolucionado)).ToList();
            }

            if (!string.IsNullOrEmpty(stringBusca))
            {
                switch (tipoBusca)
                {
                    case 1:
                    {
                        reclamacoes = reclamacoes.Where(w => w.Pessoa.Nome.Trim().ToLower().Contains(stringBusca.Trim().ToLower())).ToList();
                        break;
                    }
                    case 2:
                    {
                        reclamacoes = reclamacoes.Where(w => w.Instituicao.Nome.Trim().ToLower().Contains(stringBusca.Trim().ToLower())).ToList();
                        break;
                    }
                    case 4:
                    {
                        reclamacoes = reclamacoes.Where(w => w.Titulo.Trim().ToLower().Contains(stringBusca.Trim().ToLower())).ToList();
                        break;
                    }
                    case 3:
                    {
                        reclamacoes = reclamacoes.Where(w => w.Descricao.Trim().ToLower().Contains(stringBusca.Trim().ToLower())).ToList();
                        break;
                    }
                    case 6:
                    {
                        reclamacoes = reclamacoes.Where(w => w.Departamento.Trim().ToLower().Contains(stringBusca.Trim().ToLower())).ToList();
                        break;
                    }
                    case 5:
                    {
                        reclamacoes = reclamacoes.Where(w => 
                            w.Titulo.Trim().ToLower().Contains(stringBusca.Trim().ToLower()) || 
                            w.Descricao.Trim().ToLower().Contains(stringBusca.Trim().ToLower()) || 
                            w.Instituicao.Nome.Trim().ToLower().Contains(stringBusca.Trim().ToLower()) || 
                            w.Pessoa.Nome.Trim().ToLower().Contains(stringBusca.Trim().ToLower()) ||
                            w.Departamento.Trim().ToLower().Contains(stringBusca.Trim().ToLower())
                        ).ToList();
                        break;
                    }
                }
            }

            reclamacoes = reclamacoes.OrderByDescending(o => o.DataHora).ToList();

            Session["ReclamacaoFiltro"] = reclamacoes;

            ViewData["Pagina"] = 1;

            return PartialView("_ReclamacoesRecentesView", reclamacoes);
        }

        public ActionResult Reclamacoes()
        {
            var model = ConnectionDbClass.DataModel;

            var recs = model.Reclamacaos.Where(w => w.IsAtivo.Value).OrderByDescending(o => o.DataHora).ToList();

            ViewBag.Pagina = 1;

            Session["ReclamacaoFiltro"] = recs;

            return View(recs);
        }

        [HttpPost]
        public PartialViewResult NavegarPaginaReclamacao(int pagina)
        {
            var model = ConnectionDbClass.DataModel;

            var recs = model.Reclamacaos.OrderByDescending(o => o.DataHora).ToList();
            if (Session["ReclamacaoFiltro"] != null)
            {
                recs = (List<Reclamacao>)Session["ReclamacaoFiltro"];
            }

            ViewData["Pagina"] = pagina;

            return PartialView("_ReclamacoesRecentesView", recs);
        }

        public ActionResult IncluirReclamacao()
        {
            return View();
        }

        public PartialViewResult _MarcarDesmarcarMelhorReposta(int id, int idRec, bool marcar)
        {
            var model = ConnectionDbClass.DataModel;

            var rec = model.Reclamacaos.FirstOrDefault(f => f.Id == idRec);

            Pessoa pessoa = null;

            if (rec != null)
            {
                var comentario = rec.Comentarios.FirstOrDefault(f => f.Id == id);

                if (comentario != null) comentario.IsSolucao = marcar;

                pessoa = comentario.Pessoa;

                Log_participapao log = null;
                if (marcar)
                {
                    log = new Log_participapao()
                    {
                        Acesso = false,
                        Datahora = DateTime.Now,
                        Id_pessoa = comentario.Id_pessoa,
                        Id_reclamacao = comentario.Id_reclamacao,
                        Tipo = "Macou como solução"
                    };
                }
                else
                {
                    log = new Log_participapao()
                    {
                        Acesso = false,
                        Datahora = DateTime.Now,
                        Id_pessoa = comentario.Id_pessoa,
                        Id_reclamacao = comentario.Id_reclamacao,
                        Tipo = "Desmacou como solução"
                    };
                }

                model.Add(log);
            }

            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            
            if (pessoa != null)
            {
                if (marcar)
                {
                    PontuacaoUtils.Pontuar(pessoa, PontuacaoEnum.MarcarSolucaoComentario);
                }
                else
                {
                    PontuacaoUtils.Pontuar(pessoa, PontuacaoEnum.DesmarcarSolucaoComentario);
                }
            }

            return PartialView("_VerComentariosView", rec);
        }

        public PartialViewResult _IncluirReclamacao(string tipo)
        {
            Reclamacao rec = null;
            if (Session["rec"] != null)
            {
                rec = (Reclamacao) Session["rec"];
            }

            return PartialView(tipo == "m" ? "_IncluirReclamacaoMidia" : "_IncluirReclamacaoTexto", rec);
        }

        public ActionResult DesativarReclamacao(int Id)
        {
            var model = ConnectionDbClass.DataModel;

            var rec = model.Reclamacaos.FirstOrDefault(f => f.Id == Id);

            rec.IsAtivo = false;

            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            var usuario = (Usuario)Session["Usuario"];

            return RedirectToAction("Perfil", "Usuario", usuario.Pessoa);
        }

        public ActionResult AcessarReclamacao(int Id)
        {
            var model = ConnectionDbClass.DataModel;

            var rec = model.Reclamacaos.FirstOrDefault(f => f.Id == Id);

            return View(rec);
        }

        public ActionResult AlterarReclamacao(int Id)
        {
            var model = ConnectionDbClass.DataModel;

            var rec = model.Reclamacaos.FirstOrDefault(f => f.Id == Id);

            Session["rec"] = rec;

            return View("IncluirReclamacao", rec); 
        }

        public ActionResult GravarReclamacao(Reclamacao reclamacao, HttpPostedFileBase file)
        {
            var usuario = (Usuario)Session["Usuario"];

            var model = ConnectionDbClass.DataModel;
            
            if (file != null) { 
                if (file.ContentType.Contains("image"))
                {
                    if (file.ContentLength > 2097152)
                    {
                        ViewBag.Erro = "O Arquivo de imagem ultrapassa o limite de 2Mb.";
                        return View("IncluirReclamacao");
                    }
                }
                else if (file.ContentType.Contains("video"))
                {
                    if (file.ContentLength > 26214400)
                    {
                        ViewBag.Erro = "O Arquivo anexo ultrapassa o limite de 25Mb.";
                        return View("IncluirReclamacao");
                    }
                }
                else
                {
                    ViewBag.Erro = "Somente é permitda a inclusão de fotos ou vídeos.";
                    return View("IncluirReclamacao");
                }

                if (file.ContentLength > 0)
                {
                    var fileName = "";
                    if (file.ContentType.Contains("image"))
                    {
                        if (string.IsNullOrEmpty(fileName))
                        {
                            fileName = "rec_" + usuario.Pessoa.Id + "_" + DateTime.Now.ToString("yyyyMd_hhmmss") + ".jpg";
                        }
                    }
                    else
                    {
                        string extensao = Path.GetExtension(file.FileName);

                        fileName = "rec_" + usuario.Pessoa.Id + "_" + DateTime.Now.ToString("yyyyMd_hhmmss") + extensao;
                    }

                    var path = Path.Combine(Server.MapPath("~/Images/Uploads/Queixa"), fileName );

                    reclamacao.Anexo = fileName;

                    file.SaveAs(path);

                    PontuacaoUtils.Pontuar(usuario.Pessoa, PontuacaoEnum.ReclamacaoEmMidia);
                }
            }

            if (string.IsNullOrEmpty(reclamacao.Descricao) || string.IsNullOrEmpty(reclamacao.Titulo))
            {
                ViewBag.Erro = "Não é permitida a Inclusão de Reclamações sem Descrição ou Título.";
                return View("IncluirReclamacao");
            }

            reclamacao.Id_pessoa = usuario.Pessoa.Id;
            reclamacao.DataHora = DateTime.Now;
            reclamacao.IsSolucionado = false;
            reclamacao.IsAtivo = true;
            
            if (reclamacao.Id == 0 || reclamacao.Id == null)
            {
                model.Add(reclamacao);
            }
            else
            {
                var rec = model.Reclamacaos.FirstOrDefault(f => f.Id == reclamacao.Id);
                
                rec.Departamento = reclamacao.Departamento;
                rec.Id_instituicao = reclamacao.Id_instituicao;
                rec.Descricao = reclamacao.Descricao;
                rec.Titulo = reclamacao.Titulo;
                rec.Anexo = reclamacao.Anexo;
            }

            if (reclamacao.Id == 0 || reclamacao.Id == null)
            {   
                var log = new Log_participapao()
                {
                    Datahora = DateTime.Now,
                    Acesso = false,
                    Id_pessoa = reclamacao.Id_pessoa,
                    Tipo = "Criou uma Reclamação"
                };

                reclamacao.Log_participapaos.Add(log);
                log.Reclamacao = reclamacao;

                model.Add(log);
            }

            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            PontuacaoUtils.Pontuar(usuario.Pessoa, PontuacaoEnum.CadastroReclamacao);

            return RedirectToAction("AcessarReclamacao", "Ouvidoria", new { Id = reclamacao.Id });
        }
        
        public string _AtualizarLikesDeslikes(int id, bool tipo)
        {
            var model = ConnectionDbClass.DataModel;

            var reclamacao = model.Reclamacaos.FirstOrDefault(f => f.Id == id);

            var usuario = (Usuario)Session["Usuario"];

            if (reclamacao != null)
            {
                if (tipo)
                {
                    reclamacao.Likes = reclamacao.Likes != null ? reclamacao.Likes + 1 : 1;
                }
                else
                {
                    reclamacao.Deslikes = reclamacao.Deslikes != null ? reclamacao.Deslikes + 1 : 1;
                }

                Log_participapao log = null;
                if (tipo)
                {
                    log = new Log_participapao()
                    {
                        Acesso = false,
                        Datahora = DateTime.Now,
                        Id_pessoa = reclamacao.Pessoa.Id,
                        Id_reclamacao = reclamacao.Id,
                        Tipo = usuario.Pessoa.Nome + " Curtiu",
                        Id_curtidor = usuario.Id_pessoa
                    };
                }
                else
                {
                    log = new Log_participapao()
                    {
                        Acesso = false,
                        Datahora = DateTime.Now,
                        Id_pessoa = reclamacao.Pessoa.Id,
                        Id_reclamacao = reclamacao.Id,
                        Tipo = usuario.Pessoa.Nome + " Descurtiu",
                        Id_curtidor = usuario.Id_pessoa
                    };
                }

                model.Add(log);

                if (model.HasChanges)
                {
                    model.SaveChanges();
                }

                if (tipo)
                {
                    PontuacaoUtils.Pontuar(usuario.Pessoa, PontuacaoEnum.CurtirReclamacao);
                    PontuacaoUtils.Pontuar(reclamacao.Pessoa, PontuacaoEnum.ReclamacaoCurtida);

                    return "<span class='glyphicon glyphicon-thumbs-up'></span><span>" + (reclamacao.Likes ?? 0) + "</span>";
                }
                else
                {
                    PontuacaoUtils.Pontuar(usuario.Pessoa, PontuacaoEnum.DescurtirReclamacao);
                    PontuacaoUtils.Pontuar(reclamacao.Pessoa, PontuacaoEnum.ReclamacaoDescurtida);

                    return "<span class='glyphicon glyphicon-thumbs-down'></span><span>" + (reclamacao.Deslikes ?? 0) + "</span>";
                }
            }

            return "";
        }

        public string _AtualizarLikesDeslikesComentario(int id, bool tipo)
        {
            var model = ConnectionDbClass.DataModel;

            var comentario = model.Comentarios.FirstOrDefault(f => f.Id == id);

            var usuario = (Usuario)Session["Usuario"];

            if (comentario != null)
            {
                Log_participapao log = null;

                if (tipo)
                {
                    comentario.Likes = comentario.Likes != null ? comentario.Likes + 1 : 1;

                    log = new Log_participapao()
                    {
                        Acesso = false,
                        Datahora = DateTime.Now,
                        Id_pessoa = comentario.Pessoa.Id,
                        Id_reclamacao = comentario.Id_reclamacao,
                        Tipo = usuario.Pessoa.Nome + " Curtiu seu Comentário",
                        Id_comentario = id,
                        Id_curtidor = usuario.Id_pessoa
                    };
                }
                else
                {
                    comentario.Deslikes = comentario.Deslikes != null ? comentario.Deslikes + 1 : 1;
                    log = new Log_participapao()
                    {
                        Acesso = false,
                        Datahora = DateTime.Now,
                        Id_pessoa = comentario.Pessoa.Id,
                        Id_reclamacao = comentario.Id_reclamacao,
                        Tipo = usuario.Pessoa.Nome + " Descurtiu Seu Comentário",
                        Id_comentario = id,
                        Id_curtidor = usuario.Id_pessoa
                    };
                }
                
                model.Add(log);

                if (model.HasChanges)
                {
                    model.SaveChanges();
                }

                if (tipo)
                {
                    PontuacaoUtils.Pontuar(usuario.Pessoa, PontuacaoEnum.CurtirComentario);
                    PontuacaoUtils.Pontuar(comentario.Pessoa, PontuacaoEnum.ComentarioCurtido);

                    return "<span class='glyphicon glyphicon-thumbs-up'></span><span>" + (comentario.Likes ?? 0) + "</span>";
                }
                else
                {
                    PontuacaoUtils.Pontuar(usuario.Pessoa, PontuacaoEnum.DescurtirComentario);
                    PontuacaoUtils.Pontuar(comentario.Pessoa, PontuacaoEnum.ComentarioDescurtido);

                    return "<span class='glyphicon glyphicon-thumbs-down'></span><span>" + (comentario.Deslikes ?? 0) + "</span>";
                }
            }

            return "";
        }

        [HttpPost]
        public ActionResult AddComentarioAjax(Comentario comentario, HttpPostedFileBase file, string tipo)
        {
            var model = ConnectionDbClass.DataModel;

            var reclamacao = model.Reclamacaos.FirstOrDefault(f => f.Id == comentario.Id_reclamacao);

            var usuario = (Usuario)Session["Usuario"];

            if (file != null)
            {
                if (file.ContentType.Contains("image"))
                {
                    if (file.ContentLength > 2097152)
                    {
                        ViewBag.Erro = "O Arquivo de imagem ultrapassa o limite de 2Mb.";
                        return View("AcessarReclamacao", reclamacao);
                    }
                }
                else if (file.ContentType.Contains("video"))
                {
                    if (file.ContentLength > 26214400)
                    {
                        ViewBag.Erro = "O Arquivo anexo ultrapassa o limite de 25Mb.";
                        return View("AcessarReclamacao", reclamacao);
                    }
                }
                else
                {
                    ViewBag.Erro = "Somente é permitda a inclusão de fotos ou vídeos.";
                    return View("AcessarReclamacao", reclamacao);
                }

                if (file.ContentLength > 0)
                {
                    var fileName = "";
                    if (file.ContentType.Contains("image"))
                    {
                        if (string.IsNullOrEmpty(fileName))
                        {
                            fileName = "rec_solucao_" + usuario.Pessoa.Id + "_" + DateTime.Now.ToString("yyyyMd_hhmmss") + ".jpg";
                        }
                    }
                    else
                    {
                        string extensao = Path.GetExtension(file.FileName);

                        fileName = "rec_solucao_" + usuario.Pessoa.Id + "_" + DateTime.Now.ToString("yyyyMd_hhmmss") + extensao;
                    }

                    var path = Path.Combine(Server.MapPath("~/Images/Uploads/Queixa"), fileName);

                    comentario.Anexo = fileName;
                    
                    file.SaveAs(path);
                }
            }

            var logParticipacao = new Log_participapao()
            {
                Id_pessoa = comentario.Id_pessoa,//usuario.Id_pessoa,
                Id_reclamacao = comentario.Id_reclamacao,
                Datahora = DateTime.Now,
                Acesso = false,
                Tipo = usuario.Pessoa.Nome + " Comentou sua reclamação"
            };
            
            comentario.Likes = 0;
            comentario.Deslikes = 0;
            comentario.Id_pessoa = usuario.Id_pessoa;
            comentario.DataComentario = DateTime.Now;
            comentario.IsSolucao = false;
            comentario.IsSolucao = comentario.IsSolucaoFinal ?? false;
            comentario.IsSolucaoFinal = comentario.IsSolucaoFinal ?? false;

            if (comentario.IsSolucaoFinal.Value)
            {
                logParticipacao.Tipo = "Solucionou";
                reclamacao.IsSolucionado = true;
            }

            model.Add(comentario);
            model.Add(logParticipacao);
            
            if (model.HasChanges)
            {
                model.SaveChanges();
            }

            if (comentario.IsSolucaoFinal.Value)
            {
                PontuacaoUtils.Pontuar(usuario.Pessoa, PontuacaoEnum.SolucioncarReclamacao);
            }
            else
            {
                PontuacaoUtils.Pontuar(usuario.Pessoa, PontuacaoEnum.ComentarReclamacao);
            }

            if (tipo == "http")
            {
                return View("AcessarReclamacao", reclamacao);
            }
            else
            {
                return PartialView("_VerComentariosView", reclamacao);
            }
        }

        [HttpPost]
        public ActionResult DeletarComentarioAjax(int idComentario)
        {
            var model = ConnectionDbClass.DataModel;
            
            var comentario = model.Comentarios.FirstOrDefault(f => f.Id == idComentario);

            var idRec = comentario.Id_reclamacao;

            if (comentario != null)
            {
                model.Delete(comentario);

                if (model.HasChanges)
                {
                    model.SaveChanges();
                }
                
            }

            var reclamacao = model.Reclamacaos.FirstOrDefault(f => f.Id == idRec);

            return PartialView("_VerComentariosView", reclamacao);
        }
    }
}