using System;

namespace ReactionDiffusion
{

    public class Cell
    {
        //Initialize substance A and B and Color
        public double A;
        public double B;
        public RGBcolor Color;

        //Constructor
        public Cell()
        {
            //start with 0 amount of cell A and B and set random color
            A = 1;
            B = 0;
            Color = new RGBcolor();
            UpdateColor();

        }
        public double LimitToRange(double value, double inclusiveMinimum, double inclusiveMaximum)
        {
            if (value < inclusiveMinimum) { return inclusiveMinimum; }
            if (value > inclusiveMaximum) { return inclusiveMaximum; }
            return value;
        }
        public void UpdateColor()
        {

            double c = Math.Floor((A - B) * 255);
            c = LimitToRange(c, 0, 255);
            Color.R = c;
            Color.G = c;
            Color.B = c;

        }
    }



    public class ReactionDiffusionController
    {
        //Create the algorithm inputs: grid, KillRate, FeedRate, Time, next grid
        public Cell[,] Grid;

        public double KillRate;
        public double FeedRate;
        public double Time;

        public Cell[,] Next;

        public double DA;
        public double DB;


        //Constructor
        public ReactionDiffusionController(int width, int height, double killRate, double feedRate, double Da, double Db)
        {
            //Set class agruments = to algorithms inputs and Time to 0 
            KillRate = killRate;
            FeedRate = feedRate;
            Time = 0;
            DA = Da;
            DB = Db;

            //Create new cell with inputs width, height
            Grid = new Cell[width, height];
            Next = new Cell[width, height];

            //Iterate on the cell respecting width
            for (int x = 0; x < width; x++)
            {
                //Iterate on the cell respecting height
                for (int y = 0; y < height; y++)
                {
                    //create new cell and array 2D
                    Grid[x, y] = new Cell();
                    Next[x, y] = new Cell();
                }
            }

        }

        //Run algorithm
        public void Run(int width, int height)
        {
            //swap method to update value of Ai and Bi
            Swap(width, height);
            //2D array iteration
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    //2D array new Cell c on x and y
                    Cell c = Grid[x, y];
                    //compute new Value of A
                    Next[x, y].A = ComputeAvalue(c.A, c.B, x, y);
                    //compute new Value of B
                    Next[x, y].B = ComputeBvalue(c.A, c.B, x, y);
                    Next[x, y].A = Next[x, y].LimitToRange(Next[x, y].A, 0, 1);
                    Next[x, y].B = Next[x, y].LimitToRange(Next[x, y].B, 0, 1);
                    Next[x, y].UpdateColor();
                }
            }
        }
        public void AssignBvalues(int firstCorner, int secondCorner, int thirdCorner, int fourthCorner)
        {
            for (int i = firstCorner; i < secondCorner; i++)
            {
                for (int j = thirdCorner; j < fourthCorner; j++)
                {
                    Grid[i, j].B = 1;
                    Grid[i, j].UpdateColor();
                }
            }
        }

        //Swap method
        public void Swap(int width, int height)
        {
            //create temp value =grid to not overwrite the intiale value
            Cell[,] temp = Grid;
            Grid = Next;
            Next = temp;
        }
        //Compute A value method
        public double ComputeAvalue(double A, double B, int x, int y)
        {
            double convolutionA = ConvolutionA(x, y);
            double Aone = A + (DA * convolutionA * convolutionA * A) - (A * B * B) + (FeedRate * (1 - A));
            return Aone;
        }
        //Compute B value method
        public double ComputeBvalue(double A, double B, int x, int y)
        {
            double convolutionB = ConvolutionB(x, y);
            double Bone = B + (DB * convolutionB * convolutionB * B) + (B * B * A) - ((KillRate + FeedRate) * B);
            return Bone;
        }
        //Convolution A method
        public double ConvolutionA(int x, int y)
        {
            double sumA = 0;
            sumA += Grid[x, y].A * -1;
            sumA += Grid[x - 1, y].A * 0.2;
            sumA += Grid[x + 1, y].A * 0.2;
            sumA += Grid[x, y + 1].A * 0.2;
            sumA += Grid[x, y - 1].A * 0.2;
            sumA += Grid[x - 1, y - 1].A * 0.05;
            sumA += Grid[x + 1, y - 1].A * 0.05;
            sumA += Grid[x + 1, y + 1].A * 0.05;
            sumA += Grid[x - 1, y + 1].A * 0.05;

            return sumA;
        }
        //Convolution B method
        public double ConvolutionB(int x, int y)
        {
            double sumB = 0;
            sumB += Grid[x, y].B * -1;
            sumB += Grid[x - 1, y].B * 0.2;
            sumB += Grid[x + 1, y].B * 0.2;
            sumB += Grid[x, y + 1].B * 0.2;
            sumB += Grid[x, y - 1].B * 0.2;
            sumB += Grid[x - 1, y - 1].B * 0.05;
            sumB += Grid[x + 1, y - 1].B * 0.05;
            sumB += Grid[x + 1, y + 1].B * 0.05;
            sumB += Grid[x - 1, y + 1].B * 0.05;
            return sumB;
        }

    }

    public class RGBcolor
    {
        // Initialize colors R G B
        public double R;
        public double G;
        public double B;


        //string format to export .text file for grasshopper
        public string ToCSV()
        {
            return R + ";" + G + ";" + B;

        }
        //Create random color class
        public static RGBcolor CreateRandomColor()
        {
            RGBcolor color = new RGBcolor();
            Random rnd = new Random();
            color.R = rnd.NextDouble() * 255;
            color.B = rnd.NextDouble() * 255;
            color.G = rnd.NextDouble() * 255;
            return color;
        }
    }
}