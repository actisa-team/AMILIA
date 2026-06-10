using System;
class Test {
static void Main() {
// Simular Rellenar_centro tal como está codificado
// dirección: xc1,yc1=punto nuevo, xc,yc=punto anterior
// Para recta1: azr = Rellenar_centro(ultimo_punto, penultimo_punto, 1)
//   => p=ultimo, p_a=penultimo => Dx=ux-px, Dy=uy-py
// Para recta2: azr = Rellenar_centro(ultimo_punto, penultimo_punto, 1)
static double Rellenar_Az(double xc1, double yc1, double xc, double yc) {
    double Dx = xc1 - xc;
    double Dy = yc1 - yc;
    double Ad1, Ad2;
    if (Dx==0 || Dy==0) Ad1=0; else Ad1=Math.Atan(Dy/Dx);
    Ad2=Ad1*(180/Math.PI);
    int signod = (Ad1==0)?0:(Ad1<0)?1:2;
    int signodx = (Dx==0)?0:(Dx<0)?1:2;
    int signody = (Dy==0)?0:(Dy<0)?1:2;
    int Dc = (signod==0)?2:1;
    double Az;
    if (Dc==2) {
        if (Dy==0) Az=(Dx<0)?270:90;
        else Az=(Dy<0)?180:0;
    } else {
        if (signod==1) Az=(signodx==2)?90-Ad2:270-Ad2;
        else Az=(signodx==2)?90-Ad2:270-Ad2;
    }
    return Az;
}

// RECTA 1: P1(1887.5717,1780.9643) P2(2308.4811,2142.8987)
// Rellenar_Recta usa lista_puntos[Count-1] como xc1,yc1 y lista_puntos[Count-2] como xc,yc
// Suponemos que tiene 2 puntos: [0]=(1887,1780), [1]=(2308,2142)
// => xc1=2308.4811,yc1=2142.8987  xc=1887.5717,yc=1780.9643
double az1 = Rellenar_Az(2308.4811,2142.8987, 1887.5717,1780.9643);
Console.WriteLine($"c1.azr (Rellenar_centro) = {az1:F4}°");
double az1_std = Math.Atan2(2308.4811-1887.5717, 2142.8987-1780.9643)*180/Math.PI;
if(az1_std<0)az1_std+=360;
Console.WriteLine($"az1 con Atan2(dx,dy) estándar = {az1_std:F4}°");

// RECTA 2: P1(2515.8408,2102.6838) P2(2797.4787,1932.5437)
// [0]=(2515,2102), [1]=(2797,1932)
// =>  xc1=2797.4787,yc1=1932.5437  xc=2515.8408,yc=2102.6838
double az2 = Rellenar_Az(2797.4787,1932.5437, 2515.8408,2102.6838);
Console.WriteLine($"c3.azr (Rellenar_centro) = {az2:F4}°");
double az2_std = Math.Atan2(2797.4787-2515.8408, 1932.5437-2102.6838)*180/Math.PI;
if(az2_std<0)az2_std+=360;
Console.WriteLine($"az2 con Atan2(dx,dy) estándar = {az2_std:F4}°");

Console.WriteLine($"\nDiferencia az1: {az1-az1_std:F4}°");
Console.WriteLine($"Diferencia az2: {az2-az2_std:F4}°");

// Ahora calcular los puntos con az1 y az2 según Rellenar_centro
double D2R=Math.PI/180;
double iRc=200.7855, A=166.961771277628;
int var=1;
double r1x1=1887.5717, r1y1=1780.9643, r1x2=2308.4811, r1y2=2142.8987;
double r2x1=2515.8408, r2y1=2102.6838, r2x2=2797.4787, r2y2=1932.5437;
double m1=(r1y2-r1y1)/(r1x2-r1x1), b1=r1y1-m1*r1x1;
double m2=(r2y2-r2y1)/(r2x2-r2x1), b2=r2y1-m2*r2x1;
double vx=(b2-b1)/(m1-m2), vy=m1*vx+b1;
Console.WriteLine($"\nVertice=({vx:F4},{vy:F4})");

double miDelta=Math.Abs(az2-az1)>180?360-Math.Abs(az2-az1):Math.Abs(az2-az1);
string sentido;
if(az1>=0&&az1<=180) sentido=((az1-az2)<0&&Math.Abs(az1-az2)<180)?"Horario":"Antihorario";
else sentido=((az1-az2)>0&&Math.Abs(az1-az2)<180)?"Antihorario":"Horario";
Console.WriteLine($"Delta={miDelta:F4}  Sentido={sentido}");

double miLe=A*A/iRc, miQe=miLe/(2*iRc), qeDeg=miQe*180/Math.PI;
double miYe=((miQe/3)-(Math.Pow(miQe,3)/42))*miLe;
double miDR=miYe-iRc*(1-Math.Cos(miQe));
double miXe=(1-Math.Pow(miQe,2)/10+Math.Pow(miQe,4)/216)*miLe;
double miXM=miXe-iRc*Math.Sin(miQe);
double miPhi=180-miDelta;
double miEe=((iRc+miDR)/Math.Abs(Math.Cos(miDelta/2*D2R)))-iRc;

double Pcx,Pcy,Pc1x,Pc1y,Pc2x,Pc2y;
if(sentido=="Horario") {
  Pcx=vx+(miEe/var+iRc)*Math.Sin((az2+miPhi/2)*D2R);
  Pcy=vy+(miEe/var+iRc)*Math.Cos((az2+miPhi/2)*D2R);
  Pc1x=Pcx+iRc*Math.Sin((az1-90+qeDeg)*D2R);
  Pc1y=Pcy+iRc*Math.Cos((az1-90+qeDeg)*D2R);
  Pc2x=Pcx+iRc*Math.Sin((az2-90-qeDeg)*D2R);
  Pc2y=Pcy+iRc*Math.Cos((az2-90-qeDeg)*D2R);
} else {
  Pcx=vx+(miEe/var+iRc)*Math.Sin((az2-miPhi/2)*D2R);
  Pcy=vy+(miEe/var+iRc)*Math.Cos((az2-miPhi/2)*D2R);
  Pc1x=Pcx+iRc*Math.Sin((az1+90-qeDeg)*D2R);
  Pc1y=Pcy+iRc*Math.Cos((az1+90-qeDeg)*D2R);
  Pc2x=Pcx+iRc*Math.Sin((az2+90+qeDeg)*D2R);
  Pc2y=Pcy+iRc*Math.Cos((az2+90+qeDeg)*D2R);
}
Console.WriteLine($"Centro =({Pcx:F4},{Pcy:F4})");
Console.WriteLine($"Pc1 (ini arco) =({Pc1x:F4},{Pc1y:F4})");
Console.WriteLine($"Pc2 (fin arco) =({Pc2x:F4},{Pc2y:F4})");
Console.WriteLine($"\nUsuario: entrada=(2562.3918,1842.0316)  salida=(2231.7162,1814.3941)");
double e1=Math.Sqrt(Math.Pow(Pc1x-2231.7162,2)+Math.Pow(Pc1y-1814.3941,2));
double e2=Math.Sqrt(Math.Pow(Pc2x-2562.3918,2)+Math.Pow(Pc2y-1842.0316,2));
Console.WriteLine($"Error Pc1 vs usuario-salida: {e1:F2}m");
Console.WriteLine($"Error Pc2 vs usuario-entrada: {e2:F2}m");
}
static double Rellenar_Az(double xc1, double yc1, double xc, double yc) {
    double Dx=xc1-xc, Dy=yc1-yc;
    double Ad1=0;
    if (Dx!=0 && Dy!=0) Ad1=Math.Atan(Dy/Dx);
    double Ad2=Ad1*(180/Math.PI);
    int signod=(Ad1==0)?0:(Ad1<0)?1:2;
    int signodx=(Dx==0)?0:(Dx<0)?1:2;
    int Dc=(signod==0)?2:1;
    double Az;
    if(Dc==2){if(Dy==0)Az=(Dx<0)?270:90;else Az=(Dy<0)?180:0;}
    else{Az=(signodx==2)?90-Ad2:270-Ad2;}
    return Az;
}
}
