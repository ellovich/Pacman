using System;
using System.Drawing;

namespace Pacman
{
    public abstract class Character
    {
        public PointF Home { get; protected set; }
        protected Sprite Sprite { get; set; }
        public float Speed { get; protected set; }
        public PointF Location { get; protected set; }

        protected void Move(int dx, int dy)
        {
            float newX = Location.X + dx * Speed;
            float newY = Location.Y + dy * Speed;
            Location = new PointF(newX, newY);

            Sprite.MoveSprite(dx, dy, Speed);
        }

        protected PointF Destination;

        public bool _isEaten;
        
        public void SetSprite(Bitmap image) 
            => Sprite.SetSprite(image);
        
        public void SetSpeed(float speed) 
            => Speed = speed;

        public abstract void Update();

        public void Draw(Graphics gr)
        {
            var x = (int)(Location.X * Game.TileSize.Width  - 6);
            var y = (int)(Location.Y * Game.TileSize.Height - 6);
            Sprite.Draw(gr, new System.Drawing.Point(x, y));
        }
    }
}