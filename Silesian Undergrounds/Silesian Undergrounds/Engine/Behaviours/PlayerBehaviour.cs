using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Components;
using Silesian_Undergrounds.Engine.Utils;
using Silesian_Undergrounds.Engine.Particles;

namespace Silesian_Undergrounds.Engine.Behaviours
{
  public enum PlayerOrientation
  {
    ORIENTATION_NORTH,
    ORIENTATION_SOUTH,
    ORIENTATION_EAST,
    ORIENTATION_WEST,
  }

  public class PlayerBehaviour : IComponent
  {
    // IComponent inherited
    public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Rectangle Rect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public GameObject Parent { get; private set; }
    // PlayerBehaviour variables
    private Player playerOwner { get; }
    private PlayerOrientation playerOrientation;
    private TimedEventsScheduler eventsScheduler;
    private bool isAttackOnCooldown;
    private Animator animator;

    private int attackCooldown = 2000;
    private float attackSpeed = 1f;

    public PlayerBehaviour(GameObject parent)
    {
      Parent = parent;
      playerOwner = Parent as Player;
      attackSpeed = playerOwner.PlayerStatistic.AttackSpeed;
      attackCooldown = (int)(2000 / attackSpeed);
      isAttackOnCooldown = false;
      eventsScheduler = new TimedEventsScheduler();
      animator = new Animator(parent);
      LoadAnimations();
    }

    public void RegisterSelf() { }
    public void UnRegisterSelf() { }
    public void Draw(SpriteBatch batch) { }

    public void CleanUp()
    {
      Parent = null;
    }

    public void Update(GameTime gameTime)
    {
      eventsScheduler.Update(gameTime);

      if (Keyboard.GetState().IsKeyDown(Keys.Space))
        HandleAttack();
    }

    public void SetOwnerOrientation(PlayerOrientation orientation)
    {
      playerOrientation = orientation;
    }

    public void ChangeAttackSpeed(float newValueOfPlayerAttackSpeed)
    {
      attackSpeed = newValueOfPlayerAttackSpeed;
      attackCooldown = (int)(2000 / attackSpeed);
    }

    private void HandleAttack()
    {
      // Do not send attack when its on cooldown period
      if (isAttackOnCooldown)
        return;

      isAttackOnCooldown = true;
      eventsScheduler.ScheduleEvent(attackCooldown, false, () =>
      {
        // Clear attack cooldown
        isAttackOnCooldown = false;
      });

      Vector2 particleForce = new Vector2(0, 0);
      Vector2 particlePos = new Vector2(0, 0);

      switch (playerOrientation)
      {
        case PlayerOrientation.ORIENTATION_NORTH:
          particleForce = new Vector2(0, -1);
          particlePos.X = Parent.position.X + (Parent.Rectangle.Width / 4);
          particlePos.Y = Parent.position.Y - 2;
          break;
        case PlayerOrientation.ORIENTATION_SOUTH:
          particleForce = new Vector2(0, 1);
          particlePos.X = Parent.position.X + (Parent.Rectangle.Width / 4);
          particlePos.Y = Parent.position.Y + Parent.Rectangle.Height + 2;
          break;
        case PlayerOrientation.ORIENTATION_EAST:
          particleForce = new Vector2(1, 0);
          particlePos.X = Parent.position.X + Parent.Rectangle.Width + 2;
          particlePos.Y = Parent.position.Y + (Parent.Rectangle.Height / 4);
          break;
        case PlayerOrientation.ORIENTATION_WEST:
          particleForce = new Vector2(-1, 0);
          particlePos.X = Parent.position.X - 2;
          particlePos.Y = Parent.position.Y + (Parent.Rectangle.Height / 4);
          break;
      }

      Particle particle = new Particle("test", 0.5f, 0.5f, particlePos, particleForce, 1.5f, 15.0f, Parent);
      particle.OnParticleHit += OnParticleHit;
      particle.Animator.AddAnimation("PickAtackAnimation", TextureMgr.Instance.GetAnimation("pickAtack"), 1000, true, false);
      particle.Animator.PlayAnimation("PickAtackAnimation");
      particle.Launch();
    }

    private void OnParticleHit(object sender, Collisions.CollisionNotifyData e)
    {
      HostileBehaviour hostileBehaviour = e.obj.GetComponent<HostileBehaviour>();
      if (hostileBehaviour == null)
      {
        (sender as Particle).AddAndPlayOnHitAnimation(textures: TextureMgr.Instance.GetAnimation("pickHit"), animDuration: 500);
        return;
      }

      Player plr = Parent as Player;
      int dmg = plr.PlayerStatistic.BaseDamage;
      if (hostileBehaviour.OnParticleHitAnimationConfig != null) (sender as Particle).AddAndPlayOnHitAnimation(hostileBehaviour.OnParticleHitAnimationConfig);
      else (sender as Particle).AddAndPlayOnHitAnimation(textures: TextureMgr.Instance.GetAnimation("pickHit"), animDuration: 500);
      hostileBehaviour.RegisterIncomeDmg(dmg, Parent);
    }

    public Animator GetAnimator()
    {
      return animator;
    }

    private void LoadAnimations()
    {

      TextureMgr.Instance.LoadAnimationFromSpritesheet(
          fileName: "pick_sprite",
          animName: "pickAtack",
          spritesheetRows: 1,
          spritesheetColumns: 4,
          index: 0, amount: 4,
          spacingX: 0,
          spacingY: 0,
          canAddToExisting: false,
          loadByColumn: false
      );

      TextureMgr.Instance.LoadAnimationFromSpritesheet(
         fileName: "smoke",
         animName: "pickHit",
         spritesheetRows: 1,
         spritesheetColumns: 3,
         index: 0,
         amount: 3,
         spacingX: 0,
         spacingY: 0,
         canAddToExisting: false
      );
    }
  }
}
