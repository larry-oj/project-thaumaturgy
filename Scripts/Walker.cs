using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scenes.Levels;
using Vector2 = Godot.Vector2;

namespace projectthaumaturgy.Scripts;

/// <summary>
/// A walker instance that walks around the level and creates a path.<br/>
/// <b>Must be <see cref="GodotObject.Free"/>'d</b> after use.
/// </summary>
public partial class Walker : GodotObject
{
    private Level _level;
    private float[] _turnChance;
    private float[] _turnChanceFormatted;
    private float _roomChance;
    private int[] _roomSize;

    [Export] public Vector2I position;
    [Export] public Vector2 direction;

    /// <summary>
    /// Initializes a new walker.
    /// </summary>
    /// <param name="level">A <see cref="Level"/> reference</param>
    /// <param name="position">A relative position on the map</param>
    /// <param name="turnChance">(optional) An array of turn chances for each direction<br/>
    /// <i>[left, forward, right, backward]</i></param>
    /// <param name="roomChance">(optional) A chance that a step will create a room</param>
    /// <param name="roomSize">(optional) An array of width and height of a room<br/>
    /// <i>[width, height]</i></param>
    /// <param name="direction">(optional) A starting facing direction</param>
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

    /// <summary>
    /// Walk 1 step
    /// </summary>
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
            if (!_level.walkableTiles.Select(x => x.Position).Contains(tile))
            {
                _level.walkableTiles.Add(new WalkableTile(tile));
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