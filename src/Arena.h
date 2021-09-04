#ifndef ARENA_H
#define ARENA_H
#include <iomanip>
#include <sstream>
#include <string>

#include <Godot.hpp>

#include <Spatial.hpp>
#include <Label.hpp>
#include <Navigation.hpp>
#include <AnimationPlayer.hpp>
#include <AudioStreamPlayer.hpp>
#include <AudioStreamSample.hpp>
#include <PackedScene.hpp>
#include <ResourceLoader.hpp>
#include <SceneTree.hpp>
#include <RandomNumberGenerator.hpp>
#include "Config.h"

namespace godot {

    class Arena : public Spatial
    {
        //GODOT STRUCTURE
        GODOT_CLASS(Arena, Spatial)

    public:
        Arena();
        ~Arena();
        static void _register_methods();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);
    private:
        void process_game(const double p_delta);
        void random_spawn_ground(Ref<PackedScene> item);
        void enemy_spawn();
        void item_spawn();
        void update_score(int delta);
        
    //CLASS STRUCUTRE
    private:
        
        bool debug_mode = false;
        uint8_t wave = 0, subwave = 0, max_subwaves = 3;
        int score = 0, randomItem = 0;
        float spawnRate = 20, time = 0;

        
        Ref<godot::RandomNumberGenerator> _random;
        Config* _config = nullptr;
        Label *_topText = nullptr, *_FPScounter = nullptr;
        Navigation* _nav = nullptr;
        AnimationPlayer* _dj = nullptr;
        AudioStreamPlayer *_maestro = nullptr, *_announcer = nullptr, *_crowd = nullptr;
        Ref<AudioStreamSample> _an_waveComplete = (Ref<AudioStreamSample>)ResourceLoader::get_singleton()->load("res://Sounds/announcer/countdown20sec.wav");
        Ref<AudioStreamSample> _an_go = (Ref<AudioStreamSample>)ResourceLoader::get_singleton()->load("res://Sounds/announcer/GO-1.wav");
        Ref<AudioStreamSample> _mus_level = (Ref<AudioStreamSample>)ResourceLoader::get_singleton()->load("res://Sounds/music/pathetique_weapons.wav");
        Ref<AudioStreamSample> _mus_boss = (Ref<AudioStreamSample>)ResourceLoader::get_singleton()->load("res://Sounds/music/pestered_archfiend.wav");
        //Array _enemyTiers{};
        Ref<PackedScene> enemy; //ONLY HERE FOR TESTING
        
        Ref<PackedScene> _enemyTier1[4];
        Ref<PackedScene> _enemyTier2[4];
        Ref<PackedScene> _bossTier[3];
        Ref<PackedScene> _itemBoxes[4];
    };
}

#endif
