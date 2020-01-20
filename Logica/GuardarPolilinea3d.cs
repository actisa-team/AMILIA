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
    public class GuardarPolilinea3d
    {
        public GuardarPolilinea3d(ref dsApp a)
        {
            Set_Polilinea3d(ref a);
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
                    foreach (DataRow r in a.Polilinea3d.Rows)
                    {
                        string x = (string)r["X"];
                        string y = (string)r["Y"];
                        string z = (string)r["Z"];
                        sw2.WriteLine(double.Parse(x) + "," + double.Parse(y)+","+ double.Parse(z));
                    }
                    sw2.Close();

                }
            }
            else if (result == DialogResult.No)
            {
            }
        }
        private void Set_Polilinea3d(ref dsApp a)
        {
            Document acDoc2 = Application.DocumentManager.MdiActiveDocument;
            Database AcCurDb2 = acDoc2.Database;
            using (DocumentLock docLock = acDoc2.LockDocument())
            {
                //Para cambiar a puntos 3d hay que cambiar Polyline por Polyline3d


                //Selecciono la Entidad Generica
                Polyline3d miEjeVisibilidadUsuario = oSs.seleccionUsuario<Polyline3d>("Selecciona una polilinea", "No has seleccionado una polilinea");
                /*int c_p = miEjeVisibilidadUsuario.NumberOfVertices;
                
                for (int i=0;i<c_p;i++)
                {
                    a.Polilinea.Rows.Add(miEjeVisibilidadUsuario.GetPoint3dAt(i).X, miEjeVisibilidadUsuario.GetPoint3dAt(i).Y, i+1);
                }*/

                if (miEjeVisibilidadUsuario != null)
                {
                    Polyline3d pol = new Polyline3d();
                    //Debo de Abrir el Objeto para poder cargarlo en la Capa
                    using (oEntidad<Polyline3d> miLw = new oEntidad<Polyline3d>(miEjeVisibilidadUsuario))
                    {
                        miLw.open();
                        engCadNet.oLayer.addLayer("Polilinea seleccionada", 4, false);
                        pol = miLw.entidad;
                        miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, 4);
                        miLw.entidad.Layer = "Polilinea seleccionada";
                        /*Para 3d*/
                        double x, y, z;
                        for (int i = 0; i < miLw.entidad.EndParam; i++)
                        {
                            a.Polilinea3d.Rows.Add(miLw.entidad.GetPointAtParameter(i).X, miLw.entidad.GetPointAtParameter(i).Y, miLw.entidad.GetPointAtParameter(i).Z, i + 1);
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
