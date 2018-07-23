using Godot;
using System;

public class Coin : Area2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        
    }

    public void Pickup()
    {
        GD.Print("hello");
    }
}
