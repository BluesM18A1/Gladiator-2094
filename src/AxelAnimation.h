#ifndef AXELANIMATION_H
#define AXELANIMATION_H

#include <Godot.hpp>
#include <MeshInstance.hpp>

namespace godot {

    class AxelAnimation : public MeshInstance
    {
        //GODOT STRUCTURE
        GODOT_CLASS(AxelAnimation, MeshInstance)

    public:
        AxelAnimation();
        ~AxelAnimation();
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