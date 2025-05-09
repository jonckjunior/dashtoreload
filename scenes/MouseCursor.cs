using Godot;
using System;

public partial class MouseCursor : Sprite2D
{
    public override void _Ready()
    {
        // capture the mouse
        Input.SetMouseMode(Input.MouseModeEnum.Hidden);
    }
    public override void _Process(double delta)
    {
        var mousePosition = GetGlobalMousePosition();
        Position = mousePosition;
    }
}
