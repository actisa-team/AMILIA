using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{

    using System.ComponentModel;



    /// <summary>
    /// TIPO SECCION (Calzada,Tunel,Puente)
    /// </summary>
    public enum eRoadSeccion
    {
        calzada,
        tunel,
        puente,
    }


    /// <summary>
    /// TIPOS DE SANEO TERRAPLEN CON ESCALON
    /// </summary>
    public enum eSaneoTerraplenEscalon
    {
        simple,
        dobleInferior,
        dobleSuperior,
    }


    /// <summary>
    /// APP KEYS
    /// </summary>
    public enum eAppConfigKey
    {
        /// <summary>
        /// Lenguaje
        /// </summary>
        lan,
        /// <summary>
        /// Mostrar Error Secciones
        /// </summary>
        errorSectionDisplay,
        /// <summary>
        /// Modo Depuracion
        /// </summary>
        mod,
        /// <summary>
        /// Valoraciones Ultimo Abanico
        /// </summary>
        val,
    }
   /// <summary>
   /// IDIOMA
   /// </summary>
    public enum eIdioma
    {
        es,
        fr,
        en,
    }
    /// <summary>
    /// LADO
    /// </summary>
    public enum eLado
    {
        DER,
        IZQ,
    }
    /// <summary>
    /// TIPO ABANICO
    /// </summary>
    public enum eAbanicoTipo
    {
        corto,
        largos,
    }
    /// <summary>
    /// EJE BASICO TIPO TRAMO
    /// </summary>
    public enum eTramoTipoEjeBasico
    {
        avanceReducido,
        avanceCorto,
        avanceLargo,
    }
    /// <summary>
    /// PUNTO SALIDA - LLEGADA
    /// </summary>
    public enum ePtoSalidaLlegada
    {
        /// <summary>
        /// Punto Salida
        /// </summary>
        puntoSalida,
        /// <summary>
        /// Punto LLegada
        /// </summary>
        puntoLlegada,
    }
    /// <summary>
    /// TIPO ESTUDIO
    /// </summary>
    public enum eEstudioTipo
    {
        /// <summary>
        /// Estudio Previo
        /// </summary>
        ESTPRE,
        /// <summary>
        /// Estudio Informativo
        /// </summary>
        ESTINF,
    }
    /// <summary>
    /// TIPOS DE SECCIONES MEDICIONES ; MOVTIE-ESTEST-TUNTUN
    /// </summary>
    public enum eSecMedicion
    { 
        /// <summary>
        /// Secciones Mido Movimiento Tierras
        /// </summary>
        MOVTIE,
        /// <summary>
        ///Secciones Mido los Puentes-Viaductos
        /// </summary>
        PUENTE,
        /// <summary>
        /// Secciones Mido los Tuneles
        /// </summary>
        TUNEL,

    }
    /// <summary>
    /// GRUPO CALZADAS
    /// </summary>
    public enum eSecRoadGrupo
    {
        /// <summary>
        /// Calzada Unica
        /// </summary>
        CALUNI,
        /// <summary>
        /// Calzada Doble
        /// </summary>
        CALDOB,
    }
    /// <summary>
    /// TIPO CALZADA
    /// </summary>
    public enum eSecRoadTipo
    { 
        /// <summary>
        /// Calzada Unica General
        /// </summary>
        UNIGEN,
        /// <summary>
        /// Calzada Doble Autovia
        /// </summary>
        DOBAUT,
        /// <summary>
        /// Calzada Doble Urbana
        /// </summary>
        DOBURB,
        /// <summary>
        /// Calzada Doble Sin Mediana
        /// </summary>
        DOBSIN,
    }
    /// <summary>
    /// CAPAS CALZADA
    /// </summary>
    public enum eCapaCalzada
    {   
        FIR,
        ARC,
        ASI,
    }
    /// <summary>
    /// TIPO CUNETA
    /// </summary>
    public enum eCunetaTipo
    { 
       TRIANG,
       TRAPEZ,
    }
    /// <summary>
    /// POSICION CUNETA
    /// </summary>
    public enum eCunetaPosicion
    {
        berma,
        firme,
    }
    /// <summary>
    /// TIPO SANEO
    /// </summary>
    public enum eSaneo
    { 
       terraplen,
       desmonte,
    }
    /// <summary>
    /// EJE BASICO INCUMPLLIMiENTOS
    /// </summary>
    public enum eTramoIncumplimientos
    { 
         pendMax,
         pendMin,
         desmMax,
         terrMax,
         /// <summary>
         /// Desmonte-Terraplen Viable
         /// </summary>
         desmTerrViable,
        /// <summary>
        /// Terraplen-Desmonte Viable
        /// </summary>
         terrDesmViable,
         desmTerrNoViable,
         pilaMax,
    }
    /// <summary>
    /// SI-NO
    /// </summary>
    public enum eSiNo
    {         
       Si,
       No,
    }
    /// <summary>
    /// KV PREFERENCIAS
    /// </summary>
    public enum eKvPrefer
    { 
       minimo,
       deseable,
    }
    /// <summary>
    /// TIPOS DE ACUERDO
    /// </summary>
    public enum eAcuertoTipo
    {
        convexo,
        concavo,
        plano,
    }
    /// <summary>
    /// XDATA KEY
    /// </summary>
    public enum eXdataKey
    { 
        /// <summary>
        /// Guid de la Solucion
        /// </summary>
         solucionGuid,
        /// <summary>
        /// Nombre de la Solución
        /// </summary>
         solucionName,
        /// <summary>
        /// Datos de Diseño
        /// </summary>
         roadDis,
        /// <summary>
        /// Datos de Geometria
        /// </summary>
         roadGeo,
        /// <summary>
        /// Tipo de Curvas
        /// </summary>
         ejeTrazadoCurvas,
        /// <summary>
        /// Eje Basico Estructuras
        /// </summary>
        ejeBasicoEstructuras,
        /// <summary>
        /// Codigo de la Zona 
        /// </summary>
        zonaGisCode,
         /// <summary>
        /// Id del Registro de la Zona 
        /// </summary>
        zonaGisGuid,
         /// <summary>
        /// Handle de los Objetos Vinculados
        /// </summary>
        zonaGisLinkHandle,           
    }
    /// <summary>
    /// EJE TRAZADO ENTIDADES
    /// </summary>
    public enum eTrazadoPlantaEntidades
    {
        recta,
        curva,
        espiral,
    }
    /// <summary>
    /// PERFIL LONGITUDINAL ENTIDADES
    /// </summary>
    public enum eTrazadoPerfilEntidades
    {
        recta,
        encuentro,
    }
    /// <summary>
    /// EJE TRAZADO ENTIDADES TRAMO CIVIL 3D
    /// </summary>
    public enum eRoadTramoEntidad
    { 
       FixedLine,
       FloatLineSpiral,
       FreeSSBetweenCurves, 
       FreeSTS,
    }
    /// <summary>
    /// EJE TRAZADO ENTIDADES VERTICE CIVIL 3D
    /// </summary>
    public enum eRoadVerticeEntidad
    { 
         FixedCurve,
         FreeCurve,
         FreeSpiral,
    }
    /// <summary>
    /// EJE TRAZADO TIPO CURVA
    /// </summary>
    public enum eRoadCurva
    { 
        Paso,
        NoPaso,
    }
    /// <summary>
    /// TIPO CARRETERA // PREFERENCIAS CURVAS - RECTAS
    /// </summary>
    public enum eRoadPreferencias
    { 
        curvas,
        rectas,
    }
    /// <summary>
    /// GRUPO CARRETERA (Grupo1-Grupo2)
    /// </summary>
    public enum eRoadGrupo
    { 
        Grupo1,
        Grupo2,
    }
    /// <summary>
    /// TIPO EXCAVACION
    /// </summary>
    public enum eExcavacion
    {          
        desmonte,
        terraplen,
        acota,  
    }
    /// <summary>
    /// TIPO TALUD
    /// </summary>
    public enum eTalud
    { 
        terraplen,
        desmonte,
        horizontal,
        vertical,
    }
    ///// <summary>
    ///// TIPO SECCION (Calzada,Tunel,Puente)
    ///// </summary>
    //public enum eRoadSeccion
    //{ 
    //     calzada,
    //     tunel,
    //     puente,  
    //}
    /// <summary>
    /// EJE BASICO TRAMOS INCUMPLIMIENTOS
    /// </summary>
    public enum eTramoEjeBasicoError
    {
        [Description("Error no identificado")]
        errorNoIdentificado,

        [Description("Existen Puntos del Tramo Fuera del Terreno")]
        puntoExteriorSuperficie,

        [Description("Tramo Incumple Altura Máxima Desmonte")]
        desmonteSuperior,

        [Description("Tramo Incumple Altura Máxima Terraplen")]
        terraplenSuperior,

        [Description("Tramo en Zona de No Paso")]
        zonaNoPaso,

        [Description("Tramo Incumple Altura Máxima de Pila")]
        puenteAlturaSuperior,

        [Description("Tramo Incumple Alturas Máximas Desmonte o Terraplen sin Estructuras")]
        alturaMovimientoTierrasSinEstructuras,

        [Description("Tramo Incumple Longitud Minima Abanico Primario")]
        LongitudMinimaAbanicoPrimario,

        [Description("Tramo Incumple Ángulo Minimo entre Tramos Consecutivos[AijMinimoMinimo]")]
        anguloMinimoTramosAijMinimoMinimo,

        [Description("Tramo Incumple Ángulo Minimo entre Tramos Consecutivos")]
        anguloMinimoTramosConsecutivos,

        [Description("Tramo Incumple Criterios de Diseño con Estructuras")]
        alturaMovimientoTierrasConEstructuras,

        [Description("Tramo incumple por ángulo de cruce con eje de DPH")]
        anguloDeCruceConDPHNoPermitido,

        [Description("Tramo incumple por ángulo de cruce con eje de infraestructura")]
        anguloDeCruceConInfNoPermitido,

        [Description("Tramo incumple por estar contenido en DPH ")]
        dentroDeDPHNoPermitido,

        [Description("La pendiente entre el punto final del tramo y el punto final es mayor a la permitida")]
        pendienteTramoCercanoOnjetivoNoValida,
        
        [Description("Las cotas de los tramos correspondientes en el entronque no se pueden ajustar")]
        entronqueFinalPuntoMedioCotasInvalido,
        
        [Description("Los puntos P1 y P2 están demasiado cerca")]
        entronqueFinalPuntoMedioPuntosMuyCercanos,

        [Description("La pendiente entre P1 y P2 no es valida, casos especiales (no se tiene en cuenta el mov de tierra)")]
        pendienteTramoEspecialInvalida,

        [Description("La pendiente entre P1 y P2 no es valida, casos especiales (entronque)")]
        pendienteTramoEntronquePM,

        [Description("Ángulos formado con el tramo de llegada definido por el usuario no valido")]
        anguloMinimoTramosConsecutivosConLlegadaUsuario,

        [Description("P2 demasiado cerca del borde del terreno")]
        tramoMuyCercanoAlBordeDelTerreno,

        [Description("Tramo no válido ángulos consecutivos")]
        angulosconsecutivos,

        [Description("La pendiente entre el punto final del tramo y el punto final de la otra rama es mayor a la permitida")]
        pendienteTramoFinalRamasTijeraInvalida,


        [Description("Longitud del tramo tijera menor a la mínima permitida")]
        longitudTramoTijeraMenorAMinima,

        [Description("Diferencia de pendiente tramo anterior mayor a la permitida")]
        pendienteConTramoAnteriorSuperiorAPermitidaTijera,

        [Description("Error entronque intersección entre tramos")]
        entronqueInterseccionTramosTijera,

        [Description("Longitud mínima de tramo, deifinida por el usuario, no se cumple")]
        DescartadoUsuarioLongitudMinima
    }



    public enum eEstructuraOld
    {
        SinEstructura,
        Tunel,
        Pila,
        TunelPila,
    }
    public enum eApoyo
    {
        /// <summary>
        /// A cota
        /// </summary>
        estCota,
        /// <summary>
        /// Terraplen
        /// </summary>
        estTer,
        /// <summary>
        /// Desmonte
        /// </summary>
        estDes,
        /// <summary>
        /// Tunel
        /// </summary>
        estTun,
        /// <summary>
        /// Puente o Pila
        /// </summary>
        estPue,
        /// <summary>
        /// Error
        /// </summary>
        estError,
    }



}
