using Godot;
using System;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ShowMessage(string message) {
		Label lblMessage = GetNode<Label>("MessageLabel");
		lblMessage.Text = message;
		lblMessage.Show();

		GetNode<Timer>("MessageTimer").Start();
	}

	async public void ShowGameOver() {
		ShowMessage("Game over");

		Timer messageTimer = GetNode<Timer>("MessageTimer");
		// wait for message timer to finish
		await ToSignal(messageTimer, Timer.SignalName.Timeout);

		Label lblMessage = GetNode<Label>("MessageLabel");
		lblMessage.Text = "Dodge the Creeps";
		lblMessage.Show();

		// create a timer in the scene and wait for it to finish
		await ToSignal(GetTree().CreateTimer(1), SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("StartButton").Show();
	}

	public void UpdateScore(int score) {
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}

	public void OnMessageTimerTimeout() {
		Label lblMessage = GetNode<Label>("MessageLabel");
		lblMessage.Hide();
	}

	public void OnStartButtonPressed() {
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
	}
}
