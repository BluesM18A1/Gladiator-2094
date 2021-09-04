#include "Combatant.h"

using namespace godot;

void Player::_register_methods() {
    register_method("_process", &Player::_process);
	register_method("_physics_process", &Player::_physics_process);
    register_method("_ready", &Player::_ready);
	register_method("_input", &Player::_input);
	register_method("process_input", &Player::process_input);
	register_method("process_health_meter", &Player::process_health_meter);
	register_method("update_health", &Player::update_health);
    register_property("Max HP", &Player::maxHP, 100);
    register_property("Max Fuel", &Player::maxFuel, 100);
    register_property("Recharge Rate", &Player::rechargeRate, 100.0f);
    register_property("Fuel Drain Rate", &Player::fuelDrainRate, 60.0f);
    register_property("Jet Force", &Player::jetForce, 3.0f);
    register_property("Normal Speed", &Player::normalSpeed, 7.0f);
    register_property("Sprint Speed", &Player::sprintSpeed, 14.0f);
    register_property("Overheal Decharge Rate", &Player::overhealDecrementRate, 0.25f);
    register_property("Aim Sensitivity", &Player::mouseSensitivity, 0.3f);
}

Player::Player() {

}

Player::~Player() {
    // add your cleanup here
}

void Player::_init() {
	_inputt = nullptr;
	_config = nullptr;
    _camera = nullptr;
    _gun = nullptr;
    _healthMeter = nullptr;
	_fuelMeter = nullptr;
    _healthNum = nullptr;
    _screenAni = nullptr;
    _boostSnd = nullptr, _medSnd = nullptr, _hurtSnd = nullptr, _landSnd = nullptr;
    alive = true;
    HPcounter = 0;
    fuel = 0;
    flameThrowerOn = false;
    overhealTimer = 0;
    canPlayBumpSound = false;
}
void Player::_ready() {
    _config = get_node<Config>("/root/Config");
    _boostSnd = get_node<AudioStreamPlayer>("boostSnd");
    _medSnd = get_node<AudioStreamPlayer>("medSnd");
    _hurtSnd = get_node<AudioStreamPlayer>("hurtSnd");
    _landSnd = get_node<AudioStreamPlayer>("landSnd");
    _head = get_node<Spatial>("Neck");
    _camera = get_node<Camera>("Neck/Camera");
    _gun = get_node<PlayerGun>("Neck/Gun");
    _healthMeter = get_node<TextureProgress>("HUD/HealthMeter");
    _fuelMeter = get_node<TextureProgress>("HUD/FuelMeter");
    _healthNum = get_node<Label>("HUD/HealthMeter/HealthNum");
    _screenAni = get_node<AnimationPlayer>("HUD/ScreenFlash/ScreenTransitions");
    _healthMeter->set_max(maxHP);
    _fuelMeter->set_max(maxFuel);
    mouseSensitivity = _config->mouseSensitivity;
    HP = maxHP;
    _healthNum->set_text(std::to_string(HPcounter).c_str());
    _healthMeter->set_value(HPcounter);
	_inputt = Input::get_singleton();
	_inputt->set_mouse_mode(_inputt->MOUSE_MODE_CAPTURED);

}
void Player::_process(const double p_delta) {
	//Analog stick Aiming
	_head->rotate_x(Math::deg2rad(_inputt->get_joy_axis(0, 3) * -mouseSensitivity));
	rotate_y(Math::deg2rad(-_inputt->get_joy_axis(0, 2)* mouseSensitivity));
	//overheal mechanics
	if (HP > _healthMeter->get_max())
	{
		//overheal = true;
		overhealTimer += p_delta;
		if (overhealTimer >= overhealDecrementRate)
		{
			overhealTimer = 0;
			HP--;
			_healthNum->set_text(std::to_string(HP).c_str());
		}
	}
	//else overheal = false;
}
void Player::_physics_process(const double p_delta)
{	
	//process_health_meter(p_delta);
	process_movement(p_delta); //it is very important that you do MoveAndSlide() before you do
	process_input(p_delta); //the IsOnFloor() calls that the input processing will do.
}
void Player::_input(Variant event) {
	mouseSensitivity = _config->mouseSensitivity;
	Input* _input = Input::get_singleton();
    if(_input == nullptr) return;   

	
	
	//Mouse Aiming
	Ref<InputEventMouseMotion> mouseEvent = event; //I'm not sure if this is how you're supposed to attempt a recast but it won't let me use Object::cast_to<>(). Using dynamic_cast doesn't work either.
	//Ref<InputEventMouseMotion> mouseEvent = Object::cast_to<InputEventMouseMotion>(event);
	//because of the above problem, I sincerely doubt that the null check below will actually work.
	if (mouseEvent != NULL && _input->get_mouse_mode() == _input->MOUSE_MODE_CAPTURED)
	{
		_head->rotate_x(Math::deg2rad(mouseEvent->get_relative().y * -mouseSensitivity));
		rotate_y(Math::deg2rad(-mouseEvent->get_relative().x * mouseSensitivity));
		//apply vertical clamping to rotation
		Vector3 cameraRot = _head->get_rotation_degrees();
		//TIL the GDnative C++ implementation of clamp() does not like int values under any circumstances. Must be real_t or float.
		cameraRot.x = Math::clamp(cameraRot.x, -85.0f, 85.0f);
		_head->set_rotation_degrees(cameraRot);
	}
}

void Player::process_input(const double p_delta){
	//  Walking
	
	Transform camXform = _camera->get_global_transform();
	Vector2 inputMovementVector = Vector2{0,0};
		
		
	dir = Vector3{0,0,0};
	if (_inputt->is_action_pressed("player_forward"))
		inputMovementVector.y += 1;
	if (_inputt->is_action_pressed("player_backward"))
		inputMovementVector.y -= 1;
	if (_inputt->is_action_pressed("player_left"))
		inputMovementVector.x -= 1;
	if (_inputt->is_action_pressed("player_right"))
		inputMovementVector.x += 1;

	inputMovementVector = inputMovementVector.normalized();
	
	if (alive)
	{
		//apply movement vector
		dir += -camXform.basis.z.normalized() * inputMovementVector.y;
		dir += camXform.basis.x.normalized() * inputMovementVector.x;

		//  Jumping / Jetpack thrust
		if (_inputt->is_action_pressed("player_jump") && _fuelMeter->get_value() > 0)
		{
			Godot::print("JUMP PLSS????");
			if (_inputt->is_action_just_pressed("player_jump")) _boostSnd->play();
		
			vel.y = (fuel / 10);
			fuel -= fuelDrainRate * p_delta;
		}

		//sprinting
		if (_inputt->is_action_pressed("player_sprint") && _fuelMeter->get_value() > 0)
		{
			if (_inputt->is_action_just_pressed("player_sprint")) _boostSnd->play();
			
			maxSpeed = sprintSpeed;
			fuel -= (fuelDrainRate / 3) * p_delta;
		}
		else maxSpeed = normalSpeed;

		//stop jetpack sounds
		if ((!_inputt->is_action_pressed("player_sprint") && !_inputt->is_action_pressed("player_jump")) || _fuelMeter->get_value() == 0) _boostSnd->stop();
		
		//recharging
		if (is_on_floor())
		{   
			if (canPlayBumpSound)
			{
				_landSnd->play(); //this doesn't quite work as predictably as I'd like. Oh well.
				canPlayBumpSound = false;
			}
			if (!_inputt->is_action_pressed("player_sprint") && !flameThrowerOn)
			{
				if (fuel < 100)
				{
					fuel += rechargeRate * p_delta;
				}
			}
		}
		else canPlayBumpSound = true;
	}
	else
	{
		get_tree()->change_scene("res://Scenes/GameOver.tscn");
	}
	fuel = Math::clamp(fuel, 0.0f, 100.0f);
	_fuelMeter->set_value(fuel);
}

void Player::process_health_meter(const double p_delta) {
    if (HP != HPcounter)
		{
			//flash the color between blue and white every frame
			if (_healthMeter->get_tint_progress() == Color{1,1,1,1})
			{
				_healthMeter->set_tint_progress(Color {0,0,1,1});
			}
			else _healthMeter->set_tint_progress(Color {1,1,1,1});
			//increment/decrement the meter towards the HP value every frame
			if (HP > HPcounter)
			{
				HPcounter++;
				
			}
			else if (HP < HPcounter)
			{
				HPcounter--;
			}
		}
		else
		{
			_healthMeter->set_tint_progress(Color {0,0,1,1});
		}
		_healthNum->set_text(std::to_string(HPcounter).c_str());
		_healthMeter->set_value(HPcounter);
}
void Player::update_health(const int delta) {
    if (delta < 0)
	{
		if (alive)
		{
			_hurtSnd->play();
			_screenAni->play("Hurt");
		}
	}
	else
	{
		fuel = 100;
		_medSnd->play();
	}
	HP += delta;
	if (HP <= 0)
	{
		alive = false;
		_gun->disabled = true;
	}
}