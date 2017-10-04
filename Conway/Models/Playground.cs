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
        public int SizeX { get; set; }
        public int SizeY { get; set; }

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
            Update();
        }

        public string Save()
        {
            string output = string.Format("{0}\n{1}\n", SizeX, SizeY);

            bool currentState = Cells.First().IsCurrentlyAlive;
            int counter = 0;

            for (int i = 0; i < Cells.Count; ++i)
            {
                if (currentState != Cells[i].IsCurrentlyAlive)
                {
                    output += string.Format("{0} {1}\n", counter, currentState);
                    counter = 1;
                    currentState = Cells[i].IsCurrentlyAlive;
                }
                else
                {
                    counter++;
                }
            }
            output += string.Format("{0} {1}\n", counter, currentState);
            //Console.WriteLine(output);
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

            int currentIndex = 0;
            for (int i = 2; i < lines.Length; i++)
            {
                string[] content = lines[i].Split(' ');
                if (content.Length == 2)
                {
                    int n = int.Parse(content.First());
                    bool state = bool.Parse(content.Last());

                    while (n > 0)
                    {
                        int posX = currentIndex % SizeX;
                        int posY = (currentIndex - posX) / SizeX;
                        Cells.Add(new Cell(posX, posY) { IsCurrentlyAlive = state });

                        currentIndex++;
                        n--;
                    }
                }
            }
            SetupNeighborhood();
        }
    }
}
