using Godot;

namespace projectthaumaturgy.Scripts;

public partial class HealthChange : GodotObject
{
    public float BeforeHealth { get; set; }
    public float AfterHealth { get; set; }
    public float Change => Mathf.Abs(BeforeHealth - AfterHealth);
    public bool IsHealing => BeforeHealth < AfterHealth;
}