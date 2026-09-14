using Godot;

public partial class Character : RigidBody2D
{
	[Export]
	public float upForce = 200.0f; // Upward force of flapping

	private bool isDead = false;

	private AnimatedSprite2D anim;

	private int score = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		anim = GetNode<AnimatedSprite2D>(new NodePath("AnimatedSprite2D"));

		//Connect the signals to their respective functions
		GameSignals.Instance.KillPlayer += Die;

		BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isDead) return;

		if (Input.IsActionJustPressed("jump"))
		{
			anim.Play("jump");
			// Forcing it to always restart (instead of continuing the animation while continually pressing jump)
			anim.Frame = 0;

			anim.Rotation = Mathf.DegToRad(-20);

			// Apply upward force to the flappy bird.
			LinearVelocity = new Vector2(0, -upForce);
		}

		anim.Rotation = Mathf.Lerp(anim.Rotation, Mathf.DegToRad(30), (float)delta * 1.5f);
	}

	private void OnBodyEntered(Node body)
	{
		if (isDead) {
			return;
		}

		if (body.IsInGroup("collisions"))
		{
			// emit signal that player has died
			GameSignals.Instance.EmitSignal(GameSignals.SignalName.KillPlayer);
		}
	}

	public void Die()
	{
		LinearVelocity = Vector2.Zero;

		isDead = true;

		anim.Play("dead");
	}

	public void incrementScore()
	{
		score += 100;
		GD.Print(score);
	}
}
