using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Components;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.Enum;
using Silesian_Undergrounds.Engine.Particles;

namespace Silesian_Undergrounds.Engine.Behaviours
{
  public enum AnimType
  {
    ON_DEATH,
    ON_ATTACK,
    ON_MOVE_RIGHT,
    ON_MOVE_LEFT,
    ON_HIT_LEFT,
    ON_HIT_RIGHT,
  }
  internal static class AnimNames
  {
    internal const string ON_DEATH = "Death";
    internal const string ON_ATTACK = "Attack";
    internal const string ON_MOVE_RIGHT = "MoveRight";
    internal const string ON_MOVE_LEFT = "MoveLeft";
    internal const string ON_HIT_LEFT = "OnHitLeft";
    internal const string ON_HIT_RIGHT = "OnHitRight";
  }

  public class HostileBehaviour : IComponent
  {
    private enum PosComparisionSide
    {
      CORNER_LEFT,
      CORNER_RIGHT,
      CORNER_BOTTOM_LEFT,
      CORNER_BOTTOM_RIGHT,
    }

    // Component inherited
    public Vector2 Position { get; set; }
    public Rectangle Rect { get; set; }

    public GameObject Parent { get; private set; }

    // HostileBehaviour specific
    private bool IsInCombat;
    private GameObject enemy;
    private BoxCollider enemyCollider;
    private CircleCollider aggroArea;
    private BoxCollider collider;
    private bool IsMoveNeeded;
    private TimedEventsScheduler events;
    private float MinDistToEnemy;
    AttackPattern attackPattern;
    private float BonusMoveSpeed;
    private int health;
    private int maxHealth;
    private int moneyReward;
    private Animator Animator { get; set; }
    private MovementDirectionEnum currentDirection;
    private MovementDirectionEnum previousDirection;
    private bool isMovementLockedByAnim;
    private bool isMovingOnPath;
    private int currentPathNode;
    private List<Vector2> waypath;
    private Vector2 collisionDerivedMoveForce;
    private Vector2 lastMoveForce;
    private PosComparisionSide posComparisionSide;

    public HostileBehaviour(GameObject parent, AttackPattern pattern, int health, int moneyRew, float bonusMoveSpeed = 0.0f, float minDist = 1)
    {
      Parent = parent;
      Position = new Vector2(0, 0);
      Rect = new Rectangle(0, 0, 0, 0);

      IsInCombat = false;
      enemy = null;
      IsMoveNeeded = false;
      MinDistToEnemy = minDist;
      BonusMoveSpeed = bonusMoveSpeed;

      aggroArea = new CircleCollider(Parent, 70, 0, 0, true);
      aggroArea.MarkAsAggroArea();
      collider = new BoxCollider(Parent, 70, 70, 0, 0, false);
      Parent.AddComponent(collider);
      Parent.AddComponent(aggroArea);
      Parent.OnCollision += NotifyCollision;

      events = new TimedEventsScheduler();
      attackPattern = pattern;

      this.health = health;
      maxHealth = health;
      moneyReward = moneyRew;

      Animator = new Animator(Parent);
      Parent.AddComponent(Animator);
      Animator.OnAnimationEnd += OnAnimationEnd;

      Parent.ChangeDrawAbility(false);
      isMovementLockedByAnim = false;
      isMovingOnPath = false;
      currentPathNode = 0;
      waypath = null;
      collisionDerivedMoveForce = new Vector2();
      lastMoveForce = new Vector2();
    }

    public void AddAnimation(AnimType animType, List<Texture2D> textures, int animDuration, bool repeatable = false, bool useFirstFrameAsTexture = false, bool isPermanent = false) => this.Animator.AddAnimation(this.animTypeToSting(animType), textures, animDuration, repeatable, useFirstFrameAsTexture, isPermanent);

    private string animTypeToSting(AnimType animType)
    {
      switch (animType)
      {
        case AnimType.ON_ATTACK: return AnimNames.ON_ATTACK;
        case AnimType.ON_DEATH: return AnimNames.ON_DEATH;
        case AnimType.ON_MOVE_RIGHT: return AnimNames.ON_MOVE_RIGHT;
        case AnimType.ON_MOVE_LEFT: return AnimNames.ON_MOVE_LEFT;
        case AnimType.ON_HIT_LEFT: return AnimNames.ON_HIT_LEFT;
        case AnimType.ON_HIT_RIGHT: return AnimNames.ON_HIT_RIGHT;
        default: return "";
      }
    }

    public void CleanUp()
    {
      DropCombat();
      Parent = null;
      aggroArea = null;
      collider = null;
    }

    public void Draw(SpriteBatch batch)
    {
      Animator.Draw(batch);
    }

    public void RegisterSelf() { }

    public void UnRegisterSelf() { }

    public void Update(GameTime gameTime)
    {
      if (IsInCombat)
      {
        CheckDistanceToEnemy();
        events.Update(gameTime);
      }
    }

    private void OnAnimationEnd(object sender, string animName)
    {
      switch (animName)
      {
        case AnimNames.ON_ATTACK:
          isMovementLockedByAnim = false;
          break;
        case AnimNames.ON_DEATH:
          Scene.SceneManager.GetCurrentScene().DeleteObject(Parent);
          break;
      }
    }

    public void RegisterIncomeDmg(int dmg, GameObject source)
    {
      health -= dmg;

      if (!IsInCombat && source is Player)
        StartCombatWith(source);

      if (health <= 0)
      {
        Player plr = enemy as Player;
        plr.AddMoney(moneyReward);
        DropCombat();
        bool isPlayingDeathAnimation = !Animator.PlayAnimation(AnimNames.ON_DEATH);

        if (isPlayingDeathAnimation)
          Scene.SceneManager.GetCurrentScene().DeleteObject(Parent);
      }
      else if (this.currentDirection == MovementDirectionEnum.DIRECTION_LEFT || this.currentDirection == MovementDirectionEnum.DIRECTION_DOWN)
      {
        Animator.PlayAnimation(AnimNames.ON_HIT_LEFT);
      }
      else if (this.currentDirection == MovementDirectionEnum.DIRECTION_RIGHT || this.currentDirection == MovementDirectionEnum.DIRECTION_UP)
      {
        Animator.PlayAnimation(AnimNames.ON_HIT_RIGHT);
      }
    }

    private void DropCombat()
    {
      events.ClearAll();
      IsInCombat = false;
      IsMoveNeeded = false;
      enemy = null;
      enemyCollider = null;
    }

    public void NotifyCollision(object sender, CollisionNotifyData data)
    {
      if (!IsInCombat && data.source == aggroArea)
      {
        if (data.obj is Player)
          StartCombatWith(data.obj);
      }

      // Call helpers if object is moving on path
      if (isMovingOnPath)
      {
        UpdateComparisonSide(data.collisionSides);
        PathMovementCollisionHelper(data.collisionSides);
      }
    }

    public void StartCombatWith(GameObject obj)
    {
      IsInCombat = true;
      enemy = obj;
      enemyCollider = enemy.GetComponent<BoxCollider>();
      CheckDistanceToEnemy();
      events.ScheduleEvent(time: 50, repeat: true, func: UpdateMovement);
      PrepareAttackEvents();
    }

    private void UpdateMovement()
    {
      if (!IsMoveNeeded || isMovementLockedByAnim)
        return;

      if (!isMovingOnPath && !CheckMovePathCond())
        MoveWithoutPath();
      else
      {
        if (!isMovingOnPath)
        {
          isMovingOnPath = true;
          Pathfinding.PathfindingSystem.GetInstance().GetPathWithCallback(Parent.position, enemy.position, OnPathFound);
        }
        else
          MoveOnPath();
      }
    }

    private bool CheckMovePathCond()
    {
      double dist = Vector2.Distance(Parent.position, enemy.position);

      if (dist > ResolutionMgr.TileSize)
        return true;

      return false;
    }

    private void MoveWithoutPath()
    {
      Vector2 moveForce = new Vector2(0, 0);
      float distanceX = Math.Abs(enemy.position.X - Parent.position.X);
      float distanceY = Math.Abs(enemy.position.Y - Parent.position.Y);
      if (distanceX <= 1)
        Parent.position.X = enemy.position.X;
      else if (enemy.position.X < Parent.position.X)
        moveForce.X = -1;
      else if (enemy.position.X > Parent.position.X)
        moveForce.X = 1;
      else moveForce.X = 0;

      if (distanceY <= 1)
        Parent.position.Y = enemy.position.Y;
      else if (enemy.position.Y < Parent.position.Y)
        moveForce.Y = -1;
      else if (enemy.position.Y > Parent.position.Y)
        moveForce.Y = 1;
      else
        moveForce.Y = 0;

      DoMovementByForce(moveForce);
    }

    private void MoveOnPath()
    {
      if (waypath == null)
        return;

      Vector2 currentNode = waypath[currentPathNode];
      Vector2 moveForce = new Vector2(0, 0);
      Vector2 sourcePos = GetSourcePosByComparisionSide();

      if (currentNode.X + 1 < sourcePos.X)
        moveForce.X = -1;
      else if (currentNode.X - 1 > sourcePos.X)
        moveForce.X = 1;

      if (currentNode.Y + 1 < sourcePos.Y)
        moveForce.Y = -1;
      else if (currentNode.Y - 1 > sourcePos.Y)
        moveForce.Y = 1;

      if (Parent.Rectangle.Contains(currentNode))
      {
        moveForce.X = 0;
        moveForce.Y = 0;
      }

      if (collisionDerivedMoveForce.X != 0 || collisionDerivedMoveForce.Y != 0)
      {
        moveForce.X = collisionDerivedMoveForce.X;
        moveForce.Y = collisionDerivedMoveForce.Y;

        collisionDerivedMoveForce.X = 0;
        collisionDerivedMoveForce.Y = 0;
      }

      if (moveForce.X == 0 && moveForce.Y == 0)
      {
        ++currentPathNode;

        if (currentPathNode == waypath.Count)
        {
          OnWaypathEnd();
          return;
        }
      }

      lastMoveForce.X = moveForce.X;
      lastMoveForce.Y = moveForce.Y;

      DoMovementByForce(moveForce);
    }

    private void DoMovementByForce(Vector2 moveForce)
    {
      SelectMovementAnimation(moveForce);

      moveForce *= (Parent.speed + BonusMoveSpeed);
      collider.Move(moveForce);
    }

    // Callback function executed by Pathfinding System after found path during async task
    // and forward path to object which scheduled job of finding path
    private void OnPathFound(List<Vector2> path)
    {
      waypath = path;
      OnWaypathStart();
    }

    private void OnWaypathStart()
    {

    }

    private void OnWaypathEnd()
    {
      isMovingOnPath = false;
      currentPathNode = 0;
      waypath = null;
      posComparisionSide = PosComparisionSide.CORNER_LEFT;
    }

    // Function to add additional force while object is moving on path
    // this is one of 2 methods to prevent object get stuck on corner tile.
    // It use movementForce used during previous movement update(saved in lastMoveForce variable)
    // to deduct amount of additional force
    private void PathMovementCollisionHelper(RectCollisionSides collisionSides)
    {
      if ((collisionSides & RectCollisionSides.SIDE_RIGHT) != 0 ||
          (collisionSides & RectCollisionSides.SIDE_LEFT) != 0)
      {
        if (lastMoveForce.Y > 0.0f)
          collisionDerivedMoveForce.Y = 0.8f;
        else if (lastMoveForce.Y <= 0.0f)
          collisionDerivedMoveForce.Y = -0.8f;
      }

      if ((collisionSides & RectCollisionSides.SIDE_UP) != 0 ||
          (collisionSides & RectCollisionSides.SIDE_BOTTOM) != 0)
      {
        if (lastMoveForce.X > 0.0f)
          collisionDerivedMoveForce.X = 0.8f;
        else if (lastMoveForce.X <= 0.0f)
          collisionDerivedMoveForce.X = -0.8f;
      }
    }

    // Function called on collision while object is moving on path.
    // It use last movement force to deduct which corner of object rectangle
    // should get used in order to calculate movement force in current movement update
    private void UpdateComparisonSide(RectCollisionSides collisionSides)
    {
      if ((collisionSides & RectCollisionSides.SIDE_RIGHT) != 0)
      {
        if (lastMoveForce.Y < 0 && posComparisionSide != PosComparisionSide.CORNER_BOTTOM_RIGHT)
          posComparisionSide = PosComparisionSide.CORNER_BOTTOM_RIGHT;
        else if (lastMoveForce.Y > 0 && posComparisionSide != PosComparisionSide.CORNER_RIGHT)
          posComparisionSide = PosComparisionSide.CORNER_RIGHT;
      }

      if ((collisionSides & RectCollisionSides.SIDE_LEFT) != 0)
      {
        if (lastMoveForce.Y < 0 && posComparisionSide != PosComparisionSide.CORNER_BOTTOM_LEFT)
          posComparisionSide = PosComparisionSide.CORNER_BOTTOM_LEFT;
        else if (lastMoveForce.Y > 0 && posComparisionSide != PosComparisionSide.CORNER_LEFT)
          posComparisionSide = PosComparisionSide.CORNER_LEFT;
      }

      if ((collisionSides & RectCollisionSides.SIDE_UP) != 0)
      {
        if (lastMoveForce.X < 0 && posComparisionSide != PosComparisionSide.CORNER_RIGHT)
          posComparisionSide = PosComparisionSide.CORNER_RIGHT;
        else if (lastMoveForce.X > 0 && posComparisionSide != PosComparisionSide.CORNER_LEFT)
          posComparisionSide = PosComparisionSide.CORNER_LEFT;
      }

      if ((collisionSides & RectCollisionSides.SIDE_BOTTOM) != 0)
      {
        if (lastMoveForce.X < 0 && posComparisionSide != PosComparisionSide.CORNER_BOTTOM_RIGHT)
          posComparisionSide = PosComparisionSide.CORNER_BOTTOM_RIGHT;
        else if (lastMoveForce.X > 0 && posComparisionSide != PosComparisionSide.CORNER_BOTTOM_LEFT)
          posComparisionSide = PosComparisionSide.CORNER_BOTTOM_LEFT;
      }
    }

    // Returns point used to calculate movement force based on currently
    // currently set state of posComparisionSide variable
    private Vector2 GetSourcePosByComparisionSide()
    {
      Vector2 pos = new Vector2(Parent.position.X, Parent.position.Y);

      switch (posComparisionSide)
      {
        case PosComparisionSide.CORNER_RIGHT:
          pos.X += Parent.Rectangle.Width;
          break;
        case PosComparisionSide.CORNER_BOTTOM_LEFT:
          pos.Y += Parent.Rectangle.Height;
          break;
        case PosComparisionSide.CORNER_BOTTOM_RIGHT:
          pos.X += Parent.Rectangle.Width;
          pos.Y += Parent.Rectangle.Height;
          break;
      }

      return pos;
    }

    private void SelectMovementAnimation(Vector2 moveForce)
    {
      if (moveForce.X == -1) // LEFT anim
      {
        Animator.PlayAnimation(AnimNames.ON_MOVE_LEFT);
        previousDirection = currentDirection;
        currentDirection = MovementDirectionEnum.DIRECTION_LEFT;
      }
      else if (moveForce.X == 1) // RIGHT anim
      {
        Animator.PlayAnimation(AnimNames.ON_MOVE_RIGHT);
        previousDirection = currentDirection;
        currentDirection = MovementDirectionEnum.DIRECTION_RIGHT;
      }
      else if (moveForce.Y == -1) // UP anim
      {
        Animator.PlayAnimation(AnimNames.ON_MOVE_RIGHT);
        previousDirection = currentDirection;
        currentDirection = MovementDirectionEnum.DIRECTION_UP;
      }
      else if (moveForce.Y == 1) // DOWN anim
      {
        Animator.PlayAnimation(AnimNames.ON_MOVE_LEFT);
        previousDirection = currentDirection;
        currentDirection = MovementDirectionEnum.DIRECTION_DOWN;
      }
    }

    private float GetDistToEnemy()
    {
      float dist = Vector2.Distance(collider.Position, enemyCollider.Position);
      dist -= (collider.Rect.Width / 2);
      // fix me!
      if (enemy != null)
        dist -= (enemyCollider.Rect.Width / 2);
      else
        dist -= (enemy.Rectangle.Width / 2);

      return dist;
    }

    private void CheckDistanceToEnemy()
    {
      float dist = GetDistToEnemy();

      if (dist > MinDistToEnemy)
        IsMoveNeeded = true;
      else
        IsMoveNeeded = false;
    }

    private void PrepareAttackEvents()
    {
      foreach (var attack in attackPattern.attacks)
        ScheduleAttack(attack);
    }

    private void ScheduleAttack(AttackData attack)
    {
      events.ScheduleEvent(attack.AttackTimer, attack.IsRepeatable, () =>
      {
        // Additional check just for safety
        if (enemy == null)
          return;

        // Check distance between unit and enemy in order to validate attack with its data
        float dist = GetDistToEnemy();
        // validate attack
        if (attack.MinRange > 0.0f && dist < attack.MinRange)
          return;
        if (attack.MaxRange < dist)
          return;

        Random rng = new Random();
        int dmgValue = rng.Next(attack.MinDamage, attack.MaxDamage);

        if (attack.type == AttackType.ATTACK_TYPE_RANGED && attack.particleTextureName != null)
        {
          Particle particle = new Particle(attack.particleTextureName, 0.5f, 0.5f, collider.Position, CalculateParticleForce(), 1.5f, 20.0f, Parent);
          if (attack.particleAnim != null)
            particle.Animator.AddAnimation("OnHit", attack.particleAnim, animDuration: 1000);

          SetRangedAttackDmg(dmgValue, particle);
          particle.Launch();
        }
        else
        {
          Player plr = enemy as Player; // TODO: Change it to more flex code via some kind of system
          plr.DecreaseLiveValue(dmgValue);
        }

        if (attack.type == AttackType.ATTACK_TYPE_MELEE && Animator.PlayAnimation(AnimNames.ON_ATTACK))
          isMovementLockedByAnim = true;
      });
    }

    private Vector2 CalculateParticleForce()
    {
      Vector2 vector = new Vector2();

      if (enemy.position.X > Parent.position.X)
        vector.X = 1;
      else if (enemy.position.X < Parent.position.X)
        vector.X = -1;

      if (enemy.position.Y > Parent.position.Y)
        vector.Y = 1;
      else if (enemy.position.Y < Parent.position.Y)
        vector.Y = -1;

      return vector;
    }

    private void SetRangedAttackDmg(int dmg, Particle particle)
    {
      particle.OnParticleHit += (sender, data) =>
      {
        if (data.obj == enemy)
        {
          Player plr = enemy as Player;
          plr.DecreaseLiveValue(dmg);
        }
      };
    }
  }
}
