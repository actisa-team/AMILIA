using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Interfaz
{

    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;

    using System.ComponentModel;

  
    using tadLayLogica.logica.medicion;




    public interface ISecPk
    {
        /// <summary>
        /// PK de la Seccion
        /// </summary>
        double pk { get; set; }   
    }




    public interface ISecDrawSaneo: ISecMedicion
    {
        eSaneo saneoTipo { get;}
        Polyline lwSaneo { get; }
        void drawSaneo(string iLayer);
    }


 
    public interface ISecDrawPlus :ISecPk, ISecMedicion
    {

        /// <summary>
        /// Lado de la Sección
        /// </summary>
        eLado lado { get; set; }

        /// <summary>
        /// Registro los Decoradores que Vamos Añadiendo
        /// </summary>
        Dictionary<int, ISecDrawPlus> dicIsecDraw { get; set; }

        /// <summary>
        /// Accedo al Decorador Padre (La Sección Padre)
        /// </summary>
        ISecDrawPlus parent { get; }

        /// <summary>
        /// Accedo al Decorador Previo
        /// </summary>
        ISecDrawPlus previo { get; }
         
        /// <summary>
        /// Determina Si Dibujamos el Talud
        /// </summary>
        bool taludDraw { get; set; }

        /// <summary>
        /// Punto donde Se Insertara su Hijo si Existe
        /// </summary>
        Point3d ptoInsertChild { get; }


        /// <summary>
        /// Coleccion Puntos Fijan la Envolvente Inferior
        /// </summary>
        /// <remarks>Se utiliza para la medición Terraplen-Desmonte</remarks>
        Point3dCollection envolvente { get; }


        /// <summary>
        /// Coleccion Puntos Marcan la Lineas Talud
        /// </summary>
        Point3dCollection taludLstPol { get; set; }
        
        /// <summary>
        /// Metodo Dibujar la Sección Decorator
        /// </summary>
        void draw( string iLayer, Matrix3d? iMatrix);

        /// <summary>
        /// Añadir Decorator
        /// </summary>
        void addDecorator(ISecDrawPlus iSecDecorator);
    }

}
