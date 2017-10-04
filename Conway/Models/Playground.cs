using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conway.Models
{
    // TODO Playground baut lediglich die Zellen auf
    public class Playground
    {
        public List<Cell> Cells { get; private set; }
        private int SizeX;
        private int SizeY;

        public Playground(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;

            Cells = new List<Cell>(sizeX * sizeY);

            for (int i = 0; i < sizeX * sizeY; i++)
            {
                int posX = i % sizeX;
                int posY = (i - posX) / sizeX; 
                Cells.Add(new Cell(posX, posY));
            }

            SetupNeighborhood();
        }

        private void SetupNeighborhood()
        {
            foreach (var cell in Cells)
            {
                List<Cell> neighbors = new List<Cell>();

                for (int y = cell.PosY - 1; y <= cell.PosY + 1; y++)
                {
                    int _Y = y;

                    if (y < 0)
                        _Y += SizeY;

                    if (y >= SizeY)
                        _Y -= SizeY;

                    for (int x = cell.PosX - 1; x <= cell.PosX + 1; x++)
                    {
                        int _X = x;

                        if (x < 0)
                            _X += SizeY;

                        if (x >= SizeY)
                            _X -= SizeY;

                        int index = SizeX * _Y + _X;
                        neighbors.Add(Cells[index]);
                    }
                }

                cell.SetNeighbors(neighbors);
            }
        }

        public void Update()
        {
            foreach (var cell in Cells)
            {
                cell.Update();
            }

            foreach (var cell in Cells)
            {
                cell.Evolve();
            }
        }

        public void Clear()
        {
            foreach (var cell in Cells)
            {
                cell.Reset();
            }
        }

        public void Randomize()
        {
            Random rand = new Random();
            foreach (var cell in Cells)
            {
                cell.IsCurrentlyAlive = rand.Next(0, 10) < 5 ? false : true;
            }
        }

        public string Save()
        {
            // TODO Compression

            string output = string.Empty;

            output += string.Format("{0}\n{1}\n", SizeX, SizeY);

            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                    output += Cells[x + SizeX * y].IsCurrentlyAlive ? "1" : "0";
                }
                output += "\n";
            }

            Console.WriteLine(output);
            return output;
        }

        public void Load(string input)
        {
            // TODO Decompression

            string[] lines = input.Split('\n');

            SizeX = int.Parse(lines[0]);
            SizeY = int.Parse(lines[1]);

            Cells.Clear();
            Cells = new List<Cell>(SizeX * SizeY);
            for (int i = 0; i < SizeX * SizeY; i++)
            {
                int posX = i % SizeX;
                int posY = (i - posX) / SizeX;
                Cells.Add(new Cell(posX, posY));
            }
            SetupNeighborhood();
        }
    }
}
