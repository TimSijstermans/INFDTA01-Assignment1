using System;
using System.Collections.Generic;
using System.Linq;

namespace INFDTA01_1_week1
{
    class NearestNeighboursFinder
    {
        public List<ScoreClass> Find(Dictionary<int, UserPreferences> dataSet, int targetUser, double minThreshold, IMeasureSimularity type, int neighbourAmount)
        {
            var neighbours = new List<ScoreClass>();
            var client = new MeasureClient(type);
            ScoreClass lowestNeighbour = null;
            var targetUserPrefKeys = dataSet[targetUser].Prefs.Keys.ToList();

            //Loop through dataset of UserPreferences classes
            foreach (var pair in dataSet)
            {
                
                if (pair.Key != targetUser)
                {
                    
                    var scorevalue = client.Measure(dataSet[targetUser], pair.Value);
                    var uPrefsKeys = pair.Value.Prefs.Keys.ToList();

                    // ScoreClass is a simple class that stores a score with an user id 
                    var score = new ScoreClass
                    {
                        User = pair.Value,
                        Score = scorevalue,
                        ExtraArticles = uPrefsKeys.Except(targetUserPrefKeys).ToList().Count,
                        SameArticles = uPrefsKeys.Intersect(targetUserPrefKeys).ToList().Count
                    };

                    // Score has to be larger than treshHold AND have additional articles relative to the target user
                    if (score.Score >= minThreshold && score.ExtraArticles > 0 && score.SameArticles > 2)
                    {
                        
                        // neighbours list isn't full yet, add the score.
                        if (neighbours.Count() < neighbourAmount)
                        {
                            if (lowestNeighbour == null || score.Score < lowestNeighbour.Score)
                            {
                                lowestNeighbour = score;
                            }
                            neighbours.Add(score);
                        }
                        // else if score is higher than lowestNeighbour score
                        else if (score.Score > lowestNeighbour.Score)
                        {
                            var index = neighbours.IndexOf(lowestNeighbour);

                            if (index == -1)
                            {
                                break;
                            }
                            neighbours.RemoveAt(index);
                            neighbours.Add(score);
                            lowestNeighbour = GetLowestNeighbour(neighbours);
                        }
                        // else if score == lowestneighbour and score has more "value" (more new articles) 
                        else if (score.ExtraArticles > lowestNeighbour.ExtraArticles && score.Score == lowestNeighbour.Score)
                        {
                            var index = neighbours.IndexOf(lowestNeighbour);

                            if (index == -1)
                            {
                                break;
                            }
                            neighbours.RemoveAt(index);
                            neighbours.Add(score);
                            lowestNeighbour = GetLowestNeighbour(neighbours);
                        }
                            //Console.WriteLine("User " + targetUser + " scores   " + score.Score.ToString("0.000") + "   with user " + pair.Key + " and has " + score.ExtraArticles + " extra values");
                        }
                        //else { Console.WriteLine("Score was too low to be considered, or user didn't have added value)"); }
                }
            }
            var sortedNeighbours = neighbours.OrderByDescending(s => s.Score).ToList();
            return sortedNeighbours;
        }

        private static ScoreClass GetLowestNeighbour(List<ScoreClass> list)
        {
            ScoreClass res = null;
            foreach (var x in list)
            {
                if (res != null)
                {
                    if (x.Score < res.Score)
                    {
                        res = x;
                    } else if (x.Score == res.Score) {
                        if (x.ExtraArticles < res.ExtraArticles || x.SameArticles < res.SameArticles)
                            res = x;
                    }
                }
                else
                {
                    res = x;
                }
            }
            return res;
        }
    }
}