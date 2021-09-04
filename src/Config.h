#ifndef CONFIG_H
#define CONFIG_H

#include <Godot.hpp>
#include <Node.hpp>
#include <ConfigFile.hpp>
#include <AudioServer.hpp>
#include <OS.hpp>
#include <Viewport.hpp>
#include <Engine.hpp>

namespace godot {

    class Config : public Node
    {
        //GODOT STRUCTURE
        GODOT_CLASS(Config, Node)

    public:
        Config();
        ~Config();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);
        static void _register_methods();
        void save_conf();
        void load_conf();
    
    //CLASS STRUCUTRE
    public:
        int8_t difficulty, startWave;
        int score, highScore;
        float mouseSensitivity, resolutionScale;
    protected:
        
    private:
    
    };
}

#endif