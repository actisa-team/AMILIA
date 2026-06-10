using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLogica.datos;
using Newtonsoft.Json;

namespace tadLayLogica
{
    using tadLayLogica.datos.proyecto;

    using engCadNet;
    using engNet.ClassT;


    using tadLayShare;
    using tadLayShare.puntos;

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using Terrenos;
    using tadLayLan.Tdm;
    using tadLayLan;
    using System.IO;
    using tadLayLogica.Comandos;
    using tadLayLogica.logica.EjeBasicoNew;
    using System.Globalization;

    public class oSingletonPuntosTerreno : IDisposable
    {
        private static oSingletonPuntosTerreno mInstance = null;
        private nubepuntos nube = new nubepuntos { };
        private List<Punto3d> puntos = null;
        private Triangulacion mTerreno = null;
        private static Polyline mEnvolvente = null;
        private bool todo = false;
        private string path;
        private KDTree2D tree;
        private bool preciso = true;
        #region "Constructor"
        public static oSingletonPuntosTerreno getInstance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new oSingletonPuntosTerreno();
                }

                return mInstance;
            }
        }
        private oSingletonPuntosTerreno()
        {

        }
        public void Set_Preciso(bool p)
        {
            preciso = p;
        }
        public bool Get_Preciso()
        {
            return preciso;
        }
        public void Set_Envolvente(Polyline m)
        {
            mEnvolvente = m;
        }
        public Triangulacion Cargar(bool cargar = false)
        {
            //Cargo el Fichero
            //cargar = false;
            string miFile = oTadil.data.Files.getFileTxtOrMdt();
            path = miFile;
            try
            {
                string jsonContent = File.ReadAllText(miFile);
                try
                {
                    //CargarPuntosASC(2);
                    nube = JsonConvert.DeserializeObject<nubepuntos>(jsonContent);
                    puntos = nube.puntos;
                }
                catch (Exception e)
                {
                    puntos = JsonConvert.DeserializeObject<List<Punto3d>>(jsonContent);
                    nube = new nubepuntos
                    {
                        miMax = 1000,
                        miPendienteMax = 200,
                        ucNodosPorHoja = 150,
                        puntos = puntos
                    };
                }



                if (puntos != null)
                {
                    if (cargar)
                    {

                        this.tree = new KDTree2D(puntos);
                        oSingletonTerreno.getInstance.tipo = 2;
                        using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                        {
                            Database db = HostApplicationServices.WorkingDatabase;
                            using (Transaction tr = db.TransactionManager.StartTransaction())
                            {
                                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                                mEnvolvente = CrearPolilineaEnvolvente();

                                if (mEnvolvente != null)
                                {
                                    btr.AppendEntity(mEnvolvente);
                                    tr.AddNewlyCreatedDBObject(mEnvolvente, true);
                                }

                                tr.Commit();
                            }
                        }
                        //Cargar_MDT(2);
                    }
                }
                this.todo = cargar;

                return mTerreno;

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<Punto3d> Get_puntos()
        {
            return puntos;
        }
        public void MDT_Abanico(List<oTramoAbanico> miLstTramosAvancesCortos)
        {
            double x_max, x_min, y_max, y_min;
            var target = miLstTramosAvancesCortos[0].P1;
            x_min = target.X;
            x_max = target.X;
            y_min = target.Y;
            y_max = target.Y;
            foreach (var item in miLstTramosAvancesCortos)
            {
                var target2 = item.P2;
                if (x_min > target2.X)
                {
                    x_min = target2.X;
                }
                if (x_max < target2.X)
                {
                    x_max = target2.X;
                }
                if (y_min > target2.Y)
                {
                    y_min = target2.Y;
                }
                if (y_max < target2.Y)
                {
                    y_max = target2.Y;
                }
            }

            double dis_extra = 5;
            x_min -= dis_extra;
            x_max += dis_extra;
            y_min -= dis_extra;
            y_max += dis_extra;

            //var nube_puntos = Get_puntos();

            if (puntos == null)
            {
                //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                return;
            }
            if (puntos.Count() == 0)
            {
                //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                return;
            }
            // Filtrar los puntos dentro del rango
            List<Punto3d> puntos_en_rango = puntos
                .Where(p => p.coordenadaX >= x_min && p.coordenadaX <= x_max && p.coordenadaY >= y_min && p.coordenadaY <= y_max)
                .ToList();

            List<Triangulo> _znpTriangulos = new List<Triangulo>();
            var mTerreno = oComandoTerreno.crearMallaNoUI(puntos_en_rango, "nombre", nube.miMax, nube.miPendienteMax,
                           "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, nube.ucNodosPorHoja);
            oSingletonTerreno.getInstance.Set_Terreno(mTerreno);
            this.todo = false;
        }
        public void MDT_Abanico(List<Punto3d> lista_puntos)
        {
            double x_max, x_min, y_max, y_min;
            var target = lista_puntos[0];
            x_min = target.coordenadaX;
            x_max = target.coordenadaX;
            y_min = target.coordenadaY;
            y_max = target.coordenadaY;
            foreach (var item in lista_puntos)
            {
                var target2 = item;
                if (x_min > target2.coordenadaX)
                {
                    x_min = target2.coordenadaX;
                }
                if (x_max < target2.coordenadaX)
                {
                    x_max = target2.coordenadaX;
                }
                if (y_min > target2.coordenadaY)
                {
                    y_min = target2.coordenadaY;
                }
                if (y_max < target2.coordenadaY)
                {
                    y_max = target2.coordenadaY;
                }
            }

            double dis_extra = 500;
            x_min -= dis_extra;
            x_max += dis_extra;
            y_min -= dis_extra;
            y_max += dis_extra;

            //var nube_puntos = Get_puntos();
            if (puntos == null)
            {
                //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                return;
            }
            if (puntos.Count() == 0)
            {
                //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                return;
            }
            // Filtrar los puntos dentro del rango
            List<Punto3d> puntos_en_rango = puntos
                .Where(p => p.coordenadaX >= x_min && p.coordenadaX <= x_max && p.coordenadaY >= y_min && p.coordenadaY <= y_max)
                .ToList();

            List<Triangulo> _znpTriangulos = new List<Triangulo>();
            var mTerreno = oComandoTerreno.crearMallaNoUI(puntos_en_rango, "nombre", nube.miMax, nube.miPendienteMax,
                           "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, nube.ucNodosPorHoja);
            oSingletonTerreno.getInstance.Set_Terreno(mTerreno);
            this.todo = false;
        }
        public double MDT_Abanico_Punto(double[] punto)
        {
            try
            {
                if (this.tree!=null)
                {
                    if (oSingletonTerreno.getInstance.tipo==2)
                    {
                        return GetZ(punto[0],punto[1]);
                    }else if (oSingletonTerreno.getInstance.tipo == 3)
                    {
                        return oSingletonPuntosTerrenoASC.getInstance.GetZ(punto[0], punto[1]);
                    }
                }

                double x_max, x_min, y_max, y_min;
                var target = punto;
                x_min = target[0];
                x_max = target[0];
                y_min = target[1];
                y_max = target[1];
                double dis_extra = 100;
                x_min -= dis_extra;
                x_max += dis_extra;
                y_min -= dis_extra;
                y_max += dis_extra;

                if (puntos == null)
                {
                    //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                    return -1;
                }
                if (puntos.Count() == 0)
                {
                    //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                    return -1;
                }

                // Filtrar los puntos dentro del rango
                List<Punto3d> puntos_en_rango = puntos
                    .Where(p => p.coordenadaX >= x_min && p.coordenadaX <= x_max && p.coordenadaY >= y_min && p.coordenadaY <= y_max)
                    .ToList();

                List<Triangulo> _znpTriangulos = new List<Triangulo>();
                var mTerreno = oComandoTerreno.crearMallaNoUI(puntos_en_rango, "nombre", nube.miMax, nube.miPendienteMax,
                           "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, nube.ucNodosPorHoja);
                oSingletonTerreno.getInstance.Set_Terreno(mTerreno);
                this.todo = false;
                return 0;
            }
            catch (Exception e)
            {
                oSingletonTerreno.getInstance.Set_Terreno(null);
                return -1;
            }
        }
        public void Nuevo_MDT(List<Punto3d> puntos_en_rango)
        {
            this.todo = false;
            List<Triangulo> _znpTriangulos = new List<Triangulo>();
            var mTerreno = oComandoTerreno.crearMallaNoUI(puntos_en_rango, "nombre", nube.miMax, nube.miPendienteMax,
                           "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, nube.ucNodosPorHoja);
            oSingletonTerreno.getInstance.Set_Terreno(mTerreno);
        }
        public void Cargar_MDT(int tipo = 0)
        {
            if (!todo)
            {
                List<Triangulo> _znpTriangulos = new List<Triangulo>();
                oTadil.data.UserInfo.showInfo("La carga del terreno puede tardar varios minutos");
                mTerreno = oComandoTerreno.crearMallaNoUI(puntos, "nombre", nube.miMax, nube.miPendienteMax,
                           "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, nube.ucNodosPorHoja);
                oSingletonTerreno.getInstance.Set_Terreno(mTerreno, tipo);
                oSingletonTerreno.getInstance.Set_envolvente();
            }

            this.todo = true;
        }
        public void MDT_Abanico_2(List<oTramoAbanico> miLstTramosAvancesCortos)
        {
            double x_max, x_min, y_max, y_min;
            var target = miLstTramosAvancesCortos[0].P1;
            x_min = target.X;
            x_max = target.X;
            y_min = target.Y;
            y_max = target.Y;
            foreach (var item in miLstTramosAvancesCortos)
            {
                var target2 = item.P2;
                if (x_min > target2.X)
                {
                    x_min = target2.X;
                }
                if (x_max < target2.X)
                {
                    x_max = target2.X;
                }
                if (y_min > target2.Y)
                {
                    y_min = target2.Y;
                }
                if (y_max < target2.Y)
                {
                    y_max = target2.Y;
                }
            }

            double dis_extra = 500;
            x_min -= dis_extra;
            x_max += dis_extra;
            y_min -= dis_extra;
            y_max += dis_extra;

            if (puntos == null)
            {
                //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                return;
            }
            if (puntos.Count() == 0)
            {
                // oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                return;
            }

            // Filtrar los puntos dentro del rango
            List<Punto3d> puntos_en_rango = puntos
                .Where(p => p.coordenadaX >= x_min && p.coordenadaX <= x_max && p.coordenadaY >= y_min && p.coordenadaY <= y_max)
                .ToList();

            List<Triangulo> _znpTriangulos = new List<Triangulo>();
            var mTerreno = oComandoTerreno.crearMallaNoUI(puntos_en_rango, "nombre", nube.miMax, nube.miPendienteMax,
                           "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, nube.ucNodosPorHoja);
            oSingletonTerreno.getInstance.Set_Terreno(mTerreno);
            this.todo = false;
        }
        public void MDT_Entronque(oTramoAbanico miTramoEntronque, oP3d iPtoEntronque)
        {
            double x_max, x_min, y_max, y_min;
            var target = iPtoEntronque;
            x_min = target.X;
            x_max = target.X;
            y_min = target.Y;
            y_max = target.Y;

            var target2 = miTramoEntronque.tramoPrevio.P1;
            if (x_min > target2.X)
            {
                x_min = target2.X;
            }
            if (x_max < target2.X)
            {
                x_max = target2.X;
            }
            if (y_min > target2.Y)
            {
                y_min = target2.Y;
            }
            if (y_max < target2.Y)
            {
                y_max = target2.Y;
            }
            var target3 = miTramoEntronque.tramoPrevio.P2;
            if (x_min > target3.X)
            {
                x_min = target3.X;
            }
            if (x_max < target3.X)
            {
                x_max = target3.X;
            }
            if (y_min > target3.Y)
            {
                y_min = target3.Y;
            }
            if (y_max < target3.Y)
            {
                y_max = target3.Y;
            }

            double dis_extra = 200;
            x_min -= dis_extra;
            x_max += dis_extra;
            y_min -= dis_extra;
            y_max += dis_extra;


            // Filtrar los puntos dentro del rango
            List<Punto3d> puntos_en_rango = puntos
                .Where(p => p.coordenadaX >= x_min && p.coordenadaX <= x_max && p.coordenadaY >= y_min && p.coordenadaY <= y_max)
                .ToList();

            List<Triangulo> _znpTriangulos = new List<Triangulo>();
            var mTerreno = oComandoTerreno.crearMallaNoUI(puntos_en_rango, "nombre", nube.miMax, nube.miPendienteMax,
                           "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, nube.ucNodosPorHoja);
            oSingletonTerreno.getInstance.Set_Terreno(mTerreno);
            this.todo = false;
        }
        public bool get_todo()
        {
            return this.todo;
        }

        struct nubepuntos
        {
            public List<Punto3d> puntos { get; set; }
            public double miMax { get; set; }
            public double miPendienteMax { get; set; }
            public int ucNodosPorHoja { get; set; }
        }
        #endregion
        /// <summary>
        /// Dispose0
        /// </summary>
        public void Dispose()
        {
            puntos = null;
            mTerreno = null;
            mEnvolvente = null;
            mInstance = null;
        }
        static string[] SplitLine(string line)
        {
            return line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
        private void CargarPuntosASC(double intervalo)
        {
            //string path = "C:\\Users\\Juanma\\Desktop\\prueba tadil\\1024-1.asc";

            string[] lines = File.ReadAllLines(path);

            int ncols = int.Parse(SplitLine(lines[0])[1]);
            int nrows = int.Parse(SplitLine(lines[1])[1]);
            double xllcorner = double.Parse(SplitLine(lines[2])[1], CultureInfo.InvariantCulture);
            double yllcorner = double.Parse(SplitLine(lines[3])[1], CultureInfo.InvariantCulture);
            double cellsize = double.Parse(SplitLine(lines[4])[1], CultureInfo.InvariantCulture);
            double nodata = double.Parse(SplitLine(lines[5])[1], CultureInfo.InvariantCulture);

            puntos = new List<Punto3d>();
            int aumento = (int)intervalo / (int)cellsize;
            int rowOffset = 6; // Donde empieza la matriz
            for (int row = 0; row < nrows; row += aumento)
            {
                string[] zValues = lines[row + rowOffset].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < ncols; col += aumento)
                {
                    double z = double.Parse(zValues[col], CultureInfo.InvariantCulture);
                    if (z == nodata) continue;

                    double x = xllcorner + col * cellsize;
                    double y = yllcorner + (nrows - 1 - row) * cellsize;

                    puntos.Add(new Punto3d
                    {
                        coordenadaX = x,
                        coordenadaY = y,
                        coordenadaZ = z
                    });
                }
            }
            nube = new nubepuntos
            {
                miMax = 1000,
                miPendienteMax = 200,
                ucNodosPorHoja = 150,
                puntos = puntos
            };


        }
        public double GetZ(double x, double y)
        {
            try
            {
                if (tree != null)
                {
                    double estimado = double.NaN;
                    if (preciso)
                    {
                        estimado = Interpolator.EstimateZQuadrants(tree, x, y);
                    }
                    else
                    {
                        estimado = Interpolator.EstimateZ(tree, x, y);
                    }
                    
                    //
                    return estimado;
                    //double bbb = SlopeCalculator.EstimateSlope(tree, xllcorner + 151, yllcorner + 261);
                }
                var target = new double[2] { x, y };
                if (mTerreno == null)
                {
                    CreaTerreno(target);
                }
                if (mTerreno == null)
                {
                    return -1;
                }
                if (mTerreno.getTriangulo(x, y) == null)
                {
                    CreaTerreno(target);
                }

                double? z = mTerreno.getCota(target[0], target[1]);
                return z ?? -1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public double GetZ(double[] punto,bool comprobar=false)
        {
            try
            {
                double x = punto[0];
                double y = punto[1];
                if (tree != null)
                {
                    double estimado = double.NaN;
                    if (preciso)
                    {
                        estimado = Interpolator.EstimateZQuadrants(tree, x, y);
                    }
                    else
                    {
                        if (comprobar)
                        {

                            bool dentro = false;
                            dentro = IsPointInsideClosedPolyline2D(new Point2d(x, y));
                            if (!dentro)
                            {
                                return double.NaN;
                            }
                        }
                        estimado = Interpolator.EstimateZ(tree, x, y);
                    }
                    //double estimado = Interpolator.EstimateZQuadrants(tree, x, y);
                    return estimado;
                    //double bbb = SlopeCalculator.EstimateSlope(tree, xllcorner + 151, yllcorner + 261);
                }
                var target = new double[2] { x, y };
                if (mTerreno == null)
                {
                    CreaTerreno(target);
                }
                if (mTerreno == null)
                {
                    return -1;
                }
                if (mTerreno.getTriangulo(x, y) == null)
                {
                    CreaTerreno(target);
                }

                double? z = mTerreno.getCota(target[0], target[1]);
                return z ?? -1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public Tuple<double, double> GetZ_Y_Slope(double x, double y)
        {
            try
            {
                double x_max, x_min, y_max, y_min;
                var target = new double[2] { x, y };
                x_min = target[0];
                x_max = target[0];
                y_min = target[1];
                y_max = target[1];
                double dis_extra = nube.miMax;
                x_min -= dis_extra;
                x_max += dis_extra;
                y_min -= dis_extra;
                y_max += dis_extra;

                if (puntos == null)
                {
                    //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                    return Tuple.Create(-1.0, -1.0);
                }
                if (puntos.Count() == 0)
                {
                    //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                    return Tuple.Create(-1.0, -1.0);
                }

                // Filtrar los puntos dentro del rango
                List<Punto3d> puntos_en_rango = puntos
                    .Where(p => p.coordenadaX >= x_min && p.coordenadaX <= x_max && p.coordenadaY >= y_min && p.coordenadaY <= y_max)
                    .ToList();

                List<Triangulo> _znpTriangulos = new List<Triangulo>();
                var mTerreno = oComandoTerreno.crearMallaNoUI(puntos_en_rango, "nombre", nube.miMax, nube.miPendienteMax,
                           "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, nube.ucNodosPorHoja);

                double? z = mTerreno.getCota(target[0], target[1]);
                double slope = mTerreno.getPendiente(mTerreno.getTriangulo(target[0], target[1]));
                return Tuple.Create(z ?? -1, slope);
            }
            catch (Exception e)
            {
                return Tuple.Create(-1.0, -1.0);
            }
        }
        public Triangulo getTrianguloInside(double? iX, double? iY)
        {
            if (iX.HasValue && iY.HasValue)
            {
                var target = new double[2] { iX.Value, iY.Value };
                if (mTerreno == null)
                {
                    CreaTerreno(target);
                }
                if (mTerreno.getTriangulo(iX.Value, iY.Value) == null)
                {
                    CreaTerreno(target);
                }

                try
                {
                    Triangulo miTriangulo = this.mTerreno.getTriangulo(iX.Value, iY.Value);
                    return miTriangulo;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }
        }
        public void CreaTerreno(double[] target)
        {

            double x_max, x_min, y_max, y_min;

            x_min = target[0];
            x_max = target[0];
            y_min = target[1];
            y_max = target[1];
            double dis_extra = 100;
            x_min -= dis_extra;
            x_max += dis_extra;
            y_min -= dis_extra;
            y_max += dis_extra;

            if (puntos == null)
            {
                //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                return;
            }
            if (puntos.Count() == 0)
            {
                //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                return;
            }

            // Filtrar los puntos dentro del rango
            List<Punto3d> puntos_en_rango = puntos
                .Where(p => p.coordenadaX >= x_min && p.coordenadaX <= x_max && p.coordenadaY >= y_min && p.coordenadaY <= y_max)
                .ToList();

            List<Triangulo> _znpTriangulos = new List<Triangulo>();
            this.mTerreno = oComandoTerreno.crearMallaNoUI(puntos_en_rango, "nombre", nube.miMax, nube.miPendienteMax,
                       "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, nube.ucNodosPorHoja);
        }
        public double? getZFromTriang(double? iX, double? iY, Triangulo iTriangulo)
        {

            if (iX.HasValue && iY.HasValue)
            {
                try
                {
                    var target = new double[2] { iX.Value, iY.Value };
                    if (mTerreno == null)
                    {
                        CreaTerreno(target);
                    }
                    if (mTerreno.getTriangulo(iX.Value, iY.Value) == null)
                    {
                        CreaTerreno(target);
                    }
                    double? myZ = null;

                    myZ = this.mTerreno.getCotaTriang(iX.Value, iY.Value, iTriangulo);

                    if (myZ.HasValue)
                    {
                        return myZ.Value;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }

        }
        public Triangulo getTrianguloInsideFromTriangulo(double? iX, double? iY, Triangulo iTriangulo)
        {
            if (iX.HasValue && iY.HasValue)
            {
                try
                {
                    var target = new double[2] { iX.Value, iY.Value };
                    if (mTerreno == null)
                    {
                        CreaTerreno(target);
                    }
                    if (mTerreno == null)
                    {
                        return null;
                    }
                    if (mTerreno.getTriangulo(iX.Value, iY.Value) == null)
                    {
                        CreaTerreno(target);
                    }
                    Triangulo miTriangulo = this.mTerreno.getTrianguloTriang(iTriangulo, new Punto3d(iX.Value, iY.Value, 0));
                    return miTriangulo;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }
        }
        public double? getSlopeFromTriang(Triangulo iTriangulo, double x = -1, double y = -1)
        {
            if (x != -1 && y != -1)
            {
                return Slope(x, y);
            }
            if (iTriangulo != null)
            {

                try
                {

                    double myPenOut = iTriangulo.getPendienteMaxima;

                    return myPenOut;

                }

                catch (SystemException)
                {
                    return null;
                }

            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }

        }

        public double Slope(double x, double y)
        {
            if (tree != null)
            {
                double estimado = SlopeCalculator.EstimateSlope(tree, x, y);
                return estimado;

            }
            return -1;
        }
        public bool Inside(double iX, double iY)
        {
            bool dentro = false;
            dentro = IsPointInsideClosedPolyline2D(new Point2d(iX, iY));
            if (!tree.IsInside(iX, iY))
            {
                return false;
            }

            return true;
        }
        public double DistanciaABorde(double x, double y)
        {
            bool dentro = false;
            dentro=IsPointInsideClosedPolyline2D(new Point2d(x,y));
            if (!dentro)
            {
                return -1;
            }
            if (puntos == null || puntos.Count == 0)
                throw new InvalidOperationException("No hay puntos cargados para calcular el bounding box.");

            // Calculamos bounding box de los puntos
            /*double xMin = puntos.Min(p => p.coordenadaX);
            double xMax = puntos.Max(p => p.coordenadaX);
            double yMin = puntos.Min(p => p.coordenadaY);
            double yMax = puntos.Max(p => p.coordenadaY);*/
            double xMin = tree.MinX;
            double xMax = tree.MaxX;
            double yMin = tree.MinY;
            double yMax = tree.MaxY;

            double distMin = double.PositiveInfinity;

            // Distancia al borde izquierdo (Xmin)
            double dIzq = Math.Abs(x - xMin);
            if (dIzq < distMin) distMin = dIzq;

            // Distancia al borde derecho (Xmax)
            double dDer = Math.Abs(xMax - x);
            if (dDer < distMin) distMin = dDer;

            // Distancia al borde inferior (Ymin)
            double dInf = Math.Abs(y - yMin);
            if (dInf < distMin) distMin = dInf;

            // Distancia al borde superior (Ymax)
            double dSup = Math.Abs(yMax - y);
            if (dSup < distMin) distMin = dSup;

            return distMin;
        }
        private Polyline CrearPolilineaEnvolvente()
        {
            if (puntos == null || puntos.Count == 0)
                return null;

            // Convertir puntos a lista de (X,Y)
            var lista = puntos.Select(p => new Point2d(p.coordenadaX, p.coordenadaY)).ToList();

            // Calcular convex hull
            var hull = GetConvexHull(lista);

            // Crear polilínea en AutoCAD
            Polyline poly = new Polyline();
            for (int i = 0; i < hull.Count; i++)
            {
                poly.AddVertexAt(i, hull[i], 0, 0, 0);
            }
            poly.Closed = true; // Cierra la polilínea
            return poly;
        }

        private List<Point2d> GetConvexHull(List<Point2d> points)
        {
            if (points.Count <= 1)
                return new List<Point2d>(points);

            points = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            List<Point2d> lower = new List<Point2d>();
            foreach (var p in points)
            {
                while (lower.Count >= 2 && Cross(lower[lower.Count - 2], lower[lower.Count - 1], p) <= 0)
                    lower.RemoveAt(lower.Count - 1);
                lower.Add(p);
            }

            List<Point2d> upper = new List<Point2d>();
            for (int i = points.Count - 1; i >= 0; i--)
            {
                var p = points[i];
                while (upper.Count >= 2 && Cross(upper[upper.Count - 2], upper[upper.Count - 1], p) <= 0)
                    upper.RemoveAt(upper.Count - 1);
                upper.Add(p);
            }

            upper.RemoveAt(upper.Count - 1);
            lower.RemoveAt(lower.Count - 1);
            lower.AddRange(upper);
            return lower;
        }

        private double Cross(Point2d o, Point2d a, Point2d b)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
        }
        public bool IsPointInsideClosedPolyline2D( Point2d p, double eps = 1e-9)
        {
            var tol = Tolerance.Global;

            // 1) Frontera: ¿está el punto sobre algún segmento 2D?
            if (IsPointOnPolyline2D(mEnvolvente, p, tol))
            {
                return true; // considerar dentro si está en el borde
            }

            // 2) Paridad de cruces con rayo casi horizontal hacia +X
            var ray = new Line2d(p, new Vector2d(1.0, eps));
            int crossings = 0;

            for (int i = 0; i < mEnvolvente.NumberOfVertices; i++)
            {
                var segType = mEnvolvente.GetSegmentType(i);

                if (segType == SegmentType.Line)
                {
                    var seg = mEnvolvente.GetLineSegment2dAt(i); // 2D
                    var ipts = seg.IntersectWith(ray, tol);
                    if (ipts != null && ipts.Length > 0)
                    {
                        foreach (var q in ipts)
                        {
                            if (q.X >= p.X - eps && seg.IsOn(q, tol))
                                crossings++;
                        }
                    }
                }
                else if (segType == SegmentType.Arc)
                {
                    var arc = mEnvolvente.GetArcSegment2dAt(i); // 2D
                    var ipts = arc.IntersectWith(ray, tol);
                    if (ipts != null && ipts.Length > 0)
                    {
                        foreach (var q in ipts)
                        {
                            if (q.X >= p.X - eps)
                                crossings++;
                        }
                    }
                }
            }

            return (crossings % 2) == 1;
        }

        private static bool IsPointOnPolyline2D(Polyline pl, Point2d p, Tolerance tol)
        {
            for (int i = 0; i < pl.NumberOfVertices; i++)
            {
                var segType = pl.GetSegmentType(i);
                if (segType == SegmentType.Line)
                {
                    var seg = pl.GetLineSegment2dAt(i);
                    if (seg.IsOn(p, tol)) return true;
                }
                else if (segType == SegmentType.Arc)
                {
                    var arc = pl.GetArcSegment2dAt(i);
                    if (arc.IsOn(p, tol)) return true;
                }
            }
            return false;
        }


        public List<Point3d> get_puntos_extremos(Point3d a, Point3d b)
        {
            return tree.GetBoundaryIntersections(a,b);
        }
        public bool Creado()
        {
            if (this.tree == null)
            {
                return false;
            }
            return true;
        }
    }
}
