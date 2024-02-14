extends State
class_name PlayerIdle

@export var running_state: State


func enter():
	_animation_tree.set("parameters/conditions/is_idle", true)


func exit():
	_animation_tree.set("parameters/conditions/is_idle", false)


func input_update(_event: InputEvent):
	if get_movement_direction() != Vector2.ZERO:
		Transitioned.emit(self, running_state)


func get_movement_direction() -> Vector2:
	return _input_component.get_input_direction()