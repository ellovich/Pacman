﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Collections.ObjectModel;

namespace Pacman
{
    public class PathNode
    {
        public Point Position { get; set; }
        public float PathLengthFromStart { get; set; }// path lenght from start (G)
        public PathNode CameFrom { get; set; }// point from which we came to this point
        public float HeuristicEstimatePathLength { get; set; }// approximate estimate distance to goal (H)
        public float EstimateFullPathLength { get { return this.PathLengthFromStart + this.HeuristicEstimatePathLength; } }// expected distance to goal (F)
    }
    public class GhostPathFinder
    {
        string[] lines;
        int Height, Width;

        public GhostPathFinder(string level)
        {
            lines = Properties.Resources.PacmanMap.Split(new char[] { '\n' });
            //Width = lines[0].Length - 1;
            //Height = lines.Length - 1;
        }

        public Point FindPath(Point start, Point goal, Point previousLocation)
        {
            // step 1
            var closedSet = new Collection<PathNode>();
            var openSet = new Collection<PathNode>();
            // step 2
            PathNode startNode = new PathNode()
            {
                Position = start,
                CameFrom = null,
                PathLengthFromStart = 0,
                HeuristicEstimatePathLength = GetHeuristicPathLength(start, goal)
            };
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                // step 3
                var currentNode = openSet.OrderBy(node => node.EstimateFullPathLength).First();
                // step 4
                if (currentNode.Position == goal)
                    return GetPathForNode(currentNode);

                // step 5
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
                // step 6
                foreach (var neighbourNode in GetNeighbours(currentNode, goal, lines, previousLocation))
                {
                    // step 7
                    if (closedSet.Count(node => node.Position == neighbourNode.Position) > 0)
                        continue;
                    var openNode = openSet.FirstOrDefault(node => node.Position == neighbourNode.Position);
                    // step 8
                    if (openNode == null)
                        openSet.Add(neighbourNode);
                    else
                        if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
                        {
                            // step 9
                            openNode.CameFrom = currentNode;
                            openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                        }
                }
            }
            // step 10
            return new Point();
        }

        //approximate distance estimate function       
        private float GetHeuristicPathLength(Point from, Point to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }

        //getting list of neighbours for point
        private Collection<PathNode> GetNeighbours(PathNode pathNode, Point goal, string[] field, Point previousLocation)
        {
            var result = new Collection<PathNode>();

            Point[] neighbourPoints = new Point[4];
            neighbourPoints[0] = new Point(pathNode.Position.X, pathNode.Position.Y + 1);
            neighbourPoints[1] = new Point(pathNode.Position.X + 1, pathNode.Position.Y);
            neighbourPoints[2] = new Point(pathNode.Position.X, pathNode.Position.Y - 1);
            neighbourPoints[3] = new Point(pathNode.Position.X - 1, pathNode.Position.Y);

            foreach (var point in neighbourPoints)
            {         
                if (point == previousLocation)   //to prevent backtracking
                    continue;
                if ((point.X < 0) || (point.X >= field.Length))
                    continue;
                if ((point.Y < 0) || (point.Y >= field[0].Length))
                    continue;

                if ((field[point.X][point.Y] != '.') &&   //Добавить сюда проходы чтобы через них шли призраки
                    (field[point.X][point.Y] != ' ') &&
                    (field[point.X][point.Y] != 'p'))
                    continue;

                PathNode neighbourNode = new PathNode()
                {
                    Position = point,
                    CameFrom = pathNode,
                    PathLengthFromStart = pathNode.PathLengthFromStart + 1,
                    HeuristicEstimatePathLength = GetHeuristicPathLength(point, goal)
                };
                result.Add(neighbourNode);
            }
            return result;
        }

        //getting the path
        private Point GetPathForNode(PathNode pathNode)
        {
            List<Point> result = new List<Point>();

            var currentNode = pathNode;
            while (currentNode != null)
            {
                result.Add(currentNode.Position);
                currentNode = currentNode.CameFrom;
            }
            result.Reverse();

            if (result.Count > 1)           
                return result[1];           
            else
                return new Point(0, 0);
        }
    }
}

//Создается 2 списка вершин — ожидающие рассмотрения и уже рассмотренные.
//////В ожидающие добавляется точка старта, список рассмотренных пока пуст.
//////Для каждой точки рассчитывается F = G + H.G — расстояние от старта до точки,
//H — примерное расстояние от точки до цели.
//Так же каждая точка хранит ссылку на точку, из которой в нее пришли.
//Из списка точек на рассмотрение выбирается точка с наименьшим F. Обозначим ее X.
//Если X — цель, то мы нашли маршрут.
//Переносим X из списка ожидающих рассмотрения в список уже рассмотренных.
//Для каждой из точек, соседних для X (обозначим эту соседнюю точку Y), делаем следующее:
//////Если Y уже находится в рассмотренных — пропускаем ее.
//////Если Y еще нет в списке на ожидание — добавляем ее туда,
//////запомнив ссылку на X и рассчитав Y.G (это X.G + расстояние от X до Y) и Y.H.
//Если же Y в списке на рассмотрение — проверяем, если X.G + расстояние от X до Y<Y.G,
///////значит мы пришли в точку Y более коротким путем, заменяем Y.G на X.G + расстояние от X до Y, а точку,
///////из которой пришли в Y на X.
//Если список точек на рассмотрение пуст, а до цели мы так и не дошли — значит маршрут не существует.


// A* algorithm from https://lsreg.ru/realizaciya-algoritma-poiska-a-na-c/