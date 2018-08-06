using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ouvidoria_Social_DataAccess;

namespace ouvidoria_social.Models.Adapter
{
    public class InstituicaoAdapter
    {
        public string Nome { get; set; }
        public int Id { get; set; }

        public static List<InstituicaoAdapter> Adaptar(List<Instituicao> instituicoes)
        {
            List<InstituicaoAdapter> inst = new List<InstituicaoAdapter>();

            instituicoes.ForEach(f =>
            {
                inst.Add(new InstituicaoAdapter()
                {
                    Nome = f.Nome,
                    Id = f.Id
                });
            });

            return inst;
        }

    }
}