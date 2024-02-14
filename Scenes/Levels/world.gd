extends Node2D

@onready var tileMap = $TileMap
var player = preload("res://Scenes/Characters/player.tscn")


func _ready():
	randomize()
	generate_level()


func generate_level():
	var level = Level.new(150)
	var walkerOrch = WalkerOrchestrator.new(level, Vector2(0, 0))
	walkerOrch.walk(0.11, [3, 3], [0.33, 0.34, 0.33, 0], 3, 0.15)

	tileMap.set_cells_terrain_connect(0, level.walkableTiles, 0, 0)
	tileMap.set_cells_terrain_connect(0, level.wallTiles, 0, 1)

	var player_instance = player.instantiate()
	player_instance.position = to_global(tileMap.map_to_local(level.walkableTiles[0]))
	add_child(player_instance)

	# free memory
	level.queue_free()
	walkerOrch.queue_free()


func _input(event):
	if event.is_action_pressed("ui_accept"):
		get_tree().reload_current_scene()
