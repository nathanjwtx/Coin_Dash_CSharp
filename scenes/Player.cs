using Godot;
using System;

public class Player : Area2D
{
    [Export] public int Speed;
    
    public Vector2 Velocity;
    public Vector2 Screensize = new Vector2(480, 720);

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        GetInput();
        Position += Velocity * delta;
        var pos = Position;
        pos.x = Mathf.Clamp(Position.x, 0, Screensize.x);
        pos.y = Mathf.Clamp(Position.y, 0, Screensize.y);
        AnimatedSprite aniSprite = (AnimatedSprite) GetNode("AnimatedSprite");
        
        if (Velocity.Length() > 0)
        {
            aniSprite.Animation = "run";
            aniSprite.FlipH = Velocity.x < 0;
        }
        else
        {
            aniSprite.Animation = "idle";
        }
    }

    public void GetInput()
    {
        Velocity = new Vector2();
        if (Input.IsActionPressed("ui_left"))
        {
            Velocity.x += -1;
        }

        if (Input.IsActionPressed("ui_right"))
        {
            Velocity.x += 1;
        }

        if (Input.IsActionPressed("ui_up"))
        {
            Velocity.y += -1;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            Velocity.y += 1;
        }

        if (Velocity.Length() > 0)
        {
            Velocity = Velocity.Normalized() * Speed;
        }
    } 
}
