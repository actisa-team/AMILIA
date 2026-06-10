using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Autodesk.AutoCAD.DatabaseServices;
using PerfilLongitudinal.componentes;
using tadLayShare.puntos;

namespace PerfilLongitudinal
{
    public static class AlzadoAmilia
    {
        /// <summary>
        /// Reconstruye un objeto Alzado completo a partir de una lista de componentes longitudinales
        /// </summary>
        /// <param name="componentes">Lista de componentes (Recta y Parabola alternadas)</param>
        /// <param name="kvParabolas">Array con los Kv de cada parábola</param>
        /// <param name="velocidad">Velocidad de proyecto</param>
        /// <param name="kvConcavo">Kv mínimo para acuerdos cóncavos</param>
        /// <param name="kvConvexo">Kv mínimo para acuerdos convexos</param>
        /// <param name="intervaloSecciones">Intervalo entre secciones</param>
        /// <returns>Objeto Alzado original reconstruido</returns>
        public static Alzado ReconstruirAlzado(
            List<ComponenteLong> componentes,
            double[] kvParabolas,
            double velocidad,
            double intervaloSecciones,
            Polyline iEjeTrazado,
            Func<double?, double?, double?> miGetCota,
            Func<double[], double> MDT_Abanico_Punto)
        {
            if (componentes == null || componentes.Count == 0)
                throw new ArgumentException("La lista de componentes no puede estar vacía");

            // Crear una instancia vacía de Alzado usando reflexión
            var alzado = (Alzado)FormatterServices.GetUninitializedObject(typeof(Alzado));

            // Generar puntos del eje cada 10 metros (mPuntosEje)
            var puntosEje = GenerarPuntosEje(iEjeTrazado, miGetCota, MDT_Abanico_Punto);
            // Extraer vértices del alzado
            var verticesAlzado = ExtraerVertices(componentes);

            // Calcular pendientes entre vértices
            var pendientes = CalcularPendientes(verticesAlzado);

            // Determinar tipo de vértices (cóncavo/convexo)
            var tipoVertices = DeterminarTipoVertices(pendientes);

            // Calcular cambios de pendiente
            var cambioPendientes = CalcularCambioPendientes(pendientes);

            // Calcular acuerdos verticales
            var acuerdos = CalcularAcuerdos(
                verticesAlzado,
                kvParabolas,
                cambioPendientes,
                pendientes);

            // Calcular criterios de visibilidad
            var criterioVis = CalcularCriterioVisibilidad(velocidad, cambioPendientes);

            // Calcular Kv sin solape
            var kvSinSolape = CalcularKvSinSolape(verticesAlzado, cambioPendientes);

            // Calcular cotas máxima y mínima
            double maxZ = double.NegativeInfinity;
            double minZ = double.PositiveInfinity;
            foreach (var vertice in verticesAlzado)
            {
                if (vertice[2] > maxZ) maxZ = vertice[2];
                if (vertice[2] < minZ) minZ = vertice[2];
            }

            // Usar reflexión para establecer los campos privados
            SetPrivateField(alzado, "mVerticesAlzado", verticesAlzado);
            SetPrivateField(alzado, "mPendientes", pendientes);
            SetPrivateField(alzado, "mTipoVertices", tipoVertices);
            SetPrivateField(alzado, "mCambioPendientes", cambioPendientes);
            SetPrivateField(alzado, "mKvElegido", kvParabolas);
            SetPrivateField(alzado, "mAcuerdos", acuerdos);
            SetPrivateField(alzado, "mCriterioVis", criterioVis);
            SetPrivateField(alzado, "mKvSolape", kvSinSolape);
            SetPrivateField(alzado, "mVelocidad", velocidad);
            /*SetPrivateField(alzado, "mKvConcavo", kvConcavo);
            SetPrivateField(alzado, "mKvConvexo", kvConvexo);*/
            SetPrivateField(alzado, "mIntSecciones", intervaloSecciones);
            SetPrivateField(alzado, "mMaxZ", maxZ);
            SetPrivateField(alzado, "mMinZ", minZ);
            SetPrivateField(alzado, "mEjeAlzado", componentes);
            SetPrivateField(alzado, "mPuntosEje", puntosEje);

            return alzado;
        }

        private static List<double[]> GenerarPuntosEje(
           Polyline iEjeTrazado,
           Func<double?, double?, double?> miGetCota,
           Func<double[], double> MDT_Abanico_Punto)
        {
            var puntosEje = new List<double[]>();

            // Generar puntos cada 10 metros a lo largo del eje
            for (double i = 0; i < iEjeTrazado.Length; i += 10)
            {
                var punto = AñadePuntoDelTerreno(iEjeTrazado, miGetCota, MDT_Abanico_Punto, i);
                if (punto != null)
                {
                    puntosEje.Add(punto);
                }
            }

            // Añadir el último punto en la longitud total del eje
            var ultimoPunto = AñadePuntoDelTerreno(iEjeTrazado, miGetCota, MDT_Abanico_Punto, iEjeTrazado.Length);
            if (ultimoPunto != null)
            {
                puntosEje.Add(ultimoPunto);
            }

            return puntosEje;
        }
        private static double[] AñadePuntoDelTerreno(
            Polyline iEjeTrazado,
            Func<double?, double?, double?> miGetCota,
            Func<double[], double> MDT_Abanico_Punto,
            double distancia)
        {
            var miPunto = iEjeTrazado.GetPointAtDist(distancia);
            double[] miPuntoCota = new double[3];
            miPuntoCota[0] = miPunto.X;
            miPuntoCota[1] = miPunto.Y;

            // Obtener cota usando la función MDT_Abanico_Punto (KD-Tree)
            double? miCota = MDT_Abanico_Punto(miPuntoCota);

            if (miCota != null)
            {
                miPuntoCota[2] = (double)miCota;
                return miPuntoCota;
            }
            else
            {
                throw new Exception("Existen puntos fuera de la cartografia PERFIL LONGITUDINAL");
            }
        }
        private static void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                field.SetValue(obj, value);
            }
        }

        private static List<double[]> ExtraerVertices(List<ComponenteLong> componentes)
        {
            var vertices = new List<double[]>();

            // Primer vértice (inicio de la primera recta)
            var primerComponente = componentes[0];
            var puntoInicial = primerComponente.getPuntoEntrada;
            vertices.Add(new double[]
            {
                puntoInicial.coordenadaX,
                puntoInicial.coordenadaY,
                puntoInicial.coordenadaY, // Cota Z (usamos Y como Z en perfil longitudinal)
                primerComponente.getPkIni()
            });

            // Extraer vértices intermedios
            // El patrón es: Recta -> Parábola -> Recta -> Parábola...
            // Los vértices están entre cada par Recta-Parábola
            for (int i = 1; i < componentes.Count; i += 2)
            {
                if (i < componentes.Count &&
                    componentes[i].getTipoComponente() == ComponenteLong.tipoComponente.parabola)
                {
                    var parabola = componentes[i] as Parabola;

                    // El vértice (VPI) es el punto donde se cruzan las dos rectas tangentes.
                    // En acuerdos simétricos, el PK del vértice está en el punto medio.
                    double pkEntrada = parabola.getPkIni();
                    double longitudParabola = parabola.getLongutid();
                    double semilongitud = longitudParabola / 2.0;
                    double pkVertice = pkEntrada + semilongitud;

                    double xVertice = parabola.getPuntoEntrada.coordenadaX + semilongitud;

                    // Obtenemos la pendiente de la recta de entrada para calcular la cota del vértice teórico.
                    // El vértice está sobre la prolongación de la recta tangente de entrada.
                    double pendienteEntrada = 0;
                    if (i > 0 && componentes[i - 1] is Recta rectaAnterior)
                    {
                        pendienteEntrada = rectaAnterior.getPendiente();
                    }

                    // La cota del vértice (VPI) se calcula proyectando la pendiente de entrada desde el punto de entrada
                    // hasta el PK del vértice (distancia = semilongitud).
                    double yVertice = parabola.getPuntoEntrada.coordenadaY + pendienteEntrada * semilongitud;

                    vertices.Add(new double[]
                    {
                        xVertice,
                        yVertice,
                        yVertice,
                        pkVertice
                    });
                }
            }

            // Último vértice (final de la última recta)
            var ultimoComponente = componentes[componentes.Count - 1];
            var puntoFinal = ultimoComponente.getPuntoSalida;
            double pkFinal = ultimoComponente.getPkIni() + ultimoComponente.getLongutid();

            vertices.Add(new double[]
            {
                puntoFinal.coordenadaX,
                puntoFinal.coordenadaY,
                puntoFinal.coordenadaY,
                pkFinal
            });

            return vertices;
        }

        private static double[] CalcularPendientes(List<double[]> vertices)
        {
            double[] pendientes = new double[vertices.Count - 1];

            for (int i = 0; i < vertices.Count - 1; i++)
            {
                double deltaZ = vertices[i + 1][2] - vertices[i][2];
                double deltaPK = vertices[i + 1][3] - vertices[i][3];

                pendientes[i] = deltaPK != 0 ? deltaZ / deltaPK : 0;
            }

            return pendientes;
        }

        private static Alzado.tipoVertice[] DeterminarTipoVertices(double[] pendientes)
        {
            var tipos = new Alzado.tipoVertice[pendientes.Length - 1];

            for (int i = 0; i < pendientes.Length - 1; i++)
            {
                tipos[i] = pendientes[i] > pendientes[i + 1]
                    ? Alzado.tipoVertice.convexo
                    : Alzado.tipoVertice.concavo;
            }

            return tipos;
        }

        private static double[] CalcularCambioPendientes(double[] pendientes)
        {
            double[] cambios = new double[pendientes.Length - 1];

            for (int i = 0; i < pendientes.Length - 1; i++)
            {
                cambios[i] = Math.Abs(pendientes[i + 1] - pendientes[i]);
            }

            return cambios;
        }

        private static List<List<double[]>> CalcularAcuerdos(
            List<double[]> vertices,
            double[] kvElegido,
            double[] cambioPendientes,
            double[] pendientes)
        {
            var acuerdos = new List<List<double[]>>();

            // El número de acuerdos está limitado por el mínimo de:
            // - kvElegido: Kv leídos del LandXML (uno por parábola)
            // - cambioPendientes: calculado como vértices.Count - 2
            // - vertices.Count - 2: necesitamos vertices[i+1] y vertices[i+2]
            int numAcuerdos = Math.Min(kvElegido.Length,
                              Math.Min(cambioPendientes.Length,
                                       vertices.Count - 2));

            for (int i = 0; i < numAcuerdos; i++)
            {
                double T = (kvElegido[i] * cambioPendientes[i]) / 2.0;

                var puntoEntrada = new double[2];
                var puntoSalida = new double[2];

                // PK de entrada y salida del acuerdo
                puntoEntrada[0] = vertices[i + 1][3] - T;
                puntoSalida[0] = vertices[i + 1][3] + T;

                // Cotas de entrada y salida
                // Cota entrada: desde el vértice anterior hacia el vértice actual
                double deltaPK_entrada = puntoEntrada[0] - vertices[i][3];
                double longitudTramo_i = vertices[i + 1][3] - vertices[i][3];
                double deltaZ_i = vertices[i + 1][2] - vertices[i][2];
                puntoEntrada[1] = vertices[i][2] + (deltaPK_entrada / longitudTramo_i) * deltaZ_i;

                // Cota salida: desde el vértice actual hacia el siguiente vértice
                double deltaPK_salida = puntoSalida[0] - vertices[i + 1][3];
                double longitudTramo_i1 = vertices[i + 2][3] - vertices[i + 1][3];
                double deltaZ_i1 = vertices[i + 2][2] - vertices[i + 1][2];
                puntoSalida[1] = vertices[i + 1][2] + (deltaPK_salida / longitudTramo_i1) * deltaZ_i1;

                var puntosAcuerdo = new List<double[]> { puntoEntrada, puntoSalida };
                acuerdos.Add(puntosAcuerdo);
            }

            return acuerdos;
        }

        private static double[] CalcularCriterioVisibilidad(double velocidad, double[] cambioPendientes)
        {
            double[] criterios = new double[cambioPendientes.Length];

            for (int i = 0; i < cambioPendientes.Length; i++)
            {
                criterios[i] = cambioPendientes[i] != 0 ? velocidad / cambioPendientes[i] : 0;
            }

            return criterios;
        }

        private static double[] CalcularKvSinSolape(List<double[]> vertices, double[] cambioPendientes)
        {
            double[] kvSinSolape = new double[vertices.Count - 2];

            for (int i = 0; i < vertices.Count - 2; i++)
            {
                double L1 = Math.Abs(vertices[i + 1][3] - vertices[i][3]);
                double L2 = Math.Abs(vertices[i + 2][3] - vertices[i + 1][3]);

                kvSinSolape[i] = cambioPendientes[i] != 0
                    ? Math.Min(L1, L2) / cambioPendientes[i]
                    : 0;
            }

            return kvSinSolape;
        }
    }
}