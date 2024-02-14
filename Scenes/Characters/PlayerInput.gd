extends Node2D

func get_facing_direction() -> int:
	var mouse_pos = get_global_mouse_position()
	var self_pos = get_global_position()
	var dir = (mouse_pos - self_pos)
	if dir.x < 0:
		return -1
	else:
		return 1


func get_input_direction() -> Vector2:
	var dir = Vector2.ZERO
	dir.x = Input.get_axis("ui_left", "ui_right")
	dir.y = Input.get_axis("ui_up", "ui_down")
	return dir.normalized()
