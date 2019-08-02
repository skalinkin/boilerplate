using BoilerplateNetFx;
using System;

namespace ConsoleHostNetFx
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            var applicationConstructor = new Bootstrapper();
            applicationConstructor.BuildComposition();

            var start = applicationConstructor.GetCompositionRoot<IServiceProvider>();
        }
        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
