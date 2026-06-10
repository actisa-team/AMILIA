using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.EjeBasicoNew
{

	using Autodesk.AutoCAD.Geometry;
	using Autodesk.AutoCAD.DatabaseServices;
	using Autodesk.AutoCAD.Colors;

	using engCadNet;

	using engNet.Extension.Double;
	using tadLayShare.puntos;

	using tadLayLogica.zonaGis;
    using tadLayLogica.estudioTipo;

	using engNet.eventos;
    using tadLayShare;
	using tadLayLan;





	public class oTramoEjeBasico
	{

		private static  Func<double[], double?> mFunGetZterreno = null;
		private static  Func<double[], double?> mFunGetSlopeTerreno = null;
        private static Func<double?, double?, bool> mFunIsPtoInsideTerreno = null;
        private static Func<double?, double?, bool> mFunIsPtoCercaBordesTerreno = null;
		private static  Func<IP2d, IP2d, bool> mFunIsTramoOnZonaNoPaso = null;





		//TRAMO BASICO
		private oP3d mP1 = null;
		private oP3d mP2 = null;

		//TRAMO ABANICO
		private int? mIdAbanico = null;

		private bool mIsTramoValido = true;

		private eTramoEjeBasicoError? mErrorTramo = null;

		private int? mIdTramo = null;

		private double? mLongitudDiscretizacionProyecto = null;
		private double? mLongitudDiscretizacionCalculo = null;
	  
		private eTramoTipoEjeBasico? mTramoTipoEjeBasico = null;
        


		#region "Constructor"
		public oTramoEjeBasico ()
		{

		}
		public oTramoEjeBasico (int iIdAbanico, int iIdTramo, oP3d iP1, double iLongitud, double iAzimutGrados,eTramoTipoEjeBasico iTipoTramo)
		{

			this.idAbanico = iIdAbanico;
			this.idTramo = iIdTramo;
			this.P1 = iP1;
			IP2d miP2 =  mP1.getPointFromAzimutAndLongitud(iAzimutGrados, iLongitud);
			/*
			 * No tenia añadido el z ** juanma **
			 */
			double[] punto = new double[2];
			punto[0] = miP2.X;
			punto[1] = miP2.Y;
			mP2 = new oP3d(miP2.X, miP2.Y, GetZ(punto));

			this.tramoTipoEjeBasico = iTipoTramo;

		}
		public oTramoEjeBasico(oP3d iP1, oP3d iP2, eTramoTipoEjeBasico iTipoTramo)
		{
			this.P1 = iP1;
			this.P2= iP2;
			this.tramoTipoEjeBasico = iTipoTramo;
		}
		public oTramoEjeBasico(int iIdAbanico,int iIdTramo,  oP3d iP1, oP3d iP2, eTramoTipoEjeBasico iTipoTramo)
		{
			this.idAbanico = iIdAbanico;
			this.idTramo = iIdTramo;
			this.P1 = iP1;
			this.P2 = iP2;
			this.tramoTipoEjeBasico = iTipoTramo;
		}
		#endregion

		#region "Propiedades"

		/// <summary>
		/// ID DE ABANICO
		/// </summary>
		public int idAbanico
		{
			get
			{
				if (mIdAbanico == null)
				{
					throw new oExPropertieNullValue("Id Abanico");
				}

				return mIdAbanico.Value;
			}

			set
			{
				mIdAbanico = value;
			}
		}

		public oP3d P1
		{
			get
			{
				if (mP1 == null)
				{
					throw new oExPropertieNullValue("P1");
				}

				return mP1;
			}


			set
			{
				mP1 = value;
			}
		
		}
		public oP3d P2
		{
			get
			{
				if (mP2 == null)
				{
					throw new oExPropertieNullValue("P2");
				}

				return mP2;
			}

		   set
			{
				mP2 = value;
			}
		}
		public double longitud2d
		{
			get
			{
				return this.P1.distTo2d(this.P2);
			}
		}
		public double longitud3d
		{
			get
			{
				return this.P1.distTo3d(this.P2);
			}
		}
		public double azimutGrados
		{     
			get
			{
				if (!this.P1.isNull && !this.P2.isNull)
				{
					return oTrigo.getAzimutGrados(this.P1, this.P2);
				}
				else
				{
					throw new NullReferenceException(this.ToString());
				}
			}

		 }
		public int idTramo
		{
			get
			{
				if (mIdTramo == null)
				{
					throw new oExPropertieNullValue("Id Tramo");
				}

				return mIdTramo.Value;
			}

		   set
			{
				mIdTramo = value;
			}


		}
		public bool isTramoValido
		{
			get
			{
				return mIsTramoValido;
			}

			set
			{
				mIsTramoValido = value;

			}
		}
		public eTramoEjeBasicoError errorTramo
		{

			get
			{
				return mErrorTramo.Value;             
			}

			set
			{
				mErrorTramo = value;
			}

		}
		public eTramoTipoEjeBasico tramoTipoEjeBasico
		{
			get
			{
				if (mTramoTipoEjeBasico == null)
				{
					throw new oExPropertieNullValue("Tramo Tipo Eje Básico");
				}

				return mTramoTipoEjeBasico.Value;

			}

			set
			{
				mTramoTipoEjeBasico = value;
			}
		}

	    public Polyline mTrazadoFinal;
        public double mCotaFinal;
        public double mPendienteUltimoTramo;
        public double mLongitudUltimoTramo;

		/// <summary>
		/// Pendiente Tramo P1 - P2xy (Z=Terreno)
		/// </summary>
		public double P1P2terrenoPendienteConSignoPC
		{
			get
			{
				return this.P1.getPendienteConSignoPC(this.P2terreno);
			}
		}
		/// <summary>
		/// P2 Z Terreno
		/// </summary>
		public oP3d P2terreno
		{
			get
			{
				double miZ = this.fGetZTerrenoFromXY(this.P2);
				return new oP3d(this.P2.X, this.P2.Y, miZ);
			   
			}
		}


		public double pendienteAbsolutaPC
		{
			get
			{
				return Math.Abs(this.pendienteConSignoPC);
			}
		}


		/// <summary>
		/// Pendiente del Tramo Con Signo [% 100]
		/// </summary>
		public double pendienteConSignoPC
		{
			get
			{
				return oTrigo.getPendiente3D(this.P1,this.P2,ePorcentaje.porCiento);
			}

		}
		/// <summary>
		/// Pendiente del Tramo Con Signo [% UNO]
		/// </summary>
		public double pendienteConSignoPU
		{ 
			get
			{
				return oTrigo.getPendiente3D(this.P1,this.P2,ePorcentaje.porUno);
			}
		}
		/// <summary>
		/// Longitud Discretización Tramo Metros (Secciones)
		/// </summary>


		public double lonDiscretizacionProyecto
		{
		   
			get
			{
				if (mLongitudDiscretizacionProyecto == null)
				{
					throw new oExPropertieNullValue("longitud Discretización Tramo");
				}

				return mLongitudDiscretizacionProyecto.Value;

			}

			 set
			{
				mLongitudDiscretizacionProyecto = value;
			}                
		}
		public double lonDiscretizacionCalculo
		{

			get
			{
				if (mLongitudDiscretizacionCalculo == null)
				{
					mLongitudDiscretizacionCalculo = this.getLongitudDiscretizacionCalculo();
				}

				return mLongitudDiscretizacionCalculo.Value;
			}

		}


		public oSeccionesEjeBasico seccion {get;set;}


		#endregion

		#region "Funciones Estaticas"

		/// <summary>
		/// Obtener la Pendiente del Terreno en un Punto
		/// </summary>
		private double fGetSlopeTerrenoFromXY(oP2d iPto)
		{
			if (mFunGetSlopeTerreno == null)
			{
				mFunGetSlopeTerreno = oSingletonTerreno.getInstance.getSlopeFromXY;
			}
			

			return mFunGetSlopeTerreno(iPto.toArray2d()).Value;
			
		}

        /// <summary>
        /// Obtener la Pendiente del Terreno en un Punto
        /// </summary>
        private double fGetSlopeTerrenoFromTriang(Triangulo iTriangulo,double x=-1,double y=-1)
        {
			try
			{
				if (oSingletonTerreno.getInstance.tipo == 1)
				{
					return (double)oSingletonTerreno.getInstance.getSlopeFromTriang(iTriangulo);
				}
				else if (oSingletonTerreno.getInstance.tipo == 2)
				{
					return (double)oSingletonPuntosTerreno.getInstance.getSlopeFromTriang(iTriangulo, x, y);
				}
				else if (oSingletonTerreno.getInstance.tipo == 3)
				{
					return (double)oSingletonPuntosTerrenoASC.getInstance.getSlopeFromTriang(iTriangulo,x,y);
				}
				else
				{
					return (double)oSingletonTerreno.getInstance.getSlopeFromTriang(iTriangulo);
				}

			}
			catch (Exception e)
			{
				return -1;
			}

        }
		/// <summary>
		/// Obtener la Z del Terreno en un Punto
		/// </summary>
		private double fGetZTerrenoFromXY(oP2d iPto)
		{

			/*if (mFunGetZterreno == null)
			{
				mFunGetZterreno = oSingletonTerreno.getInstance.getZFromXY; 

			}

			return mFunGetZterreno(iPto.toArray2d()).Value;*/
			double[] coords = new double[] { iPto.X, iPto.Y };
			return (double)GetZ(coords);
		}

        private double fGetZTerrenoFromTring(oP2d iPto, Triangulo iTriangulo)
        {
            /*double? miZ = oSingletonTerreno.getInstance.getZFromTriang(iPto.X, iPto.Y, iTriangulo);
            if(miZ==null)
            {
                miZ = oSingletonTerreno.getInstance.getZFromXY(iPto.toArray2d()).Value;
            }
            return (double)miZ;*/
			double[] coords = new double[] { iPto.X, iPto.Y };
			return (double)GetZ(coords);
		}
		/// <summary>
		/// Pto esta Dentro del Terreno
		/// </summary>
		private bool fIsPtoInsideTerreno (double? iX, double? iY)
		{

			if (mFunIsPtoInsideTerreno == null)
			{
				mFunIsPtoInsideTerreno = oSingletonTerreno.getInstance.isPtoInsideTerreno;
			}

			return mFunIsPtoInsideTerreno(iX, iY);

		}


        /// <summary>
        /// Pto esta Dentro del Terreno
        /// </summary>
        private bool fIsPtoCercaBordesTerreno(double? iX, double? iY)
        {

            if (mFunIsPtoCercaBordesTerreno == null)
            {
                mFunIsPtoCercaBordesTerreno = oSingletonTerreno.getInstance.isPtoCercaBorde;
            }

            return mFunIsPtoCercaBordesTerreno(iX, iY);

        }
		/// <summary>
		/// Is Tramo en Zona No Paso
		/// </summary>
		private bool fIsTramoOnZonaNoPaso (IP2d iP1, IP2d iP2)
		{

			if (mFunIsTramoOnZonaNoPaso == null)
			{
				mFunIsTramoOnZonaNoPaso = oSingletonZonaNoPaso.getInstance.isTramoOnZonaNoPaso; 
			}

			return mFunIsTramoOnZonaNoPaso(this.P1, this.P2);


		}

        #endregion


        /// <summary>
        /// Seccion Cuando Tenemos los P1 y P2
        /// </summary>
        public virtual void createSeccionP1P2(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes, IEstudio iEstudioData, bool isEntronqueEspecial)
        {
            mErrorTramo = eTramoEjeBasicoError.errorNoIdentificado;
            #region "Asignacion Variables"
            this.lonDiscretizacionProyecto = iLonDiscretizacionProyecto;
            this.mLongitudDiscretizacionCalculo = null;
            #endregion
            #region "Generamos la Seccion con Pendiente del Tramo"

            var tramoPendienteTijera = pendienteAbsolutaPC;
            ISeccionCalzada miSeccionCalzada = null;
            IzonaMovimientoTierras miZonaMovimentoTierras = null;
            IzonaPuentes miZonaPuentes = null;
            IzonaTuneles miZonaTuneles = null;
            bool isObligadoEstructura = false;
            Triangulo miTrianguloCercano = null;


            //Ahora Generamos la Seccion con la Pendiente de Proyecto.
            this.seccion = new oSeccionesEjeBasico();

            oSeccionEjeBasico miSeccion;

            //Puntos de la Sección
            List<oP3d> miLstPuntosSeccion = this.getLstPointSeccion();


            int miId = 0;

            foreach (var ptoSeccion in miLstPuntosSeccion)
            {
                miSeccionCalzada = iEstudioData.getISeccionCalzadaByPto(ptoSeccion);
                miZonaMovimentoTierras = iEstudioData.getIZonaMovimientoTierrasByPto(ptoSeccion);
                miZonaPuentes = iEstudioData.getIZonaPuenteByPto(ptoSeccion);
                miZonaTuneles = iEstudioData.getIZonaTunelByPto(ptoSeccion);
                isObligadoEstructura = iEstudioData.isOnZonaPasoObligadoEstructuras(ptoSeccion);

				/*
				 * Se cambia el if para que siempre coja el triangulo correcto con la nueva forma de buscar
				 */
				double[] mipunto = new double[2];
				mipunto[0] = ptoSeccion.X;
				mipunto[1] = ptoSeccion.Y;

				//todo lo que tenga **** se descomenta para segunda version
				//oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(mipunto);
				//miTrianguloCercano = oSingletonTerreno.getInstance.getTrianguloInside(ptoSeccion.X, ptoSeccion.Y);
				if (miTrianguloCercano == null)
                {
                    //miTrianguloCercano = oSingletonTerreno.getInstance.getTrianguloInside(ptoSeccion.X, ptoSeccion.Y);
					//****miTrianguloCercano = GetTriangulo(mipunto);

				}

				//miTrianguloCercano = oSingletonTerreno.getInstance.getTrianguloInsideFromTriangulo(ptoSeccion.X, ptoSeccion.Y, miTrianguloCercano);
				//****miTrianguloCercano = getTrianguloInsideFromTriangulo(mipunto, miTrianguloCercano);
				double miZ = 0;
                double miSlope = 0;

				/*
				 * Se comenta para la prueba de salvador para ver si es mas rapido buscar el punto triangulando o no
				 */
				/*if (miTrianguloCercano != null)
				{
					var result = oSingletonPuntosTerreno.getInstance.GetZ_Y_Slope(ptoSeccion.X, ptoSeccion.Y);
					miZ = result.Item1;
					miSlope = result.Item2;
				}
				else
				{

					this.isTramoValido = false;
					this.errorTramo = eTramoEjeBasicoError.puntoExteriorSuperficie;
					return;
				}*/
				/****
				if (miTrianguloCercano != null)
                {
                    //miZ = this.fGetZTerrenoFromTring(ptoSeccion, miTrianguloCercano);
                    //miSlope = this.fGetSlopeTerrenoFromTriang(miTrianguloCercano);
					miZ = (double)GetZ_triangulo(mipunto, miTrianguloCercano);
					miSlope = fGetSlopeTerrenoFromTriang(miTrianguloCercano);
				}
                else
                {

                    this.isTramoValido = false;
                    this.errorTramo = eTramoEjeBasicoError.puntoExteriorSuperficie;
                    return;
                }

				****/
				miZ = (double)this.GetZ(mipunto);
				miSlope = this.fGetSlopeTerrenoFromTriang(null, mipunto[0], mipunto[1]);
				if (double.IsNaN(miZ))
                {
					this.isTramoValido = false;
					this.errorTramo = eTramoEjeBasicoError.puntoExteriorSuperficie;
					return;
				}
				miSeccion = new oSeccionEjeBasico(miId,
                                                 ptoSeccion,
                                                 this.lonDiscretizacionCalculo,
                                                 miZ,
                                                 miSlope,
                                                 miSeccionCalzada,
                                                 miZonaMovimentoTierras,
                                                 miZonaPuentes,
                                                 miZonaTuneles,
                                                 isObligadoEstructura);



                this.seccion.Add(miSeccion);

                miId++;
            }


            #endregion
            #region "Validamos la Seccion Sin Estructuras"


            //Calculamos la Seccion Sin Estructuras
            this.seccion.calculateSeccionSinEstructuras();


            //Las Secciones son Validas ++ La pendiente es inferior a la de calzada cálculo
            if (!this.seccion.isSeccionesNoValidas() && tramoPendienteTijera <= iRoadPendientes.calzadaPendienteCalculoMaximoPC)
            {
                return;
            }
            if (tramoPendienteTijera > iRoadPendientes.calzadaPendienteCalculoMaximoPC)
            {
                this.isTramoValido = false;
                this.errorTramo = eTramoEjeBasicoError.pendienteTramoEspecialInvalida;
            }

            #endregion

            #region "Validamos la Seccion Con Estructuras"

            if (this.seccion.allowEstructuras())
            {
                //Calculo la Seccion Con Estructuras
                this.seccion.calcultateSeccionConEstructuras();

                if (tramoPendienteTijera > iRoadPendientes.estructuraPendienteCalculoMaximoPC)
                {
                    this.isTramoValido = false;
                    this.errorTramo = eTramoEjeBasicoError.pendienteTramoEspecialInvalida;
                }

                if (seccion.isSeccionesNoValidas() && this.errorTramo != eTramoEjeBasicoError.pendienteTramoEspecialInvalida)
                {
                    if (isEntronqueEspecial)
                    {
                        foreach (oSeccionEjeBasico item in this.seccion)
                        {
                            this.isTramoValido = true;
                            if (item.seccionTipo == null)
                            {
                                if (item.excavacionTipo == eExcavacion.terraplen)
                                {
                                    item.seccionTipo = eRoadSeccion.puente;
                                }
                                else
                                {
                                    item.seccionTipo = eRoadSeccion.tunel;
                                }
                            }
                        }


                    }
                    else
                    {
                        this.isTramoValido = false;
                        this.errorTramo = eTramoEjeBasicoError.alturaMovimientoTierrasConEstructuras;
                    }

                }

            }
            else
            {

                if (isEntronqueEspecial && this.errorTramo != eTramoEjeBasicoError.pendienteTramoEspecialInvalida)
                {
                    foreach (oSeccionEjeBasico item in this.seccion)
                    {
                        this.isTramoValido = true;
                        if (item.seccionTipo == null)
                        {
                            item.seccionTipo = eRoadSeccion.calzada;
                        }
                    }

                }
                else
                {
                    this.isTramoValido = false;
                    this.errorTramo = eTramoEjeBasicoError.alturaMovimientoTierrasSinEstructuras;
                }


            }

            #endregion

        }

		public double? GetZ_triangulo(double[] punto, Triangulo t)
		{

			try
			{
				if (oSingletonTerreno.getInstance.tipo == 1)
				{
					return (double)oSingletonTerreno.getInstance.getZFromTriang(punto[0], punto[1], t);
				}
				else if (oSingletonTerreno.getInstance.tipo == 2)
				{
					return (double)oSingletonPuntosTerreno.getInstance.getZFromTriang(punto[0], punto[1], t);
				}
				else if (oSingletonTerreno.getInstance.tipo == 3)
				{
					return (double)oSingletonPuntosTerrenoASC.getInstance.getZFromTriang(punto[0], punto[1], t);
				}
				else
				{
					return (double)oSingletonTerreno.getInstance.getZFromTriang(punto[0], punto[1], t);
				}

			}
			catch (Exception e)
			{
				return null;
			}
		}
		public Triangulo GetTriangulo(double[] punto)
		{
			if (oSingletonTerreno.getInstance.tipo == 1)
			{
				return oSingletonTerreno.getInstance.getTrianguloInside(punto[0], punto[1]);
			}
			else if (oSingletonTerreno.getInstance.tipo == 2)
			{
				return oSingletonPuntosTerreno.getInstance.getTrianguloInside(punto[0], punto[1]);
			}
			else if (oSingletonTerreno.getInstance.tipo == 3)
			{
				return oSingletonPuntosTerrenoASC.getInstance.getTrianguloInside(punto[0], punto[1]);
			}
			else
			{
				return oSingletonTerreno.getInstance.getTrianguloInside(punto[0], punto[1]);
			}
			return null;
		}
		public Triangulo getTrianguloInsideFromTriangulo(double[] punto, Triangulo t)
		{
			if (oSingletonTerreno.getInstance.tipo == 1)
			{
				return oSingletonTerreno.getInstance.getTrianguloInsideFromTriangulo(punto[0], punto[1],t);
			}
			else if (oSingletonTerreno.getInstance.tipo == 2)
			{
				return oSingletonPuntosTerreno.getInstance.getTrianguloInsideFromTriangulo(punto[0], punto[1],t);
			}
			else if (oSingletonTerreno.getInstance.tipo == 3)
			{
				return oSingletonPuntosTerrenoASC.getInstance.getTrianguloInsideFromTriangulo(punto[0], punto[1],t);
			}
			else
			{
				return oSingletonTerreno.getInstance.getTrianguloInsideFromTriangulo(punto[0], punto[1],t);
			}
			return null;
		}
		public double? GetZ(double[] punto)
		{
			/*
             * Funciona correctamente el 3 para que busque dentro del rango de los puntos
             */
			if (oSingletonTerreno.getInstance.tipo == 1)
			{
				return oSingletonTerreno.getInstance.getZFromXY(punto);
			}
			else if (oSingletonTerreno.getInstance.tipo == 2)
			{
				return oSingletonPuntosTerreno.getInstance.GetZ(punto);
			}
			else if (oSingletonTerreno.getInstance.tipo == 3)
			{
				return oSingletonPuntosTerrenoASC.getInstance.GetZ(punto);
			}
			else
			{
				return oSingletonTerreno.getInstance.getZFromXY(punto);
			}
			return -1;
		}

		/// <summary>
		/// Seccion Tramos Iniciales y Finales, donde se permiten que no cumplan las condiciones de diseño.
		/// </summary>
		public virtual void createSeccionTramoInicialFinal(double iLonDiscretizacionProyecto, double iPendienteTramo, IEstudio iEstudioData)
		{
			#region "Validacion"
			if (!this.isTramoValido)
			{
				return;
			}
			#endregion

			#region "Asignacion Variables"
			this.lonDiscretizacionProyecto = iLonDiscretizacionProyecto;
			this.mLongitudDiscretizacionCalculo = null;
			#endregion

			#region "Ajuste Pendiente Sin Estructuras"
			this.P2.Z = this.getP2zFromPendiente(iPendienteTramo);
			#endregion

			#region "Generamos la Seccion con Pendiente de Salida"


			//Puntos de la Sección
			List<oP3d> miLstPuntosSeccion = this.getLstPointSeccion();

			this.seccion = this.getListadoSecciones(miLstPuntosSeccion, iEstudioData);


			#endregion

			#region "Validamos la Seccion

			//Calculamos la Seccion Sin Estructuras
			this.seccion.calculateSeccionSinEstructuras();


			if (this.seccion.isSeccionesNoValidas())
			{

				if (this.seccion.allowEstructuras())
				{
					//Modifico la Pendiente pasamos de Pendiente de Carretera a Estructuras
                    //Angeles, modificacion ya que en el tramo de entrada y de salida no se pueden cambiar las pendientes
					//this.P2.Z = getP2ZFromPendienteMaximaAbsoluta(iPendienteTramo);

					//Obtego los nuevos puntos de la sección
					List<oP3d> miLstPuntosSeccionConEstructuras = this.getLstPointSeccion();

					//Actualizo los Puntos de la Seccion
					this.seccion = this.getListadoSecciones(miLstPuntosSeccionConEstructuras, iEstudioData);

					//Calculo la Seccion Con Estructuras
					this.seccion.calcultateSeccionConEstructuras();

					if (seccion.isSeccionesNoValidas())
					{
						this.isTramoValido = false;
						this.errorTramo = eTramoEjeBasicoError.alturaMovimientoTierrasConEstructuras;
					}
				}
				else
				{
					this.isTramoValido = false;
					this.errorTramo = eTramoEjeBasicoError.alturaMovimientoTierrasSinEstructuras;
				}

			}



		



			#endregion
		}





	 

		/// <summary>
		/// Seccion con distintas Posibilidades de Pendiente (Terreno-ProyectoRoad-ProyectoEstructuras)
		/// </summary>
		public virtual void createSeccion (double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes,  IEstudio iEstudioData, bool isTramoEspecial)
		{
            if(iEstudioData.isTramoObligadoAPuente(new oP2d(mP1.X, mP1.Y), new oP2d(mP2.X, mP2.Y)))
            {
                createSeccionObligadoPuente(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData);
            }
            else if (iEstudioData.isTramoObligadoAPuenteoTunel(new oP2d(mP1.X, mP1.Y), new oP2d(mP2.X, mP2.Y)))
            {
                createSeccionObligadoPuenteoTunel(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData);
            }
            else
            {
                createSeccionNormal(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData, isTramoEspecial);
            }

		}



        /// <summary>
        /// Seccion con distintas Posibilidades de Pendiente (Terreno-ProyectoRoad-ProyectoEstructuras)
        /// </summary>
        public virtual void createSeccion(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes, IEstudio iEstudioData, bool isTramoEspecial, bool isEntronque, oP3d iPuntoFinal)
        {
            if (iEstudioData.isTramoObligadoAPuente(new oP2d(mP1.X, mP1.Y), new oP2d(mP2.X, mP2.Y)))
            {
                createSeccionObligadoPuente(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData);
            }
            else if (iEstudioData.isTramoObligadoAPuenteoTunel(new oP2d(mP1.X, mP1.Y), new oP2d(mP2.X, mP2.Y)))
            {
                createSeccionObligadoPuenteoTunel(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData);
            }
            else
            {
                createSeccionNormal(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData, isTramoEspecial, isEntronque, iPuntoFinal);
            }

        }


		



		/// <summary>
		/// Angulo P1-P2 y Azimut Tramo Siguiente
		/// </summary>
		public double getAnguloConTramoNext(double iAzimutGradosTramoNext, eAng iAnguloFormato)
		{
			return oTrigo.geAngFromTwoPointsAndAzimut(this.P1, this.P2, iAzimutGradosTramoNext, iAnguloFormato);
		}


		public double getP2ZFromP1(double iLonFromOrigen)
		{
			return (this.P1.Z) + iLonFromOrigen * this.pendienteConSignoPU;
		}

	

		#region "VALIDACIONES"

		public void validarTramoP2InsideTerreno()
		{
			if (this.isTramoValido)
			{
				double[] punto = new double[2];
				punto[0] = this.P2.X;
				punto[1] = this.P2.Y;
                //if (!oSingletonPuntosTerreno.getInstance.get_todo())
                //{
					//oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(punto);
				//}
				
				//Punto Dentro Superficie
				bool miPtoInsideTerrain = this.fIsPtoInsideTerreno(this.P2.X, this.P2.Y);

				if (!miPtoInsideTerrain)
				{
					this.isTramoValido = false;
					this.errorTramo = eTramoEjeBasicoError.puntoExteriorSuperficie;
				}
			}

		}

        public void validarTramoP2NearBordesTerreno()
        {
            if (this.isTramoValido)
            {
                //Punto Dentro Superficie
                bool IsPtoCercaBordesTerreno = this.fIsPtoCercaBordesTerreno(this.P2.X, this.P2.Y);

                if (IsPtoCercaBordesTerreno)
                {
                    this.isTramoValido = false;
                    this.errorTramo = eTramoEjeBasicoError.tramoMuyCercanoAlBordeDelTerreno;
                }
            }

        }

		public virtual void validarTramoZonasNoPaso()
		{

			if (this.isTramoValido)
			{
				//Tramo No Esta Sobre Zonas No Paso
				bool miIsTramoOnZonaNoPaso = this.fIsTramoOnZonaNoPaso(this.P1, this.P2);

				if (miIsTramoOnZonaNoPaso)
				{
					this.isTramoValido = false;
					this.errorTramo = eTramoEjeBasicoError.zonaNoPaso;
				}
			}

		}




		




		#endregion

		#region "Privados"
        private void createSeccionNormal(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes, IEstudio iEstudioData, bool isTramoEspecial, bool isEntronque, oP3d iPuntoFinal)
        {
            #region "Validacion"

            if (!this.isTramoValido)
            {
                return;
            }

            #endregion

            #region "Asignacion Variables"
            this.lonDiscretizacionProyecto = iLonDiscretizacionProyecto;
            this.mLongitudDiscretizacionCalculo = null;
            #endregion

            #region "Ajuste Pendiente Sin Estructuras"
            double pendienteMaximaPC = iRoadPendientes.calzadaPendienteCalculoMaximoPC;
            if (pendienteMaximaPC < iRoadPendientes.estructuraPendienteCalculoMaximoPC) pendienteMaximaPC = iRoadPendientes.estructuraPendienteCalculoMaximoPC;

            //Pendiente Terreno > Pendiente Road Calculo Implantacion
            if (Math.Abs(this.P1P2terrenoPendienteConSignoPC) > iRoadPendientes.calzadaPendienteCalculoMaximoPC)
            {
                this.P2.Z = this.getP2ZFromPendienteMaximaAbsoluta(iRoadPendientes.calzadaPendienteCalculoMaximoPC);
            }
            else
            {
                this.P2.Z = this.P2terreno.Z;
            }

            if (isEntronque)
            {
                double pendienteFinalPC = Math.Abs(oTrigo.getPendiente3D(this.P2, iPuntoFinal, ePorcentaje.porCiento));
                if (pendienteFinalPC > pendienteMaximaPC)
                {
                    double signo = 1;
                    if (oTrigo.getPendiente3D(iPuntoFinal, this.P2, ePorcentaje.porCiento) < 0) signo = -1;
                    this.P2.Z = getP2zFromPointPendiente(iPuntoFinal, pendienteMaximaPC * signo);
                }

            }


            #endregion

            #region "Generamos la Seccion con Pendiente de Calzada"

            //Puntos de la Sección
            List<oP3d> miLstPuntosSeccion = this.getLstPointSeccion();

            this.seccion = this.getListadoSecciones(miLstPuntosSeccion, iEstudioData);


            #endregion

            #region "Validamos la Seccion"

            //Calculamos la Seccion Sin Estructuras
            this.seccion.calculateSeccionSinEstructuras();


            if (this.seccion.isSeccionesNoValidas())
            {

                if (this.seccion.allowEstructuras())
                {

                    if (this.pendienteAbsolutaPC > iRoadPendientes.estructuraPendienteCalculoMaximoPC)
                    {
                        //Modifico la Pendiente pasamos de Pendiente de Carretera a Estructuras
                        this.P2.Z = getP2ZFromPendienteMaximaAbsoluta(iRoadPendientes.estructuraPendienteCalculoMaximoPC);
                    }



                    //Obtego los nuevos puntos de la sección
                    List<oP3d> miLstPuntosSeccionConEstructuras = this.getLstPointSeccion();

                    this.seccion = this.getListadoSecciones(miLstPuntosSeccionConEstructuras, iEstudioData);

                    //Calculo la Seccion Con Estructuras
                    this.seccion.calcultateSeccionConEstructuras();

                    if (seccion.isSeccionesNoValidas())
                    {
                        if (isTramoEspecial)
                        {
                            foreach (oSeccionEjeBasico item in this.seccion)
                            {
                                this.isTramoValido = true;
                                if (item.seccionTipo == null)
                                {
                                    if (item.excavacionTipo == eExcavacion.terraplen)
                                    {
                                        item.seccionTipo = eRoadSeccion.puente;
                                    }
                                    else
                                    {
                                        item.seccionTipo = eRoadSeccion.tunel;
                                    }
                                }

                                if (this.pendienteAbsolutaPC > iRoadPendientes.estructuraPendienteCalculoMaximoPC)
                                {
                                    this.isTramoValido = false;
                                    this.errorTramo = eTramoEjeBasicoError.pendienteTramoEspecialInvalida;
                                }

                            }
                        }
                        else
                        {
                            this.isTramoValido = false;
                            this.errorTramo = eTramoEjeBasicoError.alturaMovimientoTierrasConEstructuras;
                        }
                    }
                }
                else
                {
                    if (isTramoEspecial)
                    {
                        foreach (oSeccionEjeBasico item in this.seccion)
                        {
                            this.isTramoValido = true;
                            if (item.seccionTipo == null)
                            {
                                item.seccionTipo = eRoadSeccion.calzada;
                            }
                        }

                        if (this.pendienteAbsolutaPC > iRoadPendientes.calzadaPendienteCalculoMaximoPC)
                        {
                            this.isTramoValido = false;
                            this.errorTramo = eTramoEjeBasicoError.pendienteTramoEspecialInvalida;
                        }
                    }
                    else
                    {
                        this.isTramoValido = false;
                        this.errorTramo = eTramoEjeBasicoError.alturaMovimientoTierrasSinEstructuras;
                    }
                }

            }

            #endregion
        }


        private void createSeccionNormal(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes, IEstudio iEstudioData, bool isTramoEspecial)
        {
            #region "Validacion"

            if (!this.isTramoValido)
            {
                return;
            }

            #endregion

            #region "Asignacion Variables"
            this.lonDiscretizacionProyecto = iLonDiscretizacionProyecto;
            this.mLongitudDiscretizacionCalculo = null;
            #endregion

            #region "Ajuste Pendiente Sin Estructuras"

            if (!oSingletonTerreno.getInstance.isPtoInsideTerreno(P2))
            {
                this.errorTramo = eTramoEjeBasicoError.puntoExteriorSuperficie;
                this.isTramoValido = false;
                return;
            }
            //Pendiente Terreno > Pendiente Road Calculo Implantacion
            if (Math.Abs(this.P1P2terrenoPendienteConSignoPC) > iRoadPendientes.calzadaPendienteCalculoMaximoPC)
            {
                this.P2.Z = this.getP2ZFromPendienteMaximaAbsoluta(iRoadPendientes.calzadaPendienteCalculoMaximoPC);
            }
            else
            {
                this.P2.Z = this.P2terreno.Z;
            }


            #endregion

            #region "Generamos la Seccion con Pendiente de Calzada"

            //Puntos de la Sección
            List<oP3d> miLstPuntosSeccion = this.getLstPointSeccion();

            this.seccion = this.getListadoSecciones(miLstPuntosSeccion, iEstudioData);


            #endregion

            #region "Validamos la Seccion"

            //Calculamos la Seccion Sin Estructuras
            this.seccion.calculateSeccionSinEstructuras();


            if (this.seccion.isSeccionesNoValidas())
            {

                if (this.seccion.allowEstructuras())
                {

                    if (this.pendienteAbsolutaPC > iRoadPendientes.estructuraPendienteCalculoMaximoPC)
                    {
                        //Modifico la Pendiente pasamos de Pendiente de Carretera a Estructuras
                        this.P2.Z = getP2ZFromPendienteMaximaAbsoluta(iRoadPendientes.estructuraPendienteCalculoMaximoPC);
                    }



                    //Obtego los nuevos puntos de la sección
                    List<oP3d> miLstPuntosSeccionConEstructuras = this.getLstPointSeccion();

                    this.seccion = this.getListadoSecciones(miLstPuntosSeccionConEstructuras, iEstudioData);

                    //Calculo la Seccion Con Estructuras
                    this.seccion.calcultateSeccionConEstructuras();

                    if (seccion.isSeccionesNoValidas())
                    {
                        if (isTramoEspecial)
                        {
                            foreach (oSeccionEjeBasico item in this.seccion)
                            {
                                this.isTramoValido = true;
                                if (item.seccionTipo == null)
                                {
                                    if (item.excavacionTipo == eExcavacion.terraplen)
                                    {
                                        item.seccionTipo = eRoadSeccion.puente;
                                    }
                                    else
                                    {
                                        item.seccionTipo = eRoadSeccion.tunel;
                                    }
                                }

                                if (this.pendienteAbsolutaPC > iRoadPendientes.estructuraPendienteCalculoMaximoPC)
                                {
                                    this.isTramoValido = false;
                                    this.errorTramo = eTramoEjeBasicoError.pendienteTramoEspecialInvalida;
                                }

                            }
                        }
                        else
                        {
                            this.isTramoValido = false;
                            this.errorTramo = eTramoEjeBasicoError.alturaMovimientoTierrasConEstructuras;
                        }
                    }
                }
                else
                {
                    if (isTramoEspecial)
                    {
                        foreach (oSeccionEjeBasico item in this.seccion)
                        {
                            this.isTramoValido = true;
                            if(item.seccionTipo==null)
                            {
                                item.seccionTipo = eRoadSeccion.calzada;
                            }
                        }

                        if (this.pendienteAbsolutaPC > iRoadPendientes.calzadaPendienteCalculoMaximoPC)
                        {
                            this.isTramoValido = false;
                            this.errorTramo = eTramoEjeBasicoError.pendienteTramoEspecialInvalida;
                        }
                    }
                    else
                    {
                        this.isTramoValido = false;
                        this.errorTramo = eTramoEjeBasicoError.alturaMovimientoTierrasSinEstructuras;
                    }
                }

            }

            #endregion
        }

        private void createSeccionObligadoPuente(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes, IEstudio iEstudioData)
        {
            #region "Validacion"

            if (!this.isTramoValido)
            {
                return;
            }

            #endregion

            #region "Asignacion Variables"
            this.lonDiscretizacionProyecto = iLonDiscretizacionProyecto;
            this.mLongitudDiscretizacionCalculo = null;
            #endregion


            #region "Ajuste Pendiente Sin Estructuras"

            //Pendiente Terreno > Pendiente Road Calculo Implantacion
            if (Math.Abs(this.P1P2terrenoPendienteConSignoPC) > iRoadPendientes.calzadaPendienteCalculoMaximoPC)
            {
                this.P2.Z = this.getP2ZFromPendienteMaximaAbsoluta(iRoadPendientes.calzadaPendienteCalculoMaximoPC);
            }
            else
            {
                this.P2.Z = this.P2terreno.Z;
            }


            #endregion

            #region "Generamos la Seccion con Pendiente de Calzada"

            //Puntos de la Sección
            List<oP3d> miLstPuntosSeccion = this.getLstPointSeccion();

            this.seccion = this.getListadoSecciones(miLstPuntosSeccion, iEstudioData);


            #endregion




            if (this.pendienteAbsolutaPC > iRoadPendientes.estructuraPendienteCalculoMaximoPC)
            {
                //Modifico la Pendiente pasamos de Pendiente de Carretera a Estructuras
                this.P2.Z = getP2ZFromPendienteMaximaAbsoluta(iRoadPendientes.estructuraPendienteCalculoMaximoPC);
            }

            //Obtego los nuevos puntos de la sección
            List<oP3d> miLstPuntosSeccionConEstructuras = this.getLstPointSeccion();

            this.seccion = this.getListadoSecciones(miLstPuntosSeccionConEstructuras, iEstudioData);

            //Calculo la Seccion Con Estructuras
            this.seccion.calcultateSeccionObligadoPuente();

            if (seccion.isSeccionesNoValidas())
            {
                this.isTramoValido = false;
                this.errorTramo = eTramoEjeBasicoError.alturaMovimientoTierrasConEstructuras;
            }

        }

        private void createSeccionObligadoPuenteoTunel(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes, IEstudio iEstudioData)
        {
            #region "Validacion"

            if (!this.isTramoValido)
            {
                return;
            }

            #endregion

            #region "Asignacion Variables"
            this.lonDiscretizacionProyecto = iLonDiscretizacionProyecto;
            this.mLongitudDiscretizacionCalculo = null;
            #endregion


            #region "Ajuste Pendiente Sin Estructuras"

            //Pendiente Terreno > Pendiente Road Calculo Implantacion
            if (Math.Abs(this.P1P2terrenoPendienteConSignoPC) > iRoadPendientes.calzadaPendienteCalculoMaximoPC)
            {
                this.P2.Z = this.getP2ZFromPendienteMaximaAbsoluta(iRoadPendientes.calzadaPendienteCalculoMaximoPC);
            }
            else
            {
                this.P2.Z = this.P2terreno.Z;
            }


            #endregion

            #region "Generamos la Seccion con Pendiente de Calzada"

            //Puntos de la Sección
            List<oP3d> miLstPuntosSeccion = this.getLstPointSeccion();

            this.seccion = this.getListadoSecciones(miLstPuntosSeccion, iEstudioData);


            #endregion




            if (this.pendienteAbsolutaPC > iRoadPendientes.estructuraPendienteCalculoMaximoPC)
            {
                //Modifico la Pendiente pasamos de Pendiente de Carretera a Estructuras
                this.P2.Z = getP2ZFromPendienteMaximaAbsoluta(iRoadPendientes.estructuraPendienteCalculoMaximoPC);
            }

            //Obtego los nuevos puntos de la sección
            List<oP3d> miLstPuntosSeccionConEstructuras = this.getLstPointSeccion();

            this.seccion = this.getListadoSecciones(miLstPuntosSeccionConEstructuras, iEstudioData);

            //Calculo la Seccion Con Obligacion a Puente o Tunel
            this.seccion.calcultateSeccionObligadoPuenteoTunel();

            if (seccion.isSeccionesNoValidas())
            {
                this.isTramoValido = false;
                //crear error nuevo??
                this.errorTramo = eTramoEjeBasicoError.alturaMovimientoTierrasConEstructuras;
            }

        }


		private double getP2zFromPendiente(double iPendienteTramoConSignoPC)
		{
			double miIncrementoZ = this.longitud2d * (iPendienteTramoConSignoPC / 100);

			return this.P1.Z + miIncrementoZ;
		}


        private double getP2zFromPointPendiente(oP3d iPoint, double iPendienteTramoConSignoPC)
        {
            double distancia = iPoint.distTo2d(this.P2);
            double miIncrementoZ = distancia * (iPendienteTramoConSignoPC / 100);

            return iPoint.Z + miIncrementoZ;
        }

		private double getP2ZFromPendienteMaximaAbsoluta(double iPendienteMaximaAbsPC)
		{

			double miIncrementoZmaximo = Math.Abs(iPendienteMaximaAbsPC / 100) * this.longitud2d;

			if (this.P1P2terrenoPendienteConSignoPC > 0)
			{
				return this.P1.Z + miIncrementoZmaximo;
			}
			else
			{
				return this.P1.Z - miIncrementoZmaximo;
			}



		}
		private double getLongitudDiscretizacionCalculo()
		{
			//Obtenemos numero de Segmentos
			double miSegmentosDecimal = this.longitud3d / this.lonDiscretizacionProyecto;

			int miSegmentosNum = (int)Math.Ceiling(miSegmentosDecimal);

			//Obtengo el Numero de Puntos Intermedios
			int miPuntosSeccion = miSegmentosNum + 1;

			//Longitud Segemento
			double miSegmentoLongitud = this.longitud2d / miPuntosSeccion;


			return miSegmentoLongitud;
		}

		private oSeccionesEjeBasico getListadoSecciones(List<oP3d> iLstPuntoSeccion, IEstudio iEstudioData)
		{
            //modificar este metodo



			//Ahora Generamos la Seccion con la Pendiente de Proyecto.
			oSeccionesEjeBasico misSecciones = new oSeccionesEjeBasico();

			ISeccionCalzada miSeccionCalzada = null;
			IzonaMovimientoTierras miZonaMovimentoTierras = null;
			IzonaPuentes miZonaPuentes = null;
			IzonaTuneles miZonaTuneles = null;
            bool isObligadoEstructura = false;
            Triangulo miTrianguloCercano = null;


			foreach (var ptoSeccion in iLstPuntoSeccion)
			{

				miSeccionCalzada = iEstudioData.getISeccionCalzadaByPto(ptoSeccion);
				miZonaMovimentoTierras = iEstudioData.getIZonaMovimientoTierrasByPto(ptoSeccion);
				miZonaPuentes = iEstudioData.getIZonaPuenteByPto(ptoSeccion);
				miZonaTuneles = iEstudioData.getIZonaTunelByPto(ptoSeccion);
                isObligadoEstructura = iEstudioData.isOnZonaPasoObligadoEstructuras(ptoSeccion);

				double[] mipunto = new double[2];
				mipunto[0] = ptoSeccion.X;
				mipunto[1] = ptoSeccion.Y;
				//oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(mipunto);
				//miTrianguloCercano = oSingletonTerreno.getInstance.getTrianguloInside(ptoSeccion.X, ptoSeccion.Y);
				if (miTrianguloCercano == null)
				{
					//miTrianguloCercano = oSingletonTerreno.getInstance.getTrianguloInside(ptoSeccion.X, ptoSeccion.Y);
					//****miTrianguloCercano = GetTriangulo(mipunto);

				}

				//miTrianguloCercano = oSingletonTerreno.getInstance.getTrianguloInsideFromTriangulo(ptoSeccion.X, ptoSeccion.Y, miTrianguloCercano);
				//****miTrianguloCercano = getTrianguloInsideFromTriangulo(mipunto, miTrianguloCercano);

				/*if (miTrianguloCercano == null)
                {
                    miTrianguloCercano = oSingletonTerreno.getInstance.getTrianguloInside(ptoSeccion.X, ptoSeccion.Y);
                }
                miTrianguloCercano = oSingletonTerreno.getInstance.getTrianguloInsideFromTriangulo(ptoSeccion.X, ptoSeccion.Y, miTrianguloCercano);
				*/
                double miZ = 0;
                double miSlope = 0;
				/****if (miTrianguloCercano != null)
                {
                   miZ = this.fGetZTerrenoFromTring(ptoSeccion, miTrianguloCercano);
                    miSlope = this.fGetSlopeTerrenoFromTriang(miTrianguloCercano);
					miZ = (double)GetZ_triangulo(mipunto, miTrianguloCercano);
					miSlope = fGetSlopeTerrenoFromTriang(miTrianguloCercano);
				}
                else
                {

                    this.isTramoValido = false;
                    this.errorTramo = eTramoEjeBasicoError.puntoExteriorSuperficie;
                }*////
				miZ = (double)this.GetZ(mipunto);
				miSlope = this.fGetSlopeTerrenoFromTriang(null, mipunto[0], mipunto[1]);
                if (double.IsNaN(miZ))
                {
					this.isTramoValido = false;
					this.errorTramo = eTramoEjeBasicoError.puntoExteriorSuperficie;
				}
				oSeccionEjeBasico miSeccion;

				int miId = 0;

				miSeccion = new oSeccionEjeBasico(miId,
												 ptoSeccion,
												 this.lonDiscretizacionCalculo,
                                                 miZ,
                                                 miSlope,
												 miSeccionCalzada,
												 miZonaMovimentoTierras,
												 miZonaPuentes,
												 miZonaTuneles,
                                                 isObligadoEstructura);



				misSecciones.Add(miSeccion);

				miId++;
			}


			return misSecciones;
		}

        public virtual List<oP3d> getLstPointSeccion()
		{

			List<oP3d> miLstPointIntermedios = new List<oP3d>();


			double miDistanciaOrigen;
			oP2d miPtoSeccion2D;
			double miPtoSeccionZ;


			// The example displays the following output to the console:
			//         Value          Ceiling          Floor
			//       
			//          7.03                8              7
			//          7.64                8              7
			//          0.12                1              0


			//Obtenemos numero de Segmentos
			double miSegmentosDecimal = this.longitud3d / this.lonDiscretizacionProyecto;

			int miSegmentosNum = (int)Math.Ceiling(miSegmentosDecimal);


			//Obtengo el Numero de Puntos Intermedios
			int miPuntosSeccion = miSegmentosNum + 1;

			//Longitud Segemento
			double miSegmentoLongitud = this.longitud2d / miPuntosSeccion;

			for (int i = 1; i <= miPuntosSeccion; i++)
			{
				//LongitudSegemnto
				miDistanciaOrigen = miSegmentoLongitud * i;

				//Coordenadas del Punto Seccion
				miPtoSeccion2D = this.P1.getPointFromAzimutAndLongitud(this.azimutGrados,miDistanciaOrigen);

				//Obtengo la CoordendaZ
				miPtoSeccionZ =  this.getP2ZFromP1(miDistanciaOrigen);

				//Añado el Punto a la Colección
				miLstPointIntermedios.Add(new oP3d(miPtoSeccion2D.X, miPtoSeccion2D.Y, miPtoSeccionZ));
			}


			return miLstPointIntermedios;
		
		}
		public virtual void drawTramo2D(string iCapa)
		{

			Line miLine = engCadNet.oLine.addLine2d(this.P1.X, this.P1.Y, this.P2.X, this.P2.Y, iCapa);



			using (oEntidad<Line> miEntidad = new oEntidad<Line>(miLine))
			{

				miEntidad.open();

				if (this.isTramoValido && this.seccion == null)
				{
					miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cBlancoIndex);
				}
				else if (this.isTramoValido &&  this.seccion !=null && ! this.seccion.hasEstructuras())
				{
					miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cMoradoIndex);
				}
				else if (this.isTramoValido && this.seccion !=null &&  this.seccion.hasEstructuras())
				{
					miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cAzulIndex);
				}
				else if (!this.isTramoValido)
				{
					miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cRojoIndex);
				}
				else
				{
					throw new Exception("Opción No Configurada");
				}


				miEntidad.save();
			}


			oP3d miPtoTexto = new oP3d(this.P2.X, this.P2.Y - 4,0);

			engCadNet.oMTexto.addMText2D(this.ToString(),miPtoTexto.toArray3d(), 0.15, 0, iCapa);

		}
		public virtual void drawSeccion (string iCapa)
		{

			if (isTramoValido && seccion !=null)
			{
				foreach (var item in seccion)
				{
					item.draw(iCapa);
				}
			}
		 
		}
		public virtual void drawTramo3D (string iCapa)
		{
			engCadNet.oLine.addLine3dVoid(this.P1.toArray3d(), this.P2.toArray3d(), iCapa);
		}
		public override string ToString()
		{

			StringBuilder miStr = new StringBuilder();

			miStr.AppendLine("Tramo : " + this.idTramo.ToString());
			miStr.AppendLine("P1 : " + this.P1.ToString());
			miStr.AppendLine("P2 : " + this.P2.ToString());
			miStr.AppendLine("P2terreno : " + this.P2terreno.ToString());
			miStr.AppendLine("Lon 2d : " + this.longitud2d.roundOff(2));
			miStr.AppendLine("Pendiente P1-P2terreno : " + this.P1P2terrenoPendienteConSignoPC.roundOff(2));
			miStr.AppendLine("Pendiente P1-P2 : " + this.pendienteConSignoPC.roundOff(2));
			miStr.AppendLine("Es Tramo Valido : " + this.isTramoValido.ToString());

			if (!this.isTramoValido)
			{
				miStr.AppendLine("Error Tramo : " + this.errorTramo.ToString());
			}

			return miStr.ToString();

		}

		#endregion


	}

	public abstract class oTramoAbanico : oTramoEjeBasico
	{


        public static event EventHandler<oEventArgs<oTramoAbanico>> evTramo;



		private int? mIdPosicion = null;

		private oTramoEjeBasico mTramoPrevio = null;

		private oP2d mPtoTarget = null;

		private List<oTramoAbanico> mLstTramos = new List<oTramoAbanico>();


		private double? mValoracionPonderadaGlobal_0_10 = null;

		private double? mValoracionDistanciaGlobal_0_10 = null;
		private double? mValoracionPendienteGlobal_0_10 = null;
		private double? mValoracionCosteImplantacion_0_10 = null;

		private double? mAnguloMinimoEntreTramos = 20;


		#region "Constructor"

		public oTramoAbanico()
		{

		}

		#endregion
		#region "Propiedades"


		/// <summary>
		/// POSICION DEL TRAMO DENTRO DEL ABANICO
		/// </summary>
		public int idPosicion
		{
			get
			{

				if (mIdPosicion == null)
				{
					throw new oExPropertieNullValue("Id Posición Tramo en Abanico");
				}

				return mIdPosicion.Value;
			}


			set
			{
				mIdPosicion = value;
			}
		}

		/// <summary>
		/// PTO TARGET DEL ABANICO
		/// </summary>
		public oP2d ptoTarget
		{
			get
			{
				if (mPtoTarget == null)
				{
					throw new oExPropertieNullValue("Punto Target");
				}

				return mPtoTarget;

			}

			set
			{
				mPtoTarget = value;
			}


		}


		/// <summary>
		/// TRAMO PREVIO DEL ABANICO
		/// </summary>
		public oTramoEjeBasico tramoPrevio
		{

			get
			{
				if (mTramoPrevio == null)
				{
					throw new oExPropertieNullValue("tramo Previo");
				}

				return mTramoPrevio;
			}


			set
			{
				mTramoPrevio = value;
			}


		}

		/// <summary>
		/// ANGULO CON EL TRAMO PREVIO [GRADOS]
		/// </summary>
		public double anguloGradosTramoPrevio
		{
			get
			{
				return this.tramoPrevio.P2.angleFrom3Points(this.tramoPrevio.P1, this.P2, eAng.grados);
			}
		}


		/// <summary>
		/// LISTADO DE TRAMOS
		/// </summary>
		public List<oTramoAbanico> lstTramos
		{
			get
			{
				return mLstTramos;
			}

			set
			{
				mLstTramos = value;
			}

		}


		/// <summary>
		/// Distancia P2 to Target
		/// </summary>
		public double distanciaP2ToTarget
		{
			get
			{
				return this.P2.distTo2d(this.ptoTarget);
			}
		}
		/// <summary>
		/// Distancia Origen Abanico to P2
		/// </summary>
		public double distanciaOrigenAbanicoToP2
		{
			get
			{
				double miLongitudTramosPrevios = (from p in this.lstTramos select p.longitud2d).Sum();
				return miLongitudTramosPrevios;
			}
		}
		/// <summary>
		/// Valoración Global del Tramo por Distancia
		/// (Es Comparando con los restantes Tramos
		/// </summary>
		public double valoracionDistanciaGlobal_0_10
		{
				get
				{
					if (mValoracionDistanciaGlobal_0_10==null)
					{
				  
						throw new oExPropertieNullValue("valoracionDistanciaGlobal_0_10");
					}

					return mValoracionDistanciaGlobal_0_10.Value;

				}

			set
			{
				mValoracionDistanciaGlobal_0_10= value;

			}


		}
		/// <summary>
		/// Valoración Global del Tramo por Pendiente del Terreno
		/// (Es Comparando con los restantes Tramos
		/// </summary>
		public double valoracionPendienteGlobal_0_10
		{
				get
				{
					if (mValoracionPendienteGlobal_0_10==null)
					{
				  
						throw new oExPropertieNullValue("valoracionPendienteGlobal_0_10");
					}

					return mValoracionPendienteGlobal_0_10.Value;

				}

			set
			{
				mValoracionPendienteGlobal_0_10= value;

			} 
		}
		/// <summary>
		/// Valoración Global del Tramo por Coste Implantacion
		/// (Es Comparando con los restantes Tramos
		/// </summary>
		public double valoracionCosteImplantacionGlobal_0_10
		{
				get
				{
					if (mValoracionCosteImplantacion_0_10==null)
					{
				  
						throw new oExPropertieNullValue("valoracionCosteImplantacionGlobal_0_10");
					}

					return mValoracionCosteImplantacion_0_10.Value;

				}

			set
			{
				mValoracionCosteImplantacion_0_10= value;

			} 
		}
		/// <summary>
		/// Valoracion del Tramo Ponderada (%Dis ; %Pen ; %Coste)
		/// </summary>
		public double valoracionPonderadaGlobal_0_10
		{

			get
			{
				if (mValoracionPonderadaGlobal_0_10 == null)
				{
					throw new oExPropertieNullValue("valoracionPonderadaGlobal");  
				}

				return mValoracionPonderadaGlobal_0_10.Value;

			}

			set
			{
				mValoracionPonderadaGlobal_0_10 = value;
			}
		}


		#endregion

		#region "Validaciones"

		/// <summary>
		/// Validar Tramos, Cuando Este Marcada la Opción.
		/// </summary>
		public void validarAijDesviacionesMaximas(bool iInvalidarTramosIncrementoLongitud, double iIncrementoLongitudMaximaPC)
		{

			if (this.isTramoValido && iInvalidarTramosIncrementoLongitud)
			{
				double miIncrementoLongitudTramoPrevioActual = Math.Abs(tramoPrevio.longitud2d - this.longitud2d);
				double miPorcentajePU = iIncrementoLongitudMaximaPC / 100;

				double miIncrementoLongitudMaxima = miPorcentajePU * tramoPrevio.longitud2d;

				if (miIncrementoLongitudTramoPrevioActual > miIncrementoLongitudMaxima)
				{
					this.isTramoValido = false;
					this.errorTramo = eTramoEjeBasicoError.LongitudMinimaAbanicoPrimario;
				}

			}

		}
		/// <summary>
		/// Validar Tramos, si el tramo Previo es AijMinimoMinimo, restringimos el ángulo entre tramos
		/// </summary>
		/// <param name="fAnguloMinimoTramos">Aij,Angulo</param>
		public void validarAnguloEntreTramosAijMinimoMinimo(Func<double, eAng, double> fAnguloMinimoTramos)
		{
			if (this.isTramoValido && this.tramoPrevio.tramoTipoEjeBasico == eTramoTipoEjeBasico.avanceReducido)
			{
				double miAnguloGradosMinimoTramoPrevio = fAnguloMinimoTramos(this.tramoPrevio.longitud2d, eAng.grados);

				if (miAnguloGradosMinimoTramoPrevio > this.anguloGradosTramoPrevio)
				{
					this.isTramoValido = false;
					this.errorTramo = eTramoEjeBasicoError.anguloMinimoTramosAijMinimoMinimo;
				}
			}
		}

		/// <summary>
		/// Validamos los tramos con Angulo Minimo con el TramoPrevio
		/// </summary>
		public void validarAnguloEntreTramos ()
		{
			if (this.isTramoValido)
			{
				if (this.mAnguloMinimoEntreTramos.Value >= this.anguloGradosTramoPrevio)
				{
					this.isTramoValido = false;
					this.errorTramo = eTramoEjeBasicoError.anguloMinimoTramosConsecutivos;
				}           
			}
		}

        /// <summary>
        /// Validamos los tramos con Angulo Minimo con el tramo dado
        /// </summary>
        public void validarAnguloEntreTramosDadoTramo(oTramoEjeBasico iTramo)
        {
            if (this.isTramoValido)
            {
                double miAngulo = this.P2.angleFrom3Points(this.P1, iTramo.P2, eAng.grados);
                if (this.mAnguloMinimoEntreTramos.Value >= miAngulo)
                {
                    this.isTramoValido = false;
                    this.errorTramo = eTramoEjeBasicoError.anguloMinimoTramosConsecutivosConLlegadaUsuario;
                }
            }
        }

        public void validarCruceDPH(IEstudio iEstudioData)
        {
            
            if (this.isTramoValido)
            {
                bool miIsValido = iEstudioData.isValidoCruceConDPH(this.P1, this.P2);
                if (!miIsValido)
                {
                    this.isTramoValido = false;
                    this.errorTramo = eTramoEjeBasicoError.anguloDeCruceConDPHNoPermitido;
                }
            }
        }

        public void validarTramoCercanoPF(double iTresXAjmin, oP3d iPuntoObjetivo, double iPendiente)
        {
            if (this.isTramoValido)
            {
                if (this.P1.distTo3d(iPuntoObjetivo) <= iTresXAjmin)
                {
                    double miDifCotas = this.P2.Z - iPuntoObjetivo.Z;
                    double miDistancia = this.P2.distTo2d(new oP2d(iPuntoObjetivo.X, iPuntoObjetivo.Y));
                    double miPend = miDifCotas / miDistancia;

                    if (miPend > iPendiente)
                    {
                        this.isTramoValido = false;
                        this.errorTramo = eTramoEjeBasicoError.pendienteTramoCercanoOnjetivoNoValida;
                    }
                }
            }

        }

        public void validarDistanciaP2P1PuntoMedio(oP3d iP1, oP3d iP2, double iMinDist)
        {
            if (this.isTramoValido)
            {
                if (iP1.distTo2d(iP2) < iMinDist)
                {
                    this.isTramoValido = false;
                    this.errorTramo = eTramoEjeBasicoError.entronqueFinalPuntoMedioPuntosMuyCercanos;
                }
            }
        }




        //public void validarCruceInf(IEstudio iEstudioData)
        //{
        //    if (this.isTramoValido)
        //    {
        //        bool miIsValido = iEstudioData.isValidoCruceConInf(this.P1, this.P2);
        //        if (!miIsValido)
        //        {
        //            this.isTramoValido = false;
        //            this.errorTramo = eTramoEjeBasicoError.anguloDeCruceConInfNoPermitido;
        //        }
        //    }
        //}


        public void validarDentroDPH(IEstudio iEstudioData)
        {
            if (this.isTramoValido)
            {
                bool miIsValido = iEstudioData.isValidoTramoDentroZonaDPH(this.P1, this.P2);
                if (!miIsValido)
                {
                    this.isTramoValido = false;
                    this.errorTramo = eTramoEjeBasicoError.dentroDeDPHNoPermitido;
                }
            }
        }
		


		#endregion

		#region "Valoraciones"


		/// <summary>
		/// Valoración Local Distancia
		/// </summary>
		public double valoracionDistanciaLocal()
		{

			if (this.tramoTipoEjeBasico == eTramoTipoEjeBasico.avanceCorto)
			{
				return this.longitud2d + this.distanciaP2ToTarget;     
			}
			else if (this.tramoTipoEjeBasico == eTramoTipoEjeBasico.avanceLargo)
			{
				return this.distanciaP2ToTarget;
			}
			else if (this.tramoTipoEjeBasico == eTramoTipoEjeBasico.avanceReducido)
			{
				return this.longitud2d + this.distanciaP2ToTarget;
			}
			else
			{
				throw new oExEnumNotImplemented(this.tramoTipoEjeBasico.ToString());
			}
							  
		}

		public double valoracionPendienteTramo()
		{
			return this.seccion.valoracionSlopeSuma();
		}
		public double valoracionPendienteOrigenAbanicoToP2()
		{

			double miSuma = (from p in this.lstTramos select p.seccion.valoracionSlopeSuma()).Sum();

			return miSuma;
		}
		public double valoracionPendienteML()
		{

			double miCosteOrigen = this.valoracionPendienteOrigenAbanicoToP2();
			double miLonOrigen = this.distanciaOrigenAbanicoToP2;

			return miCosteOrigen / miLonOrigen;
		}


		public double valoracionCosteImplantacionTramo()
		{
			return this.seccion.valoracionCosteImplantacionSuma();
		}
		public double valoracionCosteImplantacionOrigenAbanicoToP2()
		{

			double miSuma = (from p in this.lstTramos select p.seccion.valoracionCosteImplantacionSuma()).Sum();

			return miSuma;
		}
		public double valoracionCosteImplantacionML()
		{
			double miCosteOrigenP2 = this.valoracionCosteImplantacionOrigenAbanicoToP2();
			double miLonOrigen = this.distanciaOrigenAbanicoToP2;

			return miCosteOrigenP2 / miLonOrigen;
		}




		#endregion




        public override void createSeccion(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes, IEstudio iEstudioData, bool isTramoEspecial, bool isEntronque, oP3d iPuntoFinal)
        {
            base.createSeccion(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData, isTramoEspecial, isEntronque, iPuntoFinal);

            if (oTramoAbanico.evTramo != null)
            {
                oTramoAbanico.evTramo(this, new oEventArgs<oTramoAbanico>(this));
            }

        }

		public override void  createSeccion(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes, IEstudio iEstudioData, bool isTramoEspecial)
		{
            base.createSeccion(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData, isTramoEspecial);

             if (oTramoAbanico.evTramo != null)
             {
                 oTramoAbanico.evTramo(this, new oEventArgs<oTramoAbanico>(this));
             }

		}

		public override string ToString()
		{

			StringBuilder miStr = new StringBuilder();


			if (this.isTramoValido)
			{
				miStr.AppendLine("Es Tramo Valido : " + this.isTramoValido.ToString());
				miStr.AppendLine("Tramo : " + this.idTramo.ToString());
				miStr.AppendLine("Abanico Id : " + this.idAbanico.ToString());
				miStr.AppendLine("Posición Abanico : " + this.idPosicion.ToString());
				miStr.AppendLine("Contiene Estructuras :  " + this.seccion.hasEstructuras().ToString());
				miStr.AppendLine("Azimut : " + this.azimutGrados.roundOff(2).ToString());
				miStr.AppendLine("Ángulo Tramo Previo : " + this.anguloGradosTramoPrevio.roundOff(2));
				miStr.AppendLine("P1 : " + this.P1.ToString());
				miStr.AppendLine("P2terreno : " + this.P2terreno.ToString());
				miStr.AppendLine("P2 : " + this.P2.ToString());
				miStr.AppendLine("Lon 2d : " + this.longitud2d.roundOff(2));
				miStr.AppendLine("Pendiente P1-P2terreno : " + this.P1P2terrenoPendienteConSignoPC.roundOff(2));
				miStr.AppendLine("Pendiente P1-P2 : " + this.pendienteConSignoPC.roundOff(2));
				miStr.AppendLine("---------------------------------------------------------");
				miStr.AppendLine("#Valoración Distancia#");
				miStr.AppendLine("Distancia Origen Abanico-P2 :  " + this.distanciaOrigenAbanicoToP2.roundOff(2).ToString());
				miStr.AppendLine("Distancia P2 - PtoTarget :  " + this.distanciaP2ToTarget.roundOff(2).ToString());
				miStr.AppendLine("Distancia Valoración Global : " + this.valoracionDistanciaLocal().roundOff(2).ToString());
				miStr.AppendLine("#Valoración Pendiente#");
				miStr.AppendLine("Valoración Pendiente [Tramo] :  " + this.valoracionPendienteTramo().roundOff(0).ToString());
				miStr.AppendLine("Valoración Pendiente [Origen Abanico - P2] :  " + this.valoracionPendienteOrigenAbanicoToP2().roundOff(0).ToString());
				miStr.AppendLine("Valoración Pendiente [ML] :  " + this.valoracionPendienteML().roundOff(2).ToString());
				miStr.AppendLine("#Valoración Implantación#");
				miStr.AppendLine("Valoración Coste Implantacion [Tramo] :  " + this.valoracionCosteImplantacionTramo().roundOff(0).ToString());
				miStr.AppendLine("Valoración Coste Implantacion [Origen Abanico - P2] :  " + this.valoracionCosteImplantacionOrigenAbanicoToP2().roundOff(0).ToString());
				miStr.AppendLine("Valoracion Coste Implantación [ML] : " + this.valoracionCosteImplantacionML().roundOff(0).ToString());
				miStr.AppendLine("---------------------------------------------------------");
				miStr.AppendLine("Valoración Global Ponderada Tramo [0-10] : " + this.valoracionPonderadaGlobal_0_10.roundOff(3).ToString());
				miStr.AppendLine("Valoración Global Distancia [0-10] : " + this.valoracionDistanciaGlobal_0_10.roundOff(3).ToString());
				miStr.AppendLine("Valoración Global Pendiente [0-10] : " + this.valoracionPendienteGlobal_0_10.roundOff(3).ToString());
				miStr.AppendLine("Valoración Global Coste Implantacion [0-10] : " + this.valoracionCosteImplantacionGlobal_0_10.roundOff(3).ToString());
			}
			else
			{
				miStr.AppendLine("Es Tramo Valido : " + this.isTramoValido.ToString());
				miStr.AppendLine("Error Tramo : " + this.errorTramo.ToString());
			}

			return miStr.ToString();

		}

        public override void drawTramo2D(string iCapa)
        {

            Line miLine = engCadNet.oLine.addLine2d(this.P1.X, this.P1.Y, this.P2.X, this.P2.Y, iCapa);
            oXdata.setXdata(miLine.ObjectId, "idPosicion", this.idPosicion.ToString());



            using (oEntidad<Line> miEntidad = new oEntidad<Line>(miLine))
            {

                miEntidad.open();

                if (this.isTramoValido && this.seccion == null)
                {
                    miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cBlancoIndex);
                }
                else if (this.isTramoValido && this.seccion != null && !this.seccion.hasEstructuras())
                {
                    miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cMoradoIndex);
                }
                else if (this.isTramoValido && this.seccion != null && this.seccion.hasEstructuras())
                {
                    miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cAzulIndex);
                }
                else if (!this.isTramoValido)
                {
                    miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cRojoIndex);
                }
                else
                {
                    throw new Exception("Opción No Configurada");
                }


                miEntidad.save();
            }


            oP3d miPtoTexto = new oP3d(this.P2.X, this.P2.Y - 4, 0);

            engCadNet.oMTexto.addMText2D(this.ToString(), miPtoTexto.toArray3d(), 0.15, 0, iCapa);

        }


        public virtual string infoProcess()
		{
            return string.Format(strGeneralUser.uiAnalizandoAbanico, this.idAbanico.ToString(), this.idTramo.ToString(), this.idPosicion.ToString());
		}

	}

	public class oTramoAvanceCorto : oTramoAbanico
	{

		public oTramoAvanceCorto()
		{

		}

		#region "Constructor"

		/// <summary>
		/// Constructor Tramo Abanico Corto
		/// </summary>
		public oTramoAvanceCorto(eTramoTipoEjeBasico iTramoTipo,int iIdAbanico, int iIdTramoPosicionAbanico, oTramoEjeBasico iTramoPrevio, double iAzimutGrados, double iTramoLongitud, oP2d iPtoTarget)
		{

			this.tramoPrevio = iTramoPrevio;
			this.idAbanico = iIdAbanico;
			this.idPosicion = iIdTramoPosicionAbanico;
			this.idTramo = this.tramoPrevio.idTramo + 1;

			this.P1 = this.tramoPrevio.P2;
			this.P2 = (oP3d)this.P1.getPointFromAzimutAndLongitud(iAzimutGrados, iTramoLongitud).convertTo3d(null);

			this.ptoTarget = iPtoTarget;

			this.tramoTipoEjeBasico = iTramoTipo;

			this.lstTramos.Add(this);
		}


		/// <summary>
		/// Constructor Tramo Entronque
		/// </summary>
		public oTramoAvanceCorto(oTramoAbanico iTramoPrevio,oP3d iPtoEntronque)
		{

			this.tramoPrevio = iTramoPrevio;
			this.idAbanico = iTramoPrevio.idAbanico + 1;
			this.idPosicion = iTramoPrevio.idPosicion;
			this.idTramo = this.tramoPrevio.idTramo + 1;

			this.P1 = this.tramoPrevio.P2;
			this.P2 = iPtoEntronque;

			this.ptoTarget = iPtoEntronque;

			this.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceCorto;

			this.lstTramos.Add(this);
		}

        

		#endregion


	  




	}

	public class oTramoAvanceLargo : oTramoAvanceCorto
	{
   
		/// <summary>
		/// Constructor Tramos Avance Largo
		/// </summary>
		public oTramoAvanceLargo (List<oTramoAbanico> iLstTramosPrevios,double iAijMinimo)
		{
		   
			this.idAbanico = iLstTramosPrevios[0].idAbanico;
			this.idPosicion = iLstTramosPrevios[0].idPosicion;
			this.tramoPrevio = iLstTramosPrevios[0].tramoPrevio;

			this.idTramo = iLstTramosPrevios[iLstTramosPrevios.Count - 1].idTramo + 1;
		 
			this.P1 = iLstTramosPrevios[iLstTramosPrevios.Count-1].P2;
			this.P2 = (oP3d)this.P1.getPointFromAzimutAndLongitud(iLstTramosPrevios[0].azimutGrados, iAijMinimo).convertTo3d(null);

			this.ptoTarget = iLstTramosPrevios[0].ptoTarget;

			this.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceLargo;

			this.lstTramos.AddRange(iLstTramosPrevios);

			this.lstTramos.Add(this);
		}





	}

	public class oTramoAijMinimoMinimo : oTramoAvanceCorto
	{
   

		 public oTramoAijMinimoMinimo(int iIdAbanico, int iIdTramoPosicionAbanico,oTramoEjeBasico iTramoPrevio, double iAzimutGrados, double iTramoLongitud, oP2d iPtoTarget)
		{
			
			this.tramoPrevio = iTramoPrevio;
			this.idAbanico = iIdAbanico;
			this.idPosicion = iIdTramoPosicionAbanico;
			this.idTramo = this.tramoPrevio.idTramo + 1;

			this.P1 = this.tramoPrevio.P2;
			this.P2 = (oP3d) this.P1.getPointFromAzimutAndLongitud(iAzimutGrados, iTramoLongitud).convertTo3d(null);

			this.ptoTarget = iPtoTarget;
		   
			this.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceReducido;
		
			this.lstTramos.Add(this);
		}

	}


	

}
