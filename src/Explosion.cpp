#include "Explosion.h"

using namespace godot;

void Explosion::_register_methods() {
    register_method("_process", &Explosion::_process);
    register_method("_ready", &Explosion::_ready);
}

Explosion::Explosion() {

}

Explosion::~Explosion() {
    // add your cleanup here
}

void Explosion::_init() {

}
void Explosion::_ready() {

}
void Explosion::_process(const double p_delta) {


}