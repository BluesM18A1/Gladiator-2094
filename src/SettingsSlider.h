#ifndef SETTINGSSLIDER_H
#define SETTINGSSLIDER_H

#include <Godot.hpp>
#include <Slider.hpp>

namespace godot {

    class SettingsSlider : public Slider
    {
        //GODOT STRUCTURE
        GODOT_CLASS(SettingsSlider, Slider)

    public:
        SettingsSlider();
        ~SettingsSlider();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);
        static void _register_methods();
        //CLASS STRUCUTRE
    public:

    protected:
        //bloop
        //config
        //label
    private:
        
    };
}

#endif

#ifndef VOLUMESLIDER_H
#define VOLUMESLIDER_H

#include <Godot.hpp>
#include <Slider.hpp>

namespace godot {

    class VolumeSlider : public SettingsSlider
    {
        //GODOT STRUCTURE
        GODOT_CLASS(VolumeSlider, SettingsSlider)

    public:
        VolumeSlider();
        ~VolumeSlider();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);
        static void _register_methods();

        //CLASS STRUCUTRE
    public:
        //enum audiobuses
        //audiobuses bus
    protected:

    private:

    };
}

#endif

#ifndef DIFFICULTYSLIDER_H
#define DIFFICULTYSLIDER_H

#include <Godot.hpp>
#include <Slider.hpp>

namespace godot {

    class DifficultySlider : public SettingsSlider
    {
        //GODOT STRUCTURE
        GODOT_CLASS(DifficultySlider, SettingsSlider)

    public:
        DifficultySlider();
        ~DifficultySlider();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);
        static void _register_methods();

        //CLASS STRUCUTRE
    public:
        
    protected:

    private:

    };
}

#endif

#ifndef SENSITIVITYSLIDER_H
#define SENSITIVITYSLIDER_H

#include <Godot.hpp>
#include <Slider.hpp>

namespace godot {

    class SensitivitySlider : public SettingsSlider
    {
        //GODOT STRUCTURE
        GODOT_CLASS(SensitivitySlider, SettingsSlider)

    public:
        SensitivitySlider();
        ~SensitivitySlider();
        void _init(); // our initializer called by Godot
        void _ready();
        void _process(const double p_delta);
        static void _register_methods();

        //CLASS STRUCUTRE
    public:
        
    protected:

    private:

    };
}

#endif