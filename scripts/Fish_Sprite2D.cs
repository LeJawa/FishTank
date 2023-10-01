using Godot;
using System;

public partial class Fish_Sprite2D : Sprite2D
{
	Fish fish;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fish = GetParent<Fish>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (fish.Velocity.X < 0){
			FlipH = true;
		} else {
			FlipH = false;
		}
	}
}
