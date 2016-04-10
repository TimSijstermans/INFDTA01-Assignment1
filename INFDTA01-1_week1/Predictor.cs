using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFDTA01_1_week1
{
    class Predictor
    {
        public List<KeyValuePair<int, double>> predict (UserPreferences targetUser, List<ScoreClass> neighbours)
        {
            Dictionary<int, double> predictedRatings = new Dictionary<int, double>();
            List<int> articlesToCheck = new List<int>();

            foreach (var neighbour in neighbours)
            {
                foreach (var rating in neighbour.User.Prefs)
                {
                    if (!targetUser.Prefs.ContainsKey(rating.Key) && !articlesToCheck.Contains(rating.Key)){
                        articlesToCheck.Add(rating.Key);
                    }
                }
            }

            while(articlesToCheck.Any())
            {
                var article = articlesToCheck.Last();
                double sumWeightedRatings = 0;
                double sumCoefficients = 0;
                var neighboursWithArticle = 0;


                foreach (var neighbour in neighbours) {
                    if (neighbour.User.Prefs.ContainsKey(article)){
                        neighboursWithArticle++;
                        sumWeightedRatings += (neighbour.User.Prefs[article]*neighbour.Score);
                        sumCoefficients += neighbour.Score;
                    }
                }
                var predictedRating = sumWeightedRatings / sumCoefficients;
                articlesToCheck.Remove(article);

                if (neighboursWithArticle > 2)
                {
                    predictedRatings.Add(article, predictedRating);
                }
            }

            var sortedPredictions = predictedRatings.ToList();
            sortedPredictions.Sort((firstPair, nextPair) =>
            {
                return -firstPair.Value.CompareTo(nextPair.Value);
            });

            return sortedPredictions;
        }
    }
}
