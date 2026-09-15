using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class GameManager : Node2D
{
	// Scene for birds
	[Export] private PackedScene PlayerScene {get; set;}
	
	// Scene for column
	private PackedScene columnScene;
	
	//column location to know where columns should start spawning from
	private Vector2 spawnColumnLocation;

	// Score text
	private Label text;

	// Game Over Message UI
	private Control gameOverMsg;

	// Player references
	private Character p1;
	private Character p2;

	// State of the game
	private bool end = false; // true when both players have died
	
	// Timer for spawning columns
	private Timer timer;

	public override void _Ready()
	{
		// Sets up timer to spawn a column every 1.3 seconds
		timer = new Timer();
		timer.WaitTime = 1.3;
		timer.Timeout += SpawnColumn;
		timer.OneShot = false;
		timer.Autostart = true;

		// assigns columnScene with the scene 
		columnScene = ResourceLoader.Load<PackedScene>("uid://cwkpgyp5takwg");

		// gets the position of where to place columns
		var spawnColumnLocationNode = GetNode<Node2D>("SpawnColumnLocation");
		spawnColumnLocation = spawnColumnLocationNode.Position;

		AddChild(timer);

		// Get score text node
		text = GetNode<Label>(new NodePath("CanvasLayer/Score"));

		// Gets Game over Message nodes and sets the visibility to false
		gameOverMsg = GetNode<Control>(new NodePath("CanvasLayer/Control"));
		gameOverMsg.Visible = false;

		// Spawn the players in
		p1 = SpawnPlayer(1, new Vector2(960, 540), Colors.White);
		p2 = SpawnPlayer(2, new Vector2(960, 540), Colors.Orange);

		// Attach signal to update Score
		GameSignals.Instance.ScoreUpdate += updateScore;
	}

    public override void _Process(double delta)
    {	
		// when both players have died, set game state, show game over message, and stop spawning columns
		if (!end && p1.isDead && p2.isDead)
		{
			end = true;
			timer.Stop(); // Stops timer (as in resets it and doesnt count down)
			gameOverMsg.Visible = true;
			GameSignals.Instance.EmitSignal(GameSignals.SignalName.GameOver);
		}

		// restart logic check, when a player "flaps", sends out a signal to restart the game
		if (end && (Input.IsActionJustPressed("p1_jump") || Input.IsActionJustPressed("p2_jump"))) {
			timer.Start(); // Starts timer 
			gameOverMsg.Visible = false;
			end = false;
			GameSignals.Instance.EmitSignal(GameSignals.SignalName.Restart);
			updateScore();

		}
    }

	// Helper function to spawn in columns
	private void SpawnColumn()
	{
		var column = columnScene.Instantiate<Column>();

		var height = GD.RandRange(220, 670);

		column.Position = new Vector2(spawnColumnLocation.X, height);

		AddChild(column);
	}

	// helper function to update the score text
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
			GD.Print(pid + "is null");
            return null;
		}

		Character player = PlayerScene.Instantiate<Character>();
		player.PlayerId = pid;
		player.GlobalPosition = spawnPos;


		player.SetPlayerColor(c);

		AddChild(player);
		return player;
	}
}
