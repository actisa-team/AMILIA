using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeTJ
{

    using tadLayShare.puntos;
    using EjeTJ.Vertice;

   public  class oArgVertice
    {
        /// <summary>
        /// Id Vertice Base Cero
        /// </summary>
        public int idVertice { get; set; }

       /// <summary>
       /// Nueva Posicion del Vertice
       /// </summary>
        public  IVertice positionNew { get; set; }

        public oArgVertice(int iIdTramo, int iIdVertice, IVertice iPoint)
        {
            idVertice = iIdTramo +  iIdVertice;
            positionNew = iPoint;
        }
    }
}
