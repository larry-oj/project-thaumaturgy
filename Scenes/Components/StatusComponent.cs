using Godot;
using System;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

public partial class StatusComponent : Node2D
{
	private Timer _timer;
	private Status _currentStatus;
	private float _damage = 0f;
	private int _tickAmount;

	[Export] private HealthComponent _healthComponent;
	[Export] private SpritesComponent _spritesComponent;
	
	public bool IsUnderStatus { get; private set; }
	public float Multiplier { get; private set; }
	
	[Signal] public delegate void StatusChangedEventHandler(bool isCleared, Status status);
	
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
	}
	
	public void TakeStatus(Status status)
	{
		if (IsUnderStatus && _currentStatus.Type != status.Type) return;
		
		IsUnderStatus = true;
		_currentStatus = status;
		_timer.WaitTime = status.TickPeriod;
		_damage = status.Damage;
		_tickAmount = status.TicksAmount;

		switch (status.Type)
		{
			case Status.StatusType.Burning:
				_spritesComponent.SetBurning();
				break;
			case Status.StatusType.Freezing:
				_spritesComponent.SetFreezing();
				break;
			case Status.StatusType.Stunned:
				_spritesComponent.SetStunned();
				break;
			case Status.StatusType.KnockedBack:
				break;
			default:
			case Status.StatusType.None:
				break;
		}
		
		_timer.Start();
		EmitSignal(nameof(StatusChanged), false, _currentStatus);
	}
	
	public void StopStatus()
	{
		GD.Print("!IsUnderStatus: " + !IsUnderStatus);
		if (!IsUnderStatus) return;
		IsUnderStatus = false;
		_timer.Stop();
		_spritesComponent.ResetStatus();
		_currentStatus.Free();
		EmitSignal(nameof(StatusChanged), true, default);
	}

	private void OnTickTimeout()
	{
		_healthComponent.TakeStatusDamage(_currentStatus.Damage);
		_currentStatus.TicksAmount--;

		if (_currentStatus.TicksAmount > 0) return;
		StopStatus();
	}
}
