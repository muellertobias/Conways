using Conway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadedConway.Models
{
    public class ThreadedCell : Cell
    {
        public bool Alive
        {
            get
            {
                lock (this)
                {
                    return base.Alive;
                }
            }
            set
            {
                lock (this)
                {
                    base.Alive = value;
                }
            }
        }

        public List<Cell> Neighbors { get; set; }
        private Thread _thread;

        public ThreadedCell(int posX, int posY) 
            : base(posX, posY)
        {
            ThreadStart t = new ThreadStart(() => update());
            _thread = new Thread(t);
        }

        public void Start()
        {
            _thread.Start();
        }

        private void update()
        {
            while (_thread.IsAlive)
            {
                Update(Neighbors);
                Thread.Sleep(1000);
            }
        }
    }
}
