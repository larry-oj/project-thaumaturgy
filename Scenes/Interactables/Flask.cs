using Godot;
using projectthaumaturgy.Extensions;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Levels;
using projectthaumaturgy.Scenes.Pickups;
using projectthaumaturgy.Scenes.Weapons;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Interactables;

public partial class Flask : Interactable
{
	private Sprite2D _closedOutline;
	private Sprite2D _closedColor;
	private Sprite2D _openOutline;
	private Sprite2D _openColor;
	
	private Pickup.PickupType _type;
	
	[Export] public PackedScene PickupScene { get; set; }
	
	public override void _Ready()
	{
		base._Ready();
		_closedOutline = GetNode<Sprite2D>("ClosedOutline");
		_closedColor = GetNode<Sprite2D>("ClosedColor");
		_openOutline = GetNode<Sprite2D>("OpenOutline");
		_openColor = GetNode<Sprite2D>("OpenColor");
		
		_type = (Pickup.PickupType)GD.RandRange(0, 1);
		
		var color = _type switch
		{
			Pickup.PickupType.Health => Colors.Red,
			Pickup.PickupType.Mana => Colors.Blue,
			_ => Colors.Gold,
		};
		_closedColor.SelfModulate = color;
		_openColor.SelfModulate = color;
	}

	internal override void OnInteracted()
	{
		if (_isUsed) return;
        
		_closedOutline.Visible = false;
		_closedColor.Visible = false;
		_openOutline.Visible = true;
		_openColor.Visible = true;

		var pickupsAmount = _type switch
		{
			Pickup.PickupType.Health => GD.RandRange(2, 3),
			Pickup.PickupType.Mana => 4,
			_ => 1,
		};

		for (var i = 0; i < pickupsAmount; i++)
		{
			var pickup = PickupScene.Instantiate() as Pickup;
			pickup!.Type = _type;
			pickup.Position = this.Position.Copy();
			_level.CallDeferred(Node.MethodName.AddChild, pickup);
		}
        
		_isUsed = true;
	}
}