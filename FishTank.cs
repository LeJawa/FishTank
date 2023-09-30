using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FishTank : Node2D
{
	public static FishTank Instance { get; private set; }

	Vector2I tankSize;
	private PackedScene fishScene;
	private Timer timer;

	private List<Fish> fishes;

	public int LeftMargin => 0;
	public int RightMargin => tankSize.X;
	public int TopMargin => 0;
	public int BottomMargin => tankSize.Y;

    public override void _Ready()
    {
		var size = DisplayServer.WindowGetSize();
		tankSize = new Vector2I(size.X - 100, size.Y - 100);

        fishScene = GD.Load<PackedScene>("res://fish.tscn");
		timer = GetNode<Timer>("SpawnTimer");

		fishes = GetTree().GetNodesInGroup("Fish").Cast<Fish>().ToList();

		Instance = this;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// check if the space bar is pressed
		if (Input.IsActionPressed("click") && timer.IsStopped())
        {
            AddFish();
        }
    }

	public List<Fish> GetFishes()
	{
		return fishes;
	}

    private void AddFish()
    {
		// get the mouse position
        Vector2 mousePosition = GetGlobalMousePosition();
        // create a new fish
        Fish newFish = fishScene.Instantiate() as Fish;
        // set the fish's position to the mouse position
        newFish.Position = mousePosition;
        // add the fish to the scene
        AddChild(newFish);
		// add the fish to the fishes list
		fishes.Add(newFish);
        // reset the timer
        timer.Stop();
        timer.Start();
    }

}
