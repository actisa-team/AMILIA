using System;
using System.Collections.Generic;
using System.Text;

namespace engNex.Net.BussinesObject.Usb
{
   using System.IO;
   using System.Management;

    using engNex.Net.BussinesObject.Licencia;
    using tadLayLan;

   public class oUsbDiskService
   {


  

       public static List<oUsbDisk> GetUsbList()
       {

           List<oUsbDisk> myList = new List<oUsbDisk>();
           oUsbDisk myUsb;

           DriveInfo[] allDrives = DriveInfo.GetDrives();

           foreach (DriveInfo d in allDrives)
           {

               if (d.IsReady && d.DriveType == DriveType.Removable && !d.Name.StartsWith("A"))
               {

                   myUsb = new oUsbDisk(d.Name, d.VolumeLabel);

                   myList.Add(myUsb);

               }
           }

           if (myList.Count == 0)
           {
               throw new oExLicencia(strError.eUSBNoConectado);
           }
           else
           {
               return myList;

           }






       }
       private static oUsbDisk usbNull()
       {
           oUsbDisk miUsbNull = new oUsbDisk();

           return miUsbNull;
       }
       public static oUsbDisk findUsbDiskByFile(string iFolderLic, string iFileLicConExtension)
       {

           List<oUsbDisk> miLstUsb = GetUsbList();

           string miFilePathSinLetra = iFolderLic + @"\" + iFileLicConExtension;

           string miFilePathFull;

           foreach (oUsbDisk item in miLstUsb)
           {
               miFilePathFull = item.letra + @":\" + miFilePathSinLetra;

               if (System.IO.File.Exists(miFilePathFull))
               {
                   return item;
               }
           }

           return usbNull();
       }


    


   }






}
