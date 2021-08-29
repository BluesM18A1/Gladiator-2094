#include "TitleScreen.h"

using namespace godot;

void TitleScreen::_register_methods() {
    register_method("_process", &TitleScreen::_process);
    register_method("_ready", &TitleScreen::_ready);
    register_method("exit_button_pressed", &TitleScreen::exit_button_pressed);
    register_method("menu_button_pressed", &TitleScreen::menu_button_pressed);
    register_method("button_hovered", &TitleScreen::button_hovered);
}

TitleScreen::TitleScreen() {
    _config = nullptr;
    _bloop =  nullptr;
    _input = nullptr;
    _os = nullptr;
}

TitleScreen::~TitleScreen() {
    // add your cleanup here
}

void TitleScreen::_init() {

}
void TitleScreen::_ready() {
    _config = get_node<Config>("/root/Config");
    _bloop = get_node<godot::AudioStreamPlayer>("bloop");
    _input = godot::Input::get_singleton();
    _os = godot::OS::get_singleton();
    
    _input->set_mouse_mode(Input::MOUSE_MODE_VISIBLE);
    //sometimes the window shoots up really high depending on OS and monitor setup. This makes sure you can easily grab and move the window.
    if (_os->get_window_position().y < 0) _os->set_window_position(Vector2 (_os->get_window_position().x, 0));
}
void TitleScreen::_process(const double p_delta) {

}
void TitleScreen::button_hovered() {
    _bloop->play();
}
void TitleScreen::exit_button_pressed() {
    _config->save_conf();
    get_tree()->quit();
    
}
void TitleScreen::menu_button_pressed(String dir) {
    _config->save_conf();
    get_tree()->change_scene(dir);
}
