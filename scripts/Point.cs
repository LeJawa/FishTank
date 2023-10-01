using Godot;
using System;

public partial class Point : Node2D
{
	public override void _Draw(){
		DrawCircle(new Vector2(0, 0), 3, new Color(1, 1, 1, 1));
	}
}
