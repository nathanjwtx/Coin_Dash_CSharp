using Godot;
using System;

public class Main : Node
{
    [Export] public PackedScene Coin;
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
    
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    public override void _Ready()
    {
        Rand = new Random();
        Screensize = GetViewport().GetVisibleRect().Size;
        _Player = (Player) GetNode("Player");
        _Player.Screensize = Screensize;
        _Player.Hide();
        PlayerPosition = (Position2D) GetNode("PlayerStart");
        GameTimer = (Timer) GetNode("GameTimer");
    }

//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }

    public void NewGame()
    {
        Playing = true;
        Level = 1;
        Score = 0;
        Timeleft = Playtime;
        _Player.Start(PlayerPosition.Position);
        _Player.Show();
        GameTimer.Start();
//        SpawnCoins();
    }
}
