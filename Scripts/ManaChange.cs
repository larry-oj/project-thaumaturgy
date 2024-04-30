using Godot;

namespace projectthaumaturgy.Scripts;

public partial class ManaChange : GodotObject
{
    public float BeforeMana { get; set; }
    public float AfterMana { get; set; }
    public float Change => Mathf.Abs(BeforeMana - AfterMana);
    public bool IsGaining => BeforeMana < AfterMana;
}