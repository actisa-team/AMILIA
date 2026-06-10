using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet.tools
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.EditorInput;


    /// <!--JUAN - UPDATE 31-10-14-->
    public class oIntersectWithPlus
    {

        /// <summary>
        /// Metodo Interección Terreno-Talud
        /// En algunos casos, el método de autocad intersectwith, daba puntos de intersección errorenos
        /// Ahora revisamos que pertenecen a ambas entidades
        /// </summary>
        public static Point3dCollection getInterseccion(Polyline iLwTerreno, Polyline iLwTalud, Intersect iInterseccionTipo)
        {


            Point3dCollection miColInter = new Point3dCollection();

            iLwTerreno.IntersectWith(iLwTalud, iInterseccionTipo, miColInter, IntPtr.Zero, IntPtr.Zero);

            Point3dCollection miColInterCheck = new Point3dCollection();

            foreach (Point3d ptoToCheck in miColInter)
            {

                if (oLw.isPtoOnLw(iLwTerreno, ptoToCheck) && oLw.isPtoOnLw(iLwTalud, ptoToCheck))
                {
                    miColInterCheck.Add(ptoToCheck);
                }
            }

            return miColInterCheck;
        }

        /// <summary>
        /// Autocad a veces da problemas al calcular la intersección entre polilineas cuando el punto
        /// de intersección es un extremo de la polilinea, debemos de chekear con este método que pertenece a la polilinea.
        /// Si GetParameterAtPoint falla (puntos muy cerca del extremo), se usa una tolerancia geométrica como fallback.
        /// </summary>
        public static Point3dCollection checkPuntosInterseccion(Point3dCollection iColPtoInter, Polyline iLwOn)
        {
            const double TOLERANCIA_EXTREMO_METROS = 0.001; // 1 mm

            Point3dCollection miColInterCheck = new Point3dCollection();

            foreach (Point3d ptoToCheck in iColPtoInter)
            {
                bool isOn = oLw.isPtoOnLw(iLwOn, ptoToCheck);

                if (!isOn)
                {
                    // Fallback: cuando GetParameterAtPoint falla cerca de los extremos,
                    // comprobamos la distancia geométrica al punto más cercano de la polilínea
                    Point3d ptoCercano = iLwOn.GetClosestPointTo(ptoToCheck, false);
                    double distancia = ptoToCheck.DistanceTo(ptoCercano);
                    if (distancia <= TOLERANCIA_EXTREMO_METROS)
                    {
                        isOn = true;
                    }
                }

                if (isOn)
                {
                    miColInterCheck.Add(ptoToCheck);
                }
            }

            return miColInterCheck;

        }



    }
}
