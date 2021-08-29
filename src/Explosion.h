#ifndef EXPLOSION_H
#define EXPLOSION_H

#include <Godot.hpp>
#include <Area.hpp>

namespace godot {

    class Explosion : public Area
    {
        //GODOT STRUCTURE
        GODOT_CLASS(Explosion, Area)

    public:
        Explosion();
        ~Explosion();
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