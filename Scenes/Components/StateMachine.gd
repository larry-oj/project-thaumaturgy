extends Node
class_name StateMachine

@export var initial_state : State

var current_state : State
var states : Dictionary = {}


func init(character: Character, animation_tree: AnimationTree, input_component = null):
	for child in get_children():
		if child is State:
			child.Transitioned.connect(on_child_transition)
			child._animation_tree = animation_tree
			child._character = character
			child._input_component = input_component
	
	current_state = initial_state

	animation_tree.active = true

	current_state.enter()


func frame_update(delta: float):
	if current_state:
		current_state.frame_update(delta)


func physics_update(delta: float):
	if current_state:
		current_state.physics_update(delta)


func input_update(event: InputEvent):
	if current_state:
		current_state.input_update(event)


# called when State.Transitioned is emitted
func on_child_transition(state, new_state : State):
	if state != current_state:
		return
	
	if !new_state:
		return
	
	if current_state:
		current_state.exit()
	
	new_state.enter()
	current_state = new_state
