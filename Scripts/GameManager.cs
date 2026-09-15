using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class GameManager : Node2D
{
	[Export] private PackedScene PlayerScene {get; set;}
	private PackedScene columnScene;
	private Vector2 spawnColumnLocation;

	private Label text;

	private Character p1;
	private Character p2;

	private bool end = false; // true when both players have died
	
	private Timer timer;
	public override void _Ready()
	{
		timer = new Timer();
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

		p1 = SpawnPlayer(1, new Vector2(960, 540), Colors.White);
		p2 = SpawnPlayer(2, new Vector2(960, 540), Colors.Orange);

		// Attach signal to update Score
		GameSignals.Instance.ScoreUpdate += updateScore;
	}

    public override void _Process(double delta)
    {
        if (p1.isDead && p2.isDead)
		{
			timer.Stop();
			GameSignals.Instance.EmitSignal(GameSignals.SignalName.GameOver);
		}
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

	// Helper function to spawn in players and set their ids
	private Character SpawnPlayer(int pid, Vector2 spawnPos, Color c)
	{
		// should not happen, but just in case
		if (PlayerScene == null)
		{
			GD.PrintErr("PlayerScene is empty");
            return null;
		}

		Character player = PlayerScene.Instantiate<Character>();
		player.PlayerId = pid;
		player.GlobalPosition = spawnPos;

		player.SelfModulate = c;

		AddChild(player);
		return player;
	}
}
