using Godot;
using projectthaumaturgy.Scenes.Characters.Player;
using projectthaumaturgy.Scenes.Components;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Levels;

public partial class PlayerCamera : Camera2D
{
	[Export] public int targetWidth = 352;
	
	[Export] private float AttackShakeStrength = 5f;
	[Export] private float HurtShakeStrength = 5f;
	[Export] private float ShakeDecay = 0.9f;
	[Export] private float ShakeSpeed = 60f;

	private RandomNumberGenerator _rng;
	private FastNoiseLite _noise;
	private float _shakeStrength = 0f;
	private float _noiseI = 0f;
	
	public override void _Ready()
	{
		_rng = new RandomNumberGenerator();
		_rng.Randomize();
		_noise = new FastNoiseLite
		{
			NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex,
			Seed = (int)_rng.Randi(),
			Frequency = 2f
		};
		_shakeStrength = 0f;
	}
	
	public void Shake(bool isAttack)
	{
		_shakeStrength = isAttack ? AttackShakeStrength : HurtShakeStrength;
	}

	public override void _Process(double delta)
	{
		if (_shakeStrength == 0) return;
		_shakeStrength = Mathf.Lerp(_shakeStrength, 0, ShakeDecay * (float)delta);
		
		_noiseI += (float)delta * ShakeSpeed;
		Offset = new Vector2
		(
			_noise.GetNoise2D(1, _noiseI) * _shakeStrength,
			_noise.GetNoise2D(100, _noiseI) * _shakeStrength
		);
	}

	public override void _EnterTree()
	{
		UpdateZoom();
		GetViewport().SizeChanged += UpdateZoom;
		
		if (GetParent() is not Player p) return;
		p.Attacked += OnAttack;
		p.GetNode<HealthComponent>("HealthComponent").HealthChanged += OnHurt;
	}
	
	public override void _ExitTree()
	{
		GetViewport().SizeChanged -= UpdateZoom;
		
		if (GetParent() is not Player p) return;
		p.Attacked -= OnAttack;
		p.GetNode<HealthComponent>("HealthComponent").HealthChanged -= OnHurt;
	}
	
	private void OnAttack()
	{
		Shake(true);
	}
	
	private void OnHurt(HealthChange c)
	{
		if (!c.IsHealing) Shake(false);
	}

	private void UpdateZoom()
	{
		var zoom = GetViewportRect().Size.X / (float)targetWidth;
		Zoom = new Vector2(zoom, zoom);
	}
}