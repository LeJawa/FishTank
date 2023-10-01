using Godot;

public static class Vector2Extension
{

    // Implementation of the Alpha max plus beta min algorithm
    // https://en.wikipedia.org/wiki/Alpha_max_plus_beta_min_algorithm

    private static float alphaNot = 0.960433870103f;
    private static float betaNot = 0.397824734759f;
    public static float FastLength(this Vector2 v)
    {
        return alphaNot * Mathf.Max(Mathf.Abs(v.X), Mathf.Abs(v.Y)) + betaNot * Mathf.Min(Mathf.Abs(v.X), Mathf.Abs(v.Y));        
    }
}