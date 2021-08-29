#include "Gun.h"

using namespace godot;

void EnemyGun::_register_methods() {
    register_method("_process", &EnemyGun::_process);
    register_method("_ready", &EnemyGun::_ready);
}

EnemyGun::EnemyGun() {

}

EnemyGun::~EnemyGun() {
    // add your cleanup here
}

void EnemyGun::_init() {

}
void EnemyGun::_ready() {

}
void EnemyGun::_process(const double p_delta) {


}