#ifndef COMBATANT_H
#define COMBATANT_H

#include <Godot.hpp>
#include <KinematicBody.hpp>

namespace godot {

    class Combatant : public KinematicBody 
    {
        //GODOT STRUCTURE
        GODOT_CLASS(Combatant, KinematicBody)
        
        public:
            Combatant();
            ~Combatant();
            void _init(); // our initializer called by Godot
            void _process(const double p_delta);
            void _physics_process(const double p_delta);
            void process_movement(const double p_delta);
            virtual void update_health(const int delta);
            static void _register_methods();
            
        public:
            //PHYSICS VARIABLES
            float gravity, maxSpeed, accel, deAccel, maxSlopeAngle;
            //GAMEPLAY VARIABLES
            int HP;
            //COMPONENT VARIABLES
            Vector3 vel, dir;

        protected:
            Spatial* _head;
            
        private:

        //GAMEPLAY METHODS
        public:
            
        protected:
            

        private:
    };
}

#endif

#ifndef PLAYER_H
#define PLAYER_H

#include <Godot.hpp>
#include <Camera.hpp>
#include <TextureProgress.hpp>
#include <Label.hpp>
#include <AnimationPlayer.hpp>
#include <AudioStreamPlayer.hpp>
#include <Input.hpp>
#include <InputEventMouseMotion.hpp>
#include <SceneTree.hpp>
#include "Combatant.h"
#include "Config.h"
#include "Gun.h"

namespace godot {

    class Player : public Combatant
    {
        //GODOT STRUCTURE
        GODOT_CLASS(Player, Combatant)

    public:
        Player();
        ~Player();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);
        void _physics_process(const double p_delta);
        void _input(Variant event);
        void process_input(const double p_delta);
        void process_health_meter(const double p_delta);
        void update_health(int delta) override;
        static void _register_methods();

        //CLASS STRUCUTRE
    public:
        
    protected:

    private:
        //references]
        Input* _inputt;
        Config *_config;
        Camera *_camera;
        PlayerGun *_gun;
        TextureProgress *_healthMeter, *_fuelMeter;
        Label *_healthNum;
        AnimationPlayer *_screenAni;
        AudioStreamPlayer *_boostSnd, *_medSnd, *_hurtSnd, *_landSnd;
        //physics
        float rechargeRate, fuelDrainRate;
        float jetForce;
        float normalSpeed, sprintSpeed;
        float mouseSensitivity;
        //gameplay
        bool alive;
        bool flameThrowerOn;
        bool canPlayBumpSound;
        int maxHP, maxFuel;
        int HPcounter;
        float fuel = 0;
        float overhealDecrementRate;
        float overhealTimer;
    };
}

#endif

#ifndef ENEMY_H
#define ENEMY_H

#include <Godot.hpp>
#include "Combatant.h"

namespace godot {

    class Enemy : public Combatant
    {
        //GODOT STRUCTURE
        GODOT_CLASS(Enemy, Combatant)

    public:
        Enemy();
        ~Enemy();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);
        static void _register_methods();

        //CLASS STRUCUTRE
    public:

    protected:

    private:

    };
}

#endif

