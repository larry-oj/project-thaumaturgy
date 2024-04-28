using System;
using System.Linq;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes;

public partial class GameManager : Node
{
    private PlayerData _playerData;

    public int Stage { get; private set; } = 1;
    public int Substage { get; private set; } = 1;

    [Export] private Array<LevelResource> _levelResources = new();
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
    public LevelResource InitialLevelResource => _levelResources[0];
    [Signal] public delegate void LevelLoadedEventHandler(LevelResource levelResource);

    public override void _Ready()
    {
        _playerData = GetChild<PlayerData>(0);

        CurrentLevelResource = _levelResources.Where(x => x.StageNum == Stage).FirstOrDefault();
    }

    public void NextLevel()
    {
        if (Substage < _currentLevelResource.MaxSubstagesNum)
        {
            Substage++;
            EmitSignal(nameof(LevelLoaded), _currentLevelResource);
        }
        else if (Stage != _levelResources.Last().StageNum)
        {
            Stage++;
            Substage = 1;
            CurrentLevelResource = _levelResources.First(x => x.StageNum == Stage);
        }
        else
        {
            Substage = 1;
            CurrentLevelResource = _levelResources.First();
        }

        GD.Print($"Stage: {Stage}, Substage: {Substage}");
    }
}
