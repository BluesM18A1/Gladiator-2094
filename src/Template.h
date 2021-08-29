#ifndef TEMPLATE_H
#define TEMPLATE_H

#include <Godot.hpp>
#include <Node.hpp>

namespace godot {

    class Template: public Node
    {
        //GODOT STRUCTURE
        GODOT_CLASS(Template, Node)

    public:
        Template();
        ~Template();
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