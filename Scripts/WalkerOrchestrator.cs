using System.Collections.Generic;
using Godot;

namespace projectthaumaturgy.Scripts;

public partial class WalkerOrchestrator : Node
{
    public Level level;
    private Vector2I _currentPos;
    
    public WalkerOrchestrator(Level level, Vector2I position)
    {
        this.level = level;
        _currentPos = position;
    }
    
    public void Walk(float walkerRoomChance = 0.0f,
        int[] walkerRoomSize = null,
        float[] walkerTurnChance = null,
        int walkerMax = 1,
        float walkerChance = 0.0f)
    {
        var walkers = new List<Walker>();
        walkers.Add(new Walker(level, _currentPos, walkerTurnChance, walkerRoomChance, walkerRoomSize));
        while (level.walkableTiles.Count < level.size)
        {
            for (var i = 0; i < walkers.Count; i++)
            {
                walkers[i].Walk();

                if (i == 0)
                    _currentPos = walkers[i].position;
                
                if (GD.Randf() < walkerChance && walkers.Count < walkerMax)
                    walkers.Add(new Walker(level, _currentPos, walkerTurnChance, walkerRoomChance, walkerRoomSize, walkers[0].direction));
            }
        }
        
        foreach (var walker in walkers)
        {
            walker.QueueFree();
        }
        
        foreach (var tile in level.walkableTiles)
        {
            foreach (int i in new[] { -1, 0, 1 })
            {
                foreach (int j in new[] { -1, 0, 1 })
                {
                    var tmp = tile + new Vector2I(i, j);
                    if (!level.walkableTiles.Contains(tmp))
                    {
                        level.wallTiles.Add(tmp);
                    }
                }
            }
        }
    }
}