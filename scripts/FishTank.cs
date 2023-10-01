using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FishTank : Node2D
{
	public static FishTank Instance { get; private set; }

	Vector2I tankSize;

	[Export]
	private PackedScene fishScene;
	private Timer timer;

	public List<Fish> Fishes {get; private set;}

	public int LeftMargin => 0;
	public int RightMargin => tankSize.X;
	public int TopMargin => 0;
	public int BottomMargin => tankSize.Y;

	private Random random = new Random();

    public override void _Ready()
    {
		var size = DisplayServer.WindowGetSize();
		tankSize = new Vector2I(size.X - 100, size.Y - 100);

        // fishScene = GD.Load<PackedScene>(fishScenePath);
		timer = GetNode<Timer>("SpawnTimer");

		Fishes = GetTree().GetNodesInGroup("Fish").Cast<Fish>().ToList();

		Instance = this;

		// Spawn 150 fishes
		for (int i = 0; i < 150; i++)
		{
			AddFishAtRandom();
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		// check if the space bar is pressed
		if (Input.IsActionPressed("click") && timer.IsStopped())
        {
            AddFishAtMouse();
        }
    }

	private void AddFishAtMouse(){
		AddFish(GetGlobalMousePosition());
	}

    private void AddFishAtRandom()
    {
		AddFish(new Vector2(random.Next(LeftMargin, RightMargin), random.Next(TopMargin, BottomMargin)));
    }

    private void AddFish(Vector2 position)
    {        
        // create a new fish
        Fish newFish = fishScene.Instantiate() as Fish;
        // set the fish's position to the mouse position
        newFish.Position = position;
        // add the fish to the scene
        AddChild(newFish);
		// add the fish to the fishes list
		Fishes.Add(newFish);
        // reset the timer
        timer.Stop();
        timer.Start();
    }
}
