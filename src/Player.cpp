#include "Combatant.h"

using namespace godot;

void Player::_register_methods() {
    register_method("_process", &Player::_process);
    register_method("_ready", &Player::_ready);
}

Player::Player() {

}

Player::~Player() {
    // add your cleanup here
}

void Player::_init() {

}
void Player::_ready() {

}
void Player::_process(const double p_delta) {


}