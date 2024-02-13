extends Node
class_name Walker

var level : Level						# level data reference
var turnChance : Array[float]			# chance of making a trun in degrees : [-90, 0, 90, 180]
var turnChanceFormatted : Array[float]	# chances formatted for _get_new_direction()
var roomChance : float					# chance of a room creation on each step
var roomSize : Array[int]				# size of a room that will be create on each step : [width, height]

@export var position : Vector2			# walker position
@export var direction : Vector2			# walker facing direction


func _init(levelParam : Level, 
	positionParam : Vector2, 
	turnChanceParam = [0.25, 0.25, 0.25, 0.25], 
	roomChanceParam : float = 0.0, 
	roomSizeParam : Array[int] = [0, 0],
	directionParam : Vector2 = Vector2.LEFT
):
	self.level = levelParam
	self.direction = directionParam
	self.position = positionParam
	self.turnChance = turnChanceParam
	self.roomChance = roomChanceParam
	self.roomSize = roomSizeParam
	
	self.turnChanceFormatted = [
		turnChance[0],
		turnChance[0] + turnChance[1],
		turnChance[0] + turnChance[1] + turnChance[2],
		turnChance[0] + turnChance[1] + turnChance[2], turnChance[3]
	]


func walk() -> void:
	self.direction = _get_new_direction()
	self.position += self.direction
	var walkedOn : Array[Vector2] = [] 
	
	if randf() < roomChance:
		for width in range(roomSize[0]):
			for height in range(roomSize[1]):
				var coordinate = self.position + Vector2(width, height)
				walkedOn.append(coordinate)
	else:
		walkedOn.append(self.position)
	
	for tile in walkedOn:
		if not self.level.walkableTiles.has(tile):
			self.level.walkableTiles.append(tile)




func _get_new_direction() -> Vector2:
	var chance = randf()
	var result : Vector2
	if chance <= self.turnChanceFormatted[0] and self.turnChance[0] != 0:
		result = self.direction.rotated(deg_to_rad(90))
	elif chance <= self.turnChanceFormatted[1] and self.turnChance[1] != 0:
		result = self.direction
	elif chance <= self.turnChanceFormatted[2] and self.turnChance[2] != 0:
		result = self.direction.rotated(deg_to_rad(-90))
	else:
		result = self.direction.rotated(deg_to_rad(180))
	
	# probably not a good way to do this
	return round(result)
