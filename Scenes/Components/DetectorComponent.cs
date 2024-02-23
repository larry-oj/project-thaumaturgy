using Godot;
using Godot.Collections;

namespace projectthaumaturgy.Scenes.Components;

public partial class DetectorComponent : RayCast2D
{
	private Character _character;
	private Timer _intervalTimer;
	[Export] private float _detectionInterval;
	[Export] public float detectionRange;

	public CharacterBody2D BodyToDetect;
	public Vector2 VectorToDetectedBody => BodyToDetect.GlobalPosition - _character.GlobalPosition;

	[Signal] public delegate void DetectedEventHandler();

	public override void _Ready()
	{
		BodyToDetect = GetParent<Enemy>().BodyToDetect;
		_intervalTimer = GetNode<Timer>("IntervalTimer");
		_character = GetParent<Character>();

		_intervalTimer.WaitTime = _detectionInterval;
	}

	public void StartDetection()
	{
		_intervalTimer.Start();
	}
	
	public void StopDetection()
	{
		_intervalTimer.Stop();
	}

	public override void _Draw()
	{
		var from = ToLocal(_character.GlobalPosition);
		var to = ToLocal(BodyToDetect.GlobalPosition);
		var target = detectionRange > 0 ? from.DirectionTo(to) * detectionRange : to;
		DrawLine(from, target, Colors.Red, 0.5f);
	}
	
	public bool TryDetect()
	{
		QueueRedraw();
		
		var from = ToLocal(_character.GlobalPosition);
		var to = ToLocal(BodyToDetect.GlobalPosition);

		// if detectionRange is <= 0, then the target position is the target
		TargetPosition = detectionRange > 0 ? from.DirectionTo(to) * detectionRange : to;
		
		var collider = GetCollider();

		if (collider is Area2D area2D)
		{
			if (area2D.Owner != BodyToDetect) return false;
		}
		else if (collider != BodyToDetect)
		{
			return false;
		}

		EmitSignal(nameof(Detected));
		return true;
	}

	private void OnTimerTimeout()
	{
		TryDetect();
	}
}