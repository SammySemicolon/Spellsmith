using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellsmith.Projectiles.EnemyProjectiles.JungleProtector;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Spellsmith.NPCs.JungleProtector
{
    public class JungleProtector : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Guy");
		}
		public override void SetDefaults()
		{
			npc.width = 84;
			npc.height = 104;
			npc.damage = 14;
			npc.defense = 6;
			npc.lifeMax = 200;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 200000f;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			npc.noGravity = true;
			npc.boss = true;
			npc.noTileCollide = true;
			npc.dontTakeDamageFromHostiles = true;
		}

		internal ref float Phase => ref npc.ai[0];

		internal ref float PhaseValue1 => ref npc.ai[1];

		internal ref float PhaseValue2 => ref npc.ai[2];

		internal ref float PersistentValue => ref npc.ai[3];
		
		float ConstantTimer;

		float DrawTimer;

		Vector2 positionVector;

		internal float healthPercentage => 1 - ((float)npc.life / npc.lifeMax);

		internal float healthPercentageAlt => 1 + healthPercentage;

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			float pow = (float)Math.Pow(healthPercentageAlt, 4);
			DrawTimer += 0.0034f * pow;
			int orbitCount = 4;
			float distance = 20 + ((float)Math.Sin(DrawTimer) * 8 * healthPercentageAlt);
			for (int i = 0; i < orbitCount; i++)
			{
				float rot = MathHelper.Lerp(0, 6.28f, ((float)i) / orbitCount) + DrawTimer;
				Vector2 targetOffset = new Vector2(0, distance).RotatedBy(rot);
				Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
				Vector2 drawPosition = npc.position + targetOffset - Main.screenPosition + new Vector2(0f, npc.gfxOffY) + drawOrigin;
				string texture = "Spellsmith/NPCs/JungleProtector/JungleProtectorGreen";
				if (i > (orbitCount / 2) - 1)
				{
					texture = "Spellsmith/NPCs/JungleProtector/JungleProtectorPink";
				}
				spriteBatch.Draw(ModContent.GetTexture(texture), drawPosition, null, npc.GetAlpha(Color.White) * 0.3f, npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			return base.PreDraw(spriteBatch, drawColor);
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
			Vector2 overlayPosition = npc.position + new Vector2(0, 4) - Main.screenPosition + new Vector2(0f, npc.gfxOffY) + drawOrigin;
			spriteBatch.Draw(ModContent.GetTexture("Spellsmith/NPCs/JungleProtector/JungleProtectorOverlay"), overlayPosition, null, npc.GetAlpha(Color.White), npc.rotation, drawOrigin, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void AI()
		{
			base.AI();
			npc.TargetClosest(true);
			Player target = Main.player[npc.target];
			ConstantTimer++;
			switch (Phase)
			{
				case 0:
					{
						float shootCooldown = 50;
						float shootCount = 10;
						basicMovement();
						PhaseValue1++;
						if (PhaseValue1 >= shootCooldown)
						{
							basicShoot(0.25f);
							PhaseValue1 = 0;
							PhaseValue2++;
						}
						if (PhaseValue2 >= shootCount)
                        {
							switchPhase(1);
						}
						break;
					}
				case 1:
					{
						float moveCount = 3;
						bool success = goToMovement(positionVector);
						if (success)
						{
							PersistentValue++;
							if (PersistentValue < moveCount)
							{
								switchPhase(2);
							}
							else
                            {
								switchPhase(3);
							}
						}
						if (PersistentValue > moveCount)
                        {
							switchPhase(0);
							PersistentValue = 0;
						}
						break;
					}
				case 2:
					{
						float shootCooldown = 60;
						float shootCount = 8;
						PhaseValue1++;
						if (PhaseValue1 >= shootCooldown)
						{
							for (int i = 0; i < shootCount; i++)
							{
								homingShoot(1f);
							}
							switchPhase(1);
						}
						npc.velocity *= 0.94f;
						break;
					}
				case 3:
					{
						float shootCooldown = 60;
						PhaseValue1++;
						if (PhaseValue1 >= shootCooldown)
						{
							createOrbitingBolts();
							switchPhase(1);
						}
						npc.velocity *= 0.94f;
						break;
					}
			}
			npc.rotation = npc.velocity.X / 16;
			npc.spriteDirection = npc.velocity.X < 0 ? -1 : 1;
		}
		public void switchPhase(int phase)
        {
			Phase = phase;
			if (phase == 1)
			{
				Player target = Main.player[npc.target];
				float x = target.Center.X + Main.rand.NextFloat(-800, 800);
				float y = target.Center.Y + Main.rand.NextFloat(-800, 800);
				positionVector = new Vector2(x, y);
			}
			PhaseValue1 = 0;
			PhaseValue2 = 0;
		}
		public bool goToMovement(Vector2 targetPosition)
		{
			float speed = 16f;
			Vector2 desiredVelocity = Vector2.Normalize(targetPosition - npc.Center) * speed;
			Vector2 finalVelocity = Vector2.SmoothStep(npc.velocity, desiredVelocity, 0.15f);
			npc.velocity = finalVelocity;
			return Helper.CheckCircularCollision(targetPosition, npc.Hitbox, npc.height);
		}
		public void basicMovement()
		{
			Player target = Main.player[npc.target];
			Vector2 targetPosition = target.Center;
			targetPosition.Y += (float)Math.Sin(ConstantTimer / 24) * 80;
			float speed = 8f;
			Vector2 desiredVelocity = Vector2.Normalize(targetPosition - npc.Center) * speed;
			desiredVelocity.Y *= 0.75f;
			Vector2 finalVelocity = Vector2.SmoothStep(npc.velocity, desiredVelocity, 0.1f);
			npc.velocity = finalVelocity;
		}
		public void createOrbitingBolts()
		{
			int circleCount = 8;
			int distance = 250;
			for (int i = 0; i < circleCount; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					float rot = MathHelper.Lerp(0, 6.28f, ((float)i) / circleCount);
					Vector2 targetOffset = new Vector2(0, distance).RotatedBy(rot);
					Projectile bolt = Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ModContent.ProjectileType<OrbitingGreenBolt>(), 50, 2f);
					bolt.ai[0] = rot + 1.57f;
					bolt.ai[1] = j == 0 ? -4 : 4;
					OrbitingGreenBolt orbitingGreenBolt = bolt.modProjectile as OrbitingGreenBolt;
					orbitingGreenBolt.targetPosition = npc.Center + targetOffset;
					orbitingGreenBolt.dist = distance;
				}
			}
		}
		public void shootDelayedBolts()
		{
			Player target = Main.player[npc.target];
			int boltRows = 5;
			int boltCount = 5;
			for (int i = 0; i < boltRows; i++)
			{
				float rot = MathHelper.Lerp(-50, 50, (float)i / boltRows);
				for (int j = 0; j < boltCount; j++)
				{
					Vector2 offset = new Vector2(0, 500 + (j * 75)).RotatedBy(Vector2.Normalize(target.Center - npc.Center).ToRotation() + 1.57f).RotatedBy(MathHelper.ToRadians(rot));
					Projectile bolt = Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ModContent.ProjectileType<DelayedGreenBolt>(), 50, 2f);
					bolt.ai[1] = 12f;
					bolt.ai[0] = target.whoAmI;
					DelayedGreenBolt delayedGreenBolt = bolt.modProjectile as DelayedGreenBolt;
					delayedGreenBolt.targetPosition = npc.Center + offset;
					npc.ai[1] = 0;
				}
			}
		}
		public void homingShoot(float accuracy)
		{
			Player target = Main.player[npc.target];
			float rot = Main.rand.NextFloat(-80 * accuracy, 80f * accuracy);
			float speed = Main.rand.NextFloat(8f, 14f);
			Vector2 desiredVelocity = Vector2.Normalize(target.Center - npc.Center).RotatedByRandom(MathHelper.ToRadians(rot)) * speed;
			Projectile bolt = Projectile.NewProjectileDirect(npc.Center, desiredVelocity, ModContent.ProjectileType<HomingGreenBolt>(), 50, 2f);
			bolt.ai[1] = target.whoAmI;
			bolt.ai[0] = speed;
		}
		public void basicShoot(float accuracy)
		{
			Player target = Main.player[npc.target];
			float rot = Main.rand.NextFloat(-50 * accuracy, 50f * accuracy);
			float speed = Main.rand.NextFloat(12f, 20f);
			Vector2 desiredVelocity = Vector2.Normalize(target.Center - npc.Center).RotatedByRandom(MathHelper.ToRadians(rot)) * speed;
			Projectile bolt = Projectile.NewProjectileDirect(npc.Center, desiredVelocity, ModContent.ProjectileType<FadingGreenBolt>(), 50, 2f);
			bolt.timeLeft = 400;
		}
	}
}