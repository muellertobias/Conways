using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conway.Models
{
    public class Cell
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public bool Alive { get; set; }
        public bool AliveNextGeneration { get; set; }

        private List<Cell> neighbors;

        public Cell(int posX, int posY) 
        {
            PosX = posX;
            PosY = posY;
            Reset();
        }

        public void SetNeighbors(List<Cell> neighbors)
        {
            this.neighbors = neighbors;
        }

        private void Update(List<Cell> neighbors)
        {
            int count = 0;
            foreach (var cell in neighbors)
            {
                if (!cell.Equals(this))
                {
                    if (cell.Alive)
                        count++;
                }
            }

            if (Alive)
            {
                AliveNextGeneration = (count >= 2 && count <= 3) ? true : false;
            }
            else
            {
                AliveNextGeneration = count == 3 ? true : false;
            }
        }

        public void Update()
        {
            Update(neighbors);
        }

        public void NextGeneration()
        {
            Alive = AliveNextGeneration;
        }

        internal void Toogle()
        {
            Alive = !Alive;
        }

        internal void Reset()
        {
            Alive = false;
            AliveNextGeneration = false;
        }

        public override bool Equals(object obj)
        {
            if (obj != this)
                return false;

            var o = obj as Cell;

            return this.PosX == o.PosX && this.PosY == o.PosY;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
