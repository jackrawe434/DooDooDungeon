﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DooDooDungeon
{
    public class Roll
    {
        public int x, y, size;
        public string direction;

        public Roll(int _rollX, int _rollY, int _rollSize, string _rollDirection)
        {
            x = _rollX;
            y = _rollY;
            size = _rollSize;
            direction = _rollDirection;
        }

        public void Move()
        { 
            if (direction == "Right")
            {
                x += 75;
            }
            else if (direction == "Left")
            {
                x -= 75;
            }
            else if (direction == "Up")
            {
                y -= 62;
            }
            else if (direction == "Down")
            {
                y += 62;
            }
        }

        public Boolean Collision(Rectangle r)
        {
            Rectangle powerupRec = new Rectangle(r.X, r.Y, r.Width, r.Height);
            Rectangle rollRec = new Rectangle(x, y, size, size);

            return (rollRec.IntersectsWith(powerupRec));
        }
    }
}
