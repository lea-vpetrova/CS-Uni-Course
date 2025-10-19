using System;

namespace upr1_zad1
{
    public class Triangle<T>
    {
        public T SideA { get; }
        public T SideB { get; }
        public T SideC { get; }

        private Triangle(T a, T b, T c)
        {
            SideA = a;
            SideB = b;
            SideC = c;
        }

        public static bool GetInstance(T a, T b, T c, out Triangle<T> triangle)
        {
            triangle = null;

            Type typeT = typeof(T);
            bool isNumericType = (typeT == typeof(float) || typeT == typeof(int));

            if (!isNumericType)
            {
                Console.WriteLine($"Error: Generic type {typeT.Name} is not supported (must be float or int).");
                return false;
            }

            try
            {
                double sideA = Convert.ToDouble(a);
                double sideB = Convert.ToDouble(b);
                double sideC = Convert.ToDouble(c);

                if (sideA <= 0 || sideB <= 0 || sideC <= 0)
                {
                    Console.WriteLine("Error: Side lengths must be positive.");
                    return false;
                }

                if (sideA + sideB > sideC &&
                    sideA + sideC > sideB &&
                    sideB + sideC > sideA)
                {
                    triangle = new Triangle<T>(a, b, c);
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error: Sides {sideA}, {sideB}, {sideC} do not form a triangle.");
                    return false;
                }
            }
            catch (InvalidCastException)
            {
                Console.WriteLine($"Error: Could not convert generic values to numeric types for comparison.");
                return false;
            }
        }

        public string GetSideLengths()
        {
            return $"Sides: A={SideA}, B={SideB}, C={SideC}";
        }
    }


    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("<< Upr1_3 >>");

            Triangle<int> intTriangle;
            if (Triangle<int>.GetInstance(3, 4, 5, out intTriangle))
            {
                Console.WriteLine($"\nSUCCESS: Integer Triangle created. {intTriangle.GetSideLengths()}");
            }
            else
            {
                Console.WriteLine("\nFAILURE: Integer Triangle creation failed.");
            }

            Triangle<float> floatTriangle;
            if (Triangle<float>.GetInstance(1.0f, 2.0f, 10.0f, out floatTriangle))
            {
                Console.WriteLine($"\nSUCCESS: Float Triangle created. {floatTriangle.GetSideLengths()}");
            }
            else
            {
                Console.WriteLine("\nFAILURE: Float Triangle creation failed (sides are invalid).");
            }

            Triangle<string> stringTriangle;
            if (Triangle<string>.GetInstance("a", "b", "c", out stringTriangle))
            {
                Console.WriteLine($"\nSUCCESS: String Triangle created. {stringTriangle.GetSideLengths()}");
            }
            else
            {
                Console.WriteLine("\nFAILURE: String Triangle creation failed (type is invalid).");
            }
        }
    }
}