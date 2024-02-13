extends Node
class_name WalkerOrchestrator

# level data reference
var level : Level
# curren root walker position
var currentPos : Vector2


func _init(levelParam : Level, startingPos : Vector2):
	self.level = levelParam
	self.currentPos = startingPos


func walk(
	# chance of a room creation on each step
	walkerRoomChance : float = 0.0,
	# size of a room that will be create on each step : [width, height]
	walkerRoomSize : Array[int] = [0, 0],
	# chance of making a trun in degrees : [-90, 0, 90, 180]
	walkerTurnChance : Array[float] = [0.25, 0.25, 0.25, 0.25],
	# maximum walker amount
	walkerMax : int = 1,
	# chance of walker creation on every step
	walkerChance : float = 0.0
) -> void:
	var walkers : Array[Walker] = []
	walkers.append(Walker.new(self.level, self.currentPos, walkerTurnChance, walkerRoomChance, walkerRoomSize))
	
	while self.level.walkableTiles.size() < self.level.size:
		for i in range(walkers.size()):
			walkers[i].walk()
			
			if i == 0:
				self.currentPos = walkers[i].position
			
			if randf() < walkerChance and walkers.size() < walkerMax:
				walkers.append(Walker.new(self.level, self.currentPos, walkerTurnChance, walkerRoomChance, walkerRoomSize, walkers[0].direction))
	
	# free memory
	for walker in walkers:
		walker.queue_free()
	
	# find walls
	for tile in self.level.walkableTiles:
		for i in [-1, 0, 1]:
			for j in [-1, 0, 1]:
				var tmp = tile + Vector2(i, j)
				if not self.level.walkableTiles.has(tmp):
					self.level.wallTiles.append(tmp)
