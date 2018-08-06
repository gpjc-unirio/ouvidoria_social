using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ouvidoria_Social_DataAccess;

namespace ouvidoria_social.Models
{
    public class DesafiosRepository
    {
        public static void GerarDesafiosIniciais(Pessoa pessoa)
        {
            if (!pessoa.Pessoa_desafios.Any())
            {
                var model = ConnectionDbClass.DataModel;

                foreach (var desafio in model.Desafios.ToList())
                {
                    var desafioPessoa = new Pessoa_desafio()
                    {
                        Ganho = false,
                        VezesGanhas = 0,
                        IdDesafio = desafio.Id,
                        IdPessoa =  pessoa.Id
                    };

                    pessoa.Pessoa_desafios.Add(desafioPessoa);
                }

                if (model.HasChanges)
                {
                    model.SaveChanges();
                }
            }
        }
    }
}