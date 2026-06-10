using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.medicion
{
   public class oRowMaterial
    {

        public Guid idMaterial { get; private set; }
        public string nombre { get; private set; }
        public double precioPrincipal { get; private set; }
        public double precioSecundario { get; private set; }
        public Int16 idClasificacion { get;private set; }
        public string descripcion { get;private set; }


        public oRowMaterial(Guid iIdMaterial, string iNombre, string iDescripcion, double iPrecioPrincipal, double iPrecioSecundario,Int16 iIdClasificacion)
        {
            this.idMaterial = iIdMaterial;
            this.nombre = iNombre;
            this.descripcion = iDescripcion;
            this.precioPrincipal = iPrecioPrincipal;
            this.precioSecundario = iPrecioSecundario;
            this.idClasificacion = iIdClasificacion;
        }

    }
}
