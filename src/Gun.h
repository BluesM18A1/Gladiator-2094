#ifndef GUN_H
#define GUN_H

#include <Godot.hpp>
#include <Spatial.hpp>

namespace godot {

    class Gun : public Spatial
    {
        //GODOT STRUCTURE
        GODOT_CLASS(Gun, Spatial)
    public:
        enum Weapons { REPEATER = 2, BUCKSHOT = 3, GRENADES = 4, FLAMETHROWER = 1 };
        bool disabled;
        //array of transforms, barrels
        Gun();
        ~Gun();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);
        static void _register_methods();
    protected:

    private:

    };
}

#endif

#ifndef PLAYERGUN_H
#define PLAYERGUN_H

#include <Godot.hpp>

namespace godot {

    class PlayerGun : public Gun
    {
        //GODOT STRUCTURE
        GODOT_CLASS(PlayerGun, Gun)

    public:
        PlayerGun();
        ~PlayerGun();
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

#ifndef ENEMYGUN_H
#define ENEMYGUN_H

#include <Godot.hpp>

namespace godot {

    class EnemyGun : public Gun
    {
        //GODOT STRUCTURE
        GODOT_CLASS(EnemyGun, Gun)

    public:
        EnemyGun();
        ~EnemyGun();
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
