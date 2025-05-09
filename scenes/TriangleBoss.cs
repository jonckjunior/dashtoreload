using Godot;
using System;

public partial class TriangleBoss : Node2D {
    private float floatingSpeed = 2f; // Speed of floating
    private float floatingDistance = 0.1f; // Distance to float up and down
    private float time = 0.0f;
    private float rotationSpeed = 0.5f; // Speed of rotation

    public override void _Process(double delta) {
        time += (float) delta;
        // Calculate the new Y position using sine wave for floating effect
        float newY = Mathf.Sin(time * floatingSpeed) * floatingDistance;
        // Update the position of the TriangleBoss
        Position = new Vector2(Position.X, Position.Y + newY);

        // Rotate the TriangleBoss
        Rotation += (float)(rotationSpeed * delta);
    }

}
