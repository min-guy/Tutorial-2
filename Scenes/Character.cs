using Godot;
using System;
using System.Numerics;

public partial class Character : Area2D
{
	[Export]
	public float upForce = 40f; // Upward force of flap

	private bool isDead = false;

	private AnimatedSprite2D anim;

	private CollisionShape2D coll;

	public Godot.Vector2 ScreenSize; // the size of the game window

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		
		anim = GetNode<AnimatedSprite2D>(new NodePath("AnimatedSprite2D"));

		coll = GetNode<CollisionShape2D>(new NodePath("CollisionShape2D"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(isDead == false)
		{
			if (Input.IsActionJustPressed("jump"))
			{
				anim.Play("jump");

			}
		}
		Godot.Vector2 velocity = Godot.Vector2.Zero;

	}
}
