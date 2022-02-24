using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace 俄罗斯方块.Model
{
    
    public static class GameArea
    {
        static Action ScoreChanged { get; set; } = null;
        private static Color[,] Game_Area = new Color[30, 15];


        public static Color[,] Game_area
        {
            get
            {
                    return Game_Area;
            }
        }
        //public static void test()
        //{
        //    for (int y = Game_area.GetLength(0)-1; y >=20; y--)
        //    {
        //        for(int x =0; x<Game_area.GetLength(1); x++)
        //        {
        //            if (y == 28 && x == 1)
        //            {
        //                //Game_area[y, x] = Color.Black;
        //            }
        //            else
        //            {
        //                Game_area[y, x] = Color.Brown;
        //            }
        //        }
        //    }
        //}
        public static void Clear()
        {
            for(int i =0; i<15; i++)
            {
                for(int j=0; j<30; j++)
                {
                    Game_area[i, j] = Color.Black;
                }
            }
        }
        public static void LockShape(Shapes shape)
        {
            for (int y = 0; y < shape.shape.GetLength(0); y++)
            {
                for (int x = 0; x <shape.shape.GetLength(1); x++)
                {
                    if (shape.shape[y,x] != Color.Black)
                    {
                        Game_area[shape.location.Y / 20 + y, shape.location.X / 20 + x] = shape.shape[y,x];
                    }
                }
            }//Y loop ends
        }
        public static List<int> ClearLines()
        {
            bool IsClearable = true;
            List<int> result = new List<int>();
            for(int y=Game_area.GetLength(0)-1; y>=0; y--)
            {
                for(int x=0; x<Game_area.GetLength(1);x++)
                {
                    if(Game_area[y,x]==Color.Empty||Game_area[y,x]==Color.Black)//If the Line can not be cleared
                    {
                        IsClearable = false;
                        break;
                    }
                }
                if(IsClearable)//If the Line can be cleared
                {
                    result.Add(y);
                   
                    for (int x = 0; x < Game_area.GetLength(1); x++)
                    {
                        Game_area[y, x] = Color.Empty;
                    }

                }
                else
                {
                    if(result.Count>0)//If the line needs to fall
                    {
                        for (int x = 0; x < Game_area.GetLength(1); x++)
                        {
                            Game_area[y +result.Count, x ] = Game_area[y, x];
                            Game_area[y, x] = Color.Empty;
                        }
                    }
                    IsClearable = true;
                }
            }//Y loop ends
            return result;
        }
    }
}
