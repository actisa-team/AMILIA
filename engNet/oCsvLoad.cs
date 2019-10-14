using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet
{

    using System.IO;
    using System.Data;
    using System.Text.RegularExpressions;
    using System.Diagnostics;
    using System.Globalization;

    public class oCsvLoad : IDisposable
    {

        #region "Variables Clase"
        private string mFilePath;
        private string mStrSeparaCampos;
        private string mStrIniciaComentario;
        private CultureInfo mCultureFormat;
        #endregion


        #region "CONSTRUCTOR"

        public oCsvLoad(string iFilePath, string iStrSeparadorCampos, string iStrIniciaComentario, bool iIsSimboloDecimalPunto)
        {
            //Existe el Fichero
            if (File.Exists(iFilePath))
            {
                mFilePath = iFilePath;
            }
            else
            {
                throw new FileNotFoundException();
            }

            if (iIsSimboloDecimalPunto)
            {
                mCultureFormat = CultureInfo.InvariantCulture;
            }
            else
            {
                mCultureFormat = CultureInfo.GetCultureInfo("es-ES");
            }

            mStrSeparaCampos = iStrSeparadorCampos;

            mStrIniciaComentario = iStrIniciaComentario;
        }

        #endregion


        #region "PUBLICOS ; METODOS-FUNCIONES"

        /// <summary>
        /// Metodo para Cargar un Texto Plano CSV en un DataTableTipado
        /// </summary>
        /// <param name="iTb">DataTableTipado</param>
        public void loadCsvIntoDataTableTyped(DataTable iTb)
        {
            //Variables Metodo
            StreamReader miReader = new StreamReader(mFilePath, Encoding.Default);
            List<Type> miLstTipos = new List<Type>();
            string myLineRead = String.Empty;
            DataRow miRow = null;
            string miValorStr = string.Empty;
            var miValorCast = (dynamic)null;
            List<string> miLstLineSplit;
            Type miType;

            //Guardo los Tipos para Hacer el CAST
            foreach (DataColumn item in iTb.Columns)
            {
                miLstTipos.Add(item.DataType);
            }



            //Inicio el Ciclo de Lectura
            while (miReader.Peek() != -1)
            {
                myLineRead = miReader.ReadLine();

                if (this.isLineaOk(myLineRead))
                {
                    miLstLineSplit = this.fLineaSplit(myLineRead, mStrSeparaCampos);

                    miRow = iTb.NewRow();

                    for (int i = 0; i < iTb.Columns.Count; i++)
                    {
                        miValorStr = miLstLineSplit[i];
                        miType = miLstTipos[i];

                        miValorCast = Convert.ChangeType(miValorStr, miType, mCultureFormat);
                        miRow[i] = miValorCast;
                    }

                    iTb.Rows.Add(miRow);
                }
            }
        }

        #endregion


        #region "PRIVADAS: METODOS-FUNCIONES"

        /// <summary>
        /// Funcion para Filtar Lineas que no me interesa Leer del Txt
        /// </summary>
        /// <param name="itxtLine"></param>
        /// <returns>
        /// TRUE --> Quitamos la LINEA
        /// </returns>
        private bool isLineaOk(string itxtLine)
        {

            //Elimino Lineas Longitud Cero.
            if (itxtLine.Trim().Length == 0)
            {
                return false;
            }


            //Obtengo la Primera letra de la Linea
            string myStrInicio = itxtLine.Substring(0, 1);

            if (mStrIniciaComentario.Equals(myStrInicio))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Descompongo una Linea de texto, Segun un Separador
        /// </summary>
        /// <param name="iLine">Linea a Separar</param>
        /// <param name="iStrSep">Caracteres Separadores ' ' ';' '|' ':' </param>
        /// <returns>Listado de Textos</returns>
        private List<string> fLineaSplit(string iLine, string iStrSep)
        {

            string[] txtPartes;


            if (iStrSep.Trim().Length == 0)
            {
                txtPartes = Regex.Split(iLine.Trim(), @"\s+");
            }
            else
            {
                txtPartes = iLine.Split(iStrSep.ToCharArray());
            }


            List<string> myListDes = new List<string>();

            foreach (string s in txtPartes)
            {
                myListDes.Add(s);
            }

            return myListDes;


        }





        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            this.mFilePath = null;
        }

        #endregion
    }

}
