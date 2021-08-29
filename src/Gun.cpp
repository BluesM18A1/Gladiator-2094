#include "Gun.h"

using namespace godot;

void Gun::_register_methods() {
    register_method("_process", &Gun::_process);
    register_method("_ready", &Gun::_ready);
}

Gun::Gun() {

}

Gun::~Gun() {
    // add your cleanup here
}

void Gun::_init() {

}
void Gun::_ready() {

}
void Gun::_process(const double p_delta) {


}