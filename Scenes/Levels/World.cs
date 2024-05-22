using System;
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
	private ColorRect _background;

#nullable enable
	private Task? _worldLoading;
#nullable disable
	public bool IsLoadingFinished => _worldLoading?.IsCompleted ?? false;

	public delegate void WorldLoaded();
	public WorldLoaded OnWorldLoaded;
	
	public void Init(int size, WalkerProperties walkerProperties, ColorRect background)
	{
		Size = size;
		_walkerProperties = walkerProperties;
		_background = background;
	}

	public void LoadWorld(int tilesetId)
	=> _worldLoading = Task.Run(() =>
		{
			new WalkerOrchestrator(this, Vector2I.Zero)
				.AddProperties(_walkerProperties)
				.Walk()
				.QueueFree(); // !!!	

			var size = 80 / 2;
			var coords = new Vector2I(9, 2);
			for (var i = -size; i < size; i++)
			{
				for (var j = -size; j < size; j++)
				{
					var position = new Vector2I(i, j);
					base.SetCell(0, position, tilesetId, coords);
				}
			}
			
			base.SetCellsTerrainConnect(0, new Array<Vector2I>(WalkableTiles.Select(x => x.Position)), tilesetId, 1, false);
			base.SetCellsTerrainConnect(0, WallTiles, tilesetId, 0, false);
			
			for (var i = -size; i < size; i++)
			{
				for (var j = -size; j < size; j++)
				{
					var position = new Vector2I(i, j);
					if (GetCellAtlasCoords(0, position) == coords)
					{
						EraseCell(0, position);
					}
				}
			}

			// forest	7a9e54
			// caves	1a0d24
			_background.Color = new Color(tilesetId switch
			{
				1 => "7a9e54",
				2 => "1a0d24",
				3 => "5e5e71",
				_ => "4c4c4c"
			});
		});
	
	public void Wait() => _worldLoading?.Wait();
}