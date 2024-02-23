using Godot;

namespace projectthaumaturgy.Scenes.Components;

public partial class AiInputComponent : InputComponent
{
	[Export] private AiBehaviour _aiBehaviour;

	public override Vector2 MovementDirection => _aiBehaviour.GetMovementDirection();
	public override bool IsAttacking => _aiBehaviour.IsAttacking();

	public override void _Ready()
	{
		// TODO: set _aiBehaviour dynamically
	}
}