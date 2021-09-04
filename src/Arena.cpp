#include "Arena.h"
#include <string>
using namespace godot;

void Arena::_register_methods() {
    register_method("_process", &Arena::_process);
    register_method("_ready", &Arena::_ready);
	register_method("update_sore", &Arena::update_score);
	register_property("Debug Mode", &Arena::debug_mode, false);
    register_property("Enemy", &Arena::enemy, Ref<PackedScene>(nullptr), GODOT_METHOD_RPC_MODE_DISABLED, GODOT_PROPERTY_USAGE_DEFAULT, GODOT_PROPERTY_HINT_RESOURCE_TYPE);
    //register_property("EnemyTier1", &Arena::dirArray, Ref<PoolStringArray>(nullptr));
}

Arena::Arena() {
    /*
    for (int i = 0; i < sizeof(_bossTier); i++)
    {
        _bossTier[i] = nullptr;
    }*/
    
}

Arena::~Arena() {
    // add your cleanup here
}

void Arena::_init() {
    _config = nullptr;
	_topText = nullptr, _FPScounter = nullptr;
	_nav = nullptr;
	_dj = nullptr;
	_maestro = nullptr, _announcer = nullptr, _crowd = nullptr;
	_an_waveComplete = (Ref<AudioStreamSample>)ResourceLoader::get_singleton()->load("res://Sounds/announcer/countdown20sec.wav");
	_an_go = (Ref<AudioStreamSample>)ResourceLoader::get_singleton()->load("res://Sounds/announcer/GO-1.wav");
	_mus_level = (Ref<AudioStreamSample>)ResourceLoader::get_singleton()->load("res://Sounds/music/pathetique_weapons.wav");
	_mus_boss = (Ref<AudioStreamSample>)ResourceLoader::get_singleton()->load("res://Sounds/music/pestered_archfiend.wav");
}
void Arena::_ready() {
	debug_mode =true;
    _random = (Ref<RandomNumberGenerator>)RandomNumberGenerator::_new();
	_random->randomize();
	_config = get_node<Config>("/root/Config");
	_maestro = get_node<AudioStreamPlayer>("Maestro");
	_crowd = get_node<AudioStreamPlayer>("Crowd");
	_announcer = get_node<AudioStreamPlayer>("Announcer");
	_dj = get_node<AnimationPlayer>("DJ");
	_topText = get_node<Label>("TextureRect/TopText");
	_nav = get_node<Navigation>("Navigation");
	wave = _config->startWave;
	update_score(0);
}

void Arena::_process(const double p_delta) {
    if (!debug_mode) process_game(p_delta);
    
}

void Arena::process_game(const double p_delta) {
    if (subwave < max_subwaves)//note that the counter will go above 'maxsubwaves' by one before resetting
		{
			if (time >= spawnRate)
			{
				if (wave % 4 == 0 && wave > 1 && subwave == 0)
				{
					_maestro->set_stream(_mus_boss);
					_maestro->play();
					_dj->play("SongStart");
				}
				else if (subwave == 0 && wave > 1)
				{
					_announcer->set_stream(_an_go);
					_announcer->play();
					_maestro->set_stream(_mus_level);
					_maestro->play(21.474f);
					_dj->play("SongStart");
				}
				enemy_spawn();
				item_spawn();
				subwave++;
				update_score(0);
				time = 0;
			}
			else time += p_delta;
		}
		else if(!get_tree()->has_group("Enemies"))
		{
			_dj->play("SongStop");
			_announcer->set_stream(_an_waveComplete);
			_announcer->play();
			_crowd->play();
			score += wave * 100;
			wave++;
			subwave = 0;
			time = 0;
			update_score(0);
			item_spawn();
		}
    
}
void Arena::random_spawn_ground(Ref<PackedScene> item) {
    Spatial *newItem = Object::cast_to<Spatial>(item->instance());
	Vector3 randomPos = Vector3{(float)_random->randf_range(-28, 28), 2,(float)_random->randf_range(-28, 28)};
	add_child(newItem);
	newItem->set_translation(_nav->get_closest_point(randomPos));
}
void Arena::enemy_spawn() {
	if (wave % 4 == 0 && wave > 1 && subwave == 0)
		{
			randomItem = (int)_random->randi_range(0,std::size(_bossTier));
			_announcer->set_stream((Ref<AudioStreamSample>)ResourceLoader::get_singleton()->load(("res://Sounds/announcer/boss_" + std::to_string(randomItem) + ".wav").c_str()));
			//_announcer->set_stream((Ref<AudioStreamSample>)ResourceLoader::get_singleton()->load("res://Sounds/announcer/boss_1.wav"));
			_announcer->play();
			random_spawn_ground(_bossTier[randomItem]);
			item_spawn();
		}
		else
		{
			switch (wave)
			{
				case 1:
					for (uint8_t i = 0; i < 14 + (_config->difficulty * 2); i++)
					{
						randomItem = (int)_random->randi_range(0,std::size(_enemyTier1));
						random_spawn_ground(_enemyTier1[randomItem]);
					}
				break;
				default:
					for (uint8_t i = 0; i < 10 + (_config->difficulty * 2); i++)
					{
						randomItem = (int)_random->randi_range(0,std::size(_enemyTier1));
						random_spawn_ground(_enemyTier1[randomItem]);
					}
					for (uint8_t i = 0; i < 4 + (_config->difficulty); i++)
					{
						randomItem = (int)_random->randi_range(0,std::size(_enemyTier2));
						random_spawn_ground(_enemyTier2[randomItem]);
					}
				break;
			}
		}
}
void Arena::item_spawn() {
    for (uint8_t i = 0; i < 8 - (_config->difficulty * 2); i++)
		{
			//A note about RandRange:
			// the minimum value is INCLUSIVE 
			//and the max value is EXCLUSIVE
			randomItem = (int)_random->randi_range(0,std::size(_itemBoxes)); 
			random_spawn_ground(_itemBoxes[randomItem]);
		}
}
void Arena::update_score(int delta) {
    if (debug_mode)
    {
        _topText->set_text("DEBUG MODE");
    }
    else
    {
        score += delta;
		std::stringstream ss;
		//Apparently this is the most concise way to have a fixed number of leading 0s in your int-to-string. Gonna miss having C#'s more clean ToString() function.
		ss << std::setw(6) << std::setfill('0') << std::to_string(score);
        _topText->set_text(("WAVE: " + std::to_string(wave)+"."+std::to_string(subwave) +"        " + "SCORE: " + ss.str()).c_str());
    }
}
