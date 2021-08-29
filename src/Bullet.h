#ifndef BULLET_H
#define BULLET_H

#include <Godot.hpp>
#include <Node.hpp>

namespace godot {

    class Bullet : public Node
    {
        //GODOT STRUCTURE
        GODOT_CLASS(Bullet, Node)

    public:
        Bullet();
        ~Bullet();
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