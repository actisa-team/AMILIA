using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{

    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Colors;
    using engCadNet;

    using tadLayLogica.datos.proyecto;

    using tadLayLogica.zonaGis;
    using tadLayShare;

    /// <summary>
    /// CAPA NO ESPECIFICA
    /// </summary>
    public class oTadilLayer
    {

        private string mName = string.Empty;
        private short? mColor = null;





        #region "Constructor"
        public oTadilLayer()
        {

        }
        public oTadilLayer(string iSolName, string iSolFormato, short iColor)
        {
            name = string.Format(iSolFormato, iSolName);
            color = iColor;
            createLayer();
        }
        public oTadilLayer(Guid iSolId, string iSolFormato, short iColor)
        {

            string miNameSol = oSingletonDsApp.getInstance.getSolucion(iSolId).nombre;

            name = string.Format(iSolFormato, miNameSol);

            color = iColor;

            createLayer();
        }
        public oTadilLayer(string iLay, short iColor)
        {
            this.name = iLay;
            this.color = iColor;

            createLayer();
        }
        #endregion
        #region "Propiedades"

        public ObjectId layerObjId()
        {
            return oLayer.getLayerObjId(name);
        }

        public string name
        {
            get
            {
                if (string.IsNullOrEmpty(mName))
                {
                    throw new oExPropertieNullValue("Capa Nombre");
                }

                return mName;
            }
            set
            {
                mName = value;
            }
        }
        public short color
        {
            get
            {
                if (mColor == null)
                {
                    throw new oExPropertieNullValue("Capa Color");
                }

                return mColor.Value;
            }

            set
            {
                mColor = value;
            }
        }



        #endregion


        public void createLayer()
        {
            engCadNet.oLayer.addLayer(this.name, this.color, false);
        }
        public void On()
        {
            engCadNet.oLayer.vLayerOffOn(this.name, false, false);
        }
        public void Off()
        {
            engCadNet.oLayer.vLayerOffOn(this.name, true, false);
        }
        public void Current()
        {
            engCadNet.oLayer.current(this.name);
        }
        public void deleteItems()
        {
            engCadNet.oLayer.deleteByListName(new List<string>(new string[] { this.name }));
        }
        public int numeroEntidades(eEntidades iEntidad)
        {
            SelectionSet miSs = oSs.getSsByLayerAndEntidad(this.name, iEntidad);

            if (miSs == null && miSs.Count == 0)
            {
                return 0;
            }
            else
            {
                return miSs.Count;
            }
        }

        /// <summary>
        /// Obtener la Coleccion de todas las Entidades por Capa
        /// </summary>
        public ObjectIdCollection getEntidades()
        {
            return oSs.getEntidadesOnLayer(this.name);
        }
    }
    /// <summary>
    /// CAPA EJE BASICO 2D // Dibujo Planta EjeTrazado
    /// </summary>
    public class oTadilLayerEjeBasico2D : oTadilLayer
    {

        public static string KSolLayer = "_Tadil_Sol_{0}_EjeBasico2D";
        private const short KColor = oColor.cVerdeIndex;


        public oTadilLayerEjeBasico2D(Guid iIdSol)
            : base(iIdSol, KSolLayer, KColor)
        {


        }

        public oTadilLayerEjeBasico2D(string iNombreSolucion)
            : base(iNombreSolucion, KSolLayer, KColor)
        {


        }


    }
    /// <summary>
    /// CAPA EJE BASICO 3D // Dibujo Perfil Longitudinal
    /// </summary>
    public class oTadilLayerEjeBasico3D : oTadilLayer
    {

        public static string KSolLayer = "_Tadil_Sol_{0}_EjeBasico3D";
        private const short KColor = oColor.cVerdeIndex;


        public oTadilLayerEjeBasico3D(Guid iIdSol)
            : base(iIdSol, KSolLayer, KColor)
        {


        }

        public oTadilLayerEjeBasico3D(string iNombreSolucion)
            : base(iNombreSolucion, KSolLayer, KColor)
        {


        }


    }
    /// <summary>
    /// CAPA EJE TRAZADO
    /// </summary>
    public class oTadilLayerEjeTrazado : oTadilLayer
    {

        private const string KSolEjeTrazado = "_Tadil_Sol_{0}_EjeTrazado";
        private const short KColor = oColor.cVerdeIndex;


        public oTadilLayerEjeTrazado(string iSolucionNombre)
            : base(iSolucionNombre, KSolEjeTrazado, KColor)
        {


        }

        public oTadilLayerEjeTrazado(Guid iIdSol)
            : base(iIdSol, KSolEjeTrazado, KColor)
        {


        }

    }
    /// <summary>
    /// CAPA EJE TRAZADO TADIL OLD
    /// </summary>
    public class oTadilLayerEjeTrazadoOLD : oTadilLayer
    {

        private const string KSolEjeTrazado = "_Tadil_Sol_{0}_EjeTrazadoOLD";
        private const short KColor = oColor.cVerdeIndex;


        public oTadilLayerEjeTrazadoOLD(string iSolucionNombre)
            : base(iSolucionNombre, KSolEjeTrazado, KColor)
        {


        }

        public oTadilLayerEjeTrazadoOLD(Guid iIdSol)
            : base(iIdSol, KSolEjeTrazado, KColor)
        {


        }

    }
    /// <summary>
    /// CAPA EJE TRAZADO TADIL PUNTOS KILOMETRICOS
    /// </summary>
    public class oTadilLayerEjeTrazadoTadilPk : oTadilLayer
    {

        private const string KSolEjeTrazado = "_Tadil_Sol_{0}_EjeTrazadoTadilPk";
        private const short KColor = oColor.cGrisClaroIndex;


        public oTadilLayerEjeTrazadoTadilPk(string iSolucionNombre)
            : base(iSolucionNombre, KSolEjeTrazado, KColor)
        {


        }

        public oTadilLayerEjeTrazadoTadilPk(Guid iIdSol)
            : base(iIdSol, KSolEjeTrazado, KColor)
        {


        }

    }
    /// <summary>
    /// CAPA EJE TRAZADO TADIL PUNTOS SINGULARES
    /// </summary>
    public class oTadilLayerEjeTrazadoTadilPs : oTadilLayer
    {

        private const string KSolEjeTrazado = "_Tadil_Sol_{0}_EjeTrazadoTadilPs";
        private const short KColor = oColor.cGrisClaroIndex;


        public oTadilLayerEjeTrazadoTadilPs(string iSolucionNombre)
            : base(iSolucionNombre, KSolEjeTrazado, KColor)
        {


        }

        public oTadilLayerEjeTrazadoTadilPs(Guid iIdSol)
            : base(iIdSol, KSolEjeTrazado, KColor)
        {


        }

    }
    /// <summary>
    /// CAPA PERFIL LONGITUDINAL TADIL 
    /// </summary>
    public class oTadilLayerPerfilLongitudinalTadil : oTadilLayer
    {
        private const string KSolPlanta = "_Tadil_Sol_{0}_PerfilLongitudinalTadil";
        private const short KColor = oColor.cAmarilloIndex;

        public oTadilLayerPerfilLongitudinalTadil (string iSolName)
            : base(iSolName, KSolPlanta, KColor)
        {

        }

        public oTadilLayerPerfilLongitudinalTadil(Guid iIdSol)
            : base(iIdSol, KSolPlanta, KColor)
        {


        }


    }
    /// <summary>
    /// CAPA PERFIL LONGITUDINAL CIVIL3D
    /// </summary>
    public class oTadilLayerPerfilLongitudinal : oTadilLayer
    {

        private const string KSolPlanta = "_Tadil_Sol_{0}_PerfilLongitudinal";
        private const short KColor = oColor.cAmarilloIndex;

        public oTadilLayerPerfilLongitudinal(string iSolName)
            : base(iSolName, KSolPlanta, KColor)
        {

        }

        public oTadilLayerPerfilLongitudinal(Guid iIdSol)
            : base(iIdSol, KSolPlanta, KColor)
        {


        }

    }
    /// <summary>
    /// CAPA PERFIL TERRENO CIVIL3D
    /// </summary>
    public class oTadilLayerPerfilTerreno : oTadilLayer
    {

        private const string KSolPlanta = "_Tadil_Sol_{0}_PerfilTerreno";
        private const short KColor = oColor.cAmarilloIndex;

        public oTadilLayerPerfilTerreno(string iSolName)
            : base(iSolName, KSolPlanta, KColor)
        {

        }

        public oTadilLayerPerfilTerreno(Guid iIdSol)
            : base(iIdSol, KSolPlanta, KColor)
        {


        }

    }
    /// <summary>
    /// CAPA PERFIL TERRENO CIVIL3D
    /// </summary>
    public class oTadilLayerPerfilRasante : oTadilLayer
    {

        private const string KSolPlanta = "_Tadil_Sol_{0}_PerfilRasante";
        private const short KColor = oColor.cVerdeIndex;

        public oTadilLayerPerfilRasante(string iSolName)
            : base(iSolName, KSolPlanta, KColor)
        {

        }

        public oTadilLayerPerfilRasante(Guid iIdSol)
            : base(iIdSol, KSolPlanta, KColor)
        {


        }

    }
    /// <summary>
    /// CAPA SECCIONES PROYECTO
    /// </summary>
    public class oTadilLayerSecciones : oTadilLayer
    {
        private const string KSolPlanta = "_Tadil_Sol_{0}_Secciones";
        private const short KColor = oColor.cAmarilloIndex;

        public oTadilLayerSecciones(string iSolName)
            : base(iSolName, KSolPlanta, KColor)
        {

        }

        public oTadilLayerSecciones(Guid iIdSol)
            : base(iIdSol, KSolPlanta, KColor)
        {


        }

    }
    /// <summary>
    /// CAPA CONTIENE EL DIBUJO EN PLANTA
    /// </summary>
    public class oTadilLayerPlanta : oTadilLayer
    {
        private const string KSolPlanta = "_Tadil_Sol_{0}_Planta";
        private const short KColor = oColor.cAmarilloIndex;

        public oTadilLayerPlanta(string iSolName)
            : base(iSolName, KSolPlanta, KColor)
        {

        }

        public oTadilLayerPlanta(Guid iIdSol)
            : base(iIdSol, KSolPlanta, KColor)
        {


        }
    }
    /// <summary>
    /// CAPA CONTIENE LA POLILINEA BASE PARA LA EXPROPIACION
    /// </summary>
    public class oTadilLayerDominioPublicoAdyacente : oTadilLayer
    {
        private const string KSolPlanta = "_Tadil_Sol_{0}_DominioPublicoAdyacente";
        private const short KColor = oColor.cRojoIndex;




        public oTadilLayerDominioPublicoAdyacente(string iSolName)
            : base(iSolName, KSolPlanta, KColor)
        {


        }

        public oTadilLayerDominioPublicoAdyacente(Guid iSolId)
            : base(iSolId, KSolPlanta, KColor)
        {


        }


    }
    /// <summary>
    /// CAPA CONTIENE LA SUPERFICIE DE LA EXPROPIACION
    /// </summary>
    public class oTadilLayerExpropiacion : oTadilLayer
    {
        private const string KSolPlanta = "_Tadil_Sol_{0}_Expropiaciones";
        private const short KColor = oColor.cGrisClaroIndex;


        public oTadilLayerExpropiacion(string iSolName)
            : base(iSolName, KSolPlanta, KColor)
        {


        }

        public oTadilLayerExpropiacion(Guid iIdsol)
            : base(iIdsol, KSolPlanta, KColor)
        {


        }
    }

}
