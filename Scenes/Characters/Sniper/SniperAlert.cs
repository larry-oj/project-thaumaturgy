using Godot;
using projectthaumaturgy.Scenes.Components.StateMachine;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Characters.Sniper;

public partial class SniperAlert : AlertBase<Sniper>
{
	[ExportCategory("Sniper")]
	[Export] private SniperStunned _sniperStunned;
	[Export] private SniperDead _sniperDead;
	[Export] private Timer _crutch;
	
	private Line2D Laser { get; set; }
	private RayCast2D Ray { get; set; }

	private bool IsReady => _crutch.IsStopped();
		
	public override void Enter()
	{
		base.Enter();
		
		var sniper = (Weapons.Sniper.Sniper)Weapon;
		Laser = sniper.Laser;
		Ray = sniper.Ray;
		Laser.Visible = true;
		
		_crutch.Start();
	}

	public override void Exit()
	{
		base.Exit();
		
		Laser.Visible = false;
		
		_crutch.Stop();
	}

	public override void Process(double delta)
	{
		base.Process(delta);
		
		if (!IsReady) return;
		
		Ray.TargetPosition = Ray.ToLocal(Self.BodyToDetect.GlobalPosition);
		var collider = Ray.GetCollisionPoint();
		Laser.RemovePoint(1);
		Laser.AddPoint(Laser.ToLocal(collider));
	}

	public override void PhysicsProcess(double delta)
	{
		if (VelocityComponent.IsInKnockback) 
			VelocityComponent.Move(Vector2.Zero, delta);

		if (NavigationComponent.IsNavigationFinished())
		{
			if (_animationPlayer.CurrentAnimation != Options.AnimationNames.Hurt)
				_animationPlayer.Play(Options.AnimationNames.Idle);
			return;
		}

		if (_animationPlayer.CurrentAnimation != Options.AnimationNames.Hurt && _animationPlayer.CurrentAnimation != Options.AnimationNames.Run)
		{
			_animationPlayer.Play(Options.AnimationNames.Run);
		}
		VelocityComponent.Move(Self.ToLocal(NavigationComponent.GetNextPathPosition()).Normalized());
	}
	
	protected override void OnNavigationTimerTimeout()
	{
		if (IsWithinBoundary)
			return;
		
		NavigationComponent.TargetPosition = CalculateTargetPosition(LowerBoundary);
	}
	
	private Vector2 CalculateTargetPosition(float boundary)
	{
		var distance = DistanceToPlayer;
		var percent = (distance - boundary) / distance;
		return Self.GlobalPosition.Lerp(Player.GlobalPosition, percent);
	}
	
	protected override void OnHealthDepleted()
	{
		EmitSignal(State.SignalName.Transitioned, this, _sniperDead);
	}
}