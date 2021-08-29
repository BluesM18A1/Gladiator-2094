#ifndef ITEMBOX_H
#define ITEMBOX_H

#include <Godot.hpp>
#include <Area.hpp>

namespace godot {

    class ItemBox : public Area
    {
        //GODOT STRUCTURE
        GODOT_CLASS(ItemBox, Area)

    public:
        ItemBox();
        ~ItemBox();
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