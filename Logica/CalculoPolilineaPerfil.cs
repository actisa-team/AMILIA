using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using System.Windows.Forms;

namespace Logica
{
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;
    using engCadNet;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using tadLayShare.puntos;
    using EjeDeTrazado.puntosDelEje;
    using EjeDeTrazado.componentes;
    using Logica.verificacion;
    public class CalculoPolilineaPerfil
    {
        double ratio;
        List<Punto> polilinea = new List<Punto>();
        dsApp datoApp = new dsApp();
        public List<Punto> Polilinea { get => polilinea; set => polilinea = value; }
        public CalculoPolilineaPerfil(ref dsApp a, int opcion, double ratio, int it)
        {
            try
            {
                bool dibujar = true;
                if (a.Polilinea3d.Rows.Count == 0)
                {
                    dibujar = false;
                    Set_Polilinea3d(ref a);
                }

                int contador = 0;
                this.ratio = ratio;
                if (a.Polilinea3d.Rows.Count != 0)
                {
                    foreach (DataRow r in a.Polilinea3d.Rows)
                    {
                        string x = (string)r["X"];
                        string y = (string)r["Y"];
                        string z = (string)r["Z"];

                        Punto p = new Punto(new Point2d(Math.Sqrt(Math.Pow(double.Parse(x),2)+ Math.Pow(double.Parse(y), 2)), double.Parse(z)*10));
                        polilinea.Add(p);
                    }
                    Guardar();
                    Vaciar_Puntos();
                    RellenarDatos();
                    if (dibujar)
                    {
                        Dibujar(0);
                    }

                    if (it == 1)
                    {
                        bool salida = true;
                        while (salida)
                        {
                            if (opcion == 1)
                            {
                                FiltradoDisrupcion(true, ref contador);
                                if (contador == 0)
                                {
                                    salida = false;
                                }
                                contador = 0;
                                Vaciar_Puntos();
                                RellenarDatos();
                            }
                            else if (opcion == 2)
                            {
                                FiltradoCambioSentido(true, ref contador);
                                if (contador == 0)
                                {
                                    salida = false;
                                }
                                contador = 0;
                                Vaciar_Puntos();
                                RellenarDatos();
                            }
                            else if (opcion == 3)
                            {
                                FiltradoGiroLongitud(ratio, true, ref contador);
                                if (contador == 0)
                                {
                                    salida = false;
                                }
                                contador = 0;
                                Vaciar_Puntos();
                                RellenarDatos();
                            }
                        }

                    }
                    else
                    {
                        if (opcion == 1)
                        {
                            FiltradoDisrupcion(true, ref contador);
                        }
                        else if (opcion == 2)
                        {
                            FiltradoCambioSentido(true, ref contador);
                        }
                        else if (opcion == 3)
                        {
                            FiltradoGiroLongitud(ratio, true, ref contador);
                        }
                        Vaciar_Puntos();
                        RellenarDatos();
                    }


                    Dibujar(1);
                    //datoApp.WriteXml("prueba_modificada.aplitop");
                }
                else
                {
                    MessageBox.Show("No hay ninguna polilinea cargada");
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Debe realizar primero la busqueda para realizar la exportacion", ex);

            }
        }
        public CalculoPolilineaPerfil(ref dsApp a, int opcion, double ratio, int[] orden, int it)
        {
            try
            {
                bool dibujar = true;
                if (a.Polilinea3d.Rows.Count == 0)
                {
                    dibujar = false;
                    Set_Polilinea3d(ref a);
                }
                int contador = 0;
                int contador_total = 0;
                this.ratio = ratio;
                foreach (DataRow r in a.Polilinea3d.Rows)
                {

                    string x = (string)r["X"];
                    string y = (string)r["Y"];
                    string z = (string)r["Z"];

                    Punto p = new Punto(new Point2d(Math.Sqrt(Math.Pow(double.Parse(x), 2) + Math.Pow(double.Parse(y), 2)), double.Parse(z)));
                    polilinea.Add(p);
                }
                RellenarDatos();
                if (dibujar)
                {
                    Dibujar(0);
                }

                if (it == 1)//1 si 2 no
                {
                    while (salir())
                    {
                        int filtrado = 1;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int t = 0; t < 3; t++)
                            {
                                if (orden[t] == filtrado)
                                {
                                    if (t == 0)
                                    {
                                        FiltradoDisrupcion2(ref contador);
                                        contador_total += contador;
                                        contador = 0;
                                        RellenarDatos();
                                    }
                                    else if (t == 1)
                                    {
                                        FiltradoCambioSentido2(ref contador);
                                        contador_total += contador;
                                        contador = 0;
                                        RellenarDatos();
                                    }
                                    else if (t == 2)
                                    {
                                        FiltradoGiroLongitud2(ratio, ref contador);
                                        contador_total += contador;
                                        contador = 0;
                                        RellenarDatos();
                                    }
                                    filtrado++;
                                }
                            }
                        }

                    }
                }
                else
                {
                    int filtrado = 1;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int t = 0; t < 3; t++)
                        {
                            if (orden[t] == filtrado)
                            {
                                if (t == 0)
                                {
                                    FiltradoDisrupcion2(ref contador);
                                    contador_total += contador;
                                    contador = 0;
                                    RellenarDatos();
                                }
                                else if (t == 1)
                                {
                                    FiltradoCambioSentido2(ref contador);
                                    contador_total += contador;
                                    contador = 0;
                                    RellenarDatos();
                                }
                                else if (t == 2)
                                {
                                    FiltradoGiroLongitud2(ratio, ref contador);
                                    contador_total += contador;
                                    contador = 0;
                                    RellenarDatos();
                                }
                                filtrado++;
                            }
                        }
                    }
                }
                MessageBox.Show("Se han eliminado " + contador_total + " puntos");

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

                        for (int i = 0; i < polilinea.Count; i++)
                        {
                            sw2.WriteLine(polilinea[i].p.X + "," + polilinea[i].p.Y);
                        }
                        sw2.Close();

                    }




                }
                else if (result == DialogResult.No)
                {
                }
                Dibujar(1);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Debe realizar primero la busqueda para realizar la exportacion", ex);
            }
        }
        private bool salir()
        {
            int contador = 0;
            int contador2 = 0;
            FiltradoDisrupcion(false, ref contador);
            contador2 += contador;
            contador = 0;
            FiltradoCambioSentido(false, ref contador);
            contador2 += contador;
            contador = 0;
            FiltradoGiroLongitud(ratio, false, ref contador);
            contador2 += contador;
            contador = 0;
            if (contador2 == 0)
            {
                return false;
            }
            else
            {
                return true;
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
                       /* StringBuilder sb = new StringBuilder();

                        IEnumerable<string> columnNames = a.Polilinea3d.Columns.Cast<System.Data.DataColumn>().
                                                          Select(column => column.ColumnName);
                        sb.AppendLine(string.Join(";", columnNames));
                        foreach (DataRow row in a.Polilinea3d.Rows)
                        {
                            IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                            sb.AppendLine(string.Join(";", fields));
                        }
                        */
                        //File.WriteAllText("test.csv", sb.ToString());
                        miLw.save();

                    }
                }
                else
                {

                }

            }
        }
        public void Guardar()
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

                    for (int i = 0; i < polilinea.Count; i++)
                    {
                        sw2.WriteLine(polilinea[i].p.X + "," + polilinea[i].p.Y);
                    }
                    sw2.Close();

                }
            }
            else if (result == DialogResult.No)
            {
            }
        }
        public void Vaciar_Puntos()
        {
            for (int i = 0; i < polilinea.Count; i++)
            {
                polilinea[i].Vaciar();
            }
        }
        private void RellenarDatos()
        {
            for (int i = 1; i < polilinea.Count; i++)
            {
                Punto p = polilinea[i];
                Punto p_a = polilinea[i - 1];
                Punto p_s = new Punto();
                if (i < polilinea.Count - 1)
                {
                    p_s = polilinea[i + 1];
                }
                p.a = Math.Pow(Math.Pow((p.p.X - p_a.p.X), 2) + Math.Pow((p.p.Y - p_a.p.Y), 2), 0.5);
                if (i < polilinea.Count - 1)
                {
                    p.b = Math.Pow(Math.Pow((p_s.p.X - p.p.X), 2) + Math.Pow((p_s.p.Y - p.p.Y), 2), 0.5);
                    p.c = Math.Pow(Math.Pow((p_s.p.X - p_a.p.X), 2) + Math.Pow((p_s.p.Y - p_a.p.Y), 2), 0.5);
                    p.s = (p.a + p.b + p.c) / 2;
                    p.R = (p.a * p.b * p.c) / (4 * Math.Pow(p.s * (p.s - p.a) * (p.s - p.b) * (p.s - p.c), 0.5));
                    if (double.IsNaN(p.R) || double.IsInfinity(p.R))
                    {
                        p.R = 0;
                    }
                }
                p.Dx = p.p.X - p_a.p.X;
                p.Dy = p.p.Y - p_a.p.Y;
                if (p.Dx == 0)
                {
                    p.Ad1 = 0;
                }
                else
                {
                    if (p.Dy == 0)
                    {
                        p.Ad1 = 0;
                    }
                    else
                    {
                        p.Ad1 = Math.Atan(p.Dy / p.Dx);
                    }
                }
                p.Ad2 = p.Ad1 * (180 / Math.PI);

                if (p.Ad1 == 0)
                {
                    p.signod = 0;
                }
                else
                {
                    if (p.Ad1 < 0)
                    {
                        p.signod = 1;
                    }
                    else
                    {
                        p.signod = 2;
                    }
                }

                if (p.Dx == 0)
                {
                    p.signodx = 0;
                }
                else
                {
                    if (p.Dx < 0)
                    {
                        p.signodx = 1;
                    }
                    else
                    {
                        p.signodx = 2;
                    }
                }

                if (p.Dy == 0)
                {
                    p.signody = 0;
                }
                else
                {
                    if (p.Dy < 0)
                    {
                        p.signody = 1;
                    }
                    else
                    {
                        p.signody = 2;
                    }
                }

                if (p.signod == 0)
                {
                    p.Dc = 2;
                }
                else
                {
                    p.Dc = 1;
                }
                if (p.Dc == 2)
                {
                    if (p.Dy == 0)
                    {
                        p.Orientacion = "E-W";
                    }
                    else
                    {
                        p.Orientacion = "N-S";
                    }
                }

                if (p.Dc == 2)
                {
                    if (p.Orientacion == "E-W")
                    {
                        if (p.Dx < 0)
                        {
                            p.Azcardinal = 270;
                        }
                        else
                        {
                            p.Azcardinal = 90;
                        }
                    }
                    else
                    {
                        if (p.Dy < 0)
                        {
                            p.Azcardinal = 180;
                        }
                        else
                        {
                            p.Azcardinal = 0;
                        }
                    }
                }

                //cuadrante
                if (p.Dc == 2)
                {
                    p.cuadrante = 0;
                }
                else
                {
                    if (p.Dx > 0 && p.Dy > 0)
                    {
                        p.cuadrante = 1;
                    }
                    else
                    {
                        if (p.Dx > 0 && p.Dy < 0)
                        {
                            p.cuadrante = 2;
                        }
                        else
                        {
                            if (p.Dx < 0 && p.Dy < 0)
                            {
                                p.cuadrante = 3;
                            }
                            else
                            {
                                p.cuadrante = 4;
                            }
                        }
                    }
                }

                //Azimut
                if (p.Dc == 2)
                {
                    p.Az = p.Azcardinal;
                }
                else
                {
                    if (p.signod == 1)
                    {
                        if (p.signodx == 2)
                        {
                            p.Az = 90 - p.Ad2;

                        }
                        else
                        {
                            p.Az = 270 - p.Ad2;
                        }
                    }
                    else
                    {
                        if (p.signodx == 2)//esto esta mal
                        {
                            p.Az = 90 - p.Ad2;

                        }
                        else
                        {
                            p.Az = 270 - p.Ad2;
                        }
                    }
                }

                //calculo azimutal
                if (i > 1)
                {
                    if (p.Az < p_a.Az)
                    {
                        if (Math.Abs(p.Az - p_a.Az) > 180)
                        {
                            p.CambioAz = p.Az + 360 - p_a.Az;
                        }
                        else
                        {
                            p.CambioAz = Math.Truncate(p.Az * 100000) - Math.Truncate(p_a.Az * 100000);
                        }
                    }
                    else
                    {
                        p.CambioAz = Math.Truncate(p.Az * 100000) - Math.Truncate(p_a.Az * 100000);
                    }


                    if (i < polilinea.Count)
                    {
                        Punto p_aa = polilinea[i - 2];
                        if (p.CambioAz == 0)
                        {
                            p_a.Tipogiro = p_aa.Tipogiro;
                        }
                        else
                        {
                            if (getSentidoCurva(p_a.Az, p.Az) == EjeTrazado.sentidoCurva.Horario)
                            {
                                p_a.Tipogiro = 1;
                            }
                            else
                            {
                                p_a.Tipogiro = 2;
                            }
                            /*if (p_a.Az >= 0 && p_a.Az < 180)
                            {
                                if (Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000) <= 0 && Math.Abs(Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000)) <= 180)
                                {
                                    p_a.Tipogiro = 1;
                                }
                                else
                                {
                                    p_a.Tipogiro = 2;
                                }
                            }
                            else
                            {
                                if (Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000) >= 0 && Math.Abs(Math.Truncate(p_a.Az*100000) - Math.Truncate(p.Az*100000)) <= 180)
                                {
                                    p_a.Tipogiro = 2;
                                }
                                else
                                {
                                    p_a.Tipogiro = 1;
                                }
                            }*/
                        }


                        //cambiado codigo al anterior
                        /*if (p.CambioAz == 0)
                        {
                            p_a.Tipogiro = 3;
                        }
                        else if (p.CambioAz < 0)
                        {
                            p_a.Tipogiro = 2;
                        }
                        else
                        {
                            p_a.Tipogiro = 1;
                        }*/
                    }
                }
                if (i < polilinea.Count)
                {
                    if (i > 2)
                    {
                        Punto p_aa = polilinea[i - 2];
                        if (p_aa.Tipogiro == 3)
                        {
                            p_a.secuenciagiro = 2;
                        }
                        else
                        {
                            if (p_aa.Tipogiro == p_a.Tipogiro)
                            {
                                p_a.secuenciagiro = 2;
                            }
                            else
                            {
                                if (p_a.Tipogiro == 3)
                                {
                                    p_a.secuenciagiro = 2;
                                }
                                else
                                {
                                    p_a.secuenciagiro = 1;
                                }

                            }
                        }



                    }
                    if (i == polilinea.Count - 2)
                    {
                        Punto p_u = polilinea[i - 1];
                        //p_u.secuenciagiro = 2;
                        Punto p_u2 = polilinea[i];
                        p_u2.secuenciagiro = 2;
                    }
                }

            }
        }
        private void Dibujar(int a)
        {
            Point3dCollection poly = new Point3dCollection();
            for (int i = 0; i < polilinea.Count; i++)
            {

                poly.Add(new Point3d(polilinea[i].p.X, polilinea[i].p.Y, 0));
            }

            Document acDoc2 = Application.DocumentManager.MdiActiveDocument;
            Database AcCurDb2 = acDoc2.Database;
            using (DocumentLock docLock = acDoc2.LockDocument())
            {
                if (a == 0)
                {
                    engCadNet.oLayer.addLayer("polilinia original", 2, false);
                }
                else
                {
                    if (a == 3)
                    {
                        engCadNet.oLayer.addLayer("Polilinea suavizada", 6, false);
                    }
                    else
                    {
                        engCadNet.oLayer.addLayer("Nueva polilinia", 3, false);
                    }

                }

                using (Transaction acTrans = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
                {
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(AcCurDb2.BlockTableId,
                        OpenMode.ForRead) as BlockTable;
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite) as BlockTableRecord;

                    Editor e = Application.DocumentManager.MdiActiveDocument.Editor;
                    Document d = Application.DocumentManager.MdiActiveDocument;
                    Polyline3d pol = new Polyline3d(new Poly3dType(), poly, false);

                    if (a == 0)
                    {
                        pol.Layer = "polilinia original";
                    }
                    else
                    {
                        if (a == 3)
                        {
                            pol.Layer = "Polilinea suavizada";
                        }
                        else
                        {
                            pol.Layer = "Nueva polilinia";
                        }

                    }
                    acBlkTblRec.AppendEntity(pol);

                    acTrans.AddNewlyCreatedDBObject(pol, true);

                    acTrans.Commit();
                }
            }
        }
        private void FiltradoDisrupcion(bool acc, ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();
            int contador2 = 1;

            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(new Punto(new Point2d(p.p.X, p.p.Y)));
                }
                else
                {
                    if (polilinea[i].secuenciagiro == 1 && polilinea[i + 1].secuenciagiro == 1)
                    {
                        contador++;


                    }
                    else
                    {
                        if (acc)
                        {

                            polilinea_temp.Add(new Punto(new Point2d(p.p.X, p.p.Y)));
                            datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                            contador2++;
                        }

                    }
                }
            }
            if (acc)
            {
                polilinea_temp.Add((new Punto(new Point2d(polilinea[polilinea.Count - 1].p.X, polilinea[polilinea.Count - 1].p.Y))));

                polilinea = polilinea_temp;
                RellenarDatos();
                MessageBox.Show("Se han eliminado " + contador + " puntos");
                if (contador > 0)
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

                            for (int i = 0; i < polilinea.Count; i++)
                            {
                                sw2.WriteLine(polilinea[i].p.X + "," + polilinea[i].p.Y);
                            }
                            sw2.Close();

                        }




                    }
                    else if (result == DialogResult.No)
                    {
                    }
                }

            }
        }
        private void FiltradoDisrupcion2(ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();
            int contador2 = 1;
            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(p);
                }
                else
                {
                    if (polilinea[i].secuenciagiro == 1 && polilinea[i + 1].secuenciagiro == 1)
                    {
                        contador++;
                    }
                    else
                    {
                        polilinea_temp.Add(p);
                        datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                        contador2++;
                    }
                }
            }
            polilinea_temp.Add(polilinea[polilinea.Count - 1]);
            polilinea = polilinea_temp;
        }
        private void FiltradoCambioSentido(bool acc, ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();

            int contador2 = 1;
            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(p);
                }
                else
                {
                    if (polilinea[i + 1].secuenciagiro == 1 && (polilinea[i + 1].R < p.R && polilinea[i + 1].R < polilinea[i + 2].R))
                    {
                        contador++;


                    }
                    else
                    {
                        if (acc)
                        {
                            polilinea_temp.Add(p);
                            datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                            contador2++;
                        }

                    }
                }
            }
            if (acc)
            {
                polilinea_temp.Add(polilinea[polilinea.Count - 1]);
                polilinea = polilinea_temp;
                MessageBox.Show("Se han eliminado " + contador + " puntos");

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

                        for (int i = 0; i < polilinea.Count; i++)
                        {
                            sw2.WriteLine(polilinea[i].p.X + "," + polilinea[i].p.Y);
                        }
                        sw2.Close();

                    }




                }
                else if (result == DialogResult.No)
                {
                }
            }
        }
        private void FiltradoCambioSentido2(ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();

            int contador2 = 1;
            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(p);
                }
                else
                {
                    if (polilinea[i + 1].secuenciagiro == 1 && (polilinea[i + 1].R < p.R && polilinea[i + 1].R < polilinea[i + 2].R))
                    {
                        contador++;


                    }
                    else
                    {
                        polilinea_temp.Add(p);
                        datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                        contador2++;

                    }
                }
            }

            polilinea_temp.Add(polilinea[polilinea.Count - 1]);
            polilinea = polilinea_temp;

        }
        private void FiltradoGiroLongitud(double ratio, bool acc, ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();
            int contador2 = 1;
            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(p);
                }
                else
                {
                    if (Math.Abs(polilinea[i + 1].CambioAz / Distancia(polilinea[i].p, polilinea[i + 1].p)) > ratio)
                    {
                        contador++;


                    }
                    else
                    {
                        if (acc)
                        {
                            polilinea_temp.Add(p);
                            datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                            contador2++;
                        }

                    }
                }
            }
            if (acc)
            {
                polilinea_temp.Add(polilinea[polilinea.Count - 1]);
                polilinea = polilinea_temp;
                MessageBox.Show("Se han eliminado " + contador + " puntos");

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

                        for (int i = 0; i < polilinea.Count; i++)
                        {
                            sw2.WriteLine(polilinea[i].p.X + "," + polilinea[i].p.Y);
                        }
                        sw2.Close();

                    }




                }
                else if (result == DialogResult.No)
                {
                }
            }
        }
        private void FiltradoGiroLongitud2(double ratio, ref int contador)
        {
            List<Punto> polilinea_temp = new List<Punto>();
            int contador2 = 1;
            for (int i = 0; i < polilinea.Count - 1; i++)
            {
                Punto p = polilinea[i];
                if (i == 0)
                {
                    polilinea_temp.Add(p);
                }
                else
                {
                    if (Math.Abs(polilinea[i + 1].CambioAz / Distancia(polilinea[i].p, polilinea[i + 1].p)) > ratio)
                    {
                        contador++;


                    }
                    else
                    {
                        polilinea_temp.Add(p);
                        datoApp.Polilinea.Rows.Add(p.p.X, p.p.Y, contador2);
                        contador2++;
                    }
                }
            }
            polilinea_temp.Add(polilinea[polilinea.Count - 1]);
            polilinea = polilinea_temp;
        }
        private double Distancia(Point2d a, Point2d b)
        {
            double d;
            d = Math.Pow(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2), 0.5);
            return d;
        }
         public EjeTrazado.sentidoCurva getSentidoCurva(double iAzSegAnt, double iAz) {
            EjeTrazado.sentidoCurva miSent;
            if ((iAzSegAnt >= 0) && (iAzSegAnt <= 180)) {
                if (((iAzSegAnt - iAz) < 0) && (Math.Abs(iAzSegAnt - iAz) < 180)) {
                    miSent = EjeTrazado.sentidoCurva.Horario;
                } else {
                    miSent = EjeTrazado.sentidoCurva.Antihorario;
                }
            } else {
                if (((iAzSegAnt - iAz) > 0) && (Math.Abs(iAzSegAnt - iAz) < 180)) {
                    miSent = EjeTrazado.sentidoCurva.Antihorario;
                } else {
                    miSent = EjeTrazado.sentidoCurva.Horario;
                }
            }


            return miSent;
        }
    }
}
