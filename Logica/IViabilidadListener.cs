using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logica {
    public interface IViabilidadListener {
        void onNewViabilidadStatus(ViabilidadComponentesStatus viabilidadComponentesStatus, List<Componente> componentes, int whileItIndex); 
    }
}
