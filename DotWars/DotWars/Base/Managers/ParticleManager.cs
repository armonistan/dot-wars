#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace DotWars
{
    public class ParticleManager
    {
        #region Declarations

        private readonly Queue<Particle> activeParticles;
        private readonly Stack<Particle> inactiveParticles;
        private readonly int particleCap;

        private readonly Queue<Explosion> activeExplosions;
        private readonly Stack<Explosion> inactiveExplosions;
        private readonly int explodeCap;

        private readonly Queue<Fire> activeFires;
        private readonly Stack<Fire> inactiveFires;
        private readonly int fireCap;

        private readonly Queue<Gut> activeGuts;
        private readonly Stack<Gut> inactiveGuts;
        private readonly int gutCap;

        private ManagerHelper managers;

        #endregion

        public ParticleManager(int pC, int fC, int gC, int eC)
        {
            particleCap = pC;
            inactiveParticles = new Stack<Particle>(particleCap);
            activeParticles = new Queue<Particle>(particleCap);
            for (int i = 0; i < particleCap; i++)
            {
                inactiveParticles.Push(new Particle());
            }

            fireCap = fC;
            inactiveFires = new Stack<Fire>(fireCap);
            activeFires = new Queue<Fire>(fireCap);
            for (int i = 0; i < fireCap; i++)
            {
                inactiveFires.Push(new Fire());
            }

            gutCap = gC;
            inactiveGuts = new Stack<Gut>(gutCap);
            activeGuts = new Queue<Gut>(gutCap);
            for (int i = 0; i < gutCap; i++)
            {
                inactiveGuts.Push(new Gut());
            }

            explodeCap = eC;
            inactiveExplosions = new Stack<Explosion>(explodeCap);
            activeExplosions = new Queue<Explosion>(explodeCap);
            for (int i = 0; i < explodeCap; i++)
            {
                inactiveExplosions.Push(new Explosion());
            }
        }

        public void Initialize(ManagerHelper mH)
        {
            managers = mH;
        }

        public void AddParticle(string a, Vector2 p, Vector2 v, float dT, float d, float t, float rA)
        {
            AddParticle(a, 0, 0, p, v, dT, d, t, rA);
        }

        private void AddParticle(string a, int fI, int mI, Vector2 p, Vector2 v, float dT, float d, float t, float rA)
        {
            if (inactiveParticles.Count > 0)
            {
                Particle temp = inactiveParticles.Pop();
                temp.Set(a, fI, mI, p, v, dT, d, t, rA, managers);
                activeParticles.Enqueue(temp);
            }
        }

        public void AddBlood(NPC n)
        {
            string asset = "Effects/particle_blood_";

            if (n.GetType() == typeof (RedCommander) || n.GetType() == typeof (RedPlayerCommander))
            {
                asset += "red";
            }
            else if (n.GetType() == typeof (BlueCommander) || n.GetType() == typeof (BluePlayerCommander))
            {
                asset += "blue";
            }
            else if (n.GetType() == typeof (GreenCommander) || n.GetType() == typeof (GreenPlayerCommander))
            {
                asset += "green";
            }
            else if (n.GetType() == typeof (YellowCommander) || n.GetType() == typeof (YellowPlayerCommander))
            {
                asset += "yellow";
            }
            else
            {
                switch (n.GetAffiliation())
                {
                    case NPC.AffliationTypes.red:
                        asset += "red";
                        break;
                    case NPC.AffliationTypes.blue:
                        asset += "blue";
                        break;
                    case NPC.AffliationTypes.green:
                        asset += "green";
                        break;
                    case NPC.AffliationTypes.yellow:
                        asset += "yellow";
                        break;
                    case NPC.AffliationTypes.black:
                        return;
                }
            }

            AddParticle(asset, n.GetOriginPosition(),
                        PathHelper.Direction((float) (managers.GetRandom().NextDouble()*MathHelper.TwoPi))*50, 2, 0.03f,
                        1, 2);
        }

        public void AddHeal(NPC n)
        {
            int tempMode = 0;

            switch (n.GetAffiliation())
            {
                case NPC.AffliationTypes.blue:
                    tempMode = 1;
                    break;
                case NPC.AffliationTypes.green:
                    tempMode = 2;
                    break;
                case NPC.AffliationTypes.yellow:
                    tempMode = 3;
                    break;
            }

            AddParticle("Effects/particle_heal", managers.GetRandom().Next(5), tempMode, n.GetOriginPosition(),
                        PathHelper.Direction((float) (managers.GetRandom().NextDouble()*MathHelper.TwoPi))*50, 2, 0.03f,
                        1, 0);

            managers.GetAudioManager().Play(AudioManager.HEAL_SOUND, AudioManager.RandomVolume(managers),
                                            AudioManager.RandomPitch(managers), 0, false);
        }

        public void AddStandardSmoke(Vector2 p, float v)
        {
            AddParticle("Effects/smoke_standard", p,
                        PathHelper.Direction((float) (managers.GetRandom().NextDouble()*MathHelper.TwoPi))*v, 1, 0.01f,
                        1, 3);
        }

        private void RemoveParticle()
        {
            inactiveParticles.Push(activeParticles.Dequeue());
        }

        public void AddFire(Vector2 p, Vector2 v, float dT, float d, float t, float rA)
        {
            if (inactiveFires.Count > 0)
            {
                Fire temp = inactiveFires.Pop();
                temp.Set(p, v, dT, d, t, rA, managers);
                activeFires.Enqueue(temp);
            }
        }

        private void RemoveFire()
        {
            inactiveFires.Push(activeFires.Dequeue());
        }

        public void AddGut(NPC n, int f)
        {
            if (inactiveGuts.Count > 0)
            {
                Gut temp = inactiveGuts.Pop();
                temp.Set(n, f, managers);
                activeGuts.Enqueue(temp);
            }
        }

        private void RemoveGut()
        {
            inactiveGuts.Push(activeGuts.Dequeue());
        }

        public void AddExplosion(Vector2 p, NPC n, int d)
        {
            if (inactiveExplosions.Count > 0)
            {
                Explosion temp = inactiveExplosions.Pop();
                temp.Set(p, d, n, managers);
                activeExplosions.Enqueue(temp);
            }
        }

        public void AddExplosion(Vector2 p, NPC.AffliationTypes aT, int d)
        {
            if (inactiveExplosions.Count > 0)
            {
                Explosion temp = inactiveExplosions.Pop();
                temp.Set(p, d, aT, managers);
                activeExplosions.Enqueue(temp);
            }
        }

        private void RemoveExplosion()
        {
            inactiveExplosions.Push(activeExplosions.Dequeue());
        }

        public void Update()
        {
            int numDeletes = 0;
            foreach (Particle p in activeParticles)
            {
                p.Update(managers);

                if (p.GetExistanceTime() < 0)
                {
                    numDeletes++;
                }
            }
            for (int i = 0; i < numDeletes; i++)
            {
                RemoveParticle();
            }

            numDeletes = 0;
            foreach (Fire f in activeFires)
            {
                f.Update(managers);

                if (f.GetExistanceTime() < 0)
                {
                    numDeletes++;
                }
            }
            for (int i = 0; i < numDeletes; i++)
            {
                RemoveFire();
            }

            numDeletes = 0;
            foreach (Gut g in activeGuts)
            {
                g.Update(managers);

                if (g.GetExistanceTime() < 0)
                {
                    numDeletes++;
                }
            }
            for (int i = 0; i < numDeletes; i++)
            {
                RemoveGut();
            }

            numDeletes = 0;
            foreach (Explosion e in activeExplosions)
            {
                e.Update(managers);

                if (e.GetExistanceTime() < 0)
                {
                    numDeletes++;
                }
            }
            for (int i = 0; i < numDeletes; i++)
            {
                RemoveExplosion();
            }
        }

        public void DrawBottom(SpriteBatch sB, Vector2 d)
        {
            foreach (Gut g in activeGuts)
            {
                g.Draw(sB, d, managers);
            }
        }

        public void DrawTop(SpriteBatch sB, Vector2 d)
        {
            foreach (Particle p in activeParticles)
            {
                p.Draw(sB, d, managers);
            }

            foreach (Fire f in activeFires)
            {
                f.Draw(sB, d, managers);
            }

            foreach (Explosion e in activeExplosions)
            {
                e.Draw(sB, d, managers);
            }
        }

        public Queue<Particle> GetParticles()
        {
            return activeParticles;
        }

        public Queue<Gut> GetGuts()
        {
            return activeGuts;
        }

        public Queue<Explosion> GetExplosions()
        {
            return activeExplosions;
        }
    }
}