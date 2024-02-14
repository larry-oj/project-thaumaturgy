extends CharacterBody2D
class_name Character

@export var max_speed: int = 0


func _physics_process(_delta) -> void:
    move_and_slide()
