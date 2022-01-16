using System;

namespace App
{
    /// <summary>
    ///     Main method that launches the application.
    /// </summary>
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using AntSimulator game = new AntSimulator();
            game.Run();
        }
    }
}