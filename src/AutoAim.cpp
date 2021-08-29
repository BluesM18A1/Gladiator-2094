#include "AutoAim.h"

using namespace godot;

void AutoAim::_register_methods() {
    register_method("_process", &AutoAim::_process);
    register_method("_ready", &AutoAim::_ready);
}

AutoAim::AutoAim() {

}

AutoAim::~AutoAim() {
    // add your cleanup here
}

void AutoAim::_init() {

}
void AutoAim::_ready() {

}
void AutoAim::_process(const double p_delta) {


}