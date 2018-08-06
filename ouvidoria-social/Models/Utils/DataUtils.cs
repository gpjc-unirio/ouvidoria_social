using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ouvidoria_social.Models.Utils
{
    public class DataUtils
    {
        public static string ToStringPost(DateTime data)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan(data.Ticks));

            var formatado = "";

            if (ts.Days > 0)
            {
                formatado = data.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) +
                            " às " + data.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (ts.Hours > 0)
            {
                formatado = string.Format("{0:0} horas", ts.Hours);
            }
            else if (ts.Minutes > 0)
            {
                formatado = string.Format("{0:0} minutos", ts.Minutes);
            }
            else
            {
                formatado = string.Format("{0:0} segundos", ts.Seconds);
            }

            return formatado;
        }
    }
}