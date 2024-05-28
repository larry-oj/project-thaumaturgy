using Godot;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.UI;

public partial class SoundSlider  : HSlider
{
	[Export] AudioStreamPlayer _audioStreamPlayer;
	
	private int _busIndex;
	
	public override void _Ready()
	{
		_busIndex = AudioServer.GetBusIndex(Options.Audio.SoundEffectsBus);
		Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(_busIndex));
		ValueChanged += OnValueChanged;
		VisibilityChanged += OnVisibilityChanged;
	}
	
	private void OnValueChanged(double value)
	{
		AudioServer.SetBusVolumeDb(_busIndex, (float)Mathf.LinearToDb(value));
		_audioStreamPlayer.Playing = true;
	}
	
	private void OnVisibilityChanged()
	{
		if (Visible)
		{
			Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(_busIndex));
		}
	}
}