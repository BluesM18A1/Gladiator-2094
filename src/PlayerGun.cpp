#include "Gun.h"

using namespace godot;

void PlayerGun::_register_methods() {
    register_method("_process", &PlayerGun::_process);
    register_method("_ready", &PlayerGun::_ready);
}

PlayerGun::PlayerGun() {

}

PlayerGun::~PlayerGun() {
    // add your cleanup here
}

void PlayerGun::_init() {

}
void PlayerGun::_ready() {

}
void PlayerGun::_process(const double p_delta) {


}