using Godot;
using System;

public partial class UILayer : CanvasLayer
{
	private Label score;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score = GetNode<Label>(new NodePath("Label"));

		//Connect ScoreUpdate signal to the update text function
		GameSignals.Instance.ScoreUpdate += updateScore;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void updateScore()
	{
		score.Text = "Score: ";
	}
}
