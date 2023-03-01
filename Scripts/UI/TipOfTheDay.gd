extends Label

#You cannot export an array of strings as multiline in C#.
#I am not smart enough to fix that myself and make a PR, 
#so instead the engine developers are getting a mildly annoyed code comment about the problem.
@export_multiline var tips : PackedStringArray
# Called when the node enters the scene tree for the first time.
func _ready():
	text = tips[randf_range(0, tips.size())]
	pass # Replace with function body.
