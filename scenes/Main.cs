using Godot;
using System;

public class Main : Node
{
    [Export] public PackedScene CoinScene;
    [Export] public int Playtime;

    public int Level;
    public int Score;
    public float Timeleft;
    public Vector2 Screensize;
    public bool Playing = false;
    public Random Rand;
    public Player _Player;
    private Position2D PlayerPosition;
    private Timer GameTimer;
    private Node Coins;

    public override void _Ready()
    {
        Rand = new Random();
        Screensize = GetViewport().GetVisibleRect().Size;
        _Player = (Player) GetNode("Player");
        _Player.Screensize = Screensize;
        _Player.Hide();
        PlayerPosition = (Position2D) GetNode("PlayerStart");
        GameTimer = (Timer) GetNode("GameTimer");
        Coins = GetNode("CoinContainer");
        NewGame();
    }

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
        for (int i = 0; i < Level + 4; i++)
        {
            var c = (Coin) CoinScene.Instance();
            Coins.AddChild(c);
            c.GlobalPosition = new Vector2(Rand.Next(0, Convert.ToInt32(Screensize.x)), 
                Rand.Next(0, Convert.ToInt32(Screensize.y)));
            
        }
    }
}
