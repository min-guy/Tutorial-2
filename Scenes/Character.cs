using Godot;
using System;
using System.Numerics;

public partial class Character : Area2D
{
	[Export]
	public int JumpSpeed {get; set;} = 400; // the speed of the character when jumping (pixels/sec)

	[Export]
	private Godot.Vector2 FallSpeed {get; set;} = new Godot.Vector2(2, -10); // the speed of the character when jumping (pixels/sec)

	public Godot.Vector2 ScreenSize; // the size of the game window

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Godot.Vector2 velocity = Godot.Vector2.Zero;

	}
}
