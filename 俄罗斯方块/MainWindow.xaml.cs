using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Timers;
using System.Media;
using System.IO;

namespace 俄罗斯方块
{
    
    public partial class MainWindow : Window
    {
    
        bool IsDead = true;
        Model.Shapes CurrentShape;
        Model.Shapes NextShape;
        ScoreSystem Score = ScoreSystem.GetInstance();
        public WriteableBitmap paper;
        public WriteableBitmap SmallPaper;
        public Timer FallTimer = new Timer(ScoreSystem.CurrentTimerInterval);
      readonly  Random random = new Random();
        //public Timer FlashTimer = new Timer(500);
        //int FlashTimes = 0;
        List<int> ClearLine = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += Form_Load;
            FallTimer.Elapsed += OnTimedEvent_FallTimer;
            DataContext = Score;
            
            
            //FlashTimer.Elapsed += OnTimedEvent_FlashTimer;
        }
        private void Form_Load(object sender, RoutedEventArgs e)
        {
            paper = new WriteableBitmap(Convert.ToInt32(CanvasDraw.ActualWidth), Convert.ToInt32(CanvasDraw.ActualHeight), 96, 96, PixelFormats.Bgra32, null);
            DisplayImage.Source = paper;
            SmallPaper = new WriteableBitmap(Convert.ToInt32(Canvas_NextShape.ActualWidth), Convert.ToInt32(Canvas_NextShape.ActualHeight), 96, 96, PixelFormats.Bgra32, null);
            Image_NextShape.Source = SmallPaper;
            MoveSoundPlayer.SoundLocation = "Sounds\\MoveSound.wav";
            MoveSoundPlayer.Load();
            FallSoundPlayer.SoundLocation = "Sounds\\FallSound.wav";
            FallSoundPlayer.Load(); 
            OverSoundPlayer.SoundLocation = "Sounds\\OverSound.wav";
            OverSoundPlayer.Load();
            StartSoundPlayer.SoundLocation = "Sounds\\StartSound.wav";
            StartSoundPlayer.Load();
            ClearSoundPlayer.SoundLocation = "Sounds\\ClearSound.wav";
            ClearSoundPlayer.Load();
            if(!File.Exists("HighestScore"))
            {
                File.WriteAllText("HighestScore", "0");
            }
            Score.HighestScore=Convert.ToInt32( File.ReadAllText("HighestScore"));
        }
        
        //private void Draw_Click(object sender, RoutedEventArgs e)
        //{

        //    DrawOneShape(paper, CurrentShape, CanvasDraw);
        //}
        //void OnTimedEvent_FlashTimer(Object source, ElapsedEventArgs e)
        //{
        //    FlashTimes = FlashTimes + 1;
        //    DrawOneLine();
        //}
        SoundPlayer MoveSoundPlayer = new SoundPlayer();
        SoundPlayer FallSoundPlayer = new SoundPlayer();
        SoundPlayer StartSoundPlayer = new SoundPlayer();
        SoundPlayer OverSoundPlayer = new SoundPlayer();
        SoundPlayer ClearSoundPlayer = new SoundPlayer();
        private void OnTimedEvent_FallTimer(Object source, ElapsedEventArgs e)
        {
            if(! CurrentShape.DownMove())
            {
                MoveSoundPlayer.Stop();
                MoveSoundPlayer.Play();
                Model.GameArea.LockShape(CurrentShape);
                System.Drawing.Color[,] Clone_Game_area = (System. Drawing. Color[,])Model.GameArea.Game_area.Clone();
                ClearLine =Model.GameArea.ClearLines();
                if(ClearLine.Count!=0)
                {
                    ClearSoundPlayer.Play();
                    DrawOneLine(Clone_Game_area);
                }
                Score.ClearLineScoreCounting(ClearLine.Count);
                FallTimer.Interval = ScoreSystem.CurrentTimerInterval;
                int ShapeIndex = random.Next(0, 7);
                CurrentShape = new Model.Shapes(NextShape.ShapeKind);
                this.Dispatcher.Invoke(new Action(() => ClearShape(SmallPaper, NextShape, Canvas_NextShape)));
                NextShape = new Model.Shapes((Model.Shapes.ShapeName)ShapeIndex);
                NextShape.location = new System.Drawing.Point(0, 0);
                this.Dispatcher.Invoke(new Action(() => DrawOneShape(SmallPaper, NextShape, Canvas_NextShape)));
                if (!IsDead)
                {
                    Start();
                }
                if (IsDead)
                {
                    return;
                }

            }
            this.Dispatcher.Invoke(new Action(() => ClearShape(paper, CurrentShape, CanvasDraw)));
            this.Dispatcher.Invoke( new Action (()=>DrawOneShape(paper, CurrentShape, CanvasDraw)));
        }
       public async  Task DrawOneLine(System.Drawing.Color[,] Clone_GameArea)
        {

            //int FlashTimes = 1;
            for (int FlashTimes = 1; FlashTimes <= 5; FlashTimes++)
            {
                paper.Lock();
                Bitmap BackPaper = new Bitmap(Convert.ToInt32(CanvasDraw.ActualWidth), Convert.ToInt32(CanvasDraw.ActualHeight), paper.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppArgb,paper.BackBuffer);
                Graphics graphics = Graphics.FromImage(BackPaper);
                foreach (int y in ClearLine)
                {
                    for (int x = 0; x< Model.GameArea.Game_area.GetLength(1); x++)
                    {
                        System.Drawing.Point point = new System.Drawing.Point(x*20,y*20);
                        if (FlashTimes % 2 != 0)
                        {
                            DrawOneRectangle(System.Drawing.Color.White, point, graphics, paper);
                        }
                        else
                        {
                            DrawOneRectangle(Clone_GameArea[y,x], point, graphics, paper);
                        }
                    }
                }
                graphics.Flush();
                graphics.Dispose();
                graphics = null;
            BackPaper.Dispose();
            BackPaper = null;
            paper.AddDirtyRect(new Int32Rect(0, 0, Convert.ToInt32(CanvasDraw.ActualWidth), Convert.ToInt32(CanvasDraw.ActualHeight)));
         
            paper.Unlock();
            await Task.Delay(200);
                //paper.WritePixels(0, 0, Convert.ToInt32(CanvasDraw.ActualWidth), Convert.ToInt32(CanvasDraw.ActualHeight));


            }

        }
        void DrawGame_Area()
        {
            paper.Lock();
            Bitmap BackPaper = new Bitmap(Convert.ToInt32(CanvasDraw.ActualWidth), Convert.ToInt32(CanvasDraw.ActualHeight), paper.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, paper.BackBuffer);
            Graphics graphics = Graphics.FromImage(BackPaper);

            for (int x=0; x<30; x++)
            { 
                for (int y = 0; y < 15; y++)
                {
                    if (!Model.GameArea.Game_area[x,y].IsEmpty&&Model.GameArea.Game_area[x,y] != System.Drawing.Color.Black)
                    {
                        DrawOneRectangle(Model.GameArea.Game_area[x, y], new System.Drawing.Point( y* 20, x*20), graphics, paper);
                    }
                }
            }
            graphics.Flush();
            graphics.Dispose();
            graphics = null;
            BackPaper.Dispose();
            BackPaper = null;
            paper.AddDirtyRect(new Int32Rect(0, 0, Convert.ToInt32(CanvasDraw.ActualWidth), Convert.ToInt32(CanvasDraw.ActualHeight)));
            paper.Unlock();
        }

        private void DrawOneShape(WriteableBitmap paper, Model.Shapes Shape, Canvas canvas)
        {

            paper.Lock();
            Bitmap BackPaper = new Bitmap(Convert.ToInt32(canvas.ActualWidth), Convert.ToInt32(canvas.ActualHeight), paper.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, paper.BackBuffer);
            Graphics graphics = Graphics.FromImage(BackPaper);
            for (int y = 0; y< Shape.shape.GetLength(0); y++)
            {
                for (int x = 0; x < Shape.shape.GetLength(1); x++)
                {
                    if(Shape.shape[y,x]!=System.Drawing.Color.Black)
                    {
                        System.Drawing.Point temp_location = new System.Drawing.Point();
                        temp_location.X = Shape.location.X + x * 20;
                        temp_location.Y = Shape. location.Y + y * 20;
                        DrawOneRectangle(Shape.color, temp_location, graphics,paper);
                    }
                }
            }
            graphics.Flush();
            graphics.Dispose();
            graphics = null;
            BackPaper.Dispose();
            BackPaper = null;
            paper.AddDirtyRect(new Int32Rect(0, 0, Convert.ToInt32(canvas.ActualWidth), Convert.ToInt32(canvas.ActualHeight)));
            paper.Unlock();
        }

        private void DrawOneRectangle(System.Drawing.Color color, System.Drawing.Point location, Graphics graphics, WriteableBitmap paper)
        {
            SolidBrush solidBrush = new SolidBrush(color);
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.White);
            //System.Drawing.Pen pen2 = new System.Drawing.Pen(System.Drawing.Color.Black);
            System.Drawing.Rectangle rectangle = System.Drawing.Rectangle.FromLTRB(location.X, location.Y, location.X + 20, location.Y + 20);
            graphics.FillRectangle(solidBrush, rectangle);
            graphics.DrawRectangle(pen, rectangle);
            //System.Drawing.Point point1 = new System.Drawing.Point(location.X, location.Y + 20);
            //System.Drawing.Point point2 = new System.Drawing.Point(location.X+20, location.Y);
            //System.Drawing.Point point3 = new System.Drawing.Point(location.X + 20, location.Y+20);
            //graphics.DrawLine(pen, location, point1);
            //graphics.DrawLine(pen, location, point2);
            //graphics.DrawLine(pen2, point1, point3);
            //graphics.DrawLine(pen2, point2, point3);
        }
         void DrawImage()
        {
            paper.Lock();
            System.Drawing.Image image = System.Drawing.Image.FromFile("GAMEOVER.bmp");
            float x = 0.0F;
            float y = 150.0F;
            System.Drawing.RectangleF rectangle = new System.Drawing.RectangleF(0.0F, 0F, 300.0F, 169.0F);
            GraphicsUnit unit = GraphicsUnit.Pixel;
            Bitmap BackPaper = new Bitmap(Convert.ToInt32(CanvasDraw.ActualWidth), Convert.ToInt32(CanvasDraw.ActualHeight), paper.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, paper.BackBuffer);
            Graphics graphics = Graphics.FromImage(BackPaper);
            graphics.DrawImage(image, x, y, rectangle, unit);
            //paper.AddDirtyRect(new Int32Rect(0, 150, 300, 169));
            paper.AddDirtyRect(new Int32Rect(0, 0, Convert.ToInt32(CanvasDraw.ActualWidth), Convert.ToInt32(CanvasDraw.ActualHeight)));
            paper.Unlock();
        }
        void fall()
        {
            while(CurrentShape.DownMove())
            {
                
            }
            ClearShape(paper, CurrentShape, CanvasDraw);
        }
        private void ClearShape(WriteableBitmap paper, Model.Shapes Shape, Canvas canvas)
        {
            paper.Lock();
            Bitmap BackPaper = new Bitmap(Convert.ToInt32(canvas.ActualWidth), Convert.ToInt32(canvas.ActualHeight), paper.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, paper.BackBuffer);
            Graphics graphics = Graphics.FromImage(BackPaper);
            graphics.Clear(System.Drawing.Color.Black);
            DrawGame_Area();

            graphics.Flush();
            graphics.Dispose();
            graphics = null;
            BackPaper.Dispose();
            BackPaper = null;
            paper.AddDirtyRect(new Int32Rect(0, 0, Convert.ToInt32(canvas.ActualWidth), Convert.ToInt32(canvas.ActualHeight)));
            paper.Unlock();
        }

        private async void Clear_Click(object sender, RoutedEventArgs e)
        {
            //System.Threading.Thread th = new System.Threading.Thread(new Task()=> 
            //    {
            //         ClearLine = new List<int>() { 1 };
            //        for (int FlashTimes = 1; FlashTimes <= 4; FlashTimes++)
            //        {
            //            DrawOneLine(null, FlashTimes);
            //            System.Threading.Thread.Sleep(200);
            //        }
            //    });
            //await FlashLines();
            //DrawOneShape(paper, new Model.Shapes(Model.Shapes.ShapeName.I), CanvasDraw);
        }

    //private async Task FlashLines()
    //{
    //        //ClearLine = new List<int>() { 1 };
    //        for (int FlashTimes = 1; FlashTimes <= 4; FlashTimes++)
    //        {
    //            DrawOneLine(ClearLine, FlashTimes);
              
    //        }
    //    }
        void ToMoveRight()
        {
                if (IsDead)
                {
                    return;
                }
                bool result = CurrentShape.RightMove();
                if (result == true)
                {
                MoveSoundPlayer.Stop();
                MoveSoundPlayer.Play();
                ClearShape(paper, CurrentShape, CanvasDraw);
                    DrawOneShape(paper, CurrentShape, CanvasDraw);
                }
            }
        private void BT_MoveRight_Click(object sender, RoutedEventArgs e)
        {
            ToMoveRight();
        }
        void ToMoveLeft()
        {
            if (IsDead)
            {
                return;
            }
            bool result = CurrentShape.LeftMove();
            if (result == true)
            {
                MoveSoundPlayer.Stop();
                MoveSoundPlayer.Play();
                ClearShape(paper, CurrentShape, CanvasDraw);
                DrawOneShape(paper, CurrentShape, CanvasDraw);
            }
        }
        private void BT_MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            ToMoveLeft();
        }
        private void ToMoveDown()
        {
            if (IsDead)
            {
                return;
            }
            bool result = CurrentShape.DownMove();
            if (result == true)
            {
                MoveSoundPlayer.Stop();
                MoveSoundPlayer.Play();
                ClearShape(paper, CurrentShape, CanvasDraw);
                DrawOneShape(paper, CurrentShape, CanvasDraw);
            }
        }
        private void BT_MoveDown_Click(object sender, RoutedEventArgs e)
        {
            ToMoveDown();
        }

        void ToTurnAround()
        {
            if (IsDead)
            {
                return;
            }
            bool result = CurrentShape.TurnAround();
            if (result == true)
            {
                MoveSoundPlayer.Stop();
                MoveSoundPlayer.Play();
                ClearShape(paper, CurrentShape, CanvasDraw);
                DrawOneShape(paper, CurrentShape, CanvasDraw);
            }
        }
        private void BT_TurnAround_Click(object sender, RoutedEventArgs e)
        {
            ToTurnAround();
        }
        void ToFall()
        {
            if (IsDead)
            {
                return;
            }
            FallTimer.Stop();
            FallSoundPlayer.Stop();
            FallSoundPlayer.Play();
            fall();
            this.Score.FallScoreCounting();
            FallTimer.Interval = ScoreSystem.CurrentTimerInterval;
            DrawOneShape(paper, CurrentShape, CanvasDraw);
            Model.GameArea.LockShape(CurrentShape);
            System.Drawing.Color[,] Clone_Game_area = (System.Drawing.Color[,])Model.GameArea.Game_area.Clone();
            ClearLine = Model.GameArea.ClearLines();
            if (ClearLine.Count != 0)
            {
                ClearSoundPlayer.Play();
                DrawOneLine(Clone_Game_area);
            }
            Score.ClearLineScoreCounting(ClearLine.Count);
            FallTimer.Interval = ScoreSystem.CurrentTimerInterval;
            int ShapeIndex = random.Next(0, 7);
            CurrentShape = new Model.Shapes(NextShape.ShapeKind);
            ClearShape(SmallPaper, NextShape, Canvas_NextShape);
            NextShape = new Model.Shapes((Model.Shapes.ShapeName)ShapeIndex);
            Start();
            if (IsDead)
            {
                NextShape.location = new System.Drawing.Point(0, 0);
                DrawOneShape(SmallPaper, NextShape, Canvas_NextShape);
                return;
            }
            NextShape.location = new System.Drawing.Point(0, 0);
            DrawOneShape(SmallPaper, NextShape, Canvas_NextShape);
            DrawOneShape(paper, CurrentShape, CanvasDraw);
            FallTimer.Start();
        }
        private void Fall_Click(object sender, RoutedEventArgs e)
        {
            ToFall();
        }
        void ToPlayAgain()
        {
            IsDead = false;
            StartSoundPlayer.Play();
            for (int x = 0; x < Model.GameArea.Game_area.GetLength(1); x++)
            {
                for (int y = 0; y < Model.GameArea.Game_area.GetLength(0); y++)
                {
                    Model.GameArea.Game_area[y, x] = System.Drawing.Color.Black;
                }
            }
            ClearShape(paper, CurrentShape, CanvasDraw);
            ClearShape(SmallPaper, NextShape, Canvas_NextShape);
            Score.Reset();
            int ShapeIndex = random.Next(0, 7);
            CurrentShape = new Model.Shapes((Model.Shapes.ShapeName)ShapeIndex);
            NextShape = new Model.Shapes((Model.Shapes.ShapeName)ShapeIndex);
            NextShape.location = new System.Drawing.Point(0, 0);
            DrawOneShape(paper, CurrentShape, CanvasDraw);
            DrawOneShape(SmallPaper, NextShape, Canvas_NextShape);
            //Model.GameArea.test();
            FallTimer.Start();
        }
        private void BT_PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            ToPlayAgain();
        }
        public void Start()
        {
            bool IsStartable = CurrentShape.IsStartable();
            if (!IsStartable)
            {
                OverSoundPlayer.Play();
                FallTimer.Stop();
                IsDead = true;
                this.Dispatcher.Invoke(new Action(() => DrawImage()));
            }
        }
        void ToStop()
        {
            if (FallTimer.Enabled)
            {
                FallTimer.Stop();
                IsDead = true;
            }
            else
            {
                IsDead = false;
                FallTimer.Start();
            }
        }
        private void BT_Stop_Click(object sender, RoutedEventArgs e)
        {
            ToStop();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ScoreSystem.GetInstance().Reset();
            File.WriteAllText("HighestScore", ScoreSystem.m_HighestScore.ToString());
        }

        private void BT_PlayAgain_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    ToFall();
                    e.Handled = true;
                    break;
                case Key.Back:
                    ToStop();
                    break;
                case Key.Right:
                    ToMoveRight();
                    break;
                case Key.Left:
                    ToMoveLeft();
                    break;
                case Key.Up:
                    ToTurnAround();
                    break;
                case Key.Down:
                    ToMoveDown();
                    break;
                case Key.Enter:
                    ToPlayAgain();
                    break;
            }
        }
    }
}
