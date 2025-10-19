using System;

namespace upr1_2
{
    public abstract partial class Shape
    {
        public abstract double Perimeter();
        public abstract double Area();

        private int _color;

        public int Color
        {
            get
            {
                int red = (_color >> 16) & 0xFF;
                int green = (_color >> 8) & 0xFF;
                int blue = _color & 0xFF;

                if (red == 255 && green == 0 && blue == 0)
                {
                    return BasicColors.RED;
                }
                else if (red == 0 && green == 255 && blue == 0)
                {
                    return BasicColors.GREEN;
                }
                else if (red == 0 && green == 0 && blue == 255)
                {
                    return BasicColors.BLUE;
                }

                return 0;
            }

            set
            {
                unchecked
                {
                    switch (value)
                    {
                        case BasicColors.BLUE:
                            _color = (int)0xFF0000FF;
                            break;
                        case BasicColors.RED:
                            _color = (int)0xFFFF0000;
                            break;
                        case BasicColors.GREEN:
                            _color = (int)0xFF00FF00;
                            break;
                        default:
                            _color = 0;
                            break;
                    }
                }
            }
        }

        public static class BasicColors
        {
            public const int BLUE = 1;
            public const int RED = 2;
            public const int GREEN = 3;
        }
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

        public void PrintDimensions(string label)
        {
            Console.WriteLine($"\t{label}: W={Width}, H={Height}");
        }

        public void PrintDimensions()
        {
            Console.WriteLine($"\tRectangle dimensions: W={Width}, H={Height}");
        }
    }
    
    //Ostanalite klasove sa w upr1_1

    public class Program
    {
        public static void Main()
        {
            var r = new Rectangle(3, 4);

            Console.WriteLine("\n<< Upr1_2 >>");

            Console.WriteLine($"Before setting the color by default it's : {r.Color}");

            r.Color = Shape.BasicColors.RED;
            Console.WriteLine($"\n1. Set color to RED ({Shape.BasicColors.RED}).");
            Console.WriteLine($"\tGet color value: {r.Color}");

            r.Color = Shape.BasicColors.BLUE;
            Console.WriteLine($"\n2. Change color to BLUE ({Shape.BasicColors.BLUE}).");
            Console.WriteLine($"\tGet color value: {r.Color} ");

            r.Color = Shape.BasicColors.GREEN;
            Console.WriteLine($"\n3. Change color to GREEN ({Shape.BasicColors.GREEN}).");
            Console.WriteLine($"\tGet color value: {r.Color} ");
        }
    }
}