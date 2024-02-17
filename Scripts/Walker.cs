using System.Linq;
using Godot;
using Godot.Collections;
using Vector2 = Godot.Vector2;

namespace projectthaumaturgy.Scripts;

public partial class Walker : Node
{
    private Level _level;
    private float[] _turnChance;
    private float[] _turnChanceFormatted;
    private float _roomChance;
    private int[] _roomSize;

    [Export] public Vector2I position;
    [Export] public Vector2 direction;

    public Walker(Level level,
        Vector2I position,
        float[] turnChance = null,
        float roomChance = 0.0f,
        int[] roomSize = null,
        Vector2 direction = default)
    {
        _level = level;
        this.position = position;
        _turnChance = turnChance ?? new [] {0.25f, 0.25f, 0.25f, 0.25f};
        _roomChance = roomChance;
        _roomSize = roomSize ?? new [] {0, 0};
        this.direction = direction == default ? Vector2.Left : direction;

        _turnChanceFormatted = new[]
        {
            _turnChance[0],
            _turnChance[0] + _turnChance[1],
            _turnChance[0] + _turnChance[1] + _turnChance[2],
            _turnChance[0] + _turnChance[1] + _turnChance[2] + _turnChance[3]
        };
    }

    public void Walk()
    {
        direction = GetNewDirection();
        position += (Vector2I)direction;
        var walkedOn = new Array<Vector2I>();

        if (GD.Randf() < _roomChance)
        {
            for (var i = 0; i < _roomSize[0]; i++)
                for (var j = 0; j < _roomSize[1]; j++)
                {
                    var coordinate = position + new Vector2I(i, j);
                    walkedOn.Add(coordinate);
                }
        }
        else
        {
            walkedOn.Add(position);
        }

        foreach (var tile in walkedOn)
        {
            if (!_level.walkableTiles.Contains(tile))
            {
                _level.walkableTiles.Add(tile);
            }
        }
    }

    private Vector2 GetNewDirection()
    {
        var chance = GD.Randf();
        Vector2 result;
        if (chance <= _turnChanceFormatted[0] && _turnChance[0] != 0)
        {
            result = direction.Rotated(Mathf.DegToRad(90));
        }
        else if (chance <= _turnChanceFormatted[1] && _turnChance[1] != 0)
        {
            result = direction;
        }
        else if (chance <= _turnChanceFormatted[2] && _turnChance[2] != 0)
        {
            result = direction.Rotated(Mathf.DegToRad(-90));
        }
        else
        {
            result = direction.Rotated(Mathf.DegToRad(-180));
        }

        // probably not a good way to do this
        return new Vector2(Mathf.Round(result.X), Mathf.Round(result.Y));
    }
}