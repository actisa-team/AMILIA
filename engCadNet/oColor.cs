using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{

    using Autodesk.AutoCAD.Colors;

   public class oColor
    {

       private static oColor mColor = null;

       public static oColor getInstance
       {

           get
           {
               if (mColor == null)
               {
                   mColor = new oColor();
               }

               return mColor;
           }
       
       }


        
        private  Dictionary<eColor,Color>  mDicColor = null;

        public  Dictionary<eColor, Color> lstColor
        {
            get
            {
                if (mDicColor == null)
                {
                    mDicColor = new Dictionary<eColor, Color>();
                    mDicColor.Add(eColor.rojo,Color.FromColorIndex(ColorMethod.None,1));
                    mDicColor.Add(eColor.amarillo, Color.FromColorIndex(ColorMethod.None, 2));
                    mDicColor.Add(eColor.verde, Color.FromColorIndex(ColorMethod.None, 3));
                    mDicColor.Add(eColor.cyan, Color.FromColorIndex(ColorMethod.None, 4));
                    mDicColor.Add(eColor.azul, Color.FromColorIndex(ColorMethod.None, 5));
                    mDicColor.Add(eColor.morado, Color.FromColorIndex(ColorMethod.None, 6));
                    mDicColor.Add(eColor.blanco, Color.FromColorIndex(ColorMethod.None, 7));
                    mDicColor.Add(eColor.grisOscuro, Color.FromColorIndex(ColorMethod.None, 8));
                    mDicColor.Add(eColor.grisClaro, Color.FromColorIndex(ColorMethod.None, 9));               
                }

                return mDicColor;
            
            }       
        }

        public  Color rojo { get { return lstColor[eColor.rojo]; } }
        public  Color amarillo {get{return lstColor[eColor.amarillo];}}
        public  Color morado { get { return lstColor[eColor.morado];}}
        public  Color grisClaro { get { return lstColor[eColor.grisClaro];}}
        public  Color cyan { get {return lstColor[eColor.cyan];}}
        public  Color azul { get {return lstColor[eColor.azul];}}
        public  Color verde { get { return lstColor[eColor.verde]; } }


        public const short cRojoIndex = 1;
        public const short cAmarilloIndex = 2;
        public const short cVerdeIndex = 3;
        public const short cCyanIndex = 4;
        public const short cAzulIndex = 5;
        public const short cMoradoIndex = 6;
        public const short cBlancoIndex = 7;
        public const short cGrisOscuroIndex = 8;
        public const short cGrisClaroIndex = 9;
       
    
    
    }
}
