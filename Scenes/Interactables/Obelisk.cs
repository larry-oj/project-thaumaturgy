using Godot;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Pickups;

namespace projectthaumaturgy.Scenes.Interactables;

public partial class Obelisk : Interactable
{
	private Sprite2D _closed;
	private Sprite2D _open;
	
	[Export] public PackedScene PickupScene { get; set; }
	
	public override void _Ready()
	{
		base._Ready();
		_closed = GetNode<Sprite2D>("Closed");
		_open = GetNode<Sprite2D>("Open");
	}

	internal override void OnInteracted()
	{
		if (_isUsed) return;
        
		_closed.Visible = false;
		_open.Visible = true;

		var pickupsAmount = GD.RandRange(10, 15);

		for (var i = 0; i < pickupsAmount; i++)
		{
			var pickup = PickupScene.Instantiate() as Pickup;
			pickup!.Type = Pickup.PickupType.Currency;
			pickup.Position = this.Position.Copy();
			_level.CallDeferred(Node.MethodName.AddChild, pickup);
		}
        
		_isUsed = true;
	}
}