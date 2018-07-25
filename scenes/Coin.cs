using Godot;
using System;

public class Coin : Area2D
{
    private Tween _tween;
    private AnimatedSprite _sprite;
    private Timer _animationTimer;

    public override void _Ready()
    {
        Color cStart = new Color(1.0f, 1.0f, 1.0f);
        Color cEnd = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _tween = (Tween) GetNode("Tween");
        _sprite = (AnimatedSprite) GetNode("AnimatedSprite");
        _tween.InterpolateProperty(_sprite, "scale", _sprite.Scale, _sprite.Scale * 3, Convert.ToSingle(0.3),
            Tween.TransitionType.Quad, Tween.EaseType.InOut);
        _tween.InterpolateProperty(_sprite, "modulate", cStart, cEnd, Convert.ToSingle(0.3),
            Tween.TransitionType.Quad, Tween.EaseType.InOut);
        _animationTimer = (Timer) GetNode("Timer");
        _animationTimer.WaitTime = new Random().Next(3, 9);
        _animationTimer.Start();
    }

    public void Pickup()
    {
        Monitoring = false;
        _tween.Start();
    }
    
    #region Signals
    private void _on_Tween_tween_completed(Godot.Object @object, NodePath key)
    {
        QueueFree();
    }
    
    private void _on_Timer_timeout()
    {
        _sprite.Frame = 0;
        _sprite.Play();
    }
    
    private void _on_Coin_area_entered(Godot.Object area)
    {
        Node2D cactus = (Node2D) area;
        if (cactus.Name == "Cactus")
        {
            var x = (int) ProjectSettings.GetSetting("display/window/size/width");
            var y = (int) ProjectSettings.GetSetting("display/window/size/height");
            var rand = new Random();
            Position = new Vector2(rand.Next(0, x), rand.Next(0, y));
        }
    }
    #endregion
}
