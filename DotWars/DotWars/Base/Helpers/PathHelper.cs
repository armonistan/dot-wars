using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DotWars
{
    public class PathHelper
    {
        private static Node[,] field;
        private readonly Sprite square;
        private List<Node> closed;
        private int counter;
        private int length;
        private ManagerHelper managers;
        private Vector2 nodeSize;
        private List<Node> open;
        private int randomness;
        private int width;

        public PathHelper()
        {
            nodeSize = new Vector2(32);
            counter = 0;
            square = new Sprite("square", Vector2.Zero);
        }

        public void Initialize(ManagerHelper mH, int r)
        {
            if (mH.GetGametype() is Survival)
                randomness = 0;
            else
                randomness = r;

            length = (int) mH.GetLevelSize().X/(int) nodeSize.X;
            width = (int) mH.GetLevelSize().Y/(int) nodeSize.Y;
            managers = mH;

            //Node Collections
            field = new Node[length + 1,width + 1];

            //Set up field
            for (int i = 0; i <= length; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    field[i, j] = new Node(null, 0, 0, false, i, j);
                }
            }
        }

        public void LoadContent(TextureManager tM)
        {
            square.LoadContent(tM);
        }

        public void Update()
        {
            //Set up field
            for (int i = 0; i <= length; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    field[i, j].SetBlocker(false);
                }
            }

            foreach (Environment e in managers.GetEnvironmentManager().GetStaticBlockers())
            {
                foreach (Vector2 n in e.GetFrameBlockersSpecial(managers))
                {
                    field[(int) n.X, (int) n.Y].SetBlocker(true);
                }
            }

            foreach (Impassable e in managers.GetEnvironmentManager().GetImpassables())
            {
                foreach (Vector2 n in e.GetFrameBlockersSpecial(managers))
                {
                    field[(int) n.X, (int) n.Y].SetBlocker(true);
                }
            }

            foreach (Impathable e in managers.GetEnvironmentManager().GetImpathables())
            {
                foreach (Vector2 n in e.GetFrameBlockersSpecial(managers))
                {
                    field[(int) n.X, (int) n.Y].SetBlocker(true);
                }
            }

            foreach (LargeRock r in managers.GetAbilityManager().GetLargeRocks())
            {
                field[(int) (r.GetOriginPosition().X/32), (int) (r.GetOriginPosition().Y/32)].SetBlocker(true);
            }

            foreach (LightningTrail l in managers.GetAbilityManager().GetLightning())
            {
                field[(int) (l.GetOriginPosition().X/32), (int) (l.GetOriginPosition().Y/32)].SetBlocker(true);
            }
        }

        public void Draw(SpriteBatch sB, Vector2 d)
        {
            //for (int x = 0; x < length; x++)
            //{
            //    for (int y = 0; y < width; y++)
            //    {
            //        if (x * y < field.Length)
            //        {
            //            if (field[x, y].GetBlocker())
            //            {
            //                square.position.X = x * 32;
            //                square.position.Y = y * 32;
            //                square.Draw(sB, d, managers);
            //            }
            //        }
            //    }
            //}

            //foreach (var p in managers.GetLevel().GetSpawnPoints())
            //{
            //    square.position.X = p.spawnPoint.X;
            //    square.position.Y = p.spawnPoint.Y;
            //
            //    if (p.affilation == NPC.AffliationTypes.grey)
            //    {
            //        square.Draw(sB, d, managers);
            //    }
            //}

            //foreach (NPC a in managers.GetNPCManager().GetNPCs())
            //{
            //    if (a.GetPath() != null && a.GetPath().Count > 0)
            //    {
            //        square.position.X = a.GetPath().First().X;
            //        square.position.Y = a.GetPath().First().Y;
            //        square.Draw(sB, d, managers);
            //    }
            //}
        }

        public void FindClearPath(Vector2 pA, Vector2 pB, ManagerHelper mH, Path path)
        {
            counter = 0;

            open = new List<Node>(length*width);
            closed = new List<Node>(length*width);

            //Prevent excecution if current position or end position is bad
            if (pA.X < 0 || pA.X > mH.GetLevelSize().X || pA.Y < 0 || pA.Y > mH.GetLevelSize().Y ||
                pB.X < 0 || pB.X > mH.GetLevelSize().X || pB.Y < 0 || pB.Y > mH.GetLevelSize().Y)
            {
                return;
            }

            Vector2 end = new Vector2((int) (pB.X/nodeSize.X), (int) (pB.Y/nodeSize.Y)),
                    beginning = new Vector2((int) (pA.X/nodeSize.X), (int) (pA.Y/nodeSize.Y));

            if (end == beginning)
            {
                return;
            }

            if (field[(int) end.X, (int) end.Y].GetBlocker())
            {
                end = FindOpenNodePoint(end, mH);
            }

            //Set up parent according to A*
            Node parent = field[(int) beginning.X, (int) beginning.Y];
            parent.SetFScore((int) (Math.Abs(end.X - beginning.X) + Math.Abs(end.Y - beginning.Y)));
            open.Add(parent);

            //Makes the end not a blocker
            field[(int) end.X, (int) end.Y].SetBlocker(false);

            #region Loop through array until end is found

            while (counter < 100 && !(parent.GetPosition() == end))
            {
                //Checks for instance of no solution
                if (open.Count == 0)
                {
                    break;
                }

                #region Find best new parent

                Node lowest = open.First();

                //Goes through each node on the open list and compares its score to the current low
                foreach (Node x in open)
                {
                    if (x.GetFScore() < lowest.GetFScore())
                    {
                        lowest = x;
                    }
                }

                //Once the lowest is found, switch it from the open to the closed list
                parent = lowest;
                open.Remove(lowest);
                closed.Add(lowest);

                #endregion

                #region Calculate F scores of adjacent nodes

                for (int i = (int) parent.GetPosition().X - 1; i <= (int) parent.GetPosition().X + 1; i++)
                {
                    for (int j = (int) parent.GetPosition().Y - 1; j <= (int) parent.GetPosition().Y + 1; j++)
                    {
                        //If node is invalid
                        if (i < 0 || j < 0 || i >= length || j >= width || field[i, j].GetBlocker() ||
                            closed.Contains(field[i, j]))
                        {
                            continue;
                        }

                        //Current node
                        Node current = field[i, j];

                        #region Calculate cell's info

                        current.SetParent(parent);

                        current.SetGScore(parent.GetGScore() + mH.GetRandom().Next(0, randomness));

                        //Calculate other scores
                        current.SetHScore(
                            (int)
                            ((Math.Abs(current.GetPosition().X - end.X) + Math.Abs(current.GetPosition().Y - end.Y))*10));
                        current.SetFScore(current.GetGScore() + current.GetHScore());

                        #endregion

                        //If the cell is already on the open list and has a better 
                        if (open.Contains(current) && current.GetGScore() > parent.GetGScore())
                        {
                            current.SetFScore(current.GetGScore() + current.GetHScore());
                            current.SetParent(parent);
                        }
                        else
                        {
                            open.Add(current);
                        }
                    }
                }

                #endregion

                counter++;
            }

            #endregion

            #region Create Path

            //Once end is found, compile path together
            path.Clear();

            Node point = parent;
            while (!(point.GetPosition() == beginning))
            {
                if (!point.GetBlocker())
                {
                    path.Add(point.GetPosition()*nodeSize + new Vector2(16), mH);
                }
                point = point.GetParent();
            }

            #endregion
        }

        public void FindEscapePath(Vector2 pA, Vector2 pGTFA, float mD, ManagerHelper mH, float aw, Path path)
        {
            //Add blockers for enemies
            foreach (NPC n in mH.GetNPCManager().GetAlliesInRadius(NPC.AffliationTypes.black, pA, aw))
            {
                int x = -1;
                int y = -1;

                for (int adjX = -32; adjX < 32; adjX += 32)
                {
                    for (int adjY = -32; adjY < 32; adjY += 32)
                    {
                        x = (int) (n.GetOriginPosition().X/nodeSize.X) + adjX;
                        y = (int) (n.GetOriginPosition().Y/nodeSize.Y);

                        if ((y > 0 && y < mH.GetLevel().GetSizeOfLevel().Y) &&
                            (x > 0 && y < mH.GetLevel().GetSizeOfLevel().X))
                            field[x, y].SetBlocker(true);
                    }
                }
            }

            float farthest = 0;
            float convenience = mD; //forces dot to go to furtherest location that is easiest to get to
            Vector2 pointB = pA;
            int nodeX, nodeY;

            //Find best end point
            for (int x = (int) (pA.X - mD)/32; x < (pA.X + mD); x += 32)
            {
                if (x > 32 && x < mH.GetLevel().GetSizeOfLevel().X)
                {
                    for (var y = (int) (pA.Y - mD); y < (pA.Y + mD); y += 32)
                    {
                        if (y > 0 && y < mH.GetLevel().GetSizeOfLevel().Y - 32)
                        {
                            nodeX = x/32;
                            nodeY = y/32;

                            if (!field[nodeX, nodeY].GetBlocker())
                            {
                                var currentPoint = new Vector2(x, y);
                                float dist = Distance(pGTFA, currentPoint);
                                float tempCon = Distance(pA, currentPoint);

                                if (convenience > tempCon && dist > farthest)
                                {
                                    farthest = dist;
                                    convenience = tempCon;
                                    pointB = currentPoint;
                                }
                            }
                        }
                    }
                }
            }

            FindClearPath(pA, pointB, mH, path);
        }

        private Vector2 FindOpenNodePoint(Vector2 e, ManagerHelper mH)
        {
            Vector2 tempEnd = e,
                    tempNewEnd = e;

            while (field[(int) tempEnd.X, (int) tempEnd.Y].GetBlocker())
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if ((int) tempEnd.X + x < 0 || (int) tempEnd.X + x > length || tempEnd.Y + y < 0 ||
                            tempEnd.Y + y > width)
                            continue;

                        if (!field[(int) tempEnd.X + x, (int) tempEnd.Y + y].GetBlocker())
                        {
                            return new Vector2((int) tempEnd.X + x, (int) tempEnd.Y + y);
                        }
                    }
                }

                do
                {
                    tempNewEnd = new Vector2((mH.GetRandom().NextDouble() > 0.5) ? -1 : 1, mH.GetRandom().Next(-1, 1)) +
                                 tempEnd;
                } while (tempNewEnd.X < 0 || tempNewEnd.X > length || tempNewEnd.Y < 0 || tempNewEnd.Y > width);

                tempEnd = tempNewEnd;
            }

            return e;
        }


        public bool IsVectorObstructed(Vector2 pA, Vector2 pB)
        {
            var slope = new Vector2(pB.X - pA.X, pB.Y - pA.Y);
            double theta = Math.Atan2(slope.Y, slope.X);
            slope = new Vector2((float) Math.Cos(theta)*32, (float) Math.Sin(theta)*32);

            pB /= new Vector2(32);
            pB.X = (int) pB.X;
            pB.Y = (int) pB.Y;

            var t = new Vector2((int) (pA.X/32), (int) (pA.Y/32));

            if (t == pB)
            {
                return false;
            }

            return RecursiveVectorObstructed(pA, pB, slope);
        }

        private bool RecursiveVectorObstructed(Vector2 pA, Vector2 pB, Vector2 m)
        {
            int x = (int) (pA.X/32),
                y = (int) (pA.Y/32);
            var tempA = new Vector2(x, y);

            if ((x < 0 || x >= length || y < 0 || y >= width) || pB == tempA)
            {
                return false;
            }
            else if (field[x, y].GetBlocker())
            {
                return true;
            }
            else
            {
                return RecursiveVectorObstructed(pA + m, pB, m);
            }
        }

        public Vector2 GetNodeSize()
        {
            return nodeSize;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetLength()
        {
            return length;
        }

        public static bool IsNodeBlocked(Vector2 checkNode)
        {
            bool isBlocked = (field[(int) (checkNode.X/32), (int) (checkNode.Y/32)].GetBlocker());
            return isBlocked;
        }

        #region Helper Methods

        public static Vector2 MidPoint(Vector2 pA, Vector2 pB)
        {
            Vector2 midPoint; //our midpoint

            float xPoint; //our midpoint's x
            float yPoint; //our midpoint's y

            xPoint = (pA.X + pB.X)/2; //calculate our x midpoint
            yPoint = (pA.Y + pB.Y)/2; //calculate our y midpoint

            midPoint = new Vector2(xPoint, yPoint); //plug in our values

            return midPoint; //return our midpoint
        }

        public static float Distance(Vector2 pA, Vector2 pB)
        {
            return (float) Math.Sqrt(Math.Pow(pA.X - pB.X, 2) + Math.Pow(pA.Y - pB.Y, 2));
        }

        public static float DistanceSquared(Vector2 pA, Vector2 pB)
        {
            return (float) (Math.Pow(pA.X - pB.X, 2) + Math.Pow(pA.Y - pB.Y, 2));
        }

        public static float Direction(Vector2 pA, Vector2 pB)
        {
            //Slope
            float cX = pB.X - pA.X,
                  cY = pB.Y - pA.Y,
                  angle = (float) Math.Atan2(cY, cX);

            if (angle < 0)
            {
                angle += (float) (Math.PI*2);
            }
            else if (angle >= (Math.PI*2))
            {
                angle -= (float) (Math.PI*2);
            }

            return angle;
        }

        public static float Direction(Vector2 r)
        {
            return (float) Math.Atan2(r.Y, r.X);
        }

        public static Vector2 DirectionVector(Vector2 pA, Vector2 pB)
        {
            float tempAngle = Direction(pA, pB);
            return Direction(tempAngle);
        }

        public static Vector2 Direction(float r)
        {
            return new Vector2((float) Math.Cos(r), (float) Math.Sin(r));
        }

        public static float DotProduct(Vector2 v1, Vector2 v2)
        {
            float temp = v1.X*v2.X + v1.Y*v2.Y;
            return temp;
        }

        #endregion

        public class Node
        {
            private readonly Vector2 position;
            private bool blocker;

            private int fScore,
                        gScore,
                        hScore;

            private Node parent;

            public Node(Node p, int g, int h, bool b, int x, int y)
            {
                parent = p;
                gScore = g;
                hScore = h;
                fScore = g + h;
                blocker = b;
                position = new Vector2(x, y);
            }

            public void SetBlocker(bool b)
            {
                blocker = b;
            }

            public bool GetBlocker()
            {
                return blocker;
            }

            public void SetFScore(int f)
            {
                fScore = f;
            }

            public void SetGScore(int g)
            {
                gScore = g;
            }

            public void SetHScore(int h)
            {
                hScore = h;
            }

            public int GetFScore()
            {
                return fScore;
            }

            public int GetGScore()
            {
                return gScore;
            }

            public int GetHScore()
            {
                return hScore;
            }

            public void SetParent(Node p)
            {
                parent = p;
            }

            public Node GetParent()
            {
                return parent;
            }

            public Vector2 GetPosition()
            {
                return position;
            }
        }
    }
}