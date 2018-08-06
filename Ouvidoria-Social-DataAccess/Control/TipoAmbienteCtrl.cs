using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ouvidoria_Social_DataAccess.Control
{
    public class TipoAmbienteCtrl
    {

        public static bool ControlarGamificacao
        {
            get
            {
                var controlarGamificacao = false;
                if (HttpContext.Current.Session["ControlarGamificacao"] != null)
                {
                    controlarGamificacao = (bool)HttpContext.Current.Session["ControlarGamificacao"];
                }

                return controlarGamificacao;
            }
        }

    }
}
