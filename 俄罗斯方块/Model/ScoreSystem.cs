using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace 俄罗斯方块
{
   public  class ScoreSystem : INotifyPropertyChanged

    {
        public static int BasicTimerInterval = 1000;
        public static int m_CurrentScore = 0;
        public static int FallScore = 1;
        public static int BasicClearLineScore = 8;
        public static int BasicLevelUpScoreNeeded = 100;
        public static int LevelUpTimeReduce = 20;
        public static int LevelUpScoreIncrease = 50;
        public static int ClearLineScoreTimes = 4;
        public static int m_CurrentLevel = 0;
        public static int CurrentTimerInterval = BasicTimerInterval;
        public static int m_CurrentLevelUpScoreNeeded = BasicLevelUpScoreNeeded;
        public static int m_HighestScore = 0;
        private static ScoreSystem Instance = null;

        public event PropertyChangedEventHandler PropertyChanged;

        private ScoreSystem()
        {

        }

        public static  ScoreSystem GetInstance()
        {
            if (Instance == null)
                Instance = new ScoreSystem();
            return Instance;
          }

        public  int CurrentScore
        {
            get
            {
                return m_CurrentScore;
            }
            set
            {
                if (m_CurrentScore != value)
                {
                    m_CurrentScore = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentScore"));
                }
            }
        }
        public int CurrentLevel
        {
            get
            {
                return m_CurrentLevel;
            }
            set
            {
                if(m_CurrentLevel!=value)
                {
                    m_CurrentLevel = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentLevel"));
                }
            }
        }
        public int HighestScore
        {
            get
            {
                return m_HighestScore;
            }
            set
            {
                if(m_HighestScore!=value)
                {
                    m_HighestScore = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("HighestScore"));
                }
            }
        }
        public int CurrentLevelUpScoreNeeded
        {
            get
            {
                return m_CurrentLevelUpScoreNeeded;
            }
            set
            {
                if(m_CurrentLevelUpScoreNeeded != value)
                {
                    m_CurrentLevelUpScoreNeeded = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentLevelUpScoreNeeded"));
                }
            }
        }
        public  void LevelUp()
        {
            if (CurrentScore >= CurrentLevelUpScoreNeeded)
            {
                CurrentLevel = CurrentLevel + 1;
                CurrentTimerInterval = CurrentTimerInterval - LevelUpTimeReduce;
                CurrentLevelUpScoreNeeded = CurrentLevelUpScoreNeeded + LevelUpScoreIncrease*CurrentLevel+BasicLevelUpScoreNeeded;
            }
        }
        public  void Reset()
        {
            if (CurrentScore > HighestScore)
            {
                HighestScore = CurrentScore;
            }
            CurrentScore = 0;
            CurrentLevel = 0;
            CurrentTimerInterval = BasicTimerInterval;
        }
        public  void FallScoreCounting()
        {
            CurrentScore = CurrentScore + FallScore;
            LevelUp();
        }
        public  void ClearLineScoreCounting(int ClearLineCount)
        {
            CurrentScore = CurrentScore + BasicClearLineScore * ClearLineCount + ClearLineCount * ClearLineScoreTimes;
            LevelUp();
        }
    }
}
