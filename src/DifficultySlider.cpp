#include "SettingsSlider.h"

using namespace godot;

void DifficultySlider::_register_methods() {
    register_method("_process", &DifficultySlider::_process);
    register_method("_ready", &DifficultySlider::_ready);
}

DifficultySlider::DifficultySlider() {

}

DifficultySlider::~DifficultySlider() {
    // add your cleanup here
}

void DifficultySlider::_init() {

}
void DifficultySlider::_ready() {

}
void DifficultySlider::_process(const double p_delta) {


}