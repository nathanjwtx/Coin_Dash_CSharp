using Godot;
using System;

public class Main : Node
{
    [Export] public PackedScene CoinScene;
    [Export] public PackedScene PowerUpScene;
    [Export] public int Playtime;

    public int Level;
    public int Score;
    public float Timeleft;
    public Vector2 Screensize;
    public bool Playing = false;
    public Random Rand;
    public Player _Player;
    private HUD _HUD;
    private Position2D PlayerPosition;
    private Timer GameTimer;
    private Timer PowerUpTimer;
    private Node Coins;

    public override void _Ready()
    {
        Rand = new Random();
        Screensize = GetViewport().GetVisibleRect().Size;
        _Player = (Player) GetNode("Player");
        _Player.Screensize = Screensize;
        _Player.Hide();
        _HUD = (HUD) GetNode("HUD");
        _HUD.UpdateScore(Score);
        _HUD.UpdateTimer(Timeleft);
        PlayerPosition = (Position2D) GetNode("PlayerStart");
        GameTimer = (Timer) GetNode("GameTimer");
        PowerUpTimer = (Timer) GetNode("PowerUpTimer");
        Coins = GetNode("CoinContainer");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Playing && Coins.GetChildCount() == 0)
        {
            Level += 1;
            Timeleft += 5;
            SpawnCoins();
            PowerUpTimer.WaitTime = Rand.Next(5, 10);
            PowerUpTimer.Start();
        }
    }

    #region Game Play
    public void NewGame()
    {
        Playing = true;
        Level = 1;
        Score = 0;
        Timeleft = Playtime;
        _Player.Start(PlayerPosition.Position);
        _Player.Show();
        GameTimer.Start();
        SpawnCoins();
    }

    public void SpawnCoins()
    {
        AudioStreamPlayer a = (AudioStreamPlayer) GetNode("LevelSound");
        for (int i = 0; i < Level + 4; i++)
        {
            var c = (Coin) CoinScene.Instance();
            Coins.AddChild(c);
            a.Play();
            c.GlobalPosition = new Vector2(Rand.Next(0, Convert.ToInt32(Screensize.x)), 
                Rand.Next(0, Convert.ToInt32(Screensize.y)));
        }
    }

    private void GameOver()
    {
        Playing = false;
        GameTimer.Stop();
        foreach (Coin coin in Coins.GetChildren())
        {
            coin.QueueFree();
        }

        AudioStreamPlayer a = (AudioStreamPlayer) GetNode("EndSound");
        a.Play();
        _HUD.ShowGameOver();
        _Player.Die();
    }
    #endregion
    
    #region Signals
    private void _on_GameTimer_timeout()
    {
        Timeleft += -1;
        _HUD.UpdateTimer(Timeleft);
        if (Timeleft <= 0)
        {
            GameOver();
        }
    }
 
    private void _on_Player_Hurt()
    {
        GameOver();
    }

    private void _on_Player_Pickup(string objType)
    {
        switch (objType)
        {
            case "Coin":
                Score += 1;
                AudioStreamPlayer a = (AudioStreamPlayer) GetNode("CoinSound");
                a.Play();
                _HUD.UpdateScore(Score);
                break;
            case "PowerUp":
                Timeleft += 5;
                AudioStreamPlayer p = (AudioStreamPlayer) GetNode("PowerUpSound");
                p.Play();
                _HUD.UpdateTimer(Timeleft);
                break;
        }
    }
    
    private void _on_HUD_StartGame()
    {
        // Replace with function body
    }
    
    private void _on_PowerUpTimer_timeout()
    {
        var p = (PowerUp) PowerUpScene.Instance();
        AddChild(p);
        p.GlobalPosition = new Vector2(Rand.Next(0, Convert.ToInt32(Screensize.x)), 
            Rand.Next(0, Convert.ToInt32(Screensize.y)));
    }
    #endregion
}
