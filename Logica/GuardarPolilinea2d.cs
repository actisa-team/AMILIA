using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Datos;
using engCadNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Logica
{
    public class GuardarPolilinea2d
    {
        public GuardarPolilinea2d(ref dsApp a)
        {
            Set_Polilinea(ref a);
            Guardar(ref a);
        }
        public void Guardar(ref dsApp a)
        {
            DialogResult result = MessageBox.Show("¿Desea guardar el resultado?", "Salir", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {

                System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.DefaultExt = "txt";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw2 = new StreamWriter(saveFileDialog1.FileName);
                    foreach (DataRow r in a.Polilinea.Rows)
                    {
                        string x = (string)r["X"];
                        string y = (string)r["Y"];
                        sw2.WriteLine(double.Parse(x) + "," + double.Parse(y));
                    }
                    sw2.Close();

                }
            }
            else if (result == DialogResult.No)
            {
            }
        }
        private void Set_Polilinea(ref dsApp a)
        {
            Document acDoc2 = Application.DocumentManager.MdiActiveDocument;
            Database AcCurDb2 = acDoc2.Database;
            using (DocumentLock docLock = acDoc2.LockDocument())
            {

                Polyline miEjeVisibilidadUsuario = oSs.seleccionUsuario<Polyline>("Selecciona una polilinea", "No has seleccionado una polilinea");
                if (miEjeVisibilidadUsuario != null)
                {
                    Polyline pol = new Polyline();
                    //Debo de Abrir el Objeto para poder cargarlo en la Capa
                    using (oEntidad<Polyline> miLw = new oEntidad<Polyline>(miEjeVisibilidadUsuario))
                    {
                        miLw.open();
                        engCadNet.oLayer.addLayer("Polilinea seleccionada", 4, false);
                        pol = miLw.entidad;
                        miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, 4);
                        miLw.entidad.Layer = "Polilinea seleccionada";
                        int c_p = miLw.entidad.NumberOfVertices;

                        for (int i = 0; i < c_p; i++)
                        {
                            a.Polilinea.Rows.Add(miLw.entidad.GetPoint3dAt(i).X, miLw.entidad.GetPoint3dAt(i).Y, i + 1);
                        }
                        miLw.save();
                    }
                }
                else
                {

                }

            }
        }

    }
}
