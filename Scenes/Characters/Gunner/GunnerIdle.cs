using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;

namespace projectthaumaturgy.Scenes.Characters.Gunner;

public partial class GunnerIdle : State
{
	private Timer _intervalTimer;
	[Export] private float _wanderChance;
	[Export] private DetectorComponent _detectorComponent;
	[Export] private GunnerWander _gunnerWander;
	[Export] private GunnerAlert _gunnerAlert;

	public override void _Ready()
	{
		_intervalTimer = GetNode<Timer>("Timer");
	}

	public override void Enter()
	{
		_intervalTimer.Start();
		_detectorComponent.Detected += OnPlayerDetected;
	}
	
	public override void Exit()
	{
		_intervalTimer.Stop();
		_detectorComponent.Detected -= OnPlayerDetected;
	}

	public override void Process(double delta)
	{
		_detectorComponent.TryDetect();
	}
	
	private void OnTimerTimeout()
	{
		if (GD.Randf() < _wanderChance)
			EmitSignal(nameof(Transitioned), this, _gunnerWander);
		
	}
	
	private void OnPlayerDetected()
	{
		EmitSignal(nameof(Transitioned), this, _gunnerAlert);
	}
}