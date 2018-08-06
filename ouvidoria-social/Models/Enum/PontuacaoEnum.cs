using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ouvidoria_social.Models.Enum
{
    public enum PontuacaoEnum
    {
        CadastroUsuario = 1,
        CadastroReclamacao = 2,
        AcessoReclamacao = 3,
        CurtirReclamacao = 4,
        ReclamacaoCurtida = 5,
        DescurtirReclamacao = 6,
        ReclamacaoDescurtida = 7,
        ComentarReclamacao = 8,
        MarcarSolucaoComentario = 9,
        DesmarcarSolucaoComentario = 10,
        CurtirComentario = 11,
        ComentarioCurtido = 12,
        DescurtirComentario = 13,
        ComentarioDescurtido = 14,
        SolucioncarReclamacao = 15,
        ReclamacaoEmMidia = 16
    }

    public class PontuacaoEnumHelper
    {
        public static string GetDescription(PontuacaoEnum enumValue)
        {
            switch (enumValue)
            {
                case PontuacaoEnum.CadastroUsuario:
                    return "Cadastrou Usuário";
                case PontuacaoEnum.AcessoReclamacao:
                    return "Acessou Reclamação";
                case PontuacaoEnum.CadastroReclamacao:
                    return "Cadastrou Reclamação";
                case PontuacaoEnum.CurtirReclamacao:
                    return "Curtiu Reclamação";
                case PontuacaoEnum.ReclamacaoCurtida:
                    return "Reclamação Curtida";
                case PontuacaoEnum.DescurtirReclamacao:
                    return "Descurtiu Reclamação";
                case PontuacaoEnum.ReclamacaoDescurtida:
                    return "Reclamação Descurtida";
                case PontuacaoEnum.ComentarReclamacao:
                    return "Comentou Reclamação";
                case PontuacaoEnum.MarcarSolucaoComentario:
                    return "Comentário Marcado Como Solução";
                case PontuacaoEnum.DesmarcarSolucaoComentario:
                    return "Comentário Desmarcado Como Solução";
                case PontuacaoEnum.CurtirComentario:
                    return "Curtiu Comentário";
                case PontuacaoEnum.ComentarioCurtido:
                    return "Comentário Curtido";
                case PontuacaoEnum.DescurtirComentario:
                    return "Descurtiu Comentário";
                case PontuacaoEnum.ComentarioDescurtido:
                    return "Comentário Descurtido";
                case PontuacaoEnum.SolucioncarReclamacao:
                    return "Solucionou Reclamação";
                case PontuacaoEnum.ReclamacaoEmMidia:
                    return "Inseriu uma mídia na reclamação";
                default:
                    return "";
            }
        }

        public static long GetValue(PontuacaoEnum enumValue)
        {
            switch (enumValue)
            {
                case PontuacaoEnum.CadastroUsuario:
                    return 5;
                case PontuacaoEnum.AcessoReclamacao:
                    return 1;
                case PontuacaoEnum.CadastroReclamacao:
                    return 10;
                case PontuacaoEnum.CurtirReclamacao:
                    return 2;
                case PontuacaoEnum.ReclamacaoCurtida:
                    return 2;
                case PontuacaoEnum.DescurtirReclamacao:
                    return 1;
                case PontuacaoEnum.ReclamacaoDescurtida:
                    return -1;
                case PontuacaoEnum.ComentarReclamacao:
                    return 3;
                case PontuacaoEnum.MarcarSolucaoComentario:
                    return 5;
                case PontuacaoEnum.DesmarcarSolucaoComentario:
                    return -5;
                case PontuacaoEnum.CurtirComentario:
                    return 1;
                case PontuacaoEnum.ComentarioCurtido:
                    return 1;
                case PontuacaoEnum.DescurtirComentario:
                    return 1;
                case PontuacaoEnum.ComentarioDescurtido:
                    return -1;
                case PontuacaoEnum.SolucioncarReclamacao:
                    return 50;
                case PontuacaoEnum.ReclamacaoEmMidia:
                    return 3;
                default:
                    return 0;
            }
        }
    }
}