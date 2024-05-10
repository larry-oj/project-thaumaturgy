using Godot;

namespace projectthaumaturgy.Scripts;

public partial class Status : GodotObject
{
    public enum StatusType
    {
        Burning,
        Freezing,
        Stunned,
        KnockedBack,
        None,
    }
    
    public StatusType Type { get; set; }
    public float TickPeriod { get; set; } = 1f;
    public int TicksAmount { get; set; }
    public float Damage { get; set; }
    public float Multiplier { get; set; } = 1.0f;
    public Vector2 Direction { get; set; }
    
    public Status() {}
    public Status(StatusType type, float tickPeriod, int ticksAmount, float damage, float multiplier = 1.0f, Vector2 direction = default)
    {
        Type = type;
        TickPeriod = tickPeriod;
        TicksAmount = ticksAmount;
        Damage = damage;
        Multiplier = multiplier;
        Direction = direction;
    }
    
    public Status Copy()
    {
        return new Status(Type, TickPeriod, TicksAmount, Damage, Multiplier, Direction);
    }
}