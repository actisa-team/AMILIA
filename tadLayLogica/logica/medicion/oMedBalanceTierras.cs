using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using engNet.CustomAtributos;
using tadLayLan;

namespace tadLayLogica.logica.medicion
{

    using engNet.Extension.Double;

    using tadLayLogica.datos.precios;
    using tadLayLogica.datos.Gis;
    using tadLayData;


  public class oExcavacionBalanceTierras
    {

        //EXC MAT
        private dsBd.tbMaterialesRow mExcMat;
 
        //EXC M3 Prestado
        private double mM3Prestados = 0;

        
    
        private Guid? mGranularMat = null;
        private double? mGranularM3Disponible = null;

        private Guid? mAsientoMat = null;
        private double? mAsientoM3Disponible = null;

        private Guid? mTerraplenMat = null;
        private double? mTerraplenM3Disponible = null;
        private double? mTerraplenAproPc = null;


        #region "Constructores"





        public oExcavacionBalanceTierras(dsBd.tbMaterialesRow iRowMat,
                                           double iM3Excavacion,
                                           double iCoefEsponjamiento,
                                           double iCoefPasoTerraplen,
                                           Guid? iGranularAproMat,
                                           double? iGranularAproPc,
                                           Guid? iAsientoAproMat,
                                           double? iAsientoAproPc,
                                           Guid? iTerraplenAproMat,
                                           double? iTerraplenAproPc)
        {


            mExcMat = iRowMat;

            this.m3Excavacion = iM3Excavacion;

            this.coeficienteEsponjamiento = iCoefEsponjamiento;

            this.coeficientePasoTerraplen = iCoefPasoTerraplen;




            //Banco Capa Granular
            if (iGranularAproMat == null || iGranularAproPc == null || iGranularAproPc == 0)
            {
                mGranularMat = null;
                mGranularM3Disponible = 0;
            }
            else
            {
                mGranularMat = iGranularAproMat.Value;
                mGranularM3Disponible = this.coeficientePasoTerraplen * (iGranularAproPc.Value / 100) * this.m3Excavacion;
            }

            //Banco Capa Firme
            if (iAsientoAproMat == null || iAsientoAproPc == null || iAsientoAproPc == 0)
            {
                mAsientoMat = null;
                mAsientoM3Disponible = 0;
            }
            else
            {
                mAsientoMat = iAsientoAproMat.Value;
                mAsientoM3Disponible = this.coeficientePasoTerraplen * (iAsientoAproPc.Value / 100) * this.m3Excavacion;
            }

            //Banco Rellenos
            if (iTerraplenAproMat == null || iTerraplenAproMat == null || iTerraplenAproPc == 0)
            {
                mTerraplenMat = null;
                mTerraplenM3Disponible = 0;
                mTerraplenAproPc=0;
            }
            else
            {
                mTerraplenMat = iTerraplenAproMat.Value;
                mTerraplenM3Disponible = this.coeficientePasoTerraplen * (iTerraplenAproPc.Value / 100) * this.m3Excavacion;
                mTerraplenAproPc = iTerraplenAproPc;
            }

            //Material No Usado // Directamente Vertedero
            this.directoVertederoPc = 100 - mTerraplenAproPc.Value;

        }

        #endregion

        #region "Propiedades Publicas"

        /// <summary>
        /// Nombre Zona Geotécnia
        /// </summary>
        [BindingInfo(SortIndex = 1)]
        [LocalizedDisplayName("uimaterialExcavacion", typeof(strGeneral))]
        public string materialExcavacion { get { return mExcMat.nombre; } }
        /// <summary>
        /// M3 EXCAVACION
        /// </summary>
        [BindingInfo(SortIndex = 2)]
        [LocalizedDisplayName("uim3Excavacion", typeof(strGeneral))]
        public double m3Excavacion { get; private set; }
        /// <summary>
        /// Coefieciente de Esponjamiento
        /// </summary>
        [BindingInfo(SortIndex = 3)]
        [LocalizedDisplayName("uicoeficienteEsponjamiento", typeof(strGeneral))]
        public double coeficienteEsponjamiento { get; private set; }
        /// <summary>
        /// Coeficiente Paso Terraplen
        /// </summary>
        [BindingInfo(SortIndex = 4)]
        [LocalizedDisplayName("uicoeficientePasoTerraplen", typeof(strGeneral))]
        public double coeficientePasoTerraplen { get; private set; }
        /// <summary>
        /// Porcentaje Directo a Vertedero
        /// </summary>
        [BindingInfo(SortIndex = 5)]
        [LocalizedDisplayName("uidirectoVertederoPc", typeof(strGeneral))]
        public double directoVertederoPc { get; private set; }
        /// <summary>
        /// M3 TOTALES A VERTEDERO
        /// </summary>
        [BindingInfo(SortIndex = 6)]
        [LocalizedDisplayName("uim3TotalToVertedero", typeof(strGeneral))]
        public double m3TotalToVertedero
        {
            get
            {
                return this.m3SobradoToVertedero + this.m3DirectoVertedero;
            }
        }
        /// <summary>
        /// M3 MATERIAL DIRECTO VERTEDERO
        /// </summary>
        [BindingInfo(SortIndex = 7)]
        [LocalizedDisplayName("uim3DirectoVertedero", typeof(strGeneral))]
        public double m3DirectoVertedero
        {
            get
            {
                return this.m3Excavacion * this.coeficienteEsponjamiento * (this.directoVertederoPc/100);
            }

        }
        /// <summary>
        /// M3 SOBRADO A VERTEDERO
        /// </summary>
        [BindingInfo(SortIndex = 8)]
        [LocalizedDisplayName("uim3SobradoToVertedero", typeof(strGeneral))]
        public double m3SobradoToVertedero
        {
            get
            {
                double miM3 = (this.m3PrestadosMaximo - this.mM3Prestados) / this.coeficientePasoTerraplen;
                double miM3ConEsponjamiento = miM3 * this.coeficienteEsponjamiento;
                
                return miM3ConEsponjamiento;
            }


        }
        /// <summary>
        /// M3 MAXIMO MATERIAL A PRESTAR
        /// </summary>
        [BindingInfo(SortIndex = 9)]
        [LocalizedDisplayName("uim3PrestadosMaximo", typeof(strGeneral))]
        public double m3PrestadosMaximo
        {
            get
            {
                return this.m3Excavacion * this.coeficientePasoTerraplen *  (this.mTerraplenAproPc.Value / 100)  ;
            }
        }
        /// <summary>
        /// PRECIO VERTEDERO
        /// </summary>
        [BindingInfo(SortIndex = 10)]
        [LocalizedDisplayName("uiprecioVertedero", typeof(strGeneral))]
        public double precioVertedero
        {
            get
            {
                return mExcMat.precioSecundario;
            }
        }
        /// <summary>
        /// M3 PRESTADOS
        /// </summary>
        [BindingInfo(SortIndex = 11)]
        [LocalizedDisplayName("uim3Prestados", typeof(strGeneral))]
        public double m3Prestados
        {
            get
            {
                return mM3Prestados;
            }
        }
        /// <summary>
        /// GRANULAR DISPONIBLE
        /// </summary>
        [BindingInfo(SortIndex = 12)]
        [LocalizedDisplayName("uim3GranularDisponible", typeof(strGeneral))]
        public double m3GranularDisponible
        {
            get
            {
                return mGranularM3Disponible.Value;
            }
        }
        /// <summary>
        /// M3 ASIENTO DISPONIBLE
        /// </summary>
        [BindingInfo(SortIndex = 13)]
        [LocalizedDisplayName("uim3AsientoDisponible", typeof(strGeneral))]
        public double m3AsientoDisponible
        {
            get
            {
                return mAsientoM3Disponible.Value;
            }
        }
        /// <summary>
        /// M3 TERRAPLEN DISPONIBLE
        /// </summary>
        [BindingInfo(SortIndex = 14)]
        [LocalizedDisplayName("uim3TerraplenDisponible", typeof(strGeneral))]
        public double m3TerraplenDisponible
        {
            get
            {
                return mTerraplenM3Disponible.Value;
            }
        }

     

        #endregion


        #region "Metodos Publicos"
        public double prestarGranular(Guid idMatGranular, double iGranularM3Necesidad)
        {

            double miGranularPrestamo = 0;

            if (mGranularMat == null | mGranularM3Disponible == null | mGranularM3Disponible == 0)
            {
                return 0;
            }

            //Puedo Prestar Material
            if (idMatGranular == mGranularMat.Value)
            {
                if (mGranularM3Disponible.Value < 0)
                {
                    throw new Exception(string.Format("mFirmeM3 es Menor de 0 {0}", mGranularM3Disponible.Value.ToString()));
                }


                //Determino la Cuantia del Prestamo
                miGranularPrestamo = solicitarGranular(iGranularM3Necesidad);

                //Añado Material Medicion
                return miGranularPrestamo;

            }
            else
            {
                return 0;

            }

        }
        public double prestarAsiento(Guid idMatAsiento, double iAsientoM3Necesidad)
        {

            double miAsientoPrestamo = 0;

            if (mAsientoMat == null | mAsientoM3Disponible == null | mAsientoM3Disponible == 0)
            {
                return 0;
            }



            if (idMatAsiento == mAsientoMat.Value)
            {
                if (mAsientoM3Disponible.Value < 0)
                {
                    throw new Exception(string.Format("mAsientoM3 es Menor de 0 {0}", mAsientoM3Disponible.Value.ToString()));
                }

                //Determino la Cuantia del Prestamo
                miAsientoPrestamo = solicitarAsiento(iAsientoM3Necesidad);

                //Añado Material Medicion
                return miAsientoPrestamo;
            }
            else
            {
                return 0;
            }


        }
        public double prestarTerraplen(Guid idMatRelleno, double iRellenoM3Necesidad)
        {

            double miRellenoPrestamo = 0;

            if (mTerraplenMat == null | mTerraplenM3Disponible == null | mTerraplenM3Disponible == 0)
            {
                return 0;
            }



            if (idMatRelleno == mTerraplenMat.Value)
            {
                if (mTerraplenM3Disponible.Value < 0)
                {
                    throw new Exception(string.Format("mRellenoM3 es Menor de 0 {0}", mTerraplenM3Disponible.Value.ToString()));
                }

                //Determino la Cuantia del Prestamo
                miRellenoPrestamo = solicitarTerraplen(iRellenoM3Necesidad);

                //Añado Material Medicion
                return miRellenoPrestamo;
            }
            else
            {
                return 0;
            }


        }

        public List<oMedExcavacion> createEmpleoVertedero()
        {

            if (mM3Prestados -1 > m3PrestadosMaximo)
            {
                throw new Exception(string.Format("Error Balance Tierras\n m3 Prestados {0}\n m3 Prestamo Máximo {1}", mM3Prestados, m3PrestadosMaximo));
            }

            List<oMedExcavacion> miLstExca = new List<oMedExcavacion>();


            #region "MATERIAL DIRECTO VERTEDERO"

            //M3 A VERTEDERO DIRECTO
            oMedExcavacion miExcavacionDirectoVertedero;
            miExcavacionDirectoVertedero = new oMedExcavacion(mExcMat, this.m3DirectoVertedero, false);
            miLstExca.Add(miExcavacionDirectoVertedero);

            #endregion


            #region "MATERIAL NO USADO & USADO"

            oMedExcavacion miExca;

            //HE PRESTADO TODO EL MATERIAL
            if (mM3Prestados >= this.m3PrestadosMaximo)
            {
                miExca = new oMedExcavacion(mExcMat, mM3Prestados, true);
                miLstExca.Add(miExca);
            }
            else
            {
                //Prestamo
                if (mM3Prestados > 0)
                {
                    miExca = new oMedExcavacion(mExcMat, mM3Prestados, true);
                    miLstExca.Add(miExca);
                }

                //aVertedero
                if (this.m3SobradoToVertedero > 0)
                {
                    miExca = new oMedExcavacion(mExcMat, this.m3SobradoToVertedero, false);
                    miLstExca.Add(miExca);
                }
                else
                {
                    throw new Exception("Error CreateEmpleoVertedero");
                }

            }

            #endregion

     
            return miLstExca;
        }




        #endregion


        #region "Metodos Privados"
        private double solicitarGranular(double iGranularM3Necesidad)
        {

            if (mGranularM3Disponible >= iGranularM3Necesidad)
            {
                //Saco el Material
                sacarMaterialGranular(iGranularM3Necesidad);

                return iGranularM3Necesidad;
            }
            else if (iGranularM3Necesidad > mGranularM3Disponible)
            {
                double miOut = mGranularM3Disponible.Value;

                sacarMaterialGranular(mGranularM3Disponible.Value);

                return miOut;
            }
            else
            {
                throw new NotImplementedException("Solicitar Granular");
            }

        }
        private double solicitarAsiento(double iAsientoM3Necesidad)
        {

            if (mAsientoM3Disponible >= iAsientoM3Necesidad)
            {
                //Saco el Material
                sacarMaterialAsiento(iAsientoM3Necesidad);

                return iAsientoM3Necesidad;
            }
            else if (iAsientoM3Necesidad > mAsientoM3Disponible)
            {
                double miOut = mAsientoM3Disponible.Value;

                sacarMaterialAsiento(mAsientoM3Disponible.Value);

                return miOut;
            }
            else
            {
                throw new NotImplementedException("Solicitar Asiento");
            }

        }
        private double solicitarTerraplen(double iTerraplenM3Necesidad)
        {

            if (mTerraplenM3Disponible >= iTerraplenM3Necesidad)
            {
                //Saco el Material
                sacarMaterialTerraplen(iTerraplenM3Necesidad);

                return iTerraplenM3Necesidad;
            }
            else if (iTerraplenM3Necesidad > mTerraplenM3Disponible)
            {
                double miOut = mTerraplenM3Disponible.Value;

                sacarMaterialTerraplen(mTerraplenM3Disponible.Value);

                return miOut;
            }
            else
            {
                throw new NotImplementedException("Solicitar Terraplen");
            }

        }


        private void sacarMaterialGranular(double iGranularM3out)
        {
            mGranularM3Disponible = mGranularM3Disponible - iGranularM3out;
            mAsientoM3Disponible = mAsientoM3Disponible - iGranularM3out;
            mTerraplenM3Disponible = mTerraplenM3Disponible - iGranularM3out;

            mM3Prestados = mM3Prestados + iGranularM3out;
        }
        private void sacarMaterialAsiento(double iAsientoM3out)
        {
            mAsientoM3Disponible = mAsientoM3Disponible - iAsientoM3out;
            mTerraplenM3Disponible = mTerraplenM3Disponible - iAsientoM3out;

            mM3Prestados = mM3Prestados + iAsientoM3out;


        }
        private void sacarMaterialTerraplen(double iRellenoM3out)
        {
            mTerraplenM3Disponible = mTerraplenM3Disponible - iRellenoM3out;

            mM3Prestados = mM3Prestados + iRellenoM3out;
        }
        #endregion


    }

  public class oClientesBalanceTierras
    {


        private dsBd.tbMaterialesRow mMatRelleno;

        /// <summary>
        /// Necesidad de Material
        /// </summary>
        private double mM3Necesidad;

        /// <summary>
        /// Material que tomo de la Obra
        /// </summary>
        private double mM3Empleo;


        public oClientesBalanceTierras(dsBd.tbMaterialesRow  iMatRelleno, double iM3Necesidad)                       
        {
            mMatRelleno = iMatRelleno;
            mM3Necesidad = iM3Necesidad;
            mM3Empleo = 0;
        }


        #region "Propiedades"

        /// <summary>
        /// Nombre del Material
        /// </summary>
        [BindingInfo(SortIndex = 1)]
        [LocalizedDisplayName("uinameMaterial", typeof(strGeneral))]
        public string nombre { get { return mMatRelleno.nombre;}}

        /// <summary>
        /// M3 MATERIAL NECESIDAD
        /// </summary>
        [BindingInfo(SortIndex = 2)]
        [LocalizedDisplayName("uim3Necesidad", typeof(strGeneral))]
        public double m3Necesidad
        {
          get
            {
                return mM3Necesidad;
            }
        }


        /// <summary>
        /// M3 MATERIAL TOMADO OBRA
        /// </summary>
        [BindingInfo(SortIndex = 3)]
        [LocalizedDisplayName("uim3Empleo", typeof(strGeneral))]
        public double m3Empleo
      {

          get
          {
              return mM3Empleo;
          }
      }

        /// <summary>
        /// M3 MATERIAL QUE NECESITO
        /// </summary>
        [BindingInfo(SortIndex = 4)]
        [LocalizedDisplayName("uim3Prestamo", typeof(strGeneral))]
        public double m3Prestamo
        {
            get
            {
                return (mM3Necesidad - mM3Empleo);
            }       
        }
        /// <summary>
        /// PRECIO EMPLEO
        /// </summary>
        [BindingInfo(SortIndex = 5)]
        [LocalizedDisplayName("uiprecioEmpleo", typeof(strGeneral))]
        public double precioEmpleo
        {
            get
            {
                return mMatRelleno.precioPrincipal;
            }
        }

        /// <summary>
        /// PRECIO PRESTAMO
        /// </summary>
        [BindingInfo(SortIndex = 6)]
        [LocalizedDisplayName("uiprecioPrestamo", typeof(strGeneral))]
        public double precioPrestamo
        {
            get
            {
                return mMatRelleno.precioSecundario;
            }
        }

        /// <summary>
        /// ID MATERIAL
        /// </summary>
        [BindingInfo(false)]
        [LocalizedDisplayName("uiprecioPrestamo", typeof(strGeneral))]
        public Guid idMaterial
        {
            get
            {
                return mMatRelleno.idMaterial;
            }
        }
        

        /// <summary>
        /// ESTA COMPLETO
        /// </summary>
        public bool isComplete()
        {
            
            if (this.m3Prestamo<0.1)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }

        #endregion

        #region "Metodos"

        public void addM3Empleo(double iM3Empleo)
        {
            mM3Empleo = mM3Empleo + iM3Empleo;       
        }



        public List<oMedItemModel> createEmpleoPrestamo(eNodo iNodo)
        {

            List<oMedItemModel> miLstRellenos = new List<oMedItemModel>();

            oMedItemModel miRellenoEmpleo;
            oMedItemModel miRellenoPrestamo;

            //Todo EMPLEO
            if (this.isComplete())
            {      
                 miRellenoEmpleo = oFactoryPartidas.createMedicionesPartidasBalanceTierras(mMatRelleno,mM3Empleo,true,iNodo);
                 miLstRellenos.Add(miRellenoEmpleo);      
            }
            //TODO PRESTAMO
            else if (mM3Empleo ==0)
            {
                miRellenoPrestamo = oFactoryPartidas.createMedicionesPartidasBalanceTierras(mMatRelleno, m3Prestamo, false, iNodo);
                miLstRellenos.Add(miRellenoPrestamo);
            }
            //PARTE EMPLEO ; PARTE PRESTAMO
            else
            {
                miRellenoEmpleo = oFactoryPartidas.createMedicionesPartidasBalanceTierras(mMatRelleno, mM3Empleo, true, iNodo);
                miRellenoPrestamo = oFactoryPartidas.createMedicionesPartidasBalanceTierras(mMatRelleno, m3Prestamo, false, iNodo);

                miLstRellenos.Add(miRellenoEmpleo);
                miLstRellenos.Add(miRellenoPrestamo);
            
            }
        

            return miLstRellenos;
              
        }

        #endregion
    }





}
