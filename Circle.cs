using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogfighter
{
    public struct Circle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool Contains(Vector2 point)
        {
            return ((point - Center).Length() <= Radius);
        }

        public bool Intersects(Circle other)
        {
            return ((other.Center - Center).Length() < (other.Radius + Radius));
        }

        public bool BoundaryX(GraphicsDeviceManager Graphics)
        {
            if (Center.X + Radius > Graphics.PreferredBackBufferWidth || Center.X - Radius < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BoundaryY(GraphicsDeviceManager Graphics)
        {
            if (Center.Y + Radius > Graphics.PreferredBackBufferHeight || Center.Y - Radius < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsOffscreen(GraphicsDeviceManager Graphics)
        {
           return (Center.X - Radius > Graphics.PreferredBackBufferWidth || Center.X + Radius < 0 || Center.Y - Radius > Graphics.PreferredBackBufferHeight || Center.Y + Radius < 0);
        }
    }
}
