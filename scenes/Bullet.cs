using Godot;
using System;

public partial class Bullet : Node2D
{
    private Vector2 velocity = Vector2.Zero;
    private float speed = 500f; // Speed of the bullet
    private float lifetime = 2f; // Lifetime of the bullet in seconds

    public override void _Ready()
    {
        // Set the bullet's initial velocity based on its rotation
        velocity = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)) * speed;

        // Schedule the bullet to be removed after its lifetime
        GetTree().CreateTimer(lifetime).Connect("timeout", Callable.From(OnBulletTimeout));
    }

    public override void _Process(double delta)
    {
        Position += velocity * (float)delta;
    }

    private void OnBulletTimeout()
    {
        QueueFree();
    }
}
