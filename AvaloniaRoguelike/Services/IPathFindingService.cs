using AvaloniaRoguelike.Model;

using System.Collections.Generic;

namespace AvaloniaRoguelike.Services;

public interface IPathFindingService
{
    IList<PathNode> FindPath(GameField gameField, CellLocation from, CellLocation to);
}