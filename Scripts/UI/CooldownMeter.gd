extends Sprite3D
#ViewportTextures have always been highly bug-prone and problematic.
#Now in 4.0, setting a viewport texture in the editor, at least for me,
#EVEN IF YOU MAKE SURE THE SUBVIEWPORT IS LOADED FIRST
#results in an error message upon instantiation, and a garbled texture on exported builds.
#I am going to try setting the texture in script per a comment on this github issue
#to fix this problem I have been experiencing.
#https://github.com/godotengine/godot/issues/16067#issuecomment-1350397970
@export var sv : SubViewport
func _ready():
	texture = sv.get_texture();
	pass # Replace with function body.
