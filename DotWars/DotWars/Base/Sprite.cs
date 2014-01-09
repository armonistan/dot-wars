using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class Sprite
    {
        #region Declarations

        //Texture Info
        protected Vector2 acceleration;
        protected List<Vector2> accelerations;
        protected String asset;
        protected List<Vector2>[,] blockers;
        protected float drag;
        protected Rectangle frame;

        protected int frameIndex,
                   modeIndex;

        //Movement and Drawing Info
        public Vector2 origin,
                       originPosition;

        public Vector2 position;

        protected float rotation;
        private Texture2D texture;
        protected Color[,] textureData;

        protected float thrust;

        //Animation Info

        public int totalFrames,
                   totalModes;

        public Vector2 velocity;

        #endregion

        public Sprite(String a, Vector2 p, Vector2 v)
        {
            //Set up asset
            asset = a;

            //Set up movement stuff
            position = p;
            originPosition = p;
            rotation = 0;
            velocity = v;
            acceleration = Vector2.Zero;
            drag = 0;
            thrust = 0;
            accelerations = new List<Vector2>();
        }

        public Sprite(String a, Vector2 p)
            : this(a, p, Vector2.Zero)
        {
        }

        public virtual void LoadContent(TextureManager tM)
        {
            //Get the texture
            tM.GetData(asset, out texture, out frame, out textureData, out blockers);

            //Calculate animation stuff
            totalFrames = texture.Width/frame.Width;
            totalModes = texture.Height/frame.Height;
            frameIndex = frame.X/frame.Width;
            modeIndex = frame.Y/frame.Height;

            //Calculate origin
            origin = new Vector2(frame.Width/2, frame.Height/2);
            position -= origin;
        }

        public void LoadContent(ContentManager cM)
        {
            texture = cM.Load<Texture2D>(asset);
            frame = new Rectangle(0, 0, texture.Width, texture.Height);

            //Calculate origin
            origin = new Vector2(frame.Width / 2, frame.Height / 2);
            position -= origin;
        }

        public virtual void Update(ManagerHelper mH)
        {
            #region Finalize Direction

            foreach (Vector2 a in accelerations)
            {
                if (!float.IsNaN(a.X) && !float.IsNaN(a.Y))
                {
                    acceleration += a*mH.GetDeltaSeconds();
                }
            }

            velocity += thrust*acceleration - drag*velocity;

            accelerations.Clear();
            acceleration = Vector2.Zero;

            #endregion

            //Update position
            position += velocity*mH.GetDeltaSeconds();

            if (float.IsNaN(position.X) || float.IsNaN(position.Y))
            {
                //TODO: Do not leave this in.
                throw new Exception("Not sure how this happened.");
            }

            originPosition = position + origin;

            //Update frame
            if (frameIndex < 0)
            {
                frameIndex = 0;
            }
            else if (frameIndex >= totalFrames)
            {
                frameIndex = totalFrames;
            }

            if (modeIndex < 0)
            {
                modeIndex = 0;
            }
            else if (modeIndex >= totalModes)
            {
                modeIndex = totalModes;
            }

            UpdateFrame();
        }

        public void UpdateFrame()
        {
            frame.X = frameIndex * frame.Width;
            frame.Y = modeIndex*frame.Height;
        }

        public virtual void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH)
        {
            this.Draw(sB, displacement, mH, Color.White);
        }

        public virtual void Draw(SpriteBatch sB, Vector2 displacement, ManagerHelper mH, Color alpha)
        {
            float scale = 1;
            if (mH != null)
            {
                CameraManager.Camera cam = mH.GetCurrentCam();

                if (cam != null)
                {
                    scale = cam.GetZoom();
                }
            }
            sB.Draw(texture, (CameraManager.Transform(position, displacement) + origin) * scale, frame, alpha,
                    rotation, origin, scale, SpriteEffects.None, 0);
        }

        public void Turn(float d)
        {
            //Add change in rotation
            rotation += d;

            //If rotation is larger than 2pi
            if (rotation >= MathHelper.TwoPi)
            {
                //Reduce it by 2pi to keep it between 0 and 2pi exclusive
                rotation -= MathHelper.TwoPi;
            }
                //If rotation is smaller than 0
            else if (rotation < 0)
            {
                //Add 2pi to it to keep it between 0 and 2pi exclusive
                rotation += MathHelper.TwoPi;
            }
        }

        #region Sets and Gets

        public float GetRotation()
        {
            return rotation;
        }

        public String GetAsset()
        {
            return asset;
        }

        public void SetRotation(float r)
        {
            rotation = r;
        }

        public Vector2 GetOriginPosition()
        {
            return originPosition;
        }

        public void SetOriginPosition(Vector2 newOriginPosition)
        {
            originPosition = newOriginPosition;
        }

        public virtual List<Vector2> GetFrameBlockers()
        {
            return blockers[frameIndex, modeIndex];
        }

        public Color[,] GetTextureData()
        {
            return textureData;
        }

        public void AddAcceleration(Vector2 a)
        {
            accelerations.Add(a);
        }

        public Rectangle GetFrame()
        {
            return frame;
        }

        public int GetFrameIndex()
        {
            return frameIndex;
        }

        public int GetTotalFrames()
        {
            return totalFrames;
        }

        public void SetFrameIndex(int fI)
        {
            frameIndex = fI;
        }

        public int GetModeIndex()
        {
            return modeIndex;
        }

        public int GetTotalModes()
        {
            return totalModes;
        }

        public void SetModeIndex(int mI)
        {
            modeIndex = mI;
        }

        #endregion
    }
}