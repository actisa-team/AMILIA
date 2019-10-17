using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica {
    public interface IViabilidadListener {
        void onNewViabilidadStatus(ViabilidadComponentesStatus viabilidadComponentesStatus, List<Componente> componentes, int whileItIndex); 
    }
}
