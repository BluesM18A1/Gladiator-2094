#include "Arena.h"
#include "TitleScreen.h"
#include "PauseScreen.h"
#include "Combatant.h"
#include "Gun.h"
#include "Config.h"
using namespace godot;

extern "C" void GDN_EXPORT godot_gdnative_init(godot_gdnative_init_options * o) {
	Godot::gdnative_init(o);
}

extern "C" void GDN_EXPORT godot_gdnative_terminate(godot_gdnative_terminate_options * o) {
	Godot::gdnative_terminate(o);
}

extern "C" void GDN_EXPORT godot_nativescript_init(void* handle) {
	Godot::nativescript_init(handle);
	//MANAGERS
	register_class<Config>();
	register_class<Arena>();
	//UI
	register_class<TitleScreen>();
	register_class<PauseScreen>();

	//COMBATANTS
	register_class<Combatant>();
	register_class<Player>();
	
	//WEAPONS
	register_class<Gun>();
	register_class<PlayerGun>();
	register_class<EnemyGun>();
}