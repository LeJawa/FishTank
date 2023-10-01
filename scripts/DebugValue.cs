using Godot;
using System;

public partial class DebugValue : HBoxContainer
{
	HSlider slider;
	Label textLabel;
	ValueLabel valueLabel;

	public override void _Ready(){
		slider = GetNode<HSlider>("HSlider");
		textLabel = GetNode<Label>("TextLabel");
		valueLabel = GetNode<ValueLabel>("ValueLabel");

		slider.ValueChanged += valueLabel.OnValueChanged;
	}


	public string Label
	{
		set
		{
			textLabel.Text = value;
		}
	}

	public double Value
	{
		set
		{
			slider.Value = value;
			valueLabel.Value = value;
		}
	}

	public Action<double> OnValueChanged
	{
		set
		{
			slider.ValueChanged += (double newValue) => value(newValue);
		}
	}

	public void SetMinMaxValues(double min, double max)
	{
		slider.MinValue = min;
		slider.MaxValue = max;
		slider.Step = (max - min) / 100;
	}
}
