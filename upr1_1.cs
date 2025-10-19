using System;

namespace upr1_1
{
    public abstract class Shape
    {
        public abstract double Perimeter();
        public abstract double Area();
    }

    public interface IElipsa
    {
        bool IsElipsa();
    }

    public class Rectangle : Shape, IElipsa
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override double Perimeter() => 2 * (Width + Height);
        public override double Area() => Width * Height;

        public bool IsElipsa() => false;

        //Example of overloading
        public void PrintDimensions(string label)
        {
            Console.WriteLine($"\t{label}: W={Width}, H={Height}");
        }

        public void PrintDimensions()
        {
            Console.WriteLine($"\tRectangle dimensions: W={Width}, H={Height}");
        }
    }

    public class Circle : Shape, IElipsa
    {
        public double Radius { get; set; }

        public Circle(double radius) { Radius = radius; }

        public override double Perimeter() => 2 * Math.PI * Radius;
        public override double Area() => Math.PI * Radius * Radius;

        public bool IsElipsa() => true;
    }

    public class Square : Rectangle
    {
        public double Side { get; set; }

        public Square(double side) : base(side, side)
        {
            Side = side;
        }

        public sealed override double Area() => Side * Side;

        public static double CalculateArea(double side)
        {
            return side * side;
        }
    }

    public class Program
    {
        public static void Main()
        {
            var r = new Rectangle(3, 4);
            var c = new Circle(4);
            var s = new Square(5);

            Console.WriteLine("<< Upr 1.1 >>");
            Console.WriteLine($"Rectangle: P={r.Perimeter()}, S={r.Area()}, Elipsa={((IElipsa)r).IsElipsa()}");
            Console.WriteLine($"Circle: P={c.Perimeter():F2}, S={c.Area():F2}, Elipsa={((IElipsa)c).IsElipsa()}");
            Console.WriteLine($"Square: P={s.Perimeter()}, S={s.Area()}");

            Console.WriteLine($"Square Static Area (side=6): S={Square.CalculateArea(6)}");

            r.PrintDimensions("R1");
            r.PrintDimensions();
        }
    }
}