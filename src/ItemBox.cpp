#include "ItemBox.h"

using namespace godot;

void ItemBox::_register_methods() {
    register_method("_process", &ItemBox::_process);
    register_method("_ready", &ItemBox::_ready);
}

ItemBox::ItemBox() {

}

ItemBox::~ItemBox() {
    // add your cleanup here
}

void ItemBox::_init() {

}
void ItemBox::_ready() {

}
void ItemBox::_process(const double p_delta) {


}