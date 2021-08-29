#ifndef TITLESCREEN_H
#define TITLESCREEN_H

#include <Godot.hpp>
#include <OS.hpp>
#include <Control.hpp>
#include <Input.hpp>
#include <AudioStreamPlayer.hpp>
#include <SceneTree.hpp>
#include "Config.h"

namespace godot {

    class TitleScreen : public Control
    {
        //GODOT STRUCTURE
        GODOT_CLASS(TitleScreen, Control)

    public:

        static void _register_methods();
        TitleScreen();
        ~TitleScreen();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);
    
    private:
        
        void button_hovered();
        void menu_button_pressed(String dir);
        void exit_button_pressed();
    
        Config* _config;
        //wavechanger stuff needs to happen here too
        godot::Input* _input;
        godot::OS* _os;
        godot::AudioStreamPlayer* _bloop;

    public:
        
    protected:
        

    
    };
}

#endif
