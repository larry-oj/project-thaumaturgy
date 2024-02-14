extends Character
class_name Player

@export var state_machine : StateMachine
@export var animation_tree : AnimationTree
@export var player_input : Node2D
@export var animation_sprites : CanvasGroup

func _ready():
	state_machine.init(self, animation_tree, player_input)


func _process(delta: float):
	state_machine.frame_update(delta)
	animation_sprites.scale.x = player_input.get_facing_direction()


func _physics_process(delta: float):
	super._physics_process(delta)
	state_machine.physics_update(delta)


func _unhandled_input(event: InputEvent) -> void:
	state_machine.input_update(event)
