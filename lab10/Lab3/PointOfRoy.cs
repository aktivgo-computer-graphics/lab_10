using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class PointOfRoy
    {
        private float speedX, speedY, x, y;

        public PointOfRoy(float x, float y) 
        {
            this.x = x;
            this.y = y;
            speedX = 0;
            speedY = 0;
        }

        public float SpeedX { get => speedX; set => speedX = value; }
        public float SpeedY { get => speedY; set => speedY = value; }
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
    }
}
