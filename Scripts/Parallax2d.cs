using Godot;
using System;

public partial class Parallax2d : Parallax2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// automatically moves background by 200 px every second
		this.Autoscroll = new Vector2(-200, 0);

		// Links to endLoop function to end background scroll when dead
		GameSignals.Instance.GameOver += endLoop; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void endLoop()
	{
		this.Autoscroll = Vector2.Zero;
	}
}
