	using Godot;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scenes.Weapons;
	using projectthaumaturgy.Scripts;

	namespace projectthaumaturgy.Scenes.Characters.Gunner;

public partial class GunnerAlert : State
{
	[Export] private float _upperBoundary;
	[Export] private float _lowerBoundary;
	[Export] private NavigationComponent _navigationComponent;
	[Export] private DetectorComponent _detectorComponent;
	[Export] private VelocityComponent _velocityComponent;
	[Export] private GunnerIdle _gunnerIdle;
	[Export] private GunnerDead _gunnerDead;
	[Export] private Node2D _weaponPivot;
	[Export] private Weapon _weapon;
	
	private Gunner _gunner;
	private CharacterBody2D _player;
	private Timer _timer;
	private float _defaultDetectorRadius;
	private bool _isPlayerDetected;
	
	private float DistanceToPlayer => _gunner.GlobalPosition.DistanceTo(_player.GlobalPosition);
	public bool IsWithinBoundary => DistanceToPlayer <= _upperBoundary && DistanceToPlayer >= _lowerBoundary;

	public override void _Ready()
	{
		_gunner = GetNode<Gunner>(Options.PathOptions.CharacterStateToCharacter);
		_player = _gunner.BodyToDetect;
		_timer = GetNode<Timer>("Timer");
		_defaultDetectorRadius = _detectorComponent.detectionRange;
		
		_timer.Timeout += OnAttackTimerTimeout;
	}
	
	public override void Enter()
	{
		_navigationComponent.NavigationTimer.Timeout += OnNavigationTimerTimeout;
		_navigationComponent.NavigationTimer.Start();
		_detectorComponent.detectionRange = -1f;	// disable range limitation
		_timer.Start();
	}
	
	public override void Exit()
	{
		_navigationComponent.NavigationTimer.Timeout -= OnNavigationTimerTimeout;
		_navigationComponent.NavigationTimer.Stop();
		_weaponPivot.Rotation = 0;
		_detectorComponent.detectionRange = _defaultDetectorRadius;
		_timer.Stop();
	}

	public override void Process(double delta)
	{
		RotateWeaponToMouse();
		_isPlayerDetected = _detectorComponent.TryDetect();
	}
	
	public override void PhysicsProcess(double delta)
	{
		if (_navigationComponent.IsNavigationFinished())
			return;

		_velocityComponent.Move(_gunner.ToLocal(_navigationComponent.GetNextPathPosition()).Normalized());
	}
	
	private void OnNavigationTimerTimeout()
	{
		if (IsWithinBoundary)
			return;
		
		var boundary = DistanceToPlayer > _upperBoundary ? _upperBoundary : _lowerBoundary;
		
		_navigationComponent.TargetPosition = CalculateTargetPosition(boundary);
	}
	
	private void OnAttackTimerTimeout()
	{
		if (!_navigationComponent.IsNavigationFinished()) return;
		if (!_isPlayerDetected) return;
		
		_weapon.Attack();
	}

	private Vector2 CalculateTargetPosition(float boundary)
	{
		var distance = DistanceToPlayer;
		var percent = (distance - boundary) / distance;
		return _gunner.GlobalPosition.Lerp(_player.GlobalPosition, percent);
	}

	private void RotateWeaponToMouse()
	{
		var angle = _detectorComponent.VectorToDetectedBody;
		_weaponPivot.Rotation = angle.Angle();
		
		var weaponScale = _weaponPivot.Scale;
		weaponScale.Y = angle.X < 0 ? -1 : 1;
		_weaponPivot.Scale = weaponScale;
	}

	private void OnHealthDepleted()
	{
		EmitSignal(nameof(Transitioned), this, _gunnerDead);
	}
}