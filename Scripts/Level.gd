extends Node
class_name Level

# size of the level in walkable tiles
@export var size : int 
# coordinates of all walkable tiles
@export var walkableTiles : Array[Vector2] = []
@export var wallTiles : Array[Vector2] = []


func _init(size2):
	self.size = size2

