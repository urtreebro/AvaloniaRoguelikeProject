using AvaloniaRoguelike.Model;

using System.Collections.Generic;

namespace AvaloniaRoguelike.Services;

public interface IPathFindingService
{
    IList<PathNode> FindPath(Map map, CellLocation from, CellLocation to);
}