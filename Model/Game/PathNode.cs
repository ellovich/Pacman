﻿namespace Pacman
{ 
    public class PathNode
    {
        public Point Position { get; set; }
        public float PathLengthFromStart { get; set; }
        public PathNode CameFrom { get; set; }
        public float HeuristicEstimatePathLength { get; set; }
        public float EstimateFullPathLength
            => PathLengthFromStart + HeuristicEstimatePathLength; 
    }
}