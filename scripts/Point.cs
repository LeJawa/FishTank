using Godot;
using System;

public partial class Point : Node2D
{
	public override void _Draw(){
		// DrawCircle(new Vector2(0, 0), 3, new Color(1, 1, 1, 1));
		DrawRect(new Rect2(Vector2.Zero, Vector2.One * 2), new Color(1, 1, 1, 1));
	}
}
