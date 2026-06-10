using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{
    
   public static  class oExtensionEnumeraciones
  {

      public static eRoadGrupo getRoadGrupo (string iString)
      {
         return  (eRoadGrupo)Enum.Parse(typeof(eRoadGrupo),iString, true);
      }

      public static eRoadPreferencias getRoadPreferencias (string iString)
      {
          return (eRoadPreferencias)Enum.Parse(typeof(eRoadPreferencias), iString, true);
      }

      public static eKvPrefer getKvPreferencias(string iString)
      {
          return (eKvPrefer)Enum.Parse(typeof(eKvPrefer), iString, true);
      }

      public static eRoadSeccion getRoadSeccion (string iString)
      {
          return (eRoadSeccion)Enum.Parse(typeof(eRoadSeccion), iString, true);
      }

      public static eIdioma getIdioma(string iString)
      {
          return (eIdioma)Enum.Parse(typeof(eIdioma), iString, true);
      }

  }
}
