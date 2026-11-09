using System;

namespace Figuras
{
    // 1. Clase abstracta (molde obligatorio) - 3 pts
    public abstract class Figura
    {
        public abstract double CalcularArea();
        public abstract double CalcularPerimetro();
    }

    // 2. Clases hijas que desarrollan los métodos abstractos - 2 pts
    public class Rectangulo : Figura
    {
        public double Base { get; set; }
        public double Altura { get; set; }

        public Rectangulo(double b, double altura)
        {
            Base = b;
            Altura = altura;
        }

        public override double CalcularArea() => Base * Altura;
        public override double CalcularPerimetro() => 2 * (Base + Altura);
    }

    public class Cuadrado : Figura
    {
        public double Lado { get; set; }

        public Cuadrado(double lado) { Lado = lado; }

        public override double CalcularArea() => Lado * Lado;
        public override double CalcularPerimetro() => 4 * Lado;
    }

    public class Triangulo : Figura
    {
        public double LadoA { get; set; }
        public double LadoB { get; set; }
        public double LadoC { get; set; }

        public Triangulo(double a, double b, double c)
        {
            LadoA = a; LadoB = b; LadoC = c;
        }

        // Fórmula de Herón
        public override double CalcularArea()
        {
            double s = CalcularPerimetro() / 2;
            return Math.Sqrt(s * (s - LadoA) * (s - LadoB) * (s - LadoC));
        }

        public override double CalcularPerimetro() => LadoA + LadoB + LadoC;
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 3. Arreglo unidimensional de tipo PADRE - 5 pts
            Figura[] figuras = new Figura[3];
            figuras[0] = new Rectangulo(5, 3);
            figuras[1] = new Cuadrado(4);
            figuras[2] = new Triangulo(3, 4, 5);

            Console.WriteLine("=== Jerarquía de Figuras (Polimorfismo) ===\n");

            // 4. Recorrido polimórfico - 5 pts
            foreach (Figura f in figuras)
            {
                Console.WriteLine($"Figura:     {f.GetType().Name}");
                Console.WriteLine($"Área:       {f.CalcularArea():F2}");
                Console.WriteLine($"Perímetro:  {f.CalcularPerimetro():F2}");
                Console.WriteLine(new string('-', 30));
            }

            Console.ReadKey();
        }
    }
}