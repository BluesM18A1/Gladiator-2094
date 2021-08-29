#include "Bullet.h"

using namespace godot;

void Bullet::_register_methods() {
    register_method("_process", &Bullet::_process);
    register_method("_ready", &Bullet::_ready);
}

Bullet::Bullet() {

}

Bullet::~Bullet() {
    // add your cleanup here
}

void Bullet::_init() {

}
void Bullet::_ready() {

}
void Bullet::_process(const double p_delta) {


}