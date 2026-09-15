using Godot;

public partial class Character : RigidBody2D
{
	// Player ID to differentiate between player 1 and 2
	public int PlayerId {get; set;} = 1;
	
	// force of bird flapping
	[Export]
	public float upForce = 200.0f; // Upward force of flapping

	// boolean to store state of player
	public bool isDead = false;

	// reference to the animation sprite node
	private AnimatedSprite2D anim;

	// a score value to keep track of its own score
	private int score = 0;

	private string jumpAct; //saves the keybing for the jump action of each player

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		//set the jump button based on player ID
		jumpAct = $"p{PlayerId}_jump";

		anim = GetNode<AnimatedSprite2D>(new NodePath("AnimatedSprite2D"));

		//Connect the signals to their respective functions

		BodyEntered += OnBodyEntered;
		
		GameSignals.Instance.Restart += restartPlayer;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isDead) return;

		if (Input.IsActionJustPressed(jumpAct))
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

	// Helper function to deal with collisions with collideable objects (columns & floor)
	private void OnBodyEntered(Node body)
	{
		if (isDead) {
			return;
		}

		if (body.IsInGroup("collisions"))
		{
			LinearVelocity = Vector2.Zero;
			isDead = true;
			anim.Play("dead");
		}
	}

	// Helper function to increment score for passing thrpugh the column openings
	public void incrementScore()
	{
		score += 1;
	}

	// Helper function to get the score (used in other classes)
	public int getScore()
	{
		return score;
	}

	// Helper function to set color (used when spawning in player)
	public void SetPlayerColor(Color clr)
	{
		// Should not happen, but if it does, it will get animation sprite node 
		if (anim == null)
		{
			anim = GetNode<AnimatedSprite2D>(new NodePath("AnimatedSprite2D"));
		}

		anim.SelfModulate = clr;
	}

	// Helper function to restart player after death, when they decide to flap
	public void restartPlayer()
	{
		this.score = 0;
		this.isDead = false;

		// reset velocities
		this.LinearVelocity = Vector2.Zero;
		this.AngularVelocity = 0f;

		// Force the physics server to update the transform immediately
   	 	Transform2D newTransform = Transform2D.Identity;
    	newTransform.Origin = new Vector2(960, 540);
    	PhysicsServer2D.BodySetState(
        	GetRid(), 
        	PhysicsServer2D.BodyState.Transform, 
        	newTransform
    	);

		anim.Play("jump");
		anim.Rotation = 0;
				
	}
}
