using Godot;

public partial class mob : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimatedSprite2D mobSprite2d = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = mobSprite2d.SpriteFrames.GetAnimationNames();
		mobSprite2d.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnVisibleOnScreenNotifier2DScreenExited() {
		QueueFree();
	}
}
