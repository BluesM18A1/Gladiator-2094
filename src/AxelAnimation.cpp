#include "AxelAnimation.h"

using namespace godot;

void AxelAnimation::_register_methods() {
    register_method("_process", &AxelAnimation::_process);
    register_method("_ready", &AxelAnimation::_ready);
}

AxelAnimation::AxelAnimation() {

}

AxelAnimation::~AxelAnimation() {
    // add your cleanup here
}

void AxelAnimation::_init() {

}
void AxelAnimation::_ready() {

}
void AxelAnimation::_process(const double p_delta) {


}