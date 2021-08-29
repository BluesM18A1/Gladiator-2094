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
            void _process(float delta);
            void _physics_process(float delta);
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
            virtual void update_health(int damage);
        protected:
            void process_movement(float delta);

        private:
    };
}

#endif

#ifndef PLAYER_H
#define PLAYER_H

#include <Godot.hpp>
#include "Combatant.h"

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
        static void _register_methods();

        //CLASS STRUCUTRE
    public:

    protected:

    private:

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

