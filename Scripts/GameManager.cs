using System;
using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy;

public partial class GameManager : Node
{
    private PlayerData _playerData;

    public int Stage { get; private set; } = 1;
    public int Substage { get; private set; } = 1;

    [Export] private Array<LevelResource> _levelResources = new Array<LevelResource>();
    private LevelResource _currentLevelResource;
    public LevelResource CurrentLevelResource 
    {
        get => _currentLevelResource;
        set
        {
            _currentLevelResource = value;
            EmitSignal(nameof(LevelLoaded), _currentLevelResource);
        }
    }
    [Signal] public delegate void LevelLoadedEventHandler(LevelResource levelResource);

    public bool IsRunStarted { get; private set; } = false;

    public override void _Ready()
    {
        _playerData = GetChild<PlayerData>(0);

        CurrentLevelResource = _levelResources.Where(x => x.StageNum == Stage).FirstOrDefault();
    }
}
