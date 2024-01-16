using System.Diagnostics;
using Godot;

public partial class player : Area2D
{
	[Export]
	public int Speed { get; set; } = 400;

	private bool disableInput;

	public Vector2 ScreenSize;

	[Signal]
	public delegate void HitEventHandler();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!disableInput)
		{
			calculateMovement(delta);
		}
	}

	public void OnBodyEntered(Node2D body)
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dead");
		EmitSignal(SignalName.Hit);
		CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		SetCollisionMaskValue(1, false);
		this.disableInput = true;
	}

	public void Start(Vector2 position)
	{
		Position = position;
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("walk");
		SetCollisionMaskValue(1, true);
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
		this.disableInput = false;
	}

	private void calculateMovement(double delta)
	{
		// calculate direction
		Vector2 velocity = Vector2.Zero;
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}
		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}
		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		AnimatedSprite2D playerSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			playerSprite2D.Play();
		}
		else
		{
			playerSprite2D.Stop();
		}

		// change positiion of character
		Position += velocity * (float)delta;

		// limit the position to screen only
		Position = new Vector2(
			Mathf.Clamp(Position.X, 0, ScreenSize.X),
			Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);

		// flip image
		if (velocity.X != 0)
		{
			playerSprite2D.Animation = "walk";
			playerSprite2D.FlipV = false;
			playerSprite2D.FlipH = velocity.X < 0;
		}
		if (velocity.Y != 0)
		{
			playerSprite2D.Animation = "up";
			playerSprite2D.FlipV = velocity.Y > 0;
			playerSprite2D.FlipH = velocity.X < 0;
		}
	}
}
