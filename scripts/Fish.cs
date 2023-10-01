using Godot;
using System;

// Boid Algorithm based on this article by V. Hunter Adams
// https://vanhunteradams.com/Pico/Animal_Movement/Boids-algorithm.html

public partial class Fish : Node2D
{
	public Vector2 Velocity = new Vector2(0, 0);

	public static float visibleRange = 40;
	public static float protectedRange = 11;
	public static float minSpeed = 3;
	public static float maxSpeed = 6;
	public static float matchingFactor = 0.008f;
	public static float centeringFactor = 0.0004f;
	public static float avoidFactor = 0.04f;
	public static float turnfactor = 0.2f;
	public static float margin = 150;

	public static float visibleRangeSquared = visibleRange * visibleRange;
	public static float protectedRangeSquared = protectedRange * protectedRange;

	public static void OnVisibleRangeChanged(double value){
		visibleRange = (float)value;
		UpdateGizmos();
	}
	public static void OnTurnFactorChanged(double value){
    turnfactor = (float)value;
	}

	public static void OnMarginChanged(double value){
		margin = (float)value;
	}

	public static void OnProtectedRangeChanged(double value){
		protectedRange = (float)value;
		UpdateGizmos();
	}

	public static void OnAvoidFactorChanged(double value){
		avoidFactor = (float)value;
	}

	public static void OnMatchingFactorChanged(double value){
		matchingFactor = (float)value;
	}

	public static void OnCenteringFactorChanged(double value){
		centeringFactor = (float)value;
	}

	public static void OnMinSpeedChanged(double value){
		minSpeed = (float)value;
	}

	public static void OnMaxSpeedChanged(double value){
		maxSpeed = (float)value;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Velocity = GetRandomVelocity();
	}

	private Vector2 GetRandomVelocity(){
		var rand = new Random();
		var x = (float)rand.NextDouble() * 2 - 1;
		var y = (float)rand.NextDouble() * 2 - 1;
		
		return new Vector2(x, y) * maxSpeed;
	}

	public override void _PhysicsProcess(double delta)
	{
		CalculateForces();

		Velocity += Separation;
		Velocity += Cohesion;
		Velocity += Alignment;
		Velocity += BoundaryReflection;

		ImposeSpeedLimits();


		Position += Velocity * (float)delta * 50;
	}


	// Speed limits
	// We constrain the boids to move faster than some minimum speed and slower than some maximum speed. We do so in the following way:
    // Once the velocity has been updated, compute the boid speed
    // speed = sqrt(boid.vx*boid.vx + boid.vy*boid.vy)
    // If speed>maxspeed:
    // boid.vx = (boid.vx/speed)*maxspeed
    // boid.vy = (boid.vy/speed)*minspeed
    // If speed<minspeed:
    // boid.vx = (boid.vx/speed)*minspeed
    // boid.vy = (boid.vy/speed)*minspeed

    private void ImposeSpeedLimits()
    {
		var speed = Velocity.FastLength();

		if (speed > maxSpeed){
			Velocity *= maxSpeed / speed;
		}
		else if (speed < minSpeed){
			Velocity *= minSpeed / speed;
		}

    }

    // Alignment
    // Each boid attempts to match the velocity of other boids inside its visible range. It does so in the following way:
    // At the start of the update for a particular boid, three variables (xvel_avg, yvel_avg, and neighboring_boids) are zeroed
    // We loop thru every other boid. If the distance to a particular boid is less than the visible range, then
    // xvel_avg += otherboid.vx
    // yvel_avg += otherboid.vy
    // neighboring_boids += 1
    // Once we've looped through all other boids, we do the following if neighboring_boids>0:
    // xvel_avg = xvel_avg/neighboring_boids
    // yvel_avg = yvel_avg/neighboring_boids
    // We then update the velocity according to:
    // boid.vx += (xvel_avg - boid.vx)*matchingfactor
    // boid.vy += (yvel_avg - boid.vy)*matchingfactor
    // (where matchingfactor is a tunable parameter)

    private Vector2 Alignment { get; set; } = Vector2.Zero;
	
	// Cohesion
	// Each boid steers gently toward the center of mass of other boids within its visible range. 
	// It does so in the following way:
	// At the start of the update for a particular boid, three variables (xpos_avg, ypos_avg, and neighboring_boids) are zeroed
	// We loop thru every other boid. If the distance to a particular boid is less than the visible range, then
	// xpos_avg += otherboid.x
	// ypos_avg += otherboid.y
	// neighboring_boids += 1
	// Once we've looped through all other boids, we do the following if neighboring_boids>0:
	// xpos_avg = xpos_avg/neighboring_boids
	// ypos_avg = ypos_avg/neighboring_boids
	// We then update the velocity according to:
	// boid.vx += (xpos_avg - boid.x)*centeringfactor
	// boid.vy += (ypos_avg - boid.y)*centeringfactor
	// (where centeringfactor is a tunable parameter)

	private Vector2 Cohesion { get; set; } = Vector2.Zero;

	// Separation
	// Each boid attempts to avoid running into other boids.
	// If two or more boids get too close to one another (i.e. within one another's protected range), 
	// they will steer away from one another. They will do so in the following way:
    // At the start of the update for a particular boid, two accumulating variable (close_dx and close_dy) are zeroed
    // We loop thru every other boid. If the distance to a particular boid is less than the protected range, then
    // close_dx += boid.x - otherboid.x
    // close_dy += boid.y - otherboid.y.
    // Once we've looped through all other boids, then we update the velocity according to
    // boid.vx += close_dx*avoidfactor
    // boid.vy += close_dy*avoidfactor
    // (where avoidfactor is a tunable parameter)

	private Vector2 Separation { get; set; } = Vector2.Zero;

	private void CalculateForces() {
		Separation = new Vector2(0, 0);
		Cohesion = new Vector2(0, 0);
		Alignment = new Vector2(0, 0);
		int neighborCount = 0;
		foreach (var otherFish in FishTank.Instance.Fishes)
		{
			if (otherFish == this)
				continue;			
			
			var distance2 = otherFish.GlobalPosition.DistanceSquaredTo(GlobalPosition);
			
			if (distance2 > visibleRangeSquared)
				continue;

			if (distance2 < protectedRangeSquared)
			{
				Separation += GlobalPosition - otherFish.GlobalPosition;
				continue;
			}

			Cohesion += otherFish.GlobalPosition;
			Alignment += otherFish.Velocity;
			neighborCount++;
		}

		Separation *= avoidFactor; // scale

		if (neighborCount == 0)
			return;
		
		Cohesion /= neighborCount; // average
		Cohesion -= GlobalPosition; // difference with current position
		Cohesion *= centeringFactor; // scale

		Alignment /= neighborCount; // average
		Alignment -= Velocity; // difference with current velocity
		Alignment *= matchingFactor; // scale
	}
	// Screen edges
	// We want our boids to turn-around at an organic-looking turn radius when they approach an edge of the TFT. 
	// We will do so in the following way:
	// if boid.x < leftmargin:
	// 	boid.vx = boid.vx + turnfactor
	// if boid.x > rightmargin:
	// 	boid.vx = boid.vx - turnfactor
	// if boid.y > bottommargin:
	// 	boid.vy = boid.vy - turnfactor
	// if boid.y < topmargin:
	// 	boid.vy = boid.vy + turnfactor
	// where turnfactor and all margins are tunable parameters. I recommend a margin of 100 pixels on all edges.

    private Vector2 BoundaryReflection{ get {
    {
		Vector2 reflection = new Vector2(0, 0);

        if (GlobalPosition.X - FishTank.Instance.LeftMargin < margin)
			reflection += Vector2.Right;
		if (FishTank.Instance.RightMargin - GlobalPosition.X < margin )
			reflection += Vector2.Left;
		if (GlobalPosition.Y - FishTank.Instance.TopMargin < margin)
			reflection += Vector2.Down;
		if (FishTank.Instance.BottomMargin - GlobalPosition.Y < margin)
			reflection += Vector2.Up;

		reflection *= turnfactor; // scale

		return reflection;
    }}}

	private static void UpdateGizmos(){
		if (!debugGizmos)
			return;
		
		foreach (var fish in FishTank.Instance.Fishes)
		{
			fish.QueueRedraw();
		}
	}

	static bool debugGizmos = false;


	public override void _Draw(){
		if (!debugGizmos)
			return;
		
		DrawCircle(new Vector2(0, 0), protectedRange, new Color(1, 0, 0, 0.2f));
		DrawCircle(new Vector2(0, 0), visibleRange, new Color(0, 1, 0, 0.2f));
	}
}
