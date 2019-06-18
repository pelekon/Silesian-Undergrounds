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
    public class HostileBehaviour : IComponent
    {
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
        public Animator Animator { get; private set; }
        private MovementDirectionEnum currentDirection;
        private MovementDirectionEnum previousDirection;
        private bool needStandAnimUpdate;
        private bool isMovementLockedByAnim;

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

            aggroArea = new CircleCollider(Parent, 70, 0, 0);
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
            needStandAnimUpdate = false;
            isMovementLockedByAnim = false;
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
                case "Attack":
                    isMovementLockedByAnim = false;
                    break;
                case "Death":
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

                if (!Animator.PlayAnimation("Death"))
                    Scene.SceneManager.GetCurrentScene().DeleteObject(Parent);
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
        }

        public void StartCombatWith(GameObject obj)
        {
            IsInCombat = true;
            enemy = obj;
            enemyCollider = enemy.GetComponent<BoxCollider>();
            CheckDistanceToEnemy();
            events.ScheduleEvent(50, true, UpdateMovement);
            PrepareAttackEvents();
        }

        private void UpdateMovement()
        {
            if (!IsMoveNeeded || isMovementLockedByAnim)
                return;

            Vector2 moveForce = new Vector2(0, 0);

            if (enemy.position.X < Parent.position.X)
                moveForce.X = -1;
            else if (enemy.position.X > Parent.position.X)
                moveForce.X = 1;
            else moveForce.X = 0;

            if (enemy.position.Y < Parent.position.Y)
                moveForce.Y = -1;
            else if (enemy.position.Y > Parent.position.Y)
                moveForce.Y = 1;
            else
                moveForce.Y = 0;

            SelectMovementAnimation(moveForce);

            moveForce *= (Parent.speed + BonusMoveSpeed);
            collider.Move(moveForce);
        }

        private void SelectMovementAnimation(Vector2 moveForce)
        {
            if (moveForce.X == -1) // LEFT anim
            {
                Animator.PlayAnimation("MoveLeft");
                previousDirection = currentDirection;
                currentDirection = MovementDirectionEnum.DIRECTION_LEFT;
            }
            else if (moveForce.X == 1) // RIGHT anim
            {
                Animator.PlayAnimation("MoveRight");
                previousDirection = currentDirection;
                currentDirection = MovementDirectionEnum.DIRECTION_RIGHT;
            }
            else if (moveForce.Y == -1) // UP anim
            {
                Animator.PlayAnimation("MoveUp");
                previousDirection = currentDirection;
                currentDirection = MovementDirectionEnum.DIRECTION_UP;
            }
            else if (moveForce.Y == 1) // DOWN anim
            {
                Animator.PlayAnimation("MoveDown");
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


                // ????
                // if (attack.type == AttackType.ATTACK_TYPE_MELEE && IsMoveNeeded)
                //   return;

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
                        particle.Animator.AddAnimation("OnHit", attack.particleAnim, 1000);

                    SetRangedAttackDmg(dmgValue, particle);
                    particle.Launch();
                }
                else
                {
                    Player plr = enemy as Player; // TODO: Change it to more flex code via some kind of system
                    plr.DecreaseLiveValue(dmgValue);
                }

                if (attack.type == AttackType.ATTACK_TYPE_MELEE && Animator.PlayAnimation("Attack"))
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
