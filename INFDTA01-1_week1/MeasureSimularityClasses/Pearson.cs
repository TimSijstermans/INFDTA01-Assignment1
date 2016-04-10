using System;

namespace INFDTA01_1_week1
{
    class Pearson : IMeasureSimularity
    {
        public double Measure(UserPreferences user1, UserPreferences user2)
        {
            double sumXiYi = 0;
            double sumXi = 0;
            double sumYi = 0;
            double sumXiPow = 0;
            double sumYiPow = 0;
            double n = 0;

            foreach (var pair in user1.Prefs)
            {
                if (user2.Prefs.ContainsKey(pair.Key))
                {
                    n += 1;
                    var xi = pair.Value;
                    var yi = user2.Prefs[pair.Key];

                    //upper part of formula
                    //left part
                    sumXiYi += (xi*yi);
                    //right part
                    sumXi += xi;
                    sumYi += yi;

                    //bottom part also uses sumXi and sumYi
                    //bottom left 
                    sumXiPow += Math.Pow(xi, 2);
                    sumYiPow += Math.Pow(yi, 2);
                }
            }
            var topPart = sumXiYi - ((sumXi*sumYi)/n);
            var bottomLeftPart = Math.Sqrt(sumXiPow - (Math.Pow(sumXi, 2)/n));
            var bottomRightPart = Math.Sqrt(sumYiPow - (Math.Pow(sumYi, 2)/n));

            var score = Math.Round(topPart/(bottomLeftPart*bottomRightPart), 3);
            return (score);
        }
    }
}