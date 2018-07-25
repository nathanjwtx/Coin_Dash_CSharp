using Godot;
using System;

public class PowerUp : Area2D
{
    private Tween _tween;
    private AnimatedSprite _sprite;

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
    
    private void _on_PowerUp_area_entered(Godot.Object area)
    {
        Node2D powerup = (Node2D) area;
        if (powerup.Name == "PowerUp")
        {
            var x = (Int32) ProjectSettings.GetSetting("display/window/size/width");
            var y = (Int32) ProjectSettings.GetSetting("display/window/size/height");
            var rand = new Random();
            Position = new Vector2(rand.Next(0, x), rand.Next(0, y));
        }
    }
    
    private void _on_Timer_timeout()
    {
        QueueFree();
    }
    #endregion
}






