#ifndef ARENA_H
#define ARENA_H

#include <Godot.hpp>
#include <Spatial.hpp>
#include <Label.hpp>
#include <Navigation.hpp>
#include <AnimationPlayer.hpp>
#include <AudioStreamPlayer.hpp>
#include <AudioStreamSample.hpp>
#include <PackedScene.hpp>
#include <Input.hpp>
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
        //void _input(InputEvent* event);
        

    private:
        void process_game(const double p_delta);
        void random_spawn_ground(PackedScene* item);
        void enemy_spawn();
        void item_spawn();
        void update_score(int delta);
        
    //CLASS STRUCUTRE
    public:
        
       
    protected:
        
    private:
        bool debug_mode = false;
        uint8_t wave = 0, subwave = 0, max_subwaves = 3;
        int score = 0, randomItem = 0;
        float spawnRate = 20, time = 0;

        

        Config* _config = nullptr;
        Label *_topText = nullptr, *_FPScounter = nullptr;
        Navigation* _nav = nullptr;
        AnimationPlayer* _dj = nullptr;
        AudioStreamPlayer *_maestro = nullptr, *_announcer = nullptr, *_crowd = nullptr;
        AudioStreamSample *_an_waveComplete = nullptr, *_an_go = nullptr;
        AudioStreamSample *_mus_level = nullptr, *_mus_boss = nullptr;
        //TODO: check if I can make a meta-array of enemy tiers.
        
        //Array _enemyTiers{};
        Ref<PackedScene> enemy;
        //Ref<PackedScene> _enemyTier1[4];
        //PackedScene *_enemyTier2[4];
        //PackedScene *_bossTier[3];
        //PackedScene *_itemBoxes[4];
    };
}

#endif
