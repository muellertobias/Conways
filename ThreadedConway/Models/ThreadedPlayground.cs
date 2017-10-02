using Conway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadedConway.Models
{
    class ThreadedPlayground
    {
        public List<ThreadedCell> Cells { get; private set; }
        private int SizeX;
        private int SizeY;
        public ThreadedPlayground(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;

            Cells = new List<ThreadedCell>(sizeX * sizeY);
            for (int i = 0; i < sizeX * sizeY; i++)
            {
                int posX = i % sizeX;
                int posY = (i - posX) / sizeX;
                Cells.Add(new ThreadedCell(posX, posY));
            }
        }

        internal void Update()
        {
            foreach (var cell in Cells)
            {
                List<Cell> neighbors = new List<Cell>();

                for (int y = cell.PosY - 1; y <= cell.PosY + 1; y++)
                {
                    if (y >= 0 && y < SizeY)
                    {
                        for (int x = cell.PosX - 1; x <= cell.PosX + 1; x++)
                        {
                            if (x >= 0 && x < SizeX)
                            {
                                int index = SizeX * y + x;
                                neighbors.Add(Cells[index]);
                            }
                        }
                    }
                }

                cell.Neighbors = neighbors;
            }

            foreach (var cell in Cells)
            {
                cell.Start();
            }

        }

        public void Clear()
        {
            foreach (var cell in Cells)
            {
                //cell.Reset();
            }
        }

        internal void Randomize()
        {
            Random rand = new Random();
            foreach (var cell in Cells)
            {
                cell.Alive = rand.Next(0, 10) < 5 ? false : true;
            }
        }
    }
}
