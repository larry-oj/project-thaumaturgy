using Godot;
using System;
using Godot.Collections;
using projectthaumaturgy.Scenes.Characters;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

public partial class StatusComponent : Node2D
{
	private Timer _timer;
	private Status _currentStatus;
	private Area2D _spreaderHurtbox;
	private Array<Enemy> _nearbyEnemies = new();
		
	private float _damage = 0f;
	private int _tickAmount;

	[Export] private HealthComponent _healthComponent;
	[Export] private SpritesComponent _spritesComponent;
	[Export] private VelocityComponent _velocityComponent;
	
	[Export] public bool IsImmune { get; private set; }
	
	public bool IsUnderStatus { get; private set; }
	public float Multiplier { get; private set; }
	
	[Signal] public delegate void StatusChangedEventHandler(bool isCleared, Status status);
	
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		_spreaderHurtbox = GetNode<Area2D>("%SpreaderHurtbox");
		
		_spreaderHurtbox.AreaEntered += OnSpreaderEntered;
		_spreaderHurtbox.AreaExited += OnSpreaderExited;
	}
	
	public void TakeStatus(Status status)
	{
		if (IsImmune) return;
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
				_velocityComponent.Knockback(status.Direction, status.Multiplier);
				return;
			
			default:
			case Status.StatusType.None:
				break;
		}
		
		_timer.Start();
		EmitSignal(nameof(StatusChanged), false, _currentStatus);
	}
	
	public void TakeInfusion(Attack.AttackInfusion infusion, float attackDamage)
	{
		switch (infusion)
		{
			case Attack.AttackInfusion.Saturated:
				foreach (var enemy in _nearbyEnemies)
				{
					if (_currentStatus == null) return;
					enemy.GetNode<StatusComponent>("StatusComponent")
						.TakeStatus(_currentStatus.Copy());
				}
				break;
			
			case Attack.AttackInfusion.Bold:
				foreach (var enemy in _nearbyEnemies)
				{
					enemy.GetNode<HitboxComponent>("HitboxComponent")
						.Damage(attackDamage * Options.Balance.InfusionBoldDamageFraction);
				}
				break;
			
			default:
				return;
		}
	}
	
	public void StopStatus()
	{
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
	
	private void OnSpreaderEntered(Area2D area)
	{
		if (area.Owner is Enemy enemy && enemy != Owner)
		{
			_nearbyEnemies.Add(enemy);
		}
	}
	
	private void OnSpreaderExited(Area2D area)
	{
		if (area.Owner is Enemy enemy)
		{
			_nearbyEnemies.Remove(enemy);
		}
	}
}
