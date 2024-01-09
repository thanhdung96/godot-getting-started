using Godot;

public partial class main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }

	private int score;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void GameOver() {
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("Hud").ShowGameOver();
	}

	public void NewGame() {
		this.score = 0;
		HUD hud = GetNode<HUD>("Hud");
		hud.UpdateScore(score);
		hud.ShowMessage("Get ready");

		player player = GetNode<player>("player");
		Marker2D startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		GetNode<Timer>("StartTimer").Start();
	}

	public void OnStartTimerTimeout() {
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	public void OnScoreTimerTimeout() {
		this.score += 1;
		GetNode<HUD>("Hud").UpdateScore(score);
	}

	public void OnMobTimerTimeout() {
		// create a mob
		mob mob = MobScene.Instantiate<mob>();

		// get spawn location
		PathFollow2D spawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		spawnLocation.ProgressRatio = GD.Randf();

		// direction perpendicular to spawn line
		float direction = spawnLocation.Rotation + Mathf.Pi / 2;
		// more randomness
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);

		// set spawn location
		mob.Position = spawnLocation.Position;
		mob.Rotation = direction;

		// velocity
		Vector2 velocity = new Vector2(GD.RandRange(150, 250), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		// add mob to scene
		AddChild(mob);
	}
}
