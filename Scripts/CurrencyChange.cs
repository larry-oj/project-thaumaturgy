using Godot;

namespace projectthaumaturgy.Scripts;

public partial class CurrencyChange : GodotObject
{
    public float Before { get; set; }
    public float After { get; set; }
    public float Change => Mathf.Abs(Before - After);
    public bool IsGaining => Before < After;
}