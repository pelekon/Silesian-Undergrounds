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
        private PlayerOrientation playerOrientation;
        private TimedEventsScheduler eventsScheduler;
        private bool isAttackOnCooldown;

        private readonly int attackCooldown = 2000;

        public PlayerBehaviour(GameObject parent)
        {
            Parent = parent;
            isAttackOnCooldown = false;
            eventsScheduler = new TimedEventsScheduler();
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
                    particlePos.X = Parent.position.X + (Parent.Rectangle.Width / 2);
                    particlePos.Y = Parent.position.Y - 2;
                    break;
                case PlayerOrientation.ORIENTATION_SOUTH:
                    particleForce = new Vector2(0, 1);
                    particlePos.X = Parent.position.X + (Parent.Rectangle.Width / 2);
                    particlePos.Y = Parent.position.Y + Parent.Rectangle.Height + 2;
                    break;
                case PlayerOrientation.ORIENTATION_EAST:
                    particleForce = new Vector2(1, 0);
                    particlePos.X = Parent.position.X + Parent.Rectangle.Width + 2;
                    particlePos.Y = Parent.position.Y + (Parent.Rectangle.Height / 2);
                    break;
                case PlayerOrientation.ORIENTATION_WEST:
                    particleForce = new Vector2(-1, 0);
                    particlePos.X = Parent.position.X - 2;
                    particlePos.Y = Parent.position.Y + (Parent.Rectangle.Height / 2);
                    break;
            }

            Particle particle = new Particle("test", 0.5f, 0.5f, particlePos, particleForce, 1.5f, 15.0f, Parent);
            particle.OnParticleHit += OnParticleHit;
            particle.Launch();
        }

        private void OnParticleHit(object sender, Collisions.CollisionNotifyData e)
        {
            HostileBehaviour hostileBehaviour = e.obj.GetComponent<HostileBehaviour>();
            if (hostileBehaviour == null)
                return;

            Player plr = Parent as Player;
            int dmg = plr.PlayerStatistic.BaseDamage;
            hostileBehaviour.RegisterIncomeDmg(dmg, Parent);
        }
    }
}
