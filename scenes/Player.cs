using Godot;

public partial class Player : Node2D {
    Vector2 velocity = Vector2.Zero;
    Vector2 carryOn = Vector2.Zero;
    Sprite2D HandSprite;

    bool isPlayerDashing = false;
    float dashLength = 100f;
    Vector2 dashVelocity = Vector2.Zero;
    Vector2 dashPositionStart = Vector2.Zero;
    Vector2 dashPositionEnd = Vector2.Zero;
    Vector2 dashDirection = Vector2.Zero;
    

    public override void _Ready() {
        HandSprite = GetNode<Sprite2D>("Hand");
    }

    public override void _Process(double delta) {
        if (!isPlayerDashing) {
            var direction = Input.GetVector("left", "right", "up", "down");
            direction = direction.Normalized();
            var targetSpeed = direction;
            var acceleration = (float)0.1; // Adjust this value for more or less drift
            velocity = velocity.Lerp(targetSpeed, acceleration);

            var positionBeforeRounding = Position + velocity + carryOn;
            Position = positionBeforeRounding;
        }
        else {
            var dashAcceleration = (float)0.5;
            dashVelocity = dashVelocity.Lerp(dashDirection, dashAcceleration);
            Position += dashVelocity;

            var tol = (float)0.1;
            var diff = (GlobalPosition - dashPositionEnd).Length();
            if (diff <= tol) {
                ResetDashState();
            }
        }

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
        if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo && !isPlayerDashing) {
            switch (keyEvent.Keycode) {
                case Key.Space:
                    SetDashState();
                    break;
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

    private void SetDashState() {
        isPlayerDashing = true;

        var cursorPosition = GetGlobalMousePosition();
        var playerPosition = GlobalPosition;
        dashDirection = (cursorPosition - playerPosition).Normalized();

        dashPositionStart = GlobalPosition;
        dashPositionEnd = dashPositionStart + dashDirection * dashLength;
    }
    private void ResetDashState() {
        isPlayerDashing = false;
        dashVelocity = Vector2.Zero;
    }

    public override void _Draw(){
        DrawAimGuide();
    }
}
