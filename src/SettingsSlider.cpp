#include "SettingsSlider.h"

using namespace godot;

void SettingsSlider::_register_methods() {
    register_method("_process", &SettingsSlider::_process);
    register_method("_ready", &SettingsSlider::_ready);
}

SettingsSlider::SettingsSlider() {

}

SettingsSlider::~SettingsSlider() {
    // add your cleanup here
}

void SettingsSlider::_init() {

}
void SettingsSlider::_ready() {

}
void SettingsSlider::_process(const double p_delta) {


}