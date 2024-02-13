extends Node2D

@onready var tileMap = $TileMap


func _ready():
	randomize()
	generate_level()


func generate_level():
	var level = Level.new(150)
	var walkerOrch = WalkerOrchestrator.new(level, Vector2(0, 0))
	walkerOrch.walk(0.11, [3, 3], [0.33, 0.34, 0.33, 0], 3, 0.15)
	
	tileMap.set_cells_terrain_connect(0, level.walkableTiles, 0, 0)
	tileMap.set_cells_terrain_connect(0, level.wallTiles, 0, 1)
	
	# free memory
	level.queue_free()
	walkerOrch.queue_free()

func _input(event):
	if event.is_action_pressed("ui_accept"):
		get_tree().reload_current_scene()
