using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Scene;
using Silesian_Undergrounds.Engine.Components;
using System.Collections.Generic;

namespace Silesian_Undergrounds.Engine.Particles
{
  public class Particle : GameObject
  {
    public event EventHandler OnParticleTravelEnd = delegate { };
    public event EventHandler<CollisionNotifyData> OnParticleHit = delegate { };

    private Vector2 moveForce;
    private float travelSpeed;
    private float travelDist;
    // Object which emits particle
    private GameObject emiter;
    private BoxCollider collider;
    private Vector2 startingPos;
    private bool isWaitingForDelete;
    private bool isLaunched;
    public Animator Animator { get; private set; }

    public Particle(string textureName, float w, float h, Vector2 startPos, Vector2 force, float travelSpeed, float travelDist, GameObject emiter)
        : base(null, startPos, new Vector2(w * ResolutionMgr.TileSize, h * ResolutionMgr.TileSize))
    {
      texture = TextureMgr.Instance.GetTexture(textureName);
      moveForce = force;
      this.travelSpeed = travelSpeed;
      this.travelDist = travelDist * ResolutionMgr.xAxisUnit;
      this.emiter = emiter;
      startingPos = new Vector2(startPos.X, startPos.Y);

      collider = new BoxCollider(this, size.X, size.Y, 0, 0, true, true);
      AddComponent(collider);

      isWaitingForDelete = false;
      isLaunched = false;

      Animator = new Animator(this);
      AddComponent(Animator);
      ChangeDrawAbility(false);
    }

    public void Launch()
    {
      if (!isLaunched)
      {
        isLaunched = true;
        SceneManager.GetCurrentScene().AddObject(this);
      }
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      if (isWaitingForDelete)
      {
        this.ScheduleDelete();
        return;
      }

      collider.Move(moveForce * travelSpeed);

      float dist = Vector2.Distance(startingPos, position);
      if (dist >= travelDist)
      {
        OnParticleTravelEnd.Invoke(this, null);
        Animator.StopAnimation();
        ScheduleDelete();
      }
    }

    public override void NotifyCollision(GameObject gameobject, ICollider source, RectCollisionSides collisionSides)
    {
      if (gameobject == emiter || isWaitingForDelete)
        return;
      Animator.StopAnimation(); // animation should be stopped otherwise the particle will be not deleted
      OnParticleHit.Invoke(this, new CollisionNotifyData(gameobject, source, collisionSides)); // you can add new animation on hit, then the particle will be not deleted till new animation end
      OnParticleTravelEnd.Invoke(this, null);
      ScheduleDelete();
    }

    public void AddAndPlayOnHitAnimation(List<Texture2D> textures, int animDuration) => this.Animator.AddAndPlayAnimation("OnHit", textures, animDuration, repeatable: false, useFirstFrameAsTexture: false, isPermanent: true);
    public void AddAndPlayOnHitAnimation(AnimationConfig animationConfig) => this.Animator.AddAndPlayAnimation(animationConfig);

    private void ScheduleDelete()
    {
      isWaitingForDelete = true;
      // TODO: swap this code with proper handler working wiht on death anim
      if (!Animator.isPlaying)
        SceneManager.GetCurrentScene().DeleteObject(this);
    }
  }
}
