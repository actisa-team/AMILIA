using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLogica.datos.proyecto;

namespace tadLayLogica.logica.medicion
{
    
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;

    using engCadNet;
    using engNet;

    using engNet.Extension.Double;
using tadLayLan.Tdi;

   public abstract class oSeccionMedicion : IDisposable
    {

        public Guid zonaId { get; private set; }
        public double pk { get; private set; }

       public string zonaName { get; private set; }

       public abstract eSecMedicion tipo {get;}

       public List<oMedItemModel> lstMedicion = new List<oMedItemModel>();



       #region "Constructor"


       public oSeccionMedicion(Guid iZonaId, string iZonaName, double iPk)
       {
           zonaId = iZonaId;
           zonaName = iZonaName;
           pk = iPk;
       }

       #endregion



       public virtual void drawMedicion(Point3d iPto, string iLayer, Guid idSolucion)
       {

           var miQ = from entry in lstMedicion
                     group entry by new { entry.code, entry.row.idMaterial } into g
                     select new { grupo = g.First().descripcion, material = g.First().row.nombre, ud = g.First().ud, m2 = g.Sum(p => p.medicion)};


           StringBuilder miStr = new StringBuilder();

            miStr.AppendLine(strFrmInformes.uiPk+": " + pk.roundOff(3).ToString());

            miStr.AppendLine(strFrmInformes.uiZona+": " + zonaName);

           foreach (var fvar in miQ)
           {
               miStr.AppendLine(fvar.grupo + " ; "+strFrmInformes.uiMaterial+ " " + fvar.material + ": " + fvar.m2.roundOff(2) + " " + fvar.ud);


               GuardarMedicion(idSolucion, fvar.grupo, fvar.material, fvar.m2);
           }

           oMTexto.addMText2D(miStr.ToString(), iPto[0], iPto[1]-5,0.15, 0, iLayer);





        }

       private void GuardarMedicion(Guid idSolucion, string grupo, string material, double value)
       {
           var miRow = oSingletonDsApp.getInstance.dataset.tbMedicionesBySeccion.NewtbMedicionesBySeccionRow();

           miRow.idTbSolucion = idSolucion;
           miRow.grupo = grupo;
           miRow.material = material;
           miRow.pk = pk;
           miRow.valor = value;

           oSingletonDsApp.getInstance.dataset.tbMedicionesBySeccion.AddtbMedicionesBySeccionRow(miRow);
       }


       /// <summary>
        /// LAS SECCIONES SE MIDEN POR ML ; DEBEMOS PASARLAS A TOTAL 
        /// (M2  -> M3)
        /// (UDS -> ML)
        /// </summary>
       public void AddMedicionUnitaria (oMedItemModel iMedicion, double iIntervaloPk)
       {
           iMedicion.medicion = iMedicion.medicion * iIntervaloPk;
           lstMedicion.Add(iMedicion);
       }

       /// <summary>
       /// LAS SECCIONES SE MIDEN POR ML ; DEBEMOS PASARLAS A TOTAL 
       /// (M2  -> M3)
       /// (UDS -> ML)
       /// </summary>
       public void AddRangeMedicionUnitaria(List<oMedItemModel> iLstMedicion, double iIntervaloPk)
       {
           foreach (oMedItemModel item in iLstMedicion)
           {
               item.medicion = item.medicion * iIntervaloPk;
               lstMedicion.Add(item);
           }
       }



       public List<oMedItemModel> filtroByCode(List<eNodo> iLstNodos)
       {
           var miQuery = from p in lstMedicion
                         where iLstNodos.Contains(p.code)
                         select p;


           return miQuery.ToList();
       }

       public List<oMedItemModel> filtroByCode(eNodo iNodo)
       {
           var miQuery = from p in lstMedicion
                         where p.code == iNodo
                         select p;

           return miQuery.ToList();
       }


       /// <summary>
       /// Listado partidas Segun su Id de Clasificacion
       /// </summary>
       public List<oMedItemModel> getListadoPartidasByIdClasificacion (short iIdClasificacion)
       {

           
           var miQuery = from p in lstMedicion
                         where p.row.idClasificacion == iIdClasificacion
                         select p;



           return miQuery.ToList();
       }


 


    


       public void Dispose()
       {
           lstMedicion.Clear();
           lstMedicion = null;
       }
    }


   public class oSeccionMedicionMovimientoTierras :oSeccionMedicion
    {


       public oSeccionMedicionMovimientoTierras(Guid iZonaId, string iZonaName, double iPk)
         :base(iZonaId,iZonaName,iPk)
       {
          
          
       }

       public override eSecMedicion tipo
       {
           get { return eSecMedicion.MOVTIE;}
       }


    


      


    }
   public class oSeccionMedicionTuneles : oSeccionMedicion
  {

      public oSeccionMedicionTuneles(Guid iIdZona, string iZonaName, double iPk)
          :base(iIdZona,iZonaName,iPk)
      {



      }

      public override eSecMedicion tipo
      {
          get { return eSecMedicion.TUNEL; }
      }


     


  }
   public class oSeccionMedicionPuentes : oSeccionMedicion
   {
       public oSeccionMedicionPuentes(Guid iIdZona, string iZonaName, double iPk)
           : base(iIdZona, iZonaName, iPk)
       {

       }

       public override eSecMedicion tipo
       {
           get { return eSecMedicion.PUENTE; }
       }

   }

}
