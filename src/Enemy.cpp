#include "Combatant.h"

using namespace godot;

void Enemy::_register_methods() {
    register_method("_process", &Enemy::_process);
    register_method("_ready", &Enemy::_ready);
}

Enemy::Enemy() {

}

Enemy::~Enemy() {
    // add your cleanup here
}

void Enemy::_init() {

}
void Enemy::_ready() {

}
void Enemy::_process(const double p_delta) {


}