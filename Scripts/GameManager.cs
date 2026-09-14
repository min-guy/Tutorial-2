using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class GameManager : Node2D
{
	private PackedScene columnScene;
	private Vector2 spawnColumnLocation;

	public override void _Ready()
	{
		var timer = new Timer();
		timer.WaitTime = 1.3;
		timer.Timeout += SpawnColumn;
		timer.OneShot = false;
		timer.Autostart = true;

		columnScene = ResourceLoader.Load<PackedScene>("uid://cwkpgyp5takwg");

		var spawnColumnLocationNode = GetNode<Node2D>("SpawnColumnLocation");
		spawnColumnLocation = spawnColumnLocationNode.Position;

		AddChild(timer);
	}

	private void SpawnColumn()
	{
		var column = columnScene.Instantiate<Column>();

		var height = GD.RandRange(220, 670);

		column.Position = new Vector2(spawnColumnLocation.X, height);

		AddChild(column);
	}
}
