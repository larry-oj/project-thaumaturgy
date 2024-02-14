extends Node
class_name State

signal Transitioned
var _character: Character
var _animation_tree: AnimationTree
var _input_component

func enter():
	pass


func exit():
	pass


func frame_update(_delta: float):
	pass


func physics_update(_delta: float):
	pass


func input_update(_event: InputEvent):
	pass


func get_movement_direction() -> Vector2:
	return Vector2.ZERO