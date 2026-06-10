using System;
using System.Collections.Generic;
using System.Text;

namespace engNex.Net.BussinesObject.Usb
{

    using engNex.Net.BussinesObject.Licencia;
    using System.IO;
    using System.Management;

    public class oUsbDisk
    {
        #region "Constructor"
        public oUsbDisk()
        {
            this.letra = string.Empty; 
            this.name = string.Empty;         
        }
        public oUsbDisk(string iLetra, string iName)
        {
            this.letra = iLetra.ToUpper().Remove(1);
            this.name = iName;
            this.numberSerial = this.gsn(this.letra);
 
        }
        #endregion
        #region "Propiedades"

        public string letra { get; private set; }
        public string name { get; private set; }
        public string numberSerial { get; private set; }
        public string description
        {
            get
            {
                StringBuilder myStr = new StringBuilder();
                myStr.Append(letra);
                myStr.Append(":\\");
                myStr.Append(name);
                myStr.Append("");

                return myStr.ToString();
            }
        }
   
     
        #endregion
        #region "Metodos Publicos"

        public bool isNull()
        {

            if (string.IsNullOrEmpty(this.letra))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public override string ToString()
        {
            StringBuilder myStr = new StringBuilder();
            myStr.Append(letra);
            myStr.Append(":\\");
            myStr.Append(name);
            myStr.Append("");

            return myStr.ToString();
        }
        public void SetVolumeLabel(string newLabel)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady && d.DriveType == DriveType.Removable && d.VolumeLabel.Equals(this.name))
                {
                    d.VolumeLabel = newLabel;
                }
            }
        }
        public string i()
        {
            return this.numberSerial;
        }

        #endregion
        #region "Metodos Privados"

        /// <summary>
        /// Obtener el Serial Number de la USB
        /// </summary>
        /// <param name="iDriveLetter">Drive Letter</param>
        /// <returns>Id Serial</returns>
        private string gsn(string i)
        {

            string[] diskArray;
            string driveNumber;
            string driveLetter;

            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition");

            foreach (ManagementObject dm in searcher1.Get())
            {
                diskArray = null;
                driveLetter = getValueInQuotes(dm["Dependent"].ToString()).Remove(1);
                diskArray = getValueInQuotes(dm["Antecedent"].ToString()).Split(',');
                driveNumber = diskArray[0].Remove(0, 6).Trim();
                if (driveLetter == i)
                {
                    /* This is where we get the drive serial */
                    ManagementObjectSearcher disks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                    foreach (ManagementObject disk in disks.Get())
                    {

                        if (disk["Name"].ToString() == ("\\\\.\\PHYSICALDRIVE" + driveNumber) & disk["InterfaceType"].ToString() == "USB")
                        {
                            return parseSerialFromDeviceID(disk["PNPDeviceID"].ToString());
                        }
                    }
                }
            }

            throw new oExLicencia ("No Implementado");
        }
        private string parseSerialFromDeviceID(string deviceId)
        {
            string[] splitDeviceId = deviceId.Split('\\');
            string[] serialArray;
            string serial;
            int arrayLen = splitDeviceId.Length - 1;

            serialArray = splitDeviceId[arrayLen].Split('&');
            serial = serialArray[0];

            return serial;
        }
        private string getValueInQuotes(string inValue)
        {
            string parsedValue = "";

            int posFoundStart = 0;
            int posFoundEnd = 0;

            posFoundStart = inValue.IndexOf("\"");
            posFoundEnd = inValue.IndexOf("\"", posFoundStart + 1);

            parsedValue = inValue.Substring(posFoundStart + 1, (posFoundEnd - posFoundStart) - 1);

            return parsedValue;
        }


        #endregion
    }

}
