#ifndef AUTOAIM_H
#define AUTOAIM_H

#include <Godot.hpp>
#include <Spatial.hpp>

namespace godot {

    class AutoAim : public Spatial
    {
        //GODOT STRUCTURE
        GODOT_CLASS(AutoAim, Spatial)

    public:
        AutoAim();
        ~AutoAim();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);
        static void _register_methods();

        //CLASS STRUCUTRE
    public:
        //bool debug_mode;
    protected:

    private:
        //Config config
    };
}

#endif