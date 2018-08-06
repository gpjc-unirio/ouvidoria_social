using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ouvidoria_Social_DataAccess	
{
    public partial class Reclamacao
    {
        public IEnumerable<string> SelectedSources { get; set; }
    }
}