using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet.entidades
{

    using Autodesk.AutoCAD.Runtime;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.EditorInput;
    using System.Runtime.InteropServices;
    using Autodesk.AutoCAD.DatabaseServices;



    public class oComando
    {
        [DllImport("acad.exe", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "acedCmd")]
        extern static int acedCmd(System.IntPtr vlist);

        //private static extern int acedCmd(System.IntPtr vlist);


        public static void send(string iComando)
        {

           string miCmd = iComando.Remove(0, 1);
           
            ResultBuffer rb = new ResultBuffer();
            // RTSTR = 5005
            rb.Add(new TypedValue(5005, iComando));
            // start the insert command
            acedCmd(rb.UnmanagedObject);

            bool quit = false;
            // loop round while the insert command is active
            while (!quit)
            {
                // see what commands are active
                string cmdNames = (string)Autodesk.AutoCAD.ApplicationServices.Application.GetSystemVariable("CMDNAMES");

               



                // if the INSERT command is active
                if (cmdNames.ToUpper().IndexOf(miCmd) >= 0)
                {
                    // then send a PAUSE to the command line
                    rb = new ResultBuffer();
                    // RTSTR = 5005 - send a user pause to the command line
                    rb.Add(new TypedValue(5005, "\\"));
                    acedCmd(rb.UnmanagedObject);
                }
                else
                    // otherwise quit
                    quit = true;
            }
        }

    }

}
