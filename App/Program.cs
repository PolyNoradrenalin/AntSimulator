using System;

namespace App
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using AntSimulator game = new AntSimulator();
            game.Run();
        }
    }
}