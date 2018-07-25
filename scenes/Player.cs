using Godot;
using System;

public class Player : Area2D
{
    [Signal]
    delegate void Pickup();

    [Signal]
    delegate void Hurt();
        
    [Export] public int Speed;
    
    public Vector2 Velocity;
    public Vector2 Screensize = new Vector2(480, 720);
    public AnimatedSprite AniSprite;

    public override void _Ready()
    {
        AniSprite = (AnimatedSprite) GetNode("AnimatedSprite");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        GetInput();
        Position += Velocity * delta;
        var pos = Position;
        pos.x = Mathf.Clamp(Position.x, 0, Screensize.x);
        pos.y = Mathf.Clamp(Position.y, 0, Screensize.y);
        Position = pos;
        
        
        if (Velocity.Length() > 0)
        {
            AniSprite.Animation = "run";
            AniSprite.FlipH = Velocity.x < 0;
        }
        else
        {
            AniSprite.Animation = "idle";
        }
    }

    #region Gameplay Methods

    public void Start(Vector2 pos)
    {
        SetProcess(true);
        Position = pos;
        AniSprite.Animation = "idle";
    }

    public void Die()
    {
        AniSprite.Animation = "hurt";
        SetProcess(false);
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
    #endregion
    
    #region Signal Calls
    private void _on_Player_area_entered(Godot.Object area)
    {
        Node2D d = (Node2D) area;
        if (d != null && d.IsInGroup("coins"))
        {
            var c = d as Coin;
            c.Pickup();
            EmitSignal("Pickup", "Coin");
        }
        else if (d.Name == "PowerUp")
        {
            var p = (PowerUp) area;
            p.Pickup();
            EmitSignal("Pickup", "PowerUp");
        }
        else if (d.Name == "Cactus")
        {
            EmitSignal("Hurt");
        }

        if (d == null || !IsInGroup("obstacles")) return;
        EmitSignal("Hurt");
        Die();
    }

    #endregion
}
