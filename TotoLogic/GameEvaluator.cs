using System;
using System.Collections.Generic;

namespace TotoLogic
{
    public class GameEvaluator
    {
        
        private double[] pools = new double[TotoConstants.MatchesCount];
        public double[,] Profits { get; private set; } = new double[TotoConstants.MatchesCount, 2];
        private List<int> matchOfTheDayIndexes = new List<int>();

        public GameEvaluator(double[] pools, List<int> matchOfTheDayIndexes)
        {
            this.pools = pools;
            this.matchOfTheDayIndexes = matchOfTheDayIndexes;
            SetDistribution();
           // SetProfits();
        }

        private double[] distribution = new double[TotoConstants.MatchesCount + 1];
        private double matchOfTheDayDistribution = 0;

        private void SetDistribution()
        {
            matchOfTheDayDistribution = pools[matchOfTheDayIndexes[0]] * pools[matchOfTheDayIndexes[1]] * pools[matchOfTheDayIndexes[2]];
            distr(0, 0, 1);
            for (int i = 14; i > 0; i--)
                distribution[i] += distribution[i + 1];

            void distr(int currentIndex, int count, double prob)
            {
                if (currentIndex < pools.Length)
                {
                    distr(currentIndex + 1, count + 1, pools[currentIndex] * prob);
                    distr(currentIndex + 1, count, (1 - pools[currentIndex]) * prob);
                }

                else distribution[count] += prob;
            }
        }

        public void SetProfits()
        {
            setprofits(0, 0, 0, 0, 1);
            void setprofits(int currentIndex, int count, int modCount, int combination, double prob)
            {
                if (currentIndex < pools.Length)
                {
                    bool isMOD = matchOfTheDayIndexes.Contains(currentIndex);
                    setprofits(currentIndex + 1, count + 1,  isMOD ? modCount + 1 : modCount, combination + (int)Math.Pow(2, currentIndex), prob * pools[currentIndex]);
                    setprofits(currentIndex + 1, count, modCount, combination, prob * (1 -  pools[currentIndex]));
                }
                else
                {
                    var prize = GetPrize(modCount == TotoConstants.MatchOfTheDayCount, count, prob);
                    if (prize == 0) return;
                    for(int i = 0; i < TotoConstants.MatchesCount; i++)
                    {
                        int r = combination % 2;
                        var p = r == 0 ? 1 - pools[i] : pools[i];
                        Profits[i, r] += prize / p;
                        combination /= 2;
                    }
                    
                }
            }

           
        }

        public double GetPrize(bool mod, int count, double prob)
        {
            if (count == 0) return pr(0);
            double prize = 0;
            if (mod) prize += modpr();

            for (int i = 8; i <= count; i++)
                prize += pr(i);

            double pr(int index)
            {
                var value = Math.Min(TotoConstants.PrizePools[index] / distribution[index], index == 15 ? TotoConstants.MaxWin + TotoConstants.Jackpot : TotoConstants.MaxWin);
                return value * prob;
            }

            double modpr()
            {
                var value = Math.Min(TotoConstants.MATCH_OF_THE_DAY / matchOfTheDayDistribution, TotoConstants.MaxWin);
                return value * prob;
            }

            return prize;
        }

    }
}
