using AvaloniaRoguelike.Model;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaRoguelike.Services;

public sealed class AStarPathFindingService : IPathFindingService
{
    public IList<PathNode> FindPath(
        GameField gameField,
        CellLocation from,
        CellLocation to)
    {
        var closedSet = new HashSet<PathNode>();
        var openSet = new HashSet<PathNode>();

        var startNode = new PathNode(from)
        {
            PathLengthFromStart = 0,
            HeuristicEstimatePathLength = GetHeuristicPathLength(from, to)
        };

        var finishNode = new PathNode(to);

        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            var currentNode = openSet
                .OrderBy(node => node.EstimateFullPathLength)
                .First();

            if (currentNode.Position == to)
            {
                return GetPathForNode(currentNode);
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            var neighbourNodes = GetNeighbours(currentNode, finishNode, gameField);

            foreach (var neighbour in neighbourNodes)
            {
                if (closedSet.Count(node => node.Position == neighbour.Position) > 0)
                {
                    continue;
                }
                var openNode = openSet.FirstOrDefault(node => node.Position == neighbour.Position);

                if (openNode == null)
                {
                    openSet.Add(neighbour);
                }
                else if (openNode.PathLengthFromStart > neighbour.PathLengthFromStart)
                {
                    openNode.CameFrom = currentNode;
                    openNode.PathLengthFromStart = neighbour.PathLengthFromStart;
                }
            }
        }
        return null;
    }

    private static int GetHeuristicPathLength(
        CellLocation from,
        CellLocation to)
    {
        return Convert.ToInt32(Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y));
    }

    private static int GetHeuristicPathLength(
        PathNode from,
        PathNode to)
    {
        return GetHeuristicPathLength(from.Position, to.Position);
    }

    private static List<PathNode> GetNeighbours(
        PathNode from,
        PathNode to,
        GameField gameField)
    {
        var neighbourPathNodes = new List<PathNode>(4)
        {
            new(from.Position.WithXPlus(1), from),
            new(from.Position.WithXMinus(1), from),
            new(from.Position.WithYPlus(1), from),
            new(from.Position.WithYMinus(1), from)
        };

        foreach (var neighbour in neighbourPathNodes)
        {
            neighbour.PathLengthFromStart = from.PathLengthFromStart + GetDistanceBetweenNeighbours();
            neighbour.HeuristicEstimatePathLength = GetHeuristicPathLength(neighbour, to);
        }

        return neighbourPathNodes
            .Where(e => IsInMapBounds(e.Position, gameField))
            .Where(e => gameField[e.Position].IsPassable) // TODO: evma, isPassable?
            .ToList();
    }

    private static int GetDistanceBetweenNeighbours()
    {
        //можно переписать в зависимости от ландшафта
        return 1;
    }

    private static List<PathNode> GetPathForNode(PathNode pathNode)
    {
        var result = new List<PathNode>();
        var currentNode = pathNode;
        while (currentNode != null)
        {
            result.Add(currentNode);
            currentNode = currentNode.CameFrom;
        }
        result.Reverse();
        return result;
    }

    private static bool IsInMapBounds(CellLocation cell, GameField gameField)
    {
        if (cell.X < 0 || cell.X >= gameField.Width ||
            cell.Y < 0 || cell.Y >= gameField.Height)
            return false;
        return true;
    }
}
