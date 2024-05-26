using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters;

public partial class AlertBase : State
{
	[ExportCategory("Customization")]
	[Export] public float BaseTimePeriod;
	[Export] public float CustomDamage;
	[Export] protected bool ForcePlayerInView = true;
	[Export] protected float LowerBoundary;
	
	[ExportGroup("Technical")]
	[Export] protected NavigationComponent NavigationComponent;
	[Export] protected DetectorComponent DetectorComponent;
	[Export] protected VelocityComponent VelocityComponent;
	[Export] protected StatusComponent StatusComponent;
	[Export] protected Node2D WeaponPivot;
	[Export] protected Weapon Weapon;
	[Export] protected Timer Timer;
	
	protected Enemy Self;
	protected Character Player;
	protected float DefaultDetectorRadius;
	protected bool IsPlayerDetected;
	
	public float TimerPeriod
	{
		get => (float)Timer.WaitTime;
		set
		{
			Weapon.StatsComponent.SetFireRate(1 / value);
			Timer.WaitTime = value;
		}
	}
	protected float DistanceToPlayer => Self.GlobalPosition.DistanceTo(Player.GlobalPosition);
	protected virtual bool IsWithinBoundary => DistanceToPlayer >= LowerBoundary;
	
	public override void _Ready()
	{
		Self = GetNode<Enemy>(Options.PathOptions.CharacterStateToCharacter);
		Player = Self.BodyToDetect as Character;
		DefaultDetectorRadius = DetectorComponent.detectionRange;
		
		Timer.Timeout += OnAttackTimerTimeout;
	}
	
	public override void Enter()
	{
		NavigationComponent.NavigationTimer.Timeout += OnNavigationTimerTimeout;
		StatusComponent.StatusChanged += OnStatusChanged;
		_animationPlayer.AnimationFinished += OnAnimationFinished;
		NavigationComponent.NavigationTimer.Start();
		DetectorComponent.detectionRange = -1f;	// disable range limitation
		TimerPeriod = BaseTimePeriod;
		Timer.Start();
		Weapon.StatsComponent.SetDamage(CustomDamage);
	}
	
	public override void Exit()
	{
		NavigationComponent.NavigationTimer.Timeout -= OnNavigationTimerTimeout;
		StatusComponent.StatusChanged -= OnStatusChanged;
		_animationPlayer.AnimationFinished -= OnAnimationFinished;
		NavigationComponent.NavigationTimer.Stop();
		WeaponPivot.Rotation = 0;
		DetectorComponent.detectionRange = DefaultDetectorRadius;
		Timer.Stop();
	}
	
	public override void Process(double delta)
	{
		RotateWeaponToMouse();
		IsPlayerDetected = DetectorComponent.TryDetect();
	}
	
	protected virtual void RotateWeaponToMouse()
	{
		var angle = DetectorComponent.VectorToDetectedBody;
		WeaponPivot.Rotation = angle.Angle();
		
		var weaponScale = WeaponPivot.Scale;
		weaponScale.Y = angle.X < 0 ? -1 : 1;
		WeaponPivot.Scale = weaponScale;
	}
	
	protected virtual void OnAttackTimerTimeout()
	{
		if (!NavigationComponent.IsNavigationFinished() && DistanceToPlayer < LowerBoundary) return;
		if (!IsPlayerDetected) return;
		
		Weapon.Attack();
	}

	protected virtual void OnNavigationTimerTimeout()
	{
	}

	protected virtual void OnStatusChanged(bool isCleared, Status change)
	{
	}

	protected virtual void OnAnimationFinished(StringName animationName)
	{
		if (animationName == Options.AnimationNames.Hurt)
		{
			_animationPlayer.Play(Options.AnimationNames.Idle);
		}
	}
	
	protected virtual void OnDamageTaken(GodotObject _)
	{
		_animationPlayer.Play(Options.AnimationNames.Hurt);
	}
	
	protected virtual void OnHealthDepleted()
	{
	}
}