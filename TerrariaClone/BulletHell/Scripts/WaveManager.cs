﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;
using Microsoft.Xna.Framework.Graphics;


namespace GameName1.BulletHell.Scripts
{
    public class WaveManager : Script
    {
        private float spawnChance = 60;

        private int entityCount = 0;
        private bool waitingForRespawn = true;

        private void respawn()
        {
            waitingForRespawn = false;
        }

        private void playerdead()
        {
            waitingForRespawn = true;
        }

        private void fixedupdate()
        {
            if (!waitingForRespawn && entityCount < 200)
            {
                var player =  rootObject.find("player");
                var playerPosition = player.transform.DerivedPosition;

                if (random.Next(0f, spawnChance) < 0.5f)
                {
                    var enemyGO = rootObject.createChild("enemy");
                    var enemy = enemyGO.createScript<Enemy>();
                    
                    var enemyai = enemyGO.createScript<AIChaseEntity>();
                    enemyai.target = player;

                    var enemySprite = enemyGO.createScript<Sprite>();
                    enemySprite.color = Color.Orange;
                    enemySprite.material = resources.createMaterialFromTexture("content/textures/bullethell/enemy1.png");
                    enemySprite.material.SetBlendState(BlendState.Additive);
                    enemyGO.transform.Position = GetSpawnPosition(playerPosition);
                }

                if (random.Next(0f, spawnChance) < 0.5f)
                {
                    var enemyGO = rootObject.createChild("enemy");
                    var enemy = enemyGO.createScript<Enemy>();
                    
                    var enemyai = enemyGO.createScript<AIChaseEntity>();
                    enemyai.target = player;

                    var enemySprite = enemyGO.createScript<Sprite>();
                    enemySprite.color = Color.Orange;
                    enemySprite.material = resources.createMaterialFromTexture("content/textures/bullethell/enemy2.png");
                    enemySprite.material.SetBlendState(BlendState.Additive);
                    enemyGO.transform.Position = GetSpawnPosition(playerPosition);
                }

                if (random.Next(0f, spawnChance) < 0.5f)
                {
                    var enemyGO = rootObject.createChild("enemy");
                    var enemy = enemyGO.createScript<Enemy>();

                    var enemyai = enemyGO.createScript<AIChaseEntity>();
                    enemyai.target = player;

                    var enemySprite = enemyGO.createScript<Sprite>();
                    enemySprite.color = Color.Orange;
                    enemySprite.material = resources.createMaterialFromTexture("content/textures/bullethell/enemy3.png");
                    enemySprite.material.SetBlendState(BlendState.Additive);
                    enemyGO.transform.Position = GetSpawnPosition(playerPosition);
                }

                spawnChance -= 0.005f;
            }
        }

        private Vector2 GetSpawnPosition(Vector2 playerPosition)
        {
            Vector2 pos;
            do
            {
                var screenSize = mainCamera.worldBounds;
                pos = new Vector2(random.Next(screenSize.X0, screenSize.X1), random.Next(screenSize.Y0, screenSize.Y1));
            }
            while (Vector2.DistanceSquared(pos, playerPosition) < 250 * 250);

            return pos;
        }

        private void reset()
        {
            entityCount = 0;
            spawnChance = 60;
        }

        private void addEntity(Entity entity)
        {
            entityCount++;
        }

        private void removeEntity(Entity entity)
        {
            entityCount--;
        }
    }
}
