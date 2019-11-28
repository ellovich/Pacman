﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public class Pinky : Ghost
    {
        public Pinky(Pacman pacman, Level level) : base()
        {
            _pacman = pacman;
            _level = level;

            SetSpeed(0.12f);
            SetSprite(GameResources.Pinky);
            ChangeMode(ScatterMode);
            LocationF = _home = new PointF(13.5f, 17);
            _destination = _movingMode();
            _prevLocation = new Point();
            _corner = new Point(6, 4);
            _corner2 = new Point(1, 4);
        }

        public override Point ChaseMode()
        {
            int dx = 0, dy = 0;

            switch (_pacman.CurrentDir)
            {
                case Pacman.Directions.up:
                    dx = 0; dy = -5;
                    break;
                case Pacman.Directions.right:
                    dx = 5; dy = 0;
                    break;
                case Pacman.Directions.down:
                    dx = 0; dy = 5;
                    break;
                case Pacman.Directions.left:
                    dx = -5; dy = 0;
                    break;
                case Pacman.Directions.nowhere:
                    dx = 0; dy = 0;
                    break;
            }

            for (int i = 0; i < 6; i++)
            {
                if (dx > 0) dx -= 1;
                if (dx < 0) dx += 1;
                                    
                if (dy > 0) dy -= 1;
                if (dy < 0) dy += 1;
                
                Point point = new Point(_pacman.Location.X + dx, _pacman.Location.Y + dy);
                
                Debug.WriteLine(_pacman.Location + " " + point);

                if (_level.IsWalkablePoint(point))
                {
                    _goal = point;
                    break;
                }
            }

            List<Point> path = _pathFinder.FindPath(_prevLocation, Location, _goal);
            return (path.Count == 1) ? Location : path[1];
        }
    } 
}