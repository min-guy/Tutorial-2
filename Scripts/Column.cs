using Godot;
using System;

public partial class Column : Node2D
{
	Area2D scoreArea;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scoreArea = GetNode<Area2D>(new NodePath("ScoreArea"));
		scoreArea.BodyEntered += onIntersect;

		var timer = new Timer();
		timer.WaitTime = 4.2;
		timer.OneShot = true;
		timer.Autostart = true;
		timer.Timeout += DestroySelf;

		AddToGroup("collisions");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		this.Position += new Vector2(-10, 0);

	}
	private void onIntersect(Node body)
	{
		if (body is Character character)
		{
			character.incrementScore();
		}
	}

	private void DestroySelf()
	{
		QueueFree();
	}
}
