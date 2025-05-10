using Godot;
using System;

public partial class BossBullet : Sprite2D
{
    private Vector2 Direction = Vector2.Right;

    public void setDirection(Vector2 direction){
        Direction = direction;
    }

    public override void _Process(double delta){
        Position += Direction * 5f * (float)delta;
    }

}
