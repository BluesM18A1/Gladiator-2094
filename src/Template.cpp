#include "Template.h"

using namespace godot;

void Template::_register_methods() {
    register_method("_process", &Template::_process);
    register_method("_ready", &Template::_ready);
}

Template::Template() {
    
}

Template::~Template() {
    // add your cleanup here
}

void Template::_init() {

}
void Template::_ready() {

}
void Template::_process(const double p_delta) {

    
}