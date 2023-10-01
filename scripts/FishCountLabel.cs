using Godot;
using System;

public partial class FishCountLabel : Label
{
	public override void _Process(double delta)
	{
		Text = $"Fishes: {FishTank.Instance.Fishes.Count}";
	}
}
