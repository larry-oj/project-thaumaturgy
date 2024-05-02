using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Levels;

namespace projectthaumaturgy.Scripts;

public partial class WalkerOrchestrator : Node
{
    public World World;
    private Vector2I _currentPos;

    private float _walkerRoomChance = 0.0f;
    private int[] _walkerRoomSize = new[] {0, 0};
    private float[] _walkerTurnChance = new[] {0.25f, 0.25f, 0.25f, 0.25f};
    private int _walkerMax = 1;
    private float _walkerChance = 0.0f;

    public WalkerOrchestrator(World world, Vector2I position)
    {
        this.World = world;
        _currentPos = position;
    }

    public WalkerOrchestrator AddRooms(int[] heightWidth, float walkerRoomChance)
    {
        return AddRooms(heightWidth[0], heightWidth[1], walkerRoomChance);
    }
    public WalkerOrchestrator AddRooms(int height, int width, float walkerRoomChance)
    {
        _walkerRoomSize = new[] {height, width};
        _walkerRoomChance = walkerRoomChance;
        return this;
    }

    public WalkerOrchestrator AddTurnChance(float[] walkerTurnChance)
    {
        return AddTurnChance(walkerTurnChance[0], walkerTurnChance[1], walkerTurnChance[2], walkerTurnChance[3]);
    }
    public WalkerOrchestrator AddTurnChance(float left, float forward, float right, float backward)
    {
        _walkerTurnChance = new[] {left, forward, right, backward};
        return this;
    }

    public WalkerOrchestrator AddMult(int walkerMax, float walkerChance)
    {
        _walkerChance = walkerChance;
        _walkerMax = walkerMax;
        return this;
    }

    public WalkerOrchestrator AddProperties(WalkerProperties properties)
    {
        this.AddRooms(properties.RoomSize, properties.RoomChance)
            .AddTurnChance(properties.TurnChance)
            .AddMult(properties.WalkerMax, properties.WalkerChance);
        
        //properties.Free(); // !!! // yeah, this is not needed

        return this;
    }
    
    public WalkerOrchestrator Walk()
    {
        var walkers = new Array<Walker>
        {
            new(World, _currentPos, _walkerTurnChance, _walkerRoomChance, _walkerRoomSize)
        };

        while (World.WalkableTiles.Count < World.Size)
        {
            for (var i = 0; i < walkers.Count; i++)
            {
                walkers[i].Walk();

                if (i == 0)
                    _currentPos = walkers[i].position;
                
                if (GD.Randf() < _walkerChance && walkers.Count < _walkerMax)
                    walkers.Add(new Walker(World, _currentPos, _walkerTurnChance, _walkerRoomChance, _walkerRoomSize, walkers[0].direction));
            }
        }
        
        foreach (var walker in walkers)
        {
            walker.Free();
        }
        
        foreach (var tile in World.WalkableTiles.Select(x => x.Position))
        {
            foreach (int i in new[] { -1, 0, 1 })
            {
                foreach (int j in new[] { -1, 0, 1 })
                {
                    var tmp = tile + new Vector2I(i, j);
                    if (!World.WalkableTiles.Select(x => x.Position).Contains(tmp))
                    {
                        World.WallTiles.Add(tmp);
                    }
                }
            }
        }

        return this;
    }
}