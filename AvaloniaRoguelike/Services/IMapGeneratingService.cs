using AvaloniaRoguelike.Model;

namespace AvaloniaRoguelike.Services;

public interface IMapGeneratingService
{
    void InitializeMap(GameField gameField);
    void CreateRooms(GameField gameField);
    void MakeRoom(GameField gameField, Room room);
    void CreateWalls(GameField gameField);
    void MakeHorizontalTunnel(GameField gameField, int xStart, int xEnd, int yPosition);
    void MakeVerticalTunnel(GameField gameField, int yStart, int yEnd, int xPosition);
}
