#include "Arena.h"
#include <string>
using namespace godot;

void Arena::_register_methods() {
    register_method("_process", &Arena::_process);
    register_method("_ready", &Arena::_ready);
    register_property<Arena, bool>("Debug Mode", &Arena::debug_mode, false);
    register_property<Arena, Ref<PackedScene>>("Enemy", &Arena::enemy, Ref<PackedScene>(nullptr));
}

Arena::Arena() {
    /*
    for (int i = 0; i < sizeof(_bossTier); i++)
    {
        _bossTier[i] = nullptr;
    }*/
}

Arena::~Arena() {
    // add your cleanup here
}

void Arena::_init() {

}
void Arena::_ready() {

}
void Arena::_process(const double p_delta) {

    
}
void Arena::process_game(const double p_delta) {


}
void Arena::random_spawn_ground(PackedScene* item) {

}
void Arena::enemy_spawn() {

}
void Arena::item_spawn() {

}
void Arena::update_score(int delta) {
    if (debug_mode)
    {
        _topText->set_text("DEBUG MODE");
    }
    else
    {
        score += delta;
        _topText->set_text("WAVE: "); //HOW TO TOSTRING IN C++????
    }
}
