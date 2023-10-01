using Godot;
using System;

public partial class DebugPanel : Panel
{
	PackedScene debugValueScene;
	Container debugValuesContainer;

	float width = 350;
	float heightPerValue = 40;

	public override void _Ready()
	{

		Size = new Vector2(width, 0);

		debugValueScene = GD.Load<PackedScene>("res://scenes/debugValue.tscn");

		debugValuesContainer = GetNode<Container>("VBoxContainer");

		AddDebugValue("Visible Range", Fish.visibleRange, Fish.OnVisibleRangeChanged, 0, 1000);
		AddDebugValue("Protected Range", Fish.protectedRange, Fish.OnProtectedRangeChanged, 0, 500);
		AddDebugValue("Min Speed", Fish.minSpeed, Fish.OnMinSpeedChanged, 0, 1000);
		AddDebugValue("Max Speed", Fish.maxSpeed, Fish.OnMaxSpeedChanged, 0, 1000);
		AddDebugValue("Matching Factor", Fish.matchingFactor, Fish.OnMatchingFactorChanged, 0, 2);
		AddDebugValue("Centering Factor", Fish.centeringFactor, Fish.OnCenteringFactorChanged, 0, 2);
		AddDebugValue("Avoid Factor", Fish.avoidFactor, Fish.OnAvoidFactorChanged, 0, 2);
		AddDebugValue("Turn Factor", Fish.turnfactor, Fish.OnTurnFactorChanged, 0, 2);
		AddDebugValue("Margin", Fish.margin, Fish.OnMarginChanged, 0, 500);
	}

	private void AddDebugValue(string name, double value, Action<double> onValueChanged, double min = 0, double max = 100)
	{
		var debugValue = debugValueScene.Instantiate() as DebugValue;
		debugValuesContainer.AddChild(debugValue);
		debugValue.Name = name.ToPascalCase();
		debugValue.Label = name;
		debugValue.SetMinMaxValues(min, max);
		debugValue.OnValueChanged = onValueChanged;
		debugValue.Value = value;

		Size = new Vector2(Size.X, Size.Y + heightPerValue);
	}
}
