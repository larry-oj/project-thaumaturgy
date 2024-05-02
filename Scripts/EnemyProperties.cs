using System;
using Godot;
using Godot.Collections;

namespace projectthaumaturgy;

public partial class EnemyProperties : GodotObject
{
    public Dictionary<PackedScene, float> Enemies { get; set; }
    public int MaxEnemies { get; set; }
}
