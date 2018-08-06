using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ouvidoria_Social_DataAccess;

namespace ouvidoria_social.Models.Adapter
{
    public class CidadeAdapter
    {
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public int Id { get; set; }

        public static List<CidadeAdapter> Adaptar(List<Cidade> cidades)
        {
            List<CidadeAdapter> cities = new List<CidadeAdapter>();

            cidades.ForEach(f =>
            {
                cities.Add(new CidadeAdapter()
                {
                    Cidade = f.Cidade1,
                    Id = f.Id,
                    Uf = f.Uf
                });
            });

            return cities;
        }

    }
}