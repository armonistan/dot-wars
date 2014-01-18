#region

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

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

            open = new List<Node>(length*width);
            closed = new List<Node>(length*width);
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
                foreach (Vector2Int n in e.GetFrameBlockers())
                {
                    SetBlockerRelative(e, n);
                }
            }

            foreach (Impassable e in managers.GetEnvironmentManager().GetImpassables())
            {
                foreach (Vector2Int n in e.GetFrameBlockers())
                {
                    SetBlockerRelative(e, n);
                }
            }

            foreach (Impathable e in managers.GetEnvironmentManager().GetImpathables())
            {
                foreach (Vector2Int n in e.GetFrameBlockers())
                {
                    SetBlockerRelative(e, n);
                }
            }

            foreach (LargeRock r in managers.GetAbilityManager().GetLargeRocks())
            {
                field[(int) (r.GetOriginPosition().X/32f), (int) (r.GetOriginPosition().Y/32f)].SetBlocker(true);
            }

            foreach (LightningTrail l in managers.GetAbilityManager().GetLightning())
            {
                field[(int) (l.GetOriginPosition().X/32f), (int) (l.GetOriginPosition().Y/32f)].SetBlocker(true);
            }
        }

        private void SetBlockerRelative(Environment e, Vector2Int n)
        {
            int x = (int) (e.position.X/32f) + n.X;
            int y = (int) (e.position.Y/32f) + n.Y;

            if (x >= 0 && x < GetLength() && y >= 0 && y < GetWidth())
            {
                field[x, y].SetBlocker(true);
            }
        }

        public void Draw(SpriteBatch sB, Vector2 d)
        {
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    if (x * y < field.Length)
                    {
                        if (field[x, y].GetBlocker())
                        {
                            square.position.X = x * 32;
                            square.position.Y = y * 32;
                            square.Draw(sB, d, managers);
                        }
                    }
                }
            }

            foreach (var p in managers.GetLevel().GetSpawnPoints())
            {
                square.position.X = p.spawnPoint.X;
                square.position.Y = p.spawnPoint.Y;
            
                if (p.affilation == NPC.AffliationTypes.grey)
                {
                    square.Draw(sB, d, managers);
                }
            }

            foreach (NPC a in managers.GetNPCManager().GetNPCs())
            {
                if (a.GetPath().Count > 0)
                {
                    square.position.X = a.GetPath().Last().X-16;
                    square.position.Y = a.GetPath().Last().Y-16;
                    square.Draw(sB, d, managers);
                }
            }
        }

        public void FindClearPath(Vector2 pA, Vector2 pB, ManagerHelper mH, Path path)
        {
            counter = 0;

            open.Clear();
            closed.Clear();

            //Prevent excecution if current position or end position is bad
            if (pA.X < 0 || pA.X > mH.GetLevelSize().X || pA.Y < 0 || pA.Y > mH.GetLevelSize().Y ||
                pB.X < 0 || pB.X > mH.GetLevelSize().X || pB.Y < 0 || pB.Y > mH.GetLevelSize().Y)
            {
                return;
            }

            Vector2Int end = new Vector2Int((int) (pB.X/nodeSize.X), (int) (pB.Y/nodeSize.Y)),
                    beginning = new Vector2Int((int) (pA.X/nodeSize.X), (int) (pA.Y/nodeSize.Y));

            if (end == beginning)
            {
                return;
            }

            if (field[end.X, end.Y].GetBlocker())
            {
                end = FindOpenNodePoint(end, mH);
            }

            //Set up parent according to A*
            Node parent = field[(int) beginning.X, (int) beginning.Y];
            parent.SetFScore((int) (MathHelper.Distance(end.X, beginning.X) + MathHelper.Distance(end.Y, beginning.Y)));
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
                            (MathHelper.Distance(current.GetPosition().X, end.X) +
                             MathHelper.Distance(current.GetPosition().Y, end.Y))*10);
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
                    path.AddPoint(new Vector2(point.GetPosition().X, point.GetPosition().Y) * nodeSize + new Vector2(16));
                }
                point = point.GetParent();
            }

            #endregion
        }

        public void FindEscapePath(Vector2 pA, Vector2 pGTFA, float mD, ManagerHelper mH, float aw, Path path)
        {
            //Add blockers for enemies
            foreach (NPC n in mH.GetNPCManager().GetAllies(NPC.AffliationTypes.black))
            {
                if (NPCManager.IsNPCInRadius(n, pA, aw))
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
            }

            float farthest = 0;
            float convenience = mD*mD; //forces dot to go to furtherest location that is easiest to get to
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
                                float dist = DistanceSquared(pGTFA, currentPoint);
                                float tempCon = DistanceSquared(pA, currentPoint);

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

        private Vector2Int FindOpenNodePoint(Vector2Int e, ManagerHelper mH)
        {
            Vector2Int tempEnd = e,
                    tempNewEnd = e;

            while (field[tempEnd.X, tempEnd.Y].GetBlocker())
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (tempEnd.X + x < 0 || tempEnd.X + x > length || tempEnd.Y + y < 0 ||
                            tempEnd.Y + y > width)
                        {
                            continue;
                        }

                        if (!field[tempEnd.X + x, tempEnd.Y + y].GetBlocker())
                        {
                            return new Vector2Int(tempEnd.X + x, tempEnd.Y + y);
                        }
                    }
                }

                do
                {
                    tempNewEnd = new Vector2Int((mH.GetRandom().NextDouble() > 0.5) ? -1 : 1, mH.GetRandom().Next(-1, 1)) +
                                 tempEnd;
                } while (tempNewEnd.X < 0 || tempNewEnd.X > length || tempNewEnd.Y < 0 || tempNewEnd.Y > width);

                tempEnd = tempNewEnd;
            }

            return e;
        }


        public bool IsVectorObstructed(Vector2 pA, Vector2 pB)
        {
            var slope = new Vector2(pB.X - pA.X, pB.Y - pA.Y);
            float theta = DWMath.Atan2(slope.Y, slope.X);
            slope = new Vector2(DWMath.Cos(theta)*32, DWMath.Sin(theta)*32);

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

        public Node[,] GetField()
        {
            return field;
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

        public static float DistanceSquared(Vector2 pA, Vector2 pB)
        {
            return (pA.X - pB.X)*(pA.X - pB.X) + (pA.Y - pB.Y)*(pA.Y - pB.Y);
        }

        public static float Direction(Vector2 pA, Vector2 pB)
        {
            //Slope
            float cX = pB.X - pA.X,
                  cY = pB.Y - pA.Y;
            float angle = DWMath.Atan2(cY, cX);

            if (angle < 0)
            {
                angle += MathHelper.TwoPi;
            }
            else if (angle >= MathHelper.TwoPi)
            {
                angle -= (MathHelper.TwoPi);
            }

            return angle;
        }

        public static float Direction(Vector2 r)
        {
            return DWMath.Atan2(r.Y, r.X);
        }

        public static Vector2 DirectionVector(Vector2 pA, Vector2 pB)
        {
            float tempAngle = Direction(pA, pB);
            return Direction(tempAngle);
        }

        public static Vector2 Direction(float r)
        {
            return new Vector2(DWMath.Cos(r), DWMath.Sin(r));
        }

        public static float DotProduct(Vector2 v1, Vector2 v2)
        {
            float temp = v1.X*v2.X + v1.Y*v2.Y;
            return temp;
        }

        #endregion

        public struct Vector2Int
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Vector2Int(int x, int y) : this()
            {
                X = x;
                Y = y;
            }

            public static bool operator ==(Vector2Int a, Vector2Int b)
            {
                return a.X == b.X && a.Y == b.Y;
            }

            public static bool operator !=(Vector2Int a, Vector2Int b)
            {
                return !(a == b);
            }

            public static Vector2Int operator +(Vector2Int a, Vector2Int b)
            {
                return new Vector2Int(a.X + b.X, a.Y + b.Y);
            }
        }

        public class Node
        {
            private readonly Vector2Int position;
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
                position = new Vector2Int(x, y);
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

            public Vector2Int GetPosition()
            {
                return position;
            }
        }
    }
}