using Godot;
using System;

public partial class AudioSpectrumVisualizer : Control
{
	AudioEffectSpectrumAnalyzerInstance spectrum;
	[Export]
	int barCount = 16;

	const float freqMax = 8000;
	const float minDB = 70;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spectrum = (AudioEffectSpectrumAnalyzerInstance)AudioServer.GetBusEffectInstance(0, 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		QueueRedraw();
	}
	public override void _Draw()
	{
		float w = (Size.X / barCount);
		float prevHz = 60; //start your spectrum analysis here bc frequencies lower than this are nigh-inaudible
		for (int i = 0; i < barCount; i++)
		{
			float hz = i * freqMax / barCount; //select frequency based on how many bars there are + your max frequency
			float magnitude = spectrum.GetMagnitudeForFrequencyRange(prevHz, hz).Length(); //this function does all the magic of selecting a frequency range and getting magnitude
			float energy = Mathf.Clamp((minDB + Mathf.LinearToDb(magnitude))/ minDB, 0, 1); //clamp it based on db
			float height = energy * Size.Y; //finally determine the height of the bar, since energy is a "normalized" or "percentage" value more or less
			//the mask images do all the coloring work
			DrawRect(new Rect2(w * i, Size.Y - height, w, height), Colors.White);
			prevHz = hz; //your frequency range selection now starts here rather than ends here
		}
	}
}
