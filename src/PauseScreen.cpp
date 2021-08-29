#include "PauseScreen.h"

using namespace godot;

void PauseScreen::_register_methods() {
    register_method("_process", &PauseScreen::_process);
    register_method("_ready", &PauseScreen::_ready);
    register_method("exit_button_pressed", &PauseScreen::exit_button_pressed);
    register_method("resume_button_pressed", &PauseScreen::resume_button_pressed);
    register_method("button_hovered", &PauseScreen::button_hovered);
}

PauseScreen::PauseScreen() {
    _bloop = nullptr;
    _input = nullptr;
    _config = nullptr;
}

PauseScreen::~PauseScreen() {
    // add your cleanup here
}

void PauseScreen::_init() {

}
void PauseScreen::_ready() {
    _bloop = get_node<godot::AudioStreamPlayer>("bloop");
    _input = godot::Input::get_singleton();
    _config = get_node<Config>("/root/Config");
}
void PauseScreen::_process(const double p_delta) {
    
    if (_input->is_action_just_pressed("ui_cancel") == true)
    {
        if (get_tree()->is_paused() == true)
        {
            resume_button_pressed();
        }
        else
        {
            _input->set_mouse_mode(Input::MOUSE_MODE_VISIBLE);
            set_visible(true);
            get_tree()->set_pause(true);
        }
    }
}
void PauseScreen::button_hovered() {
    _bloop->play();
}
void PauseScreen::exit_button_pressed() {
    get_tree()->set_pause(false);
    get_tree()->change_scene("res://Title.tscn");

}
void PauseScreen::resume_button_pressed() {
    _config->save_conf();
    _input->set_mouse_mode(Input::MOUSE_MODE_CAPTURED);
    set_visible(false);
    get_tree()->set_pause(false);
}

