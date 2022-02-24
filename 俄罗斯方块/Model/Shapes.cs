using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace 俄罗斯方块.Model
{
    public class Shapes
    {
        public enum ShapeName
        {
            Z1, Z2, T, O, L1, L2, I
        }
        List<Color[,]> AllShapes = new List<Color[,]>();
        public ShapeName ShapeKind;
        public Color[,] shape
        {
            get
            {
                return AllShapes[CurrentShapeIndex];
            }
        }
        public int CurrentShapeIndex { get; set; } = 0;
        public Color color { get; set; } = Color.Black;
        public Point location { get; set; }
        public Shapes(ShapeName shapeName)
        {
            ShapeKind = shapeName;
            switch (shapeName)
            {
                case ShapeName.I:
                    AllShapes.Add(new Color[1, 4] {
                    { Color.Brown, Color.Brown, Color.Brown, Color.Brown }
                    });
                    AllShapes.Add(new Color[4, 1]
                    {
                        {Color.Brown },
                       {Color.Brown },
                       {Color.Brown },
                       {Color.Brown }
                    });
                    color = Color.Brown;
                    break;
                case ShapeName.O:
                    AllShapes.Add(new Color[2, 2]
                    {
                        { Color.Red, Color.Red },
                        { Color.Red, Color.Red }
                    });
                    color = Color.Red;
                    break;
                case ShapeName.T:
                    AllShapes.Add(new Color[2, 3]
                    {
                         { Color.Blue, Color.Blue, Color.Blue },
                         { Color.Black, Color.Blue, Color.Black }
                    });
                    AllShapes.Add(new Color[3, 2]
                        {
                         {Color.Black, Color.Blue },
                         {Color.Blue, Color.Blue },
                         {Color.Black, Color.Blue }
                        });
                    AllShapes.Add(new Color[2, 3]
                        {
                         { Color.Black, Color.Blue, Color.Black},
                         { Color.Blue, Color.Blue, Color.Blue }
                        });
                    AllShapes.Add(new Color[3, 2]
                        {
                         {Color.Blue, Color.Black },
                         {Color.Blue, Color.Blue },
                         {Color.Blue, Color.Black }
                        });
                    color = Color.Blue;
                    break;
                case ShapeName.L1:
                    AllShapes.Add(new Color[3, 2]
                       {
                        { Color.Green, Color.Black },
                        { Color.Green, Color.Black },
                        { Color.Green, Color.Green }
                       });
                    AllShapes.Add(new Color[2, 3]
                        {
                         { Color.Green, Color.Green, Color.Green},
                         { Color.Green, Color.Black, Color.Black }
                       });
                    AllShapes.Add(new Color[3, 2]
                       {
                        { Color.Green, Color.Green},
                        { Color.Black, Color.Green },
                        { Color.Black, Color.Green }
                       });
                    AllShapes.Add(new Color[2, 3]
                     {
                         { Color.Black, Color.Black, Color.Green},
                         { Color.Green, Color.Green, Color.Green }
                    });
                    color = Color.Green;
                    break;
                case ShapeName.L2:
                    AllShapes.Add(new Color[3, 2]
                    {
                         { Color.Black, Color.Purple },
                        { Color.Black, Color.Purple },
                        { Color.Purple, Color.Purple }
                    });
                    AllShapes.Add(new Color[2, 3]
                    {
                         { Color.Purple, Color.Black, Color.Black},
                         { Color.Purple, Color.Purple, Color.Purple }
                    });
                    AllShapes.Add(new Color[3, 2]
                    {
                         { Color.Purple, Color.Purple },
                        { Color.Purple, Color.Black },
                        { Color.Purple, Color.Black }
                    });
                    AllShapes.Add(new Color[2, 3]
                    {
                         { Color.Purple, Color.Purple, Color.Purple},
                         { Color.Black, Color.Black, Color.Purple }
                    });
                    color = Color.Purple;
                    break;
                case ShapeName.Z1:
                    AllShapes.Add(new Color[2, 3]
                    {
                        { Color.DarkCyan, Color.DarkCyan, Color.Black },
                        { Color.Black, Color.DarkCyan, Color.DarkCyan }
                    });
                    AllShapes.Add(new Color[3, 2]
                    {
                        { Color.Black, Color.DarkCyan },
                        { Color.DarkCyan, Color.DarkCyan },
                        { Color.DarkCyan, Color.Black }
                    });
                    color = Color.DarkCyan;
                    break;
                case ShapeName.Z2:
                    AllShapes.Add(new Color[2, 3]
                    {
                        {Color.Black, Color.DarkOrange, Color.DarkOrange},
                        { Color.DarkOrange, Color.DarkOrange, Color.Black}
                    });
                    AllShapes.Add(new Color[3, 2]
                    {
                        { Color.DarkOrange, Color.Black },
                        { Color.DarkOrange, Color.DarkOrange },
                        { Color.Black, Color.DarkOrange }
                    });
                    color = Color.DarkOrange;
                    break;
            }
            location = new Point((15 - shape.GetLength(1)) / 2 * 20, 0);
        }
        public enum Action { Right, Left, Down, TurnAround, Start }
        public List<Point> FindFirstColor(Action action)
        {
            List<Point> points = new List<Point>();
            int x;
            int y;

            switch (action)
            {
                case Action.Right:
                    for (y = 0; y < shape.GetLength(0); y++)
                    {
                        for (x = shape.GetLength(1) - 1; x >= 0; x--)
                        {
                            if (shape[y, x] != Color.Black && shape[y, x] != Color.Empty)
                            {
                                points.Add(new Point(location.X + x * 20, location.Y + y * 20));
                                break;
                            }
                        }
                    }
                    break;
                case Action.Left:
                    for (y = 0; y < shape.GetLength(0); y++)
                    {
                        for (x = 0; x < shape.GetLength(1); x++)
                        {
                            if (shape[y, x] != Color.Black && shape[y, x] != Color.Empty)
                            {
                                points.Add(new Point(location.X + x * 20, location.Y + y * 20));
                                break;
                            }
                        }
                    }
                    break;
                case Action.Down:
                    for (x = 0; x < shape.GetLength(1); x++)
                    {
                        for (y = shape.GetLength(0) - 1; y >= 0; y--)
                        {
                            if (shape[y, x] != Color.Black && shape[y, x] != Color.Empty)
                            {
                                points.Add(new Point(location.X + x * 20, location.Y + y * 20));
                                break;
                            }
                        }
                    }
                    break;
                case Action.TurnAround:
                case Action.Start:
                    int ShapeIndex = GetNextShapeIndex();
                    if (action == Action.Start)
                    {
                        ShapeIndex = CurrentShapeIndex;
                    }
                    for (x = 0; x < AllShapes[ShapeIndex].GetLength(1); x++)
                    {
                        for (y = 0; y < AllShapes[ShapeIndex].GetLength(0); y++)
                        {
                            if (AllShapes[ShapeIndex][y, x] != Color.Black && AllShapes[ShapeIndex][y, x] != Color.Empty)
                            {
                                points.Add(new Point(location.X + x * 20, location.Y + y * 20));
                            }
                        }
                    }
                    break;
            }
            return points;
        }
        public bool RightMove()
        {
            List<Point> points = FindFirstColor(Action.Right);
            if ((location.X + shape.GetLength(1) * 20) / 20 < 15)
            {
                foreach (Point ThePoint in points)
                {
                    if (GameArea.Game_area[ThePoint.Y / 20, ThePoint.X / 20 + 1] != Color.Black && GameArea.Game_area[ThePoint.Y / 20, ThePoint.X / 20 + 1] != Color.Empty)
                    {
                        return false;
                    }
                }
                Point temp = location;
                temp.X = temp.X + 20;
                location = temp;
                return true;
            }
            return false;
        }
        public bool LeftMove()
        {
            List<Point> points = FindFirstColor(Action.Left);
            if (location.X / 20 > 0)
            {
                foreach (Point ThePoint in points)
                {
                    if (GameArea.Game_area[ThePoint.Y / 20, ThePoint.X / 20 - 1] != Color.Black && GameArea.Game_area[ThePoint.Y / 20, ThePoint.X / 20 - 1] != Color.Empty)
                    {
                        return false;
                    }
                }
                Point temp = location;
                temp.X = temp.X - 20;
                location = temp;
                return true;
            }
            return false;
        }
        public bool DownMove()
        {
            List<Point> points = FindFirstColor(Action.Down);
            if ((location.Y + shape.GetLength(0) * 20) / 20 <= 29)
            {
                foreach (Point ThePoint in points)
                {
                    if (GameArea.Game_area[ThePoint.Y / 20 + 1, ThePoint.X / 20] != Color.Black && GameArea.Game_area[ThePoint.Y / 20 + 1, ThePoint.X / 20] != Color.Empty)
                    {
                        return false;
                    }
                }
                Point temp = location;
                temp.Y = temp.Y + 20;
                location = temp;
                return true;
            }
            return false;
        }
        int GetNextShapeIndex()
        {
            int result = 0;
            if (CurrentShapeIndex < AllShapes.Count - 1)
            {
                result = CurrentShapeIndex + 1;
            }
            return result;
        }
        public bool TurnAround()
        {
            if (AllShapes.Count > 1 && CurrentShapeIndex == AllShapes.Count - 1)// Last kind of shape
            {
                List<Point> points = FindFirstColor(Action.TurnAround);
                if ((location.Y + AllShapes[0].GetLength(0) * 20) / 20 <= 30&& (location.X + AllShapes[0].GetLength(1) * 20) / 20 <= 15)
                {
                    foreach (Point ThePoint in points)
                    {
                        if (GameArea.Game_area[ThePoint.Y / 20, ThePoint.X / 20] != Color.Black && GameArea.Game_area[ThePoint.Y / 20, ThePoint.X / 20] != Color.Empty)
                        {
                            return false;
                        }
                    }
                    CurrentShapeIndex = 0;
                    return true;
                }
                
            }
            else if (AllShapes.Count > 1 && CurrentShapeIndex < AllShapes.Count - 1)
            {
                List<Point> points = FindFirstColor(Action.TurnAround);
                if ((location.Y + AllShapes[CurrentShapeIndex + 1].GetLength(0) * 20) / 20 <= 30 && (location.X + AllShapes[CurrentShapeIndex + 1].GetLength(1) * 20) / 20 <= 15)
                {
                    foreach (Point ThePoint in points)
                    {
                        if (GameArea.Game_area[ThePoint.Y / 20, ThePoint.X / 20] != Color.Black && GameArea.Game_area[ThePoint.Y / 20, ThePoint.X / 20] != Color.Empty)
                        {
                            return false;
                        }
                    }
                    CurrentShapeIndex = CurrentShapeIndex + 1;
                    return true;
                }
            }
            return false;
        }
        public bool IsStartable()
        {
            List<Point> points = FindFirstColor(Action.Start);
                foreach (Point ThePoint in points)
                {
                    Debug.Print(ThePoint.ToString());
                    if (GameArea.Game_area[ThePoint.Y / 20, ThePoint.X / 20] != Color.Black && GameArea.Game_area[ThePoint.Y / 20, ThePoint.X / 20] != Color.Empty)
                    {
                        return false;
                    }
                }
                return true;
        }
    }
}
