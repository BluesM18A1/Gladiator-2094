#include "Config.h"

using namespace godot;

void Config::_register_methods() {
    register_method("_process", &Config::_process);
    register_method("_ready", &Config::_ready);
    register_method("save_conf", &Config::save_conf);
    register_method("load_conf", &Config::load_conf);
}

Config::Config() {
    _audioServer = nullptr;
    _os = nullptr;
    _engine = nullptr;
    difficulty = 0;
    startWave = 1;
    score = 0;
    highScore = 0;
    mouseSensitivity = 0.10f;
    resolutionScale = 1;
    
}

Config::~Config() {
    // add your cleanup here
}

void Config::_init() {
	_audioServer = godot::AudioServer::get_singleton();
    	_os = godot::OS::get_singleton();
    	_engine = godot::Engine::get_singleton();
}
void Config::_ready() {
    
    load_conf();
}
void Config::_process(const double p_delta) {

    
}
void Config::save_conf() {
    Godot::print("Saving Settings");
    /*
    ConfigFile file;
    file.set_value("Game", "Difficulty", difficulty);
    file.set_value("Controls", "LookSensitivity", mouseSensitivity);
    file.set_value("Audio", "MasterVolume", _audioServer->get_bus_volume_db(0));
    file.set_value("Audio", "BgmVolume", _audioServer->get_bus_volume_db(1));
    file.set_value("Audio", "SfxVolume", _audioServer->get_bus_volume_db(2));
    file.set_value("Video", "Fullscreen", _os->is_window_fullscreen());
    file.set_value("Video", "VSync", _os->is_vsync_enabled());
    file.set_value("Video", "Borderless", _os->get_borderless_window());
    file.set_value("Video", "HDR", get_viewport()->get_hdr());
    file.set_value("Video", "AntiAliasing", get_viewport()->get_msaa());
    file.set_value("Video", "FXAA", get_viewport()->get_use_fxaa());
    file.set_value("Video", "TargetFPS", _engine->get_target_fps());
    file.set_value("Video", "ResolutionScale", resolutionScale);
    Error err = file.save("user://userConfig.ini");
    Godot::print("Saved"); */
    //PUT SOME ERROR HANDLING CODE IN HERE LATER
}
void Config::load_conf() {
    Godot::print("Loading Settings");
    /*
    ConfigFile file;
    Error err = file.load("user://userConfig.ini");
    //ERROR HANDLING MESSAGES HERE TOO
    difficulty = file.get_value("Game", "Difficulty", 0);
    mouseSensitivity = file.get_value("Controls", "LookSensitivity", 0.1f);
    _audioServer->set_bus_volume_db(0, file.get_value("Audio", "MasterVolume", 0));
    _audioServer->set_bus_volume_db(1, file.get_value("Audio", "BgmVolume", 0));
    _audioServer->set_bus_volume_db(2, file.get_value("Audio", "SfxVolume", 0));
    _os->set_window_fullscreen(file.get_value("Video", "Fullscreen", false));
    _os->set_use_vsync(file.get_value("Video", "VSync", false));
    _os->set_borderless_window(file.get_value("Video", "Borderless", false));
    get_viewport()->set_hdr(file.get_value("Video", "HDR", true));
    get_viewport()->set_msaa(file.get_value("Video", "AntiAliasing", Viewport::MSAA_4X));
    get_viewport()->set_use_fxaa(file.get_value("Video", "FXAA", false));
    _engine->set_target_fps(file.get_value("Video", "TargetFPS", 0));
    resolutionScale = file.get_value("Video", "ResolutionScale", 1);
    get_viewport()->set_size(Vector2(_os->get_window_size().x * resolutionScale, _os->get_window_size().y * resolutionScale));
    Godot::print("Loaded"); */
}
