extends State
class_name PlayerRunning

@export var idle_state : State


func enter():
	_animation_tree.set("parameters/conditions/is_running", true)


func exit():
	_animation_tree.set("parameters/conditions/is_running", false)


func frame_update(_delta: float):
	if get_movement_direction() == Vector2.ZERO:
		Transitioned.emit(self, idle_state)
		return


func physics_update(_delta: float):
	_character.velocity = get_movement_direction() * _character.max_speed
	super.physics_update(_delta)


func get_movement_direction() -> Vector2:
	return _input_component.get_input_direction()
