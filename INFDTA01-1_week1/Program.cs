using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace INFDTA01_1_week1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ApplicationCode();
        }

        private static void ApplicationCode()
        {
            var dataSet = new Dictionary<int, UserPreferences>();
            var nearestNeighboursFinder = new NearestNeighboursFinder();
            var predictor = new Predictor();

            var minimumThreshold = 0.35;
            var targetUserId = 3;
            var neighbourAmount = 3;
            var recommendAmount = 3;

            StreamReader reader = null;
            ////////////////////////////////////////////////
            //  load dataset from file with a filereader
            //  movielens.data = 100k dataset 
            //  UserItem.data = default dataset 
            ////////////////////////////////////////////////

            var chooseDataset = SelectDataset();
            
            if (chooseDataset.Equals("movielens"))
            {
                reader = new StreamReader(File.OpenRead("../../movielens.data"));
                targetUserId = 186;
                neighbourAmount = 25;
                recommendAmount = 8;
            } else
            {
                reader = new StreamReader(File.OpenRead("../../UserItem.data"));
                targetUserId = 7;
                neighbourAmount = 3;
                recommendAmount = 3;
            }
                

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                var userId = Convert.ToInt32(values[0]);
                var article = Convert.ToInt32(values[1]);
                var rating = Convert.ToDouble(values[2], CultureInfo.InvariantCulture);

                if (dataSet.ContainsKey(userId))
                {
                    dataSet[userId].AddRating(article, rating);
                }
                else
                {
                    var up = new UserPreferences(userId, article, rating);
                    dataSet.Add(userId, up);
                }
            }

            //Comment / Uncomment to toggle writing all users and their ratings to the console
            //foreach (var user in dataSet)
            //{
            //    user.Value.PrintAllRatings();
            //}

            ////////////////////////////////////////////////
            // Write scores of comparing users to console //
            ////////////////////////////////////////////////

            var algorithm = SelectAlgorithm();

            var neighbours = nearestNeighboursFinder.Find(dataSet, targetUserId, minimumThreshold, algorithm, neighbourAmount);

            Console.WriteLine("\n15 Nearest neighbours: \n");
            foreach (var i in neighbours)
            {
                Console.WriteLine(i.User.OwnerId + " With score " + i.Score.ToString("0.0000"));
            }
            Console.WriteLine("");
            
            //Make predictions based on target user with the set of neighbours from previous step.
            List<KeyValuePair<int, double>> predictions = predictor.predict(dataSet[targetUserId], neighbours);
            var suggestions = predictions.Take(recommendAmount);

            foreach (var suggestion in suggestions)
            {
                Console.WriteLine("Recommended article " + suggestion.Key + " With score " + suggestion.Value.ToString("0.0000"));
            }

            // wait for R or Esc before restarting/exiting
            Console.WriteLine("\npress Escape to quit the application or type R to restart");
            restartOrNot();
        }

        /////////////////////////////////////////////
        // Selects dataset to use based on input //
        /////////////////////////////////////////////
        private static String SelectDataset()
        {
            Console.WriteLine("Press key followed by Enter to decide which dataset to use");
            Console.WriteLine("M : Movielens 100k");
            Console.WriteLine("U : UserItem.data ");
            String choice = "";

            while (choice == "")
            {
                var inp = Console.ReadKey();
                switch (inp.KeyChar)
                {
                    case 'm':
                    case 'M':
                        choice = "movielens";
                        break;
                    case 'u':
                    case 'U':
                        choice = "UserItem";
                        break;
                    default:
                        Console.WriteLine("Invalid selection, try again");
                        break;
                }
            }

            Console.WriteLine("");
            return choice;
        }
        
        /////////////////////////////////////////////
        // Selects algorithm to use based on input //
        /////////////////////////////////////////////
        private static IMeasureSimularity SelectAlgorithm()
        {
            Console.WriteLine("Press key followed by Enter to decide which algorithm to use");
            Console.WriteLine("E : Euclidean distance");
            Console.WriteLine("P : Pearson coefficient");
            Console.WriteLine("C : Cosine");
            IMeasureSimularity algorithm = null;

            while (algorithm == null)
            {
                var inp = Console.ReadKey();
                switch (inp.KeyChar)
                {
                    case 'e':
                    case 'E':
                        algorithm = new Euclidean();
                        break;
                    case 'p':
                    case 'P':
                        algorithm = new Pearson();
                        break;
                    case 'c':
                    case 'C':
                        algorithm = new Cosine();
                        break;
                    default:
                        Console.WriteLine("Invalid selection, try again");
                        break;
                }
            }

            Console.WriteLine("");
            return algorithm;
        }

        private static void restartOrNot()
        {
            var read = Console.ReadKey();
            if (read.KeyChar == 'r' || read.KeyChar == 'R')
            {
                Console.WriteLine("");
                ApplicationCode();
            }
            else if (read.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
            else
            {
                restartOrNot();
            }
        }
    }
}