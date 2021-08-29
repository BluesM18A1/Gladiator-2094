#include "SettingsSlider.h"

using namespace godot;

void VolumeSlider::_register_methods() {
    register_method("_process", &VolumeSlider::_process);
    register_method("_ready", &VolumeSlider::_ready);
}

VolumeSlider::VolumeSlider() {

}

VolumeSlider::~VolumeSlider() {
    // add your cleanup here
}

void VolumeSlider::_init() {

}
void VolumeSlider::_ready() {

}
void VolumeSlider::_process(const double p_delta) {


}