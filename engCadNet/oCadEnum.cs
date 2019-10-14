using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{

    public enum eVariables
    {
        MENUNAME,
        FILEDIA,
        CMDECHO,
        MODEMACRO,
        NAVVCUBE,
    }

    public enum eEntidades
    {
        NONE,
        CIRCLE,
        POINT,
        LINE,
        INSERT,
        LWPOLYLINE,
        POLYLINE,
        REGION,
        AECC_ALIGNMENT,
        HATCH,
        IMAGE,
        MESH,
    }


    public enum eVerHor
    { 
        vertical =1,
        horizontal =2,
    }


    public enum eColor
    { 
         
        rojo=1,
        amarillo=2,
        verde=3,
        cyan=4,
        azul=5,
        morado=6,
        blanco=7,
        grisOscuro=8,
        grisClaro=9,
        
    }

    public enum eFuentes
    {
        Arial = 1,
        romans = 2
    }

    public enum eHatch
    { 
       ANSI31,
       ANSI32,
       ANSI33,
       ANSI34,
       ANSI35,
       ANSI36,
       ANSI37,
       ANSI38,
       ANSI39,
       DOTS,
       SOLID,
    }

}
