using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using projectthaumaturgy.Scripts;

namespace projectthaumaturgy.Scenes.Levels;

public partial class World : TileMap
{
	public int Size { get; private set; }
	
	public Array<WalkableTile> WalkableTiles { get; private set; } = new();
	public Array<Vector2I> WallTiles { get; private set; } = new();
	
	private WalkerProperties _walkerProperties;

#nullable enable
	private Task? _worldLoading;
#nullable disable
	public bool IsLoadingFinished => _worldLoading?.IsCompleted ?? false;

	public delegate void WorldLoaded();
	public WorldLoaded OnWorldLoaded;
	
	public void Init(int size, WalkerProperties walkerProperties)
	{
		Size = size;
		_walkerProperties = walkerProperties;
	}

	public void LoadWorld()
	=> _worldLoading = Task.Run(() =>
		{
			new WalkerOrchestrator(this, Vector2I.Zero)
				.AddProperties(_walkerProperties)
				.Walk()
				.QueueFree(); // !!!

			base.SetCellsTerrainConnect(0, new Array<Vector2I>(WalkableTiles.Select(x => x.Position)), 0, 0, false);
			base.SetCellsTerrainConnect(0, WallTiles, 0, 1, false);
		});
	
	public void Wait() => _worldLoading?.Wait();
}