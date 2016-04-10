using System;

namespace INFDTA01_1_week1
{
    class Cosine : IMeasureSimularity
    {
        public double Measure(UserPreferences user1, UserPreferences user2)
        {
            double xy = 0;
            double rootX = 0;
            double rootY = 0;

            foreach (var pair in user1.Prefs)
            {
                var a = pair.Value;
                rootX += Math.Pow(Math.Abs(a), 2);

                if (user2.Prefs.ContainsKey(pair.Key))
                {
                    var b = user2.Prefs[pair.Key];
                    xy += (a*b);
                    rootY += Math.Pow(Math.Abs(b), 2);
                }
            }
            var score = (xy/(Math.Sqrt(rootX)*Math.Sqrt(rootY)));
            return score;
        }
    }
}