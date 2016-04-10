using System;
namespace INFDTA01_1_week1
{
    class Euclidean : IMeasureSimularity
    {
        public double Measure(UserPreferences user1, UserPreferences user2)
        {
            double x = 0;

            foreach (var pair in user1.Prefs)
            {
                if (user2.Prefs.ContainsKey(pair.Key))
                {
                    double pi = pair.Value;
                    double qi = user2.Prefs[pair.Key];
                    x += Math.Pow((pi - qi), 2);
                }
            }
            double distance = Math.Round(Math.Sqrt(x), 3);
            double score = 1 / (1 + distance);

            return(score);
        }
    }
}
