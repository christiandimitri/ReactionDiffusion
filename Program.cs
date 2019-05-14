using System;
using System.IO;

namespace ReactionDiffusion
{
    class Program
    {
        static void Main(string[] args)
        {
            //width and height inputs
            int width = 20;
            int height = 20;
            double KillRate = 0.062;
            double FeedRate = 0.055;
            double dA = 1;
            double dB = 0.5;
            Console.WriteLine("Hello World!");
            //reaction diffusion algorithm controller
            ReactionDiffusionController cntrl = new ReactionDiffusionController(width, height, KillRate, FeedRate, dA, dB);
            cntrl.AssignBvalues(8, 12, 8, 18);
            for (int i = 0; i < 2; i++)
            {
                cntrl.Run(width, height);

                Console.WriteLine("controller diffusiont test:{0}", cntrl.KillRate);
                Console.WriteLine("Cell value:{0}", cntrl.Grid[2, 2].A);
            }


            //string format to export .text file for grasshopper
            string text = width + ";" + height + ";0";
            //write text for each cell of the grid
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    text += "\n" + cntrl.Next[i, j].Color.ToCSV();
                }
            }


            //write text to a file
            File.WriteAllText("Result.txt", text);
        }
    }
}
