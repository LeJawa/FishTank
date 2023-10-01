using Godot;
using System;

public partial class ValueLabel : Label
{
	public override void _Ready()
	{
		Text = "0";
	}

	public void OnValueChanged(double value){
		Value = value;
	}

	public double Value { set{
		Text = value.ToString("0.0000");
	}}
}
