using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using System.ComponentModel;

namespace tadLayLogica.EjeTJ.Tramos
{

    using tadLayLogica.EjeTJ.Secciones;

    using tadLayLogica.EjeTJ.Vertice;
    using tadLayShare;
    
    public class oTramoAcuerdos : oTramoMaster<oAcuerdo>
    {

        //Propiedades Geometria
        public static oRoadGeo mRoadGeo = null;

        private eEstructuraOld? mEstructura = null;

        private double? mPendMaxPorCiento = null;
        private double? mPendMinPorCiento = null;

        private bool? mViabilidad = null;
        
        

        Dictionary<int, oSeccion> mDicSeccion = null;

        /// <summary>
        /// Diccionario Incumplimientos ; TRUE Ajustar Incumplimiento
        /// </summary>
        Dictionary<eTramoIncumplimientos,bool> mDicIncumplimientosCheck = null;
        
        
        
       

        //Funciones Delegadas
        public static Func<int, bool> funGetIsViable = null;
        public static Func<double, double> funGetZTerreno = null;
        public static Func<int, oTramoAcuerdos> funGetTramo = null;


    

        public oTramoAcuerdos ()
        {
        
        
        }


         public oTramoAcuerdos(oAcuerdo iVi, oAcuerdo iVj)    
             :base(iVi,iVj)
         
         {


         }


         #region "Propiedades"

        /// <summary>
        /// Pendiente Máxima Tramo Por Ciento
        /// </summary>
        [DisplayName ("Pendiente Máxima")]
         public double pendMaxPorCiento
         {
             get
             {
                 if (mPendMaxPorCiento.HasValue)
                 {
                     return mPendMaxPorCiento.Value;
                 }
                 else
                 {
                     throw new oExPropertieNullValue("Pendiente Máxima Tramo, Valor Nulo");
                 }
             
             }

             set
             {
                 mPendMaxPorCiento = value;                    
             }
         
         
         }
        /// <summary>
        /// Incremento Z máximo en el Tramo
        /// </summary>
        [Browsable(false)]
        public double incZmax
        {
            get
            {
                return lon2d.Value * pendMaxPorUno;          
            }       
        }
        /// <summary>
        /// Incremento Z minimo en el Tramo
        /// </summary>
        public double incZmin
        {
            get
            {
                return lon2d.Value * pendMinPorUno;
            }
        }
        /// <summary>
        /// Pendiente Máxima Tramo Por Uno
        /// </summary>
         public double pendMaxPorUno
         {
             get
             {
                 return pendMaxPorCiento / 100;     
             }
         
         }
         /// <summary>
         /// Pendiente Mínima Tramo Por Uno
         /// </summary>
         public double pendMinPorUno
         {
             get
             {
                 return pendMinPorCiento / 100;
             }
         }
         /// <summary>
         /// Pendiente Minima Tramo Por Ciento
         /// </summary>
         public double pendMinPorCiento
         {
             get
             {
                 if (mPendMinPorCiento.HasValue)
                 {
                     return mPendMinPorCiento.Value;
                 }
                 else
                 {
                     throw new oExPropertieNullValue("Pendiente Mínima Tramo, Valor Nulo");
                 }

             }

             set
             {
                 mPendMinPorCiento = value;
             }


         }
        /// <summary>
        /// Estructuras del Tramo [SinEstructuras, Tunel, Pila, TunelPila]
        /// </summary>
         public eEstructuraOld estructura
         {

             get
             {
                 if (mEstructura.HasValue)
                 {
                     return mEstructura.Value;
                 }
                 else
                 {
                     throw new oExPropertieNullValue("Estructura Tramo");

                 }


             }

             set
             {
                 mEstructura = value;
             }


         }
        /// <summary>
        /// Cumple Pendiente Max
        /// </summary>
         public bool cumplePendienteMax
         {
             get
             {

                 if (pendientePorCientoAbs3D.Value > pendMaxPorCiento)
                 {
                     return false;
                 }
                 else
                 {
                     return true;
                 }
             }
         
         }
        /// <summary>
        /// Cumple Pendiente Minima
        /// </summary>
         public bool cumplePendienteMin
         {
             get
             {

                 if (pendientePorCientoAbs3D.Value < pendMinPorCiento)
                 {
                     return false;
                 }
                 else
                 {
                     return true;
                 }
             }

         }
        /// <summary>
        /// Cumple Pendiente
        /// </summary>
         public bool cumplePendiente
         {

             get
             {
                 if (cumplePendienteMax && cumplePendienteMin)
                 {
                     return true;
                 }
                 else
                 {
                     return false;
                 }
              

             }
         }
        /// <summary>
        /// Tramo Tiene Estructuras
        /// </summary>
         public bool hasEstructuras
         {
             get
             {
                 if (estructura == eEstructuraOld.SinEstructura)
                 {
                     return false;
                 }
                 else
                 {
                     return true;                
                 }            
             }        
         }
         /// <summary>
         /// Tramo Tiene Incumplimientos
         /// </summary>
         public bool hasIncumplimientos
         {

             get
             {
                 if (lstIncumplimientos.Count>0)
                 {
                     return true;
                 }
                 else
                 {
                     return false;
                 }

             }

         }
        /// <summary>
        /// Tramo Tiene Incumplimientos Viables A Tratar
        /// </summary>
         public bool hasIncumplimientosViables
         {

             get
             {
                   if (lstIncumplimientos.ContainsValue(true))
                     {
                         return true;
                     }
                     else
                     {
                         return false;
                     }
       
                 }       
             
         }
        /// <summary>
        /// Listado de Incumplimientos
        /// </summary>
         public Dictionary<eTramoIncumplimientos,bool> lstIncumplimientos
         {

             get
             {
                 if (mDicIncumplimientosCheck== null)
                 {
                     throw new Exception("La Lista de Incumplimientos No Puede Ser Nula");
                 }
                 else
                 {
                     return mDicIncumplimientosCheck;               
                 }
             
             
             }
         
         
         
         }
         /// <summary>
         /// Tramo es Viable (True Ajuste Pendiente + TerraplenDesmonte ; False  solo Pendiente)
         /// </summary>
         public bool isTramoViable
         {
             get
             {

                 if (mViabilidad == null)
                 {
                     mViabilidad = funGetIsViable(this.id);
                 }

                 return mViabilidad.Value;
               
             }

             set
             {
                 mViabilidad = value;            
             }
         }
         [Browsable(false)]
         public oTramoAcuerdos preTramo
         {
             get
             {
                 if (this.isTramoInicial)
                 {
                     return null;
                 }
                 else
                 {
                     return funGetTramo(this.id - 1);
                 }

             }
         }
         [Browsable(false)]
         public oTramoAcuerdos nexTramo
         {
             get
             {
                 if (this.isTramoFinal)
                 {
                     return null;

                 }
                 else
                 {

                     return funGetTramo(this.id + 1);
                 }
                 
                 
                 
             }
         }

         public List<oSeccion> SeccionesIncumple
         {
             get
             {
                 //Creo la Sección
                 createSeccionTramo();


                 if (!hasEstructuras)
                 {

                     var myQuery = from p in mDicSeccion.Values
                                   where p.Apoyo.Value == eApoyo.estDes & p.ZDesfaseAbs > mRoadGeo.terraplenDesmonteMaxProyecto |
                                   p.Apoyo.Value == eApoyo.estTer & p.ZDesfaseAbs > mRoadGeo.terraplenDesmonteMaxProyecto
                                   orderby p.Id ascending
                                   select p;

                     return myQuery.ToList<oSeccion>();

                 }
                 else
                 {
                     return null;
                 }
              
             }
         
         
         
         
         }

         #endregion


         #region "Ajuste"

         public void ajusteTramo()
         {

             //Reinicio el valor de la viabilidad del tramo

            


             while (hasIncumplimientosViables)
             {


                 if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.pendMax))
                 {
                     fitToPendMaxRotateMid();
                 }

                 if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.pendMin))
                 {
                     if (this.isTramoInicial)
                     {
                         getAjustePendienteMinimaTramoInical();
                     }
                     else if (this.isTramoFinal)
                     {
                         getAjustePendienteMinimaTramoFinal();
                     }
                     else
                     {
                         getAjustePendienteMinimaTramosIntermedios();

                     }
                 }




                 if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.desmMax))
                 {
                     getAjusteDesmonteMaximo();
                 }

                 if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.terrMax))
                 {
                     getAjusteTerraplenMaximo();
                 }

                 if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.desmTerrViable))
                 {
                     getAjusteDesmonteTerraplen();               
                 }
                 if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.terrDesmViable))
                 {
                     getAjusteTerraplenDesmonte();
                 }



                 //Reinicio los Datos
                 getIncumplimientoTramo();

             }

         }




         #endregion



         #region "Ajuste Pendiente Máxima"

         /// <summary>
         /// Ajuste Pendiente Máxima
         /// </summary>
         private void fitToPendMaxRotateMid()
         {

             if (Vi.IsZKon.Value && Vj.IsZKon.Value)
             {
                 throw new Exception(string.Format("Error al Ajustar la Pendiente Máxima del Tramo {0} (Ambos Extremos Z es Constante.", this.ToString()));
             }
             else if (!Vi.IsZKon.Value && !Vj.IsZKon.Value)
             {

                 if (Vi.position.Z > Vj.position.Z)
                 {
                     Vi.position.Z = Vi.position.Z - (incZmax / 2);
                     Vj.position.Z = Vj.position.Z + (incZmax / 2);
                 }
                 else
                 {
                     Vi.position.Z = Vi.position.Z + (incZmax / 2);
                     Vj.position.Z = Vj.position.Z - (incZmax / 2);
                 }
             
             }

             else if (Vi.IsZKon.Value)
             {
                 Vj.position.Z = Vj.position.Z - incZmax;
             }
             else
             {

                 Vi.position.Z = Vi.position.Z + incZmax;
             
             }


    
         }


         #endregion
         #region "Ajustes Pendiente Minima"
         /// <summary>
         /// Ajuste Pendiente Minima Tramos Intermedios
         /// </summary>
         private void getAjustePendienteMinimaTramosIntermedios()
         {

             //Controlo que el tramo a ajustar por pendiente minima no sea K
             if (this.Vi.IsZKon.Value && this.Vj.IsZKon.Value)
             {
                 throw new Exception(string.Format("Error al Ajustar la pendiente Mínima del tramo {0}, ambos extremos tienen cota Z Constante", this.ToString()));
             }

             //Caso A1
             if (preTramo.Vi.position.Z > Vi.position.Z && nexTramo.Vj.position.Z > Vj.position.Z)
             {
               fitToPendMinViUp(); //Rev
             }
             //Caso A2
             else if (preTramo.Vi.position.Z > Vi.position.Z && nexTramo.Vj.position.Z == Vj.position.Z)
             {
               fitToPendMinViUp(); //rev
             }
             //Caso A3
             else if (preTramo.Vi.position.Z > Vi.position.Z && nexTramo.Vj.position.Z < Vj.position.Z)
             {
               fitToPendMinRotatePos(); //rev
             }
             //Caso A4
             else if (preTramo.Vi.position.Z == Vi.position.Z && nexTramo.Vj.position.Z > Vj.position.Z)
             {
                fitToPendMinViUp(); //rev
             }
             //Caso A5
             else if (preTramo.Vi.position.Z == Vi.position.Z && nexTramo.Vj.position.Z == Vj.position.Z)
             {
                 fitToPendMinRotatePos(); //rev
             }
             //Caso A6
             else if (preTramo.Vi.position.Z == Vi.position.Z && nexTramo.Vj.position.Z < Vj.position.Z)
             {
                 fitToPendMinViUp(); //rev
             }
             //Caso A7
             else if (preTramo.Vi.position.Z < Vi.position.Z && nexTramo.Vj.position.Z > Vj.position.Z)
             {
                 fitToPendMinRotateNeg(); //rev
             }
             //Caso A8
             else if (preTramo.Vi.position.Z < Vi.position.Z && nexTramo.Vj.position.Z == Vj.position.Z)
             {
                 fitToPendMinRotateNeg(); //rev
             }
             //Caso A9
             else if (preTramo.Vi.position.Z < Vi.position.Z && nexTramo.Vj.position.Z < Vj.position.Z)
             {
                fitToPendMinViDown();
             }
             else
             {
                 throw new Exception(string.Format("Error en el Ajuste de la pendiente mínima del tramo{0}", this.ToString()));
             }









         }
         /// <summary>
         /// Ajuste Pendiente Minima TRAMO INICIAL
         /// </summary>
         private void getAjustePendienteMinimaTramoInical()
         {

             if (Vi.IsZKon.Value && Vj.IsZKon.Value)
             {
                 throw new Exception(string.Format("Error al Ajustar la pendiente Mínima del tramo {0}, ambos extremos tienen cota Z Constante", this.ToString()));
             }
             else if (Vj.position.Z >= Vj.Zterreno)
             {
                 Vj.position.Z = Vi.position.Z - incZmin;
             }
             else 
             {
                 Vj.position.Z = Vi.position.Z + incZmin;
             }
      
         }
         /// <summary>
         /// Ajuste Pendiente Minima TRAMO FINAL
         /// </summary>
         private void getAjustePendienteMinimaTramoFinal()
         {

             if (Vi.IsZKon.Value && Vj.IsZKon.Value)
             {
                 throw new Exception(string.Format("Error al Ajustar la pendiente Mínima del tramo {0}, ambos extremos tienen cota Z Constante", this.ToString()));
             }
             else if (Vi.position.Z >= Vj.Zterreno)
             {
                 Vi.position.Z = Vj.position.Z - incZmin;
             }
             else
             {
                 Vi.position.Z = Vj.position.Z + incZmin;
             }
             

         }
         /// <summary>
         /// Ajuste Pendiente Minima Vi Up
         /// </summary>
         private void fitToPendMinViUp()
         {
             if (Vi.IsZKon.Value)
             {
                 Vj.position.Z = Vi.position.Z + incZmin;
 
             }
             else
             {
                 Vi.position.Z = Vj.position.Z + incZmin;
             }
         }
         /// <summary>
         /// Ajuste Pendiente Minima Vi Down
         /// </summary>
         private void fitToPendMinViDown()
         {
             if (Vi.IsZKon.Value)
             {
              Vj.position.Z = Vi.position.Z - incZmin;
 
             }
             else
             {
              Vi.position.Z = Vj.position.Z + incZmin;
             }            
         }
         /// <summary>
         /// Ajuste Pendiente Minima // Giro Horario
         /// </summary>
         private void fitToPendMinRotatePos()
         {

             if (! Vi.IsZKon.Value && ! Vj.IsZKon.Value  )
             {

                 double myIncPos = (incZmin-IncZ)/2 ; 

                 
                 Vi.position.Z = Vi.position.Z + myIncPos;

                 Vj.position.Z = Vj.position.Z - myIncPos; 


             }
             else if (Vi.IsZKon.Value)
             {
                 Vj.position.Z = Vi.position.Z + (incZmin);
             }
             else
             {
                 Vi.position.Z = Vj.position.Z + (incZmin);
             }
         }
         /// <summary>
         /// Ajuste Pendiente Minima // Giro AntiHorario
         /// </summary>
         private void fitToPendMinRotateNeg()
         {

             if (!Vi.IsZKon.Value && !Vj.IsZKon.Value)
             {
                  double myIncNeg = (incZmin - IncZ) / 2; 
                 
                 Vi.position.Z = Vi.position.Z - myIncNeg;
                 Vj.position.Z = Vj.position.Z + myIncNeg;
             }
             else if (Vi.IsZKon.Value)
             {
                 Vj.position.Z = Vj.position.Z + (incZmin);
             }
             else
             {
                 Vi.position.Z = Vi.position.Z + (incZmin);
             }            
         }
        #endregion
         #region "Ajustes Desmonte Maximo"
         private void getAjusteDesmonteMaximo()
         { 
         

             bool continuar = true ;


             while (continuar)
             {
                 //Debemos de Subir el Rasante en 1 metros

                 #region "Caso 1"
               

                 if (!Vi.IsZKon.Value && !Vj.IsZKon.Value)
                 {

                     //Aseguro que al sumar 1, no excedo el valor
                     if (Vi.position.Z + 1 > Vi.ZmaxAjuste)
                     {
                         Vi.position.Z = Vi.ZmaxAjuste;
                     }
                     else
                     {
                         Vi.position.Z = Vi.position.Z + 1;
                     }


                     //Aseguro que al sumar 1, no excedo el valor
                     if (Vj.position.Z + 1 > Vj.ZmaxAjuste)
                     {
                         Vj.position.Z = Vj.ZmaxAjuste;
                     }
                     else
                     {
                         Vj.position.Z = Vj.position.Z + 1;
                     }


                     //Reviso si tengo incumplimientos por Desmonte Máximo
                     //Si es así configuro el tramo como no viable.
                     if (Vi.position.Z == Vi.ZmaxAjuste && Vj.position.Z == Vj.ZmaxAjuste)
                     {

                       getIncumplimientoTramo();

                         if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.desmMax))
                         {
                             this.mViabilidad = false;
                         }

                     }
                     else
                     {
                         getIncumplimientoTramo();
                     }
}
                 #endregion                
                 #region "Caso 2"
                 else if (Vi.IsZKon.Value) // Vi es Constante
                 {

                     //Aseguro que al sumar 1, no excedo el valor
                     if (Vj.position.Z + 1 > Vj.ZmaxAjuste)
                     {
                         Vj.position.Z = Vj.ZmaxAjuste;
                         getIncumplimientoTramo();

                         if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.desmMax))
                         {
                             this.mViabilidad = false;
                         }


                     }
                     else
                     {
                         Vj.position.Z = Vj.position.Z + 1;
                         getIncumplimientoTramo();

                     }

                 }

                 #endregion
                 #region "Caso 3"
                 else // Vj es Constante
                 {

                     //Aseguro que al sumar 1, no excedo el valor
                     if (Vi.position.Z + 1 > Vi.ZmaxAjuste)
                     {
                         Vi.position.Z = Vi.ZmaxAjuste;

                         getIncumplimientoTramo();

                         if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.desmMax))
                         {
                             this.mViabilidad = false;
                         }

                     }
                     else
                     {
                         Vi.position.Z = Vi.position.Z + 1;

                         getIncumplimientoTramo();
                     }

                 #endregion


                //Determino si Continuo el Proceso
                 if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.desmMax) && this.mViabilidad.Value == true)
                 {
                     continuar= true;
                 }
                 else
                 {
                     continuar = false;
                 }

             }
 
             }
         }
         #endregion
         #region "Ajustes Terraplen Máximo"
         private void getAjusteTerraplenMaximo()
         {

             bool continuar = true ;


             while (continuar)
             {


                 #region "Caso 1"
               
                 //Debemos de Bajar la Rasante en 1 metros

                 if (!Vi.IsZKon.Value && !Vj.IsZKon.Value)
                 {

                     //Aseguro que al sumar 1, no excedo el valor
                     if (Vi.position.Z - 1 < Vi.ZminAjuste)
                     {
                         Vi.position.Z = Vi.ZminAjuste;
                     }
                     else
                     {
                         Vi.position.Z = Vi.position.Z - 1;
                     }


                     //Aseguro que al sumar 1, no excedo el valor
                     if (Vj.position.Z - 1 < Vj.ZminAjuste)
                     {
                         Vj.position.Z = Vj.ZminAjuste;
                     }
                     else
                     {
                         Vj.position.Z = Vj.position.Z - 1;
                     }


                     //Reviso si tengo incumplimientos por Desmonte Máximo
                     //Si es así configuro el tramo como no viable.
                     if (Vi.position.Z == Vi.ZminAjuste && Vj.position.Z == Vj.ZminAjuste)
                     {
                         getIncumplimientoTramo();

                         if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.terrMax))
                         {
                             this.mViabilidad = false;
                         }

                     }

                     else
                     {
                         getIncumplimientoTramo();
                     }


                 }
                  #endregion

                 #region "Caso3"
                

                 else if (Vi.IsZKon.Value) // Vi es Constante
                 {
                     //Aseguro que al sumar 1, no excedo el valor
                     if (Vj.position.Z - 1 < Vj.ZminAjuste)
                     {
                         Vj.position.Z = Vj.ZminAjuste;

                         if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.terrMax))
                         {
                             this.mViabilidad = false;
                         }
                     }
                     else
                     {
                         Vj.position.Z = Vj.position.Z - 1;
                         getIncumplimientoTramo();
                     }

                 }
                 #endregion


                 #region "Caso3"
               
                 else // Vj es Constante
                 {

                     //Aseguro que al sumar 1, no excedo el valor
                     if (Vi.position.Z - 1 < Vi.ZminAjuste)
                     {
                         Vi.position.Z = Vi.ZminAjuste;

                         getIncumplimientoTramo();

                         if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.terrMax))
                         {
                             this.mViabilidad = false;
                         }
                     }
                     else
                     {
                         Vi.position.Z = Vi.position.Z - 1;

                         getIncumplimientoTramo();
                     }
                 }

                 #endregion

                 //Determino si Continuo el Proceso
                 if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.terrMax) && this.mViabilidad.Value == true)
                 {
                     continuar = true;
                 }
                 else
                 {
                     continuar = false;
                 }


             }
             }   
         #endregion
         #region "Ajuste DesmonteTerraplen"

        /// <summary>
        /// Desmonte-Terraplen // Giro Positivo
        /// </summary>
         private void getAjusteDesmonteTerraplen()
         { 
         
             //Caso Giro Positivo Vi Up ; Vj  Down
             if (!Vi.IsZKon.Value && !Vj.IsZKon.Value)
             {

                 //Aseguro que al sumar 1, no excedo el valor
                 if (Vi.position.Z + 1 > Vi.ZmaxAjuste)
                 {
                     Vi.position.Z = Vi.ZmaxAjuste;
                 }
                 else
                 {
                     Vi.position.Z = Vi.position.Z + 1;
                 }


                 //Aseguro que al restar 1, no excedo el valor
                 if (Vj.position.Z - 1 < Vj.ZminAjuste)
                 {
                     Vj.position.Z = Vj.ZminAjuste;
                 }
                 else
                 {
                     Vj.position.Z = Vj.position.Z - 1;
                 }


                 //Reviso si tengo incumplimientos por Desmonte Máximo
                 //Si es así configuro el tramo como no viable.
                 if (Vi.position.Z == Vi.ZmaxAjuste && Vj.position.Z == Vj.ZminAjuste)
                 {

                     getIncumplimientoTramo();

                     if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.desmTerrViable))
                     {
                         this.mViabilidad = false;
                     }

                 }
                 else
                 {
                     getIncumplimientoTramo();
                 }




             }
             else
             {
                 mViabilidad = false;
             }
                  
         }

         #endregion
         #region "Ajuste TerraplenDesmonte"

         /// <summary>
         /// Terraplen-Desmonte // Giro Negativo
         /// </summary>
         private void getAjusteTerraplenDesmonte()
         {

             //Caso Giro Positivo Vi Up ; Vj  Down
             if (!Vi.IsZKon.Value && !Vj.IsZKon.Value)
             {

                 //Aseguro que al sumar 1, no excedo el valor
                 if (Vi.position.Z - 1 < Vi.ZminAjuste)
                 {
                     Vi.position.Z = Vi.ZminAjuste;
                 }
                 else
                 {
                     Vi.position.Z = Vi.position.Z - 1;
                 }


                 //Aseguro que al restar 1, no excedo el valor
                 if (Vj.position.Z + 1 > Vj.ZmaxAjuste)
                 {
                     Vj.position.Z = Vj.ZmaxAjuste;
                 }
                 else
                 {
                     Vj.position.Z = Vj.position.Z + 1;
                 }


                 //Reviso si tengo incumplimientos por Desmonte Máximo
                 //Si es así configuro el tramo como no viable.
                 if (Vi.position.Z == Vi.ZminAjuste && Vj.position.Z == Vj.ZmaxAjuste)
                 {

                     getIncumplimientoTramo();

                     if (mDicIncumplimientosCheck.ContainsKey(eTramoIncumplimientos.terrDesmViable))
                     {
                         this.mViabilidad = false;
                     }
                   
                 }
                 else
                 {
                     getIncumplimientoTramo();
                 }

             }
             else
             {
                 mViabilidad = false;
             }

         }

         #endregion
         #region "Metodos Privados"

         private double  getZterreno(double iPk)
         {
             return funGetZTerreno(iPk);   
         }








     
         private void createSeccionTramo()
         {
             #region "Obtengo la Sección"

             int i = 0;
             double myIncX = 0;

             mDicSeccion = new Dictionary<int, oSeccion>();


             double myPkCurrent = this.Vi.position.X;
             double myPkZter;

             while (myPkCurrent < this.Vj.position.X)
             {

                 myPkZter = getZterreno(myPkCurrent);

                 oSeccion mySeccion = new oSeccion(this.id,i, myPkCurrent, getZFromOrigen(myIncX), myPkZter, hasEstructuras, mRoadGeo.terraplenDesmonteMaxProyecto);

                 mDicSeccion.Add(i, mySeccion);

                 i++;

                 myIncX = i * mRoadGeo.pTramoLonDiscre;

                 myPkCurrent = myPkCurrent + mRoadGeo.pTramoLonDiscre;

             }

             #endregion
         }





        /// <summary>
        /// Incumplimientos de la Sección
        /// </summary>
        /// <returns></returns>
         public void getIncumplimientoTramo()
         {

             mDicIncumplimientosCheck = new Dictionary<eTramoIncumplimientos, bool>();


             if (!cumplePendienteMax)
             {
                 mDicIncumplimientosCheck.Add(eTramoIncumplimientos.pendMax, true);     
             }

             if (!cumplePendienteMin)
             {
                 mDicIncumplimientosCheck.Add(eTramoIncumplimientos.pendMin, true);               
             }

             if (!hasEstructuras)
             { 
                 
                 //Número de Incumplimientos a Desmonte
                 int myIncDes = SeccionesIncumple.Count(x => x.Apoyo.Value == eApoyo.estDes);

                 //Número de Incumplimientos a Terraplem
                 int myIncTer = SeccionesIncumple.Count(k => k.Apoyo.Value == eApoyo.estTer);

                 //Sección Cumple
                 if (myIncDes == 0 && myIncTer == 0)
                 {
                     return;
                 }

                 //Incumple a Desmonte Máximo
                 if (myIncDes > 0 && myIncTer == 0)
                 {
                     mDicIncumplimientosCheck.Add(eTramoIncumplimientos.desmMax,isTramoViable);    
                 }

                 //Incumple Terraplen Máximo
                 if (myIncDes == 0 && myIncTer > 0)
                 {
                     mDicIncumplimientosCheck.Add(eTramoIncumplimientos.terrMax, isTramoViable);                    
                 }

                 //En la misma Sección Desmonte & Terraplen
                 if (myIncDes > 0 && myIncTer > 0)
                 {

                     int myIncIdMin;
                     int myIncIdMax;
                     List<oSeccion> myLstIncumple = SeccionesIncumple.ToList<oSeccion>();


                     //Obtengo el primer apoyo que incumple
                     eApoyo myApoyoExt = myLstIncumple.First().Apoyo.Value;

                     eApoyo myApoyoInt ;

                     if (myApoyoExt == eApoyo.estDes)
                     {
                       myApoyoInt = eApoyo.estTer;
                     }
                     else
                     {
                        myApoyoInt = eApoyo.estDes;
                     }

                  
                    //Obtengo el Indice Menor y Maximo del Apoyo
                    var myQueryKO =         from p in myLstIncumple
                                            where p.Apoyo == myApoyoExt
                                            select p;


                    myIncIdMin = myQueryKO.Min(x => x.Id);
                    myIncIdMax = myQueryKO.Max(x => x.Id);

                    //Busco Si Existe algun Terraplen
                    var myQueryApoyoInt = from k in myLstIncumple
                                        where
                                        k.Id > myIncIdMin &
                                        k.Id < myIncIdMax &
                                        k.Apoyo == myApoyoInt
                                        select k;

                         if (myQueryApoyoInt.Count() == 0)
                         {
                             //Caso Desmonte-Terraplen Giro Horario

                             if (myApoyoExt == eApoyo.estDes)
                             {
                                mDicIncumplimientosCheck.Add(eTramoIncumplimientos.desmTerrViable, isTramoViable);
                             }
                             else
                             {
                               mDicIncumplimientosCheck.Add(eTramoIncumplimientos.terrDesmViable, isTramoViable);
                             }               
                         }
                         else
                         {
                             mDicIncumplimientosCheck.Add(eTramoIncumplimientos.desmTerrNoViable, false);
                         }
                     

                     
                     }
                 }
                 
             }

         

          
       #endregion


         public override object createDerivedFromBase(object myBase)
         {
             oTramoBase mySegmento = (oTramoBase)myBase;

             this.id = mySegmento.id;
             this.Vi = mySegmento.Vi as oAcuerdo;
             this.Vj = mySegmento.Vj as oAcuerdo;

             return this;


         }



         public override string ToString()
        {
            return base.ToString() + " ; Est : " + mEstructura.ToString();
        }
          
        }


    
    
}
