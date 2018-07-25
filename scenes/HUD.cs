using Godot;
using System;
using Object = Godot.Object;

public class HUD : CanvasLayer
{
    [Signal]
    delegate void StartGame();

    private Label _scoreLabel;
    private Label _timerLabel;
    private Label _messageLabel;
    private Timer _messageTimer;
    private Button _start;

    public override void _Ready()
    {
        _scoreLabel = (Label) GetNode("Container/ScoreLabel");
        _timerLabel = (Label) GetNode("Container/TimerLabel");
        _messageLabel = (Label) GetNode("MessageLabel");
        _messageTimer = (Timer) _messageLabel.GetNode("MessageTimer");
        _start = (Button) GetNode("StartButton");
    }

    async public void ShowGameOver()
    {
        ShowMessage("Game Over");
        await ToSignal(_messageTimer, "timeout");
        _start.Show();
        _messageLabel.Text = "Coin Dash!";
        _messageLabel.Show();
    }
    
    #region Signals
    private void _on_MessageTimer_timeout()
    {
        _messageLabel.Hide();
    }
    
    private void _on_StartButton_pressed()
    {
        _start.Hide();
        _messageLabel.Hide();
        EmitSignal("StartGame");
    }
    
    #endregion
    
    #region HUD Updates

    public void ShowMessage(string text)
    {
        _messageLabel.Text = text;
        _messageLabel.Show();
        _messageTimer.Start();
    }
    
    public void UpdateScore(int value)
    {
        _scoreLabel.Text = Convert.ToString(value);
    }

    public void UpdateTimer(float value)
    {
        _timerLabel.Text = Convert.ToString(value);
    }

    #endregion
}

