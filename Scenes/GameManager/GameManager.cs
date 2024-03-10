using Godot;
using projectthaumaturgy.Scenes.Levels;

namespace projectthaumaturgy.Scenes.GameManager;

public partial class GameManager : Node
{
    private UI.UI _ui;
    private World _world;
    
    public override void _Ready()
    {
        _ui = GetNode<UI.UI>("../Game/UI");
        _world = GetNode<World>("../Game/World");
        
        _ui.world = _world;
    }
}