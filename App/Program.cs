using System;

namespace App
{
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