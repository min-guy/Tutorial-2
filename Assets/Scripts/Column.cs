using Godot;
using System;

public partial class Column : Node2D
{
	Area2D scoreArea; //scores the node of the scoring area

	private bool isDestroyed; // bool to just c=double check and avoid freeing twice

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// connect onIntersect function with the collision of the scoring area to increase points
		scoreArea = GetNode<Area2D>(new NodePath("ScoreArea"));
		scoreArea.BodyEntered += onIntersect;

		//setup timer to autodelete object after it has passed the field
		var timer = new Timer();
		timer.WaitTime = 4.2;
		timer.OneShot = true;
		timer.Autostart = true;
		timer.Timeout += DestroySelf;

		// Add timer to tree
		AddChild(timer);

		GameSignals.Instance.GameOver += DestroySelf; // kills itself when game over

		AddToGroup("collisions"); // collision tag
	}

	// moves to the left by 10 every physics process
	public override void _PhysicsProcess(double delta)
	{
		this.Position += new Vector2(-10, 0);

	}

	// Helper function to handle increasing the player points when passing through the column opening
	private void onIntersect(Node body)
	{
		if (body is Character character)
		{
			character.incrementScore();
			GameSignals.Instance.EmitSignal(GameSignals.SignalName.ScoreUpdate);
		}
	}

	// helper function to free itself when the game is over or after it has passed the playing area 
	private void DestroySelf()
	{
		if (isDestroyed) return;
		
		isDestroyed = true;
		QueueFree();
	}
}
