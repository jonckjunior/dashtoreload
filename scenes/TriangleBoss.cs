using Godot;
using System;
using System.Collections.Generic;

public partial class TriangleBoss : Node2D {
    private float floatingSpeed = 2f; // Speed of floating
    private float floatingDistance = 0.1f; // Distance to float up and down
    private float time = 0.0f;
    private float rotationSpeed = 0.7f; // Speed of rotation
    private float lastShotTime = 0.0f;
    
    private List<Marker2D> spawnPoints = new List<Marker2D>();
    private Marker2D centerMarker;

    public override void _Ready() {
        centerMarker = GetNode<Marker2D>("CenterTriangle");
        spawnPoints.Add(GetNode<Marker2D>("SpawnPoint1"));
        spawnPoints.Add(GetNode<Marker2D>("SpawnPoint2"));
        spawnPoints.Add(GetNode<Marker2D>("SpawnPoint3"));
    }

    public override void _Process(double delta) {
        time += (float) delta;
        // Calculate the new Y position using sine wave for floating effect
        float newY = Mathf.Sin(time * floatingSpeed) * floatingDistance;
        // Update the position of the TriangleBoss
        Position = new Vector2(Position.X, Position.Y + newY);

        // Rotate the TriangleBoss
        Rotation += (float)(rotationSpeed * delta);

        HandleShooting(delta);
    }

    private void HandleShooting(double delta) {
        lastShotTime += (float)delta;
        if (lastShotTime > 2.0) { // Every 2 seconds
            foreach (var spawnPoint in spawnPoints) {
                Shoot(spawnPoint.GlobalPosition, spawnPoint.GlobalPosition - centerMarker.GlobalPosition);
            }
            lastShotTime = 0.0f; // Reset the shot timer
        }
    }

    private void Shoot(Vector2 spawnPosition, Vector2 direction) {
        GD.Print("Shooting from: " + spawnPosition);
        // Create a bullet instance
        var bullet = GD.Load<PackedScene>("res://scenes/boss_bullet.tscn").Instantiate<BossBullet>();
        bullet.GlobalPosition = spawnPosition;
        bullet.setDirection(direction.Normalized());
        AddSibling(bullet);
    }
}
