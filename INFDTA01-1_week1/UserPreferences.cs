using System;
using System.Collections.Generic;


namespace INFDTA01_1_week1
{
    class UserPreferences
    {
        public UserPreferences() { }
        public UserPreferences(int owner, int article, double rating)
        {
            OwnerId = owner;
            Prefs = new Dictionary<int, double> {{article, rating}};
        }

        public void AddRating(int article, double rating)
        {
            Prefs.Add(article, rating);
        }

        public Dictionary<int, double> Prefs { get; set; }
        public int OwnerId { get; set; }

        public void PrintAllRatings()
        {
            Console.WriteLine("User " + OwnerId + ":");
            var i = 0;
            foreach (var pair in Prefs)
            {
                Console.WriteLine("\tArticle: " + pair.Key + ", Rating: " + pair.Value);
                if (i > 10)
                {
                    Console.WriteLine("\tAnd " + (Prefs.Count - 10) + " more");
                    break;
                }
                i++;
            }
            Console.WriteLine("");
        }
    }
}
