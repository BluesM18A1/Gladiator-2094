#include "Combatant.h"

using namespace godot;

void Combatant::_register_methods() {
    register_property("HP", &Combatant::HP, 100);
    register_method("_process", &Combatant::_process);
    register_method("_physics_process", &Combatant::_physics_process);
    register_method("process_movement", &Combatant::process_movement);
    register_method("update_health", &Combatant::update_health);
    register_property("HP", &Combatant::HP, 100);
    register_property("Gravity", &Combatant::gravity, -20.0f);
    register_property("Max Speed", &Combatant::maxSpeed, 8.0f);
    register_property("Acceleration", &Combatant::accel, 4.5f);
    register_property("DeAcceleration", &Combatant::deAccel, 16.0f);
    register_property("Max Slope Angle", &Combatant::maxSlopeAngle, 50.0f);
}

Combatant::Combatant() {
    
}

Combatant::~Combatant() {
    // add your cleanup here
}

void Combatant::_init() {
    _head = nullptr;
    /* //wait do I even need to initialize variables here if they're registered up there?
    gravity = -20.0f;
    maxSpeed = 8.0f;
    accel = 4.5f;
    deAccel = 16.0f;
    maxSlopeAngle = 50.0f;
    HP = 100;
    */
}

void Combatant::_process(const double p_delta) {
    
}
void Combatant::_physics_process(const double p_delta) {
    process_movement(p_delta);
}
void Combatant::process_movement(const double p_delta) {
    dir.y = 0;
    dir = dir.normalized();

    vel.y += p_delta * gravity;

    Vector3 hvel = vel;
    hvel.y = 0;

    Vector3 target = dir;

    target *= maxSpeed;
    float accel;
    if (dir.dot(hvel) > 0)
        accel = accel;
    else
        accel = deAccel;

    hvel = hvel.linear_interpolate(target, accel * p_delta);
    vel.x = hvel.x;
    vel.z = hvel.z;

    hvel = hvel.linear_interpolate(target, (dir.dot(hvel) > 0) ? accel : deAccel * p_delta);
    
    vel.x = hvel.x;
    vel.z = hvel.z;
    vel = move_and_slide(vel, Vector3::UP, false, 4, Math::deg2rad(maxSlopeAngle));
}
void Combatant::update_health(const int delta) {
    HP -= delta;
}