using Godot;
using System;

public partial class Player : Node2D {
    Vector2 velocity = Vector2.Zero;
    Vector2 carryOn = Vector2.Zero;
    Sprite2D HandSprite;
    

    public override void _Ready() {
        HandSprite = GetNode<Sprite2D>("Hand");
    }

    public override void _Process(double delta) {
        var direction = Input.GetVector("left", "right", "up", "down");
        direction = direction.Normalized();
        var targetSpeed = direction;
        var acceleration = (float)0.1; // Adjust this value for more or less drift
        velocity = velocity.Lerp(targetSpeed, acceleration);

        var positionBeforeRounding = Position + velocity + carryOn;
        Position = positionBeforeRounding;

        LookAt(GetGlobalMousePosition());
    
        QueueRedraw();
    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseButton mouseButtonEvent) {
            if (mouseButtonEvent.IsPressed() && mouseButtonEvent.ButtonIndex == MouseButton.Right) {
                GlobalPosition = GetGlobalMousePosition();
            }
            if (mouseButtonEvent.IsPressed() && mouseButtonEvent.ButtonIndex == MouseButton.Left) {
                var bullet = GD.Load<PackedScene>("res://scenes/Bullet.tscn").Instantiate<Bullet>();
                bullet.GlobalPosition = HandSprite.GlobalPosition;
                bullet.Rotation = Rotation;
                AddSibling(bullet);
            }
        }
    }

    private void DrawAimGuide(){
        // Start from the center of this node (e.g., the player)
        Vector2 start = HandSprite.Position;

        // End at the global mouse position
        float end = (GetGlobalMousePosition() - GlobalPosition).Length();

        // Draw the line
        Color lineColor = Colors.White;
        lineColor.A = 0.5f; // Set the alpha value to 0.5 for transparency
        DrawLine(start, new Vector2(end, 0), lineColor, 2f); // Red line, 2 pixels wide
    }

    public override void _Draw(){
        DrawAimGuide();
    }
}
