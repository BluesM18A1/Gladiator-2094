#include "Combatant.h"

using namespace godot;

void Combatant::_register_methods() {
    register_property("HP", &Combatant::HP, 100);
    register_method("_process", &Combatant::_process);
    register_method("process_movement", &Combatant::process_movement);
    register_method("update_health", &Combatant::update_health);
    register_property("Gravity", &Combatant::gravity, -20.0f);
    register_property("Max Speed", &Combatant::maxSpeed, 8.0f);
    register_property("Acceleration", &Combatant::accel, 4.5f);
    register_property("DeAcceleration", &Combatant::deAccel, 16.0f);
    register_property("Max Slope Angle", &Combatant::maxSlopeAngle, 50.0f);
}

Combatant::Combatant() {
    _head = nullptr;
    gravity = -20.0f;
    maxSpeed = 8.0f;
    accel = 4.5f;
    deAccel = 16.0f;
    maxSlopeAngle = 50.0f;
    HP = 100;
}

Combatant::~Combatant() {
    // add your cleanup here
}

void Combatant::_init() {
    
}

void Combatant::_process(float p_delta) {
    
}
void Combatant::_physics_process(float delta) {
    process_movement(delta);
}
void Combatant::process_movement(float delta) {
    dir.y = 0;
    dir = dir.normalized();

    vel.y += delta * gravity;

    Vector3 hvel = vel;
    hvel.y = 0;

    Vector3 target = dir;

    target *= maxSpeed;

    hvel = hvel.linear_interpolate(target, (dir.dot(hvel) > 0) ? accel : deAccel * delta);
    
    vel.x = hvel.x;
    vel.z = hvel.z;
    vel = move_and_slide(vel, Vector3::UP, false, 4, Math::deg2rad(maxSlopeAngle));
}
void Combatant::update_health(int damage) {
    HP -= damage;
}