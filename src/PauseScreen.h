#ifndef PAUSESCREEN_H
#define PAUSESCREEN_H

#include <Godot.hpp>
#include <Control.hpp>
#include <Input.hpp>
#include <AudioStreamPlayer.hpp>
#include <SceneTree.hpp>
#include "Config.h"

namespace godot {

    class PauseScreen : public Control
    {
        //GODOT STRUCTURE
        GODOT_CLASS(PauseScreen, Control)

    public:

        static void _register_methods();
        PauseScreen();
        ~PauseScreen();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);

    protected:

    private:

        void button_hovered();
        void resume_button_pressed();
        void exit_button_pressed();
        Config* _config;
        godot::Input* _input;
        godot::AudioStreamPlayer* _bloop;

    };
}

#endif
