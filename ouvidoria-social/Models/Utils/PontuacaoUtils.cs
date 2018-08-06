using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ouvidoria_social.Models.Enum;
using Ouvidoria_Social_DataAccess;
using Telerik.OpenAccess;
using AjaxMinExtensions = Microsoft.Ajax.Utilities.AjaxMinExtensions;

namespace ouvidoria_social.Models.Utils
{
    public class PontuacaoUtils
    {
        public static void Pontuar(Pessoa pessoa, PontuacaoEnum tipoPonto)
        {
            var model = ConnectionDbClass.DataModel;

            var logNivel = model.Pessoa_nivel_pontos.FirstOrDefault(f => f.Id_pessoa == pessoa.Id);
            if (logNivel == null)
            {
                logNivel = new Pessoa_nivel_ponto()
                {
                    Id_pessoa = pessoa.Id,
                    Nivel = 0,
                    Pontos = -1
                };

                model.Add(logNivel);
            }
            
            var logPontos = new Log_pontos_pessoa()
            {
                Id_pessoa = pessoa.Id,
                Pontos = PontuacaoEnumHelper.GetValue(tipoPonto),
                TipoPonto = (int) tipoPonto,
                Descricao = PontuacaoEnumHelper.GetDescription(tipoPonto),
                Data = DateTime.Now
            };

            if (logNivel != null)
            {
                logNivel.Pontos += logPontos.Pontos;
            }

            if (AvancarNivel(pessoa))
            {
                var nivelAtual = model.Pessoa_nivel_pontos.First(f => f.Id_pessoa == pessoa.Id);

                nivelAtual.Nivel++;
            }

            model.Add(logPontos);

            if (model.HasChanges)
            {
                model.SaveChanges();
            }
        }

        public static bool AvancarNivel(Pessoa pessoa)
        {
            var pontosNivel = PontosProximoNivel(pessoa);
            if (pessoa.Pessoa_nivel_pontos.Count > 0)
            {
                var pessoaNivel = pessoa.Pessoa_nivel_pontos[0];

                if (pontosNivel <= pessoaNivel.Pontos)
                {
                    return true;
                }
            }

            return false;
        }

        public static long PontosProximoNivel(Pessoa pessoa)
        {
            long pontos = 0;
            
            if (pessoa.Pessoa_nivel_pontos.Count > 0)
            {
                var pessoaNivel = pessoa.Pessoa_nivel_pontos[0];

                var pontoNivel = 2*(Math.Pow(3, (pessoaNivel.Nivel.Value + 1)));

                pontos = Convert.ToInt64(pontoNivel);
            }

            return pontos;
        }

        public static void CalcularDesafios(Pessoa pessoa)
        {
            Comentador(pessoa);
            ComentadorPro(pessoa);
            ComentadorMaster(pessoa);

            Curtidor(pessoa);
            CurtidorPro(pessoa);
            CurtidorMaster(pessoa);

            RecebedorCurtidas(pessoa);
            RecebedorCurtidasPro(pessoa);
            RecebedorCurtidasMaster(pessoa);

            Resolvedor(pessoa);
            ResolvedorPro(pessoa);
            ResolvedorMaster(pessoa);

            Descurtidor(pessoa);
            DescurtidorMaster(pessoa);

            Solucionador(pessoa);

            DesafiosConcluidos(pessoa);
        }

        private static void Comentador(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id && w.Data == DateTime.Now.Date &&
                        w.TipoPonto == (int) PontuacaoEnum.ComentarReclamacao).ToList();
            
            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos/3;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 1, qtde, true);
                }
            }
        }
        
        private static void ComentadorPro(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id && w.Data == DateTime.Now.Date &&
                        w.TipoPonto == (int)PontuacaoEnum.ComentarReclamacao).ToList();

            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos / 10;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 2, qtde, true);
                }
            }
        }

        private static void ComentadorMaster(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id && w.Data == DateTime.Now.Date &&
                        w.TipoPonto == (int)PontuacaoEnum.ComentarReclamacao).ToList();

            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos / 25;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 3, qtde, true);
                }
            }
        }

        private static void Curtidor(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id && w.Data == DateTime.Now.Date &&
                        (w.TipoPonto == (int)PontuacaoEnum.CurtirReclamacao || w.TipoPonto == (int)PontuacaoEnum.CurtirComentario)).ToList();

            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos / 25;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 4, qtde, true);
                }
            }
        }

        private static void CurtidorPro(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id && w.Data == DateTime.Now.Date &&
                        (w.TipoPonto == (int)PontuacaoEnum.CurtirReclamacao || w.TipoPonto == (int)PontuacaoEnum.CurtirComentario)).ToList();

            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos / 150;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 5, qtde, true);
                }
            }
        }

        private static void CurtidorMaster(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id && w.Data == DateTime.Now.Date &&
                        (w.TipoPonto == (int)PontuacaoEnum.CurtirReclamacao || w.TipoPonto == (int)PontuacaoEnum.CurtirComentario)).ToList();

            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos / 300;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 6, qtde, true);
                }
            }
        }

        private static void RecebedorCurtidas(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id && w.Data == DateTime.Now.Date &&
                        (w.TipoPonto == (int)PontuacaoEnum.ReclamacaoCurtida || w.TipoPonto == (int)PontuacaoEnum.ComentarioCurtido)).ToList();

            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos / 15;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 7, qtde, true);
                }
            }
        }

        private static void RecebedorCurtidasPro(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id && w.Data == DateTime.Now.Date &&
                        (w.TipoPonto == (int)PontuacaoEnum.ReclamacaoCurtida || w.TipoPonto == (int)PontuacaoEnum.ComentarioCurtido)).ToList();

            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos / 30;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 8, qtde, true);
                }
            }
        }

        private static void RecebedorCurtidasMaster(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id && w.Data == DateTime.Now.Date &&
                        (w.TipoPonto == (int)PontuacaoEnum.ReclamacaoCurtida || w.TipoPonto == (int)PontuacaoEnum.ComentarioCurtido)).ToList();

            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos / 100;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 9, qtde, true);
                }
            }
        }

        private static void Resolvedor(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id &&
                        w.TipoPonto == (int)PontuacaoEnum.SolucioncarReclamacao).ToList();
            
            if (logPontos.Any())
            {
                logPontos.ForEach(f =>
                {
                    foreach (var reclamacao in pessoa.Reclamacaos)
                    {
                        var comentarios =
                            reclamacao.Comentarios.Where(
                                w => (w.IsSolucaoFinal.Value) && (w.DataComentario.Value.Date == f.Data.Value.Date) && (w.Reclamacao.DataHora.Value.Date == f.Data.Value.Date));

                        if (comentarios.Any())
                        {
                            InserirDesafio(pessoa, 12, 1, true);
                            return;
                        }
                    }
                });
            }
        }

        private static void ResolvedorPro(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id &&
                        w.TipoPonto == (int)PontuacaoEnum.SolucioncarReclamacao).ToList();

            if (logPontos.Any())
            {
                logPontos.ForEach(f =>
                {
                    foreach (var reclamacao in pessoa.Reclamacaos)
                    {
                        var ProxDataSemana =
                            new DateTime(reclamacao.DataHora.Value.Year, reclamacao.DataHora.Value.Month,
                                reclamacao.DataHora.Value.Day).AddDays(7);
                        
                        var comentarios =
                            reclamacao.Comentarios.Where(
                                w => (w.IsSolucaoFinal.Value) && (w.DataComentario.Value.Date == f.Data.Value.Date)
                                    && (w.DataComentario.Value.Date <= ProxDataSemana));

                        if (comentarios.Any())
                        {
                            InserirDesafio(pessoa, 10, 1, true);
                            return;
                        }
                    }
                });
            }
        }

        private static void ResolvedorMaster(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id &&
                        w.TipoPonto == (int)PontuacaoEnum.SolucioncarReclamacao).ToList();

            if (logPontos.Any())
            {
                logPontos.ForEach(f =>
                {
                    foreach (var reclamacao in pessoa.Reclamacaos)
                    {
                        var ProxDataMes =
                            new DateTime(reclamacao.DataHora.Value.Year, reclamacao.DataHora.Value.Month,
                                reclamacao.DataHora.Value.Day).AddMonths(1);

                        var comentarios =
                            reclamacao.Comentarios.Where(
                                w => (w.IsSolucaoFinal.Value) && (w.DataComentario.Value.Date == f.Data.Value.Date)
                                    && (w.DataComentario.Value.Date <= ProxDataMes));

                        if (comentarios.Any())
                        {
                            InserirDesafio(pessoa, 11, 1, true);
                            return;
                        }
                    }
                });
            }
        }

        private static void Descurtidor(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id && w.Data == DateTime.Now.Date &&
                        (w.TipoPonto == (int)PontuacaoEnum.DescurtirComentario || w.TipoPonto == (int)PontuacaoEnum.DescurtirReclamacao)).ToList();

            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos / 10;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 14, qtde, true);
                }
            }
        }

        private static void DescurtidorMaster(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w =>
                        w.Id_pessoa == pessoa.Id && w.Data == DateTime.Now.Date &&
                        (w.TipoPonto == (int)PontuacaoEnum.DescurtirComentario || w.TipoPonto == (int)PontuacaoEnum.DescurtirReclamacao)).ToList();

            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos / 50;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 15, qtde, true);
                }
            }
        }

        private static void Solucionador(Pessoa pessoa)
        {
            var model = ConnectionDbClass.DataModel;

            var logPontos =
                model.Log_pontos_pessoas.Where(
                    w => w.TipoPonto == (int)PontuacaoEnum.MarcarSolucaoComentario).ToList();

            if (logPontos.Any())
            {
                var pontos = logPontos.Count();
                var qtde = pontos / 25;

                if (qtde > 0)
                {
                    InserirDesafio(pessoa, 15, qtde, true);
                }
            }
        }

        private static void DesafiosConcluidos(Pessoa pessoa)
        {
            var desafio25 = pessoa.Pessoa_desafios.Count(c => c.Ganho.Value) >= 4;
            if (desafio25)
            {
                InserirDesafio(pessoa, 16, 1, true);
            }

            var desafio50 = pessoa.Pessoa_desafios.Count(c => c.Ganho.Value) >= 9;
            if (desafio50)
            {
                InserirDesafio(pessoa, 17, 1, true);
            }

            var desafio100 = pessoa.Pessoa_desafios.Count(c => c.Ganho.Value) >= 18;
            if (desafio100)
            {
                InserirDesafio(pessoa, 18, 1, true);
            }

        }

        private static void InserirDesafio(Pessoa pessoa, int idDesafio, int qtde, bool isDiario = false)
        {
            var model = ConnectionDbClass.DataModel;

            var desafio = model.Pessoa_desafios.FirstOrDefault(w => w.IdPessoa == pessoa.Id && w.IdDesafio == idDesafio);            
            
            if (desafio != null)
            {
                if (!desafio.Ganho.Value)
                {
                    desafio.Data = DateTime.Now;
                    desafio.Hora = DateTime.Now;
                    desafio.VezesGanhas = desafio.VezesGanhas + qtde;
                    desafio.Ganho = true;

                    if (model.HasChanges)
                    {
                        model.SaveChanges();
                    }

                    ConnectionDbClass.DataModel.Refresh(RefreshMode.OverwriteChangesFromStore, ConnectionDbClass.DataModel.Pessoas);
                }
            }
        }
    }
}