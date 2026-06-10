using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X:F3}, {Y:F3})";
        }
    }

    public enum RotationDirection
    {
        Antihoraria,  // Counterclockwise (ángulo positivo)
        Horaria       // Clockwise (ángulo negativo)
    }

    public class ArcRotation
    {
        /// <summary>
        /// Rota un punto alrededor de otro punto (pivote)
        /// </summary>
        /// <param name="point">Punto a rotar</param>
        /// <param name="pivot">Punto fijo (pivote)</param>
        /// <param name="angleRadians">Ángulo en radianes (positivo=antihoraria, negativo=horaria)</param>
        /// <returns>Punto rotado</returns>
        public static Point2D RotateAroundPoint(Point2D point, Point2D pivot, double angleRadians)
        {
            double cos = Math.Cos(angleRadians);
            double sin = Math.Sin(angleRadians);

            double dx = point.X - pivot.X;
            double dy = point.Y - pivot.Y;

            double xNew = cos * dx - sin * dy + pivot.X;
            double yNew = sin * dx + cos * dy + pivot.Y;

            return new Point2D(xNew, yNew);
        }
        /// <summary>
        /// Calcula el ángulo de un punto respecto a un pivote
        /// </summary>
        private static double GetAngleFromPivot(Point2D point, Point2D pivot)
        {
            return Math.Atan2(point.Y - pivot.Y, point.X - pivot.X);
        }
        /// <summary>
        /// Determina la dirección de rotación según la posición relativa del centro respecto al arco
        /// </summary>
        /// <param name="fixedPoint">Punto fijo del arco</param>
        /// <param name="movingPoint">Punto móvil del arco</param>
        /// <param name="center">Centro del arco</param>
        /// <returns>Ángulo con signo: positivo=antihoraria, negativo=horaria según dirección del centro</returns>
        public static double GetRotationAngleTowardsCenter(Point2D fixedPoint, Point2D movingPoint, Point2D center)
        {
            // Ángulos desde el punto fijo
            double angleToMoving = GetAngleFromPivot(movingPoint, fixedPoint);
            double angleToCenter = GetAngleFromPivot(center, fixedPoint);

            // Diferencia angular normalizada a [-π, π]
            double deltaAngle = angleToCenter - angleToMoving;

            // Normalizar a [-π, π]
            while (deltaAngle > Math.PI) deltaAngle -= 2 * Math.PI;
            while (deltaAngle < -Math.PI) deltaAngle += 2 * Math.PI;

            return deltaAngle;
        }
        /// <summary>
        /// Calcula el punto medio (central) del arco entre dos puntos
        /// </summary>
        /// <param name="point1">Primer punto del arco</param>
        /// <param name="point2">Segundo punto del arco</param>
        /// <param name="center">Centro del arco</param>
        /// <returns>Punto medio del arco (a mitad del ángulo barrido)</returns>
        public static Point2D GetArcMidpoint(Point2D point1, Point2D point2, Point2D center)
        {
            // Calcular ángulos de ambos puntos respecto al centro
            double angle1 = Math.Atan2(point1.Y - center.Y, point1.X - center.X);
            double angle2 = Math.Atan2(point2.Y - center.Y, point2.X - center.X);

            // Calcular el ángulo medio (siempre por el camino más corto)
            double deltaAngle = angle2 - angle1;

            // Normalizar a [-π, π] para tomar el camino más corto
            while (deltaAngle > Math.PI) deltaAngle -= 2 * Math.PI;
            while (deltaAngle < -Math.PI) deltaAngle += 2 * Math.PI;

            double midAngle = angle1 + deltaAngle / 2.0;

            // Calcular el radio
            double radius = GetRadius(point1, center);

            // Calcular el punto medio usando el ángulo medio
            double xMid = center.X + radius * Math.Cos(midAngle);
            double yMid = center.Y + radius * Math.Sin(midAngle);

            return new Point2D(xMid, yMid);
        }
        /// <summary>
        /// Rota el centro y un punto del arco alrededor del punto fijo
        /// </summary>
        /// <param name="fixedPoint">Punto del arco que se mantiene fijo</param>
        /// <param name="movingPoint">Punto del arco que se rota</param>
        /// <param name="center">Centro del arco</param>
        /// <param name="angleDegrees">Ángulo de rotación en grados</param>
        /// <param name="direction">Dirección de rotación</param>
        public static Tuple<Point2D,Point2D, Point2D> RotateArc(
            Point2D fixedPoint, 
            Point2D movingPoint, 
            Point2D center, 
            double angleDegrees)
        {
            // Determinar dirección automáticamente según posición del centro
            double directionAngle = GetRotationAngleTowardsCenter(fixedPoint, movingPoint, center);

            // Aplicar el signo correcto al ángulo de rotación
            double angleRadians = Math.Abs(angleDegrees) * Math.PI / 180.0;
            angleRadians *= Math.Sign(directionAngle);

            // Rotar ambos puntos alrededor del punto fijo
            Point2D newMovingPoint = RotateAroundPoint(movingPoint, fixedPoint, angleRadians);
            Point2D newCenter = RotateAroundPoint(center, fixedPoint, angleRadians);
            // Calcular el nuevo punto medio del arco rotado
            Point2D newMidpoint = GetArcMidpoint(fixedPoint, newMovingPoint, newCenter);

            return Tuple.Create(newMovingPoint, newCenter, newMidpoint);
        }

        /// <summary>
        /// Calcula el ángulo del arco entre el punto fijo y el punto móvil
        /// </summary>
        public static double GetArcAngleDegrees(Point2D fixedPoint, Point2D movingPoint, Point2D center)
        {
            double angle1 = Math.Atan2(fixedPoint.Y - center.Y, fixedPoint.X - center.X);
            double angle2 = Math.Atan2(movingPoint.Y - center.Y, movingPoint.X - center.X);

            double deltaAngle = angle2 - angle1;

            // Normalizar a [0, 2π)
            while (deltaAngle < 0) deltaAngle += 2 * Math.PI;
            while (deltaAngle >= 2 * Math.PI) deltaAngle -= 2 * Math.PI;

            return deltaAngle * 180.0 / Math.PI;
        }

        /// <summary>
        /// Calcula el radio del arco
        /// </summary>
        public static double GetRadius(Point2D pointOnArc, Point2D center)
        {
            double dx = pointOnArc.X - center.X;
            double dy = pointOnArc.Y - center.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
