using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conway.Models
{
    using Alive = Boolean;

    public class Cell : INotifyStateChanged
    {
        public event StateChangedEventHandler StateChanged;

        public int PosX { get; set; }
        public int PosY { get; set; }

        public Alive IsCurrentlyAlive
        {
            get { return _isCurrentlyAlive; }
            set
            {
                if (_isCurrentlyAlive != value)
                {
                    _isCurrentlyAlive = value;
                    OnStateChanged();
                }
            }
        }

        private Alive _isNextGenerationAlive;
        private Alive _isCurrentlyAlive;
        private List<Cell> _neighbors;

        public Cell(int posX, int posY) 
        {
            PosX = posX;
            PosY = posY;
            Reset();
        }

        public void SetNeighbors(List<Cell> neighbors)
        {
            _neighbors = neighbors;
            _neighbors.Remove(this);
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

            _isNextGenerationAlive = ApplyRule(nlifes);
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
            Update(_neighbors);
        }

        public void Evolve()
        {
            if (_isNextGenerationAlive != IsCurrentlyAlive)
            {
                IsCurrentlyAlive = _isNextGenerationAlive;
            }
        }

        public void Toogle()
        {
            IsCurrentlyAlive = !IsCurrentlyAlive;
        }

        public void Reset()
        {
            IsCurrentlyAlive = false;
            _isNextGenerationAlive = false;
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
