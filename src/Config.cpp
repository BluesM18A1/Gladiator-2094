#include "Config.h"

using namespace godot;

void Config::_register_methods() {
    register_method("_process", &Config::_process);
    register_method("_ready", &Config::_ready);
    register_method("save_conf", &Config::save_conf);
    register_method("load_conf", &Config::load_conf);
}

Config::Config() {
    
}

Config::~Config() {
    // add your cleanup here
}

void Config::_init() {
	difficulty = 0;
    startWave = 1;
    score = 0;
    highScore = 0;
    mouseSensitivity = 0.10f;
    resolutionScale = 1;
}
void Config::_ready() {
    Godot::print("h");
    //load_conf();
}
void Config::_process(const double p_delta) {

    
}
void Config::save_conf() {
    Godot::print("Saving Settings");
    //this crashes the game without any explanation for some reason. Investigating later.
    /*
    ConfigFile file;
    file.set_value("Game", "Difficulty", difficulty);
    file.set_value("Controls", "LookSensitivity", mouseSensitivity);
    file.set_value("Audio", "MasterVolume", AudioServer::get_singleton()->get_bus_volume_db(0));
    file.set_value("Audio", "BgmVolume", AudioServer::get_singleton()->get_bus_volume_db(1));
    file.set_value("Audio", "SfxVolume", AudioServer::get_singleton()->get_bus_volume_db(2));
    file.set_value("Video", "Fullscreen", OS::get_singleton()->is_window_fullscreen());
    file.set_value("Video", "VSync", OS::get_singleton()->is_vsync_enabled());
    file.set_value("Video", "Borderless", OS::get_singleton()->get_borderless_window());
    file.set_value("Video", "HDR", get_viewport()->get_hdr());
    file.set_value("Video", "AntiAliasing", get_viewport()->get_msaa());
    file.set_value("Video", "FXAA", get_viewport()->get_use_fxaa());
    file.set_value("Video", "TargetFPS", Engine::get_singleton()->get_target_fps());
    file.set_value("Video", "ResolutionScale", resolutionScale);
    Error err = file.save("user://userConfig.ini");
    Godot::print("Saved"); 
    //PUT SOME ERROR HANDLING CODE IN HERE LATER
    */
}
void Config::load_conf() {
    //this crashes the game without any explanation for some reason. Investigating later.
    /*
    Godot::print("Loading Settings");
    
    ConfigFile file;
    Godot::print("Declared File");
    Error err = file.load("user://userConfig.ini");
    Godot::print("Declared Error handler");
    //ERROR HANDLING MESSAGES HERE TOO
    difficulty = file.get_value("Game", "Difficulty", 0);
    mouseSensitivity = file.get_value("Controls", "LookSensitivity", 0.1f);
    AudioServer::get_singleton()->set_bus_volume_db(0, file.get_value("Audio", "MasterVolume", 0));
    AudioServer::get_singleton()->set_bus_volume_db(1, file.get_value("Audio", "BgmVolume", 0));
    AudioServer::get_singleton()->set_bus_volume_db(2, file.get_value("Audio", "SfxVolume", 0));
    OS::get_singleton()->set_window_fullscreen(file.get_value("Video", "Fullscreen", false));
    OS::get_singleton()->set_use_vsync(file.get_value("Video", "VSync", false));
    OS::get_singleton()->set_borderless_window(file.get_value("Video", "Borderless", false));
    get_viewport()->set_hdr(file.get_value("Video", "HDR", true));
    get_viewport()->set_msaa(file.get_value("Video", "AntiAliasing", Viewport::MSAA_4X));
    get_viewport()->set_use_fxaa(file.get_value("Video", "FXAA", false));
    Engine::get_singleton()->set_target_fps(file.get_value("Video", "TargetFPS", 0));
    resolutionScale = file.get_value("Video", "ResolutionScale", 1);
    get_viewport()->set_size(Vector2(OS::get_singleton()->get_window_size().x * resolutionScale, OS::get_singleton()->get_window_size().y * resolutionScale));
    Godot::print("Loaded");
    */
}
