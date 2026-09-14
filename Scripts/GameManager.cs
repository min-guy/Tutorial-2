using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class GameManager : Node2D
{
	private PackedScene columnScene;
	private Vector2 spawnColumnLocation;

	private Label text;

	private Character p1;
	private Character p2;
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

		// Get text node
		text = GetNode<Label>(new NodePath("CanvasLayer/Label"));

		// Get player nodes
		p1 = GetNode<Character>(new NodePath("Player"));
		p2 = GetNode<Character>(new NodePath("Player2"));
		// Attach signal to update Score
		GameSignals.Instance.ScoreUpdate += updateScore;
	}

	private void SpawnColumn()
	{
		var column = columnScene.Instantiate<Column>();

		var height = GD.RandRange(220, 670);

		column.Position = new Vector2(spawnColumnLocation.X, height);

		AddChild(column);
	}

	private void updateScore()
	{
		text.Text = "Score: " + (p1.getScore() + p2.getScore());
	}
}
