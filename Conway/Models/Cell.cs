﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conway.Models
{
    using Alive = Boolean;

    public class Cell : INotifyStateChanged
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public Alive IsCurrentlyAlive { get; set; }
        public Alive IsNextGenerationAlive { get; set; }

        private List<Cell> neighbors;

        public event StateChangedEventHandler StateChanged;

        public Cell(int posX, int posY) 
        {
            PosX = posX;
            PosY = posY;
            Reset();
        }

        public void SetNeighbors(List<Cell> neighbors)
        {
            this.neighbors = neighbors;
            this.neighbors.Remove(this);
        }

        private void Update(List<Cell> neighbors)
        {
            int nlifes = 0;

            foreach (var cell in neighbors)
            {
                if (cell.IsCurrentlyAlive)
                {
                    nlifes++;
                }
            }

            IsNextGenerationAlive = ApplyRule(nlifes);
        }

        public Alive ApplyRule(int nlifes)
        {
            if (IsCurrentlyAlive)
            {
                return (nlifes >= 2 && nlifes <= 3) ? true : false;
            }
            else
            {
                return nlifes == 3 ? true : false;
            }
        }

        public void Update()
        {
            Update(neighbors);
        }

        public void Evolve()
        {
            if (IsNextGenerationAlive != IsCurrentlyAlive)
            {
                IsCurrentlyAlive = IsNextGenerationAlive;
                OnStateChanged();
            }
        }

        public void Toogle()
        {
            IsCurrentlyAlive = !IsCurrentlyAlive;
            OnStateChanged();
        }

        public void Reset()
        {
            IsCurrentlyAlive = false;
            IsNextGenerationAlive = false;
            OnStateChanged();
        }

        public void OnStateChanged()
        {
            var handler = StateChanged;
            if (handler != null)
                StateChanged(this, new StateChangedEventArgs());
        }

        public override bool Equals(object obj)
        {
            if (obj != this)
                return false;

            var o = obj as Cell;

            return PosX == o.PosX && PosY == o.PosY;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
