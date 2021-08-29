#include "SettingsSlider.h"

using namespace godot;

void SensitivitySlider::_register_methods() {
    register_method("_process", &SensitivitySlider::_process);
    register_method("_ready", &SensitivitySlider::_ready);
}

SensitivitySlider::SensitivitySlider() {

}

SensitivitySlider::~SensitivitySlider() {
    // add your cleanup here
}

void SensitivitySlider::_init() {

}
void SensitivitySlider::_ready() {

}
void SensitivitySlider::_process(const double p_delta) {


}