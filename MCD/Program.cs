using System;
using System.Dynamic;

namespace AlgdeEuclides
{
    class program
    {
        static void mostrarInterfaz()
        {
            // Vista Inicial
            Console.WriteLine("CALCULADORA DE MCD Y MCM ENTRE DOS VALORES ENTEROS");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("INGRESE DOS NÚMEROS ENTEROS");

        }

        public static int obtenerValores(string msj1, int nim = 0, int xam = Int32.MaxValue)
        {
            // Variables locales
            int n;

            // Mensaje de entrada de valores

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msj1);

            // Comprobación de los datos ingresados
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                if (!int.TryParse(Console.ReadLine(), out n))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada inválida. Por favor, ingrese un valor de tipo entero");
                    continue;
                }
                if (n <= nim || n > xam)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada inválida. Por favor, ingrese un valor correcto");
                    continue;
                }
                return n;
            }
        }

        static void Main(string[] args)
        {
            mostrarInterfaz();

            //Variables locales
            int a, b, tmp, res, a1, b1;
            int c = 1;

            // Ingreso de los valores a,b
            a = obtenerValores("Ingrese el valor de a: "); a1 = a;
            b = obtenerValores("Ingrese el valor de b: "); b1 = b;

            // Verificar que a>b, sino cambiar los valores de lugar
            if (a < b)
            {
                tmp = a; a = b; b = tmp;
            }


            // Alg de Euclides
            Console.WriteLine("\nDividendo\tDivisor\t\tResto");

            do
            {
                // Obtener el resto de la division
                res = a % b;
                // Si el resto es 0, almacenar el resto anterior obtenido
                if (res == 0)
                {
                    c = b;
                }
                // UNDONE: Tabla que muestra el proceso de las divisiones
                Console.WriteLine("{0}\t\t{1}\t\t{2}", a, b, res);
                // Realizar las modificaciones de las variables
                a = b;
                b = res;

            }
            while (res != 0);

            // Mostrar los resultados en pantalla
            Console.WriteLine("\nMCD({0},{1}) = {2}", a1, b1, c);
            Console.WriteLine("mcm({0},{1}) = {2}", a1, b1, (a1 * b1) / c);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nPresione una tecla para salir . . . ");
            Console.ReadKey(true);
        }
    }
}
