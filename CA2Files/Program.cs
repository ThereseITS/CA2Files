namespace CA2Files
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "../../../bookReviews.csv"; // file path


            using (StreamWriter sw = File.CreateText(path)) // create a csv file for the book reviews
            {
                sw.WriteLine("B12345678,1");
                sw.WriteLine("B12345677,5");
                sw.WriteLine("B12345676,2");
                sw.WriteLine("B12345675,3");
                sw.WriteLine("B12345674,1");
                sw.WriteLine("ukgkyugk,4");
                sw.WriteLine("B12345672,4");

            }

            string[] bookIds = new string[10];
            double[] reviewScores = new double[10];   //here we are using arrays - you can change to use lists- see next example.

            string input = "";


            int count = 0;
            try
            {

                if (File.Exists(path))
                {
                    using (StreamReader sr = File.OpenText(path))
                    {
                        while ((input = sr.ReadLine()) != null && count < 10)  // count restricts the number of records read in.
                        {
                            string[] fields = input.Split(',');

                            if (fields.Length == 2 && IsValidBookId(fields[0]) && IsValidScore(fields[1]))
                            {


                                bookIds[count] = fields[0];
                                reviewScores[count] = Convert.ToDouble(fields[1]);


                                Console.WriteLine($"{reviewScores[count]} is the score for:  {bookIds[count]}");
                                count++;
                            }
                            else
                            {
                                Console.WriteLine("Invalid Record");
                            }

                        }
                    }
                    DisplayReport(bookIds, reviewScores, count);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        static bool IsValidBookId(string bookId)// checks that the BookId is in the required format ( can also use RegEx class here - we'll cover this later).
        {

            bookId = bookId.Trim();
            if ((bookId != null) && (bookId.Length == 9) && (bookId[0] == 'B') && (int.TryParse(bookId.Substring(1, 8), out int number)))
            {
                return true;
            }

            return false;

        }
        static bool IsValidScore(string input) // checks that the score is valid - note the ordering of conditions here
        {
            double reviewScore = 0;
            if (double.TryParse(input, out reviewScore) && (reviewScore >= 0) && (reviewScore <= 5))
            {
                return true;
            }
            return false;
        }

        static string GetRatingMessage(double reviewScore)
        {

            string[] messages = { "poor", "fair", "good", "very good", "excellent" };

            int index = GetRatingIndex(reviewScore);

            if (index != -1)
            {
                return messages[index];
            }

            return "invalid rating";

        }
        static int GetRatingIndex(double reviewScore)
        {
            int index = -1;
            switch (reviewScore)
            {
                case double rS when (rS >= 0 && rS <= 1): index = 0; break;
                case double rS when (rS > 1 && rS <= 2): index = 1; break;
                case double rS when (rS > 2 && rS <= 3): index = 2; break;
                case double rS when (rS > 3 && rS <= 4): index = 3; break;
                case double rS when (rS > 4 && rS <= 5): index = 4; break;
                default: index = -1; break;
            }
            return index;
        }

        static void DisplayReport(string[] bookIds, double[] reviewScores, int count) 
        {
            string[] ratingLabel = { "0-1", "1-2", "2-3", "3-4", "4-5" };
            double[] totals = new double[5];
            int[] scoreCounts = new int[5];

            double totalScores = 0;
            double highestScore = reviewScores[0];
            int highestIndex = 0;

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"{bookIds[i]}   {reviewScores[i]}");

                    int index = GetRatingIndex(reviewScores[i]);

                    totals[index] += reviewScores[i];
                    scoreCounts[index]++;

                    totalScores += reviewScores[i];

                    if (reviewScores[i] > highestScore)
                    {
                        highestScore = reviewScores[i];
                        highestIndex = i;
                    }

                }

                for (int i = 0; i < totals.Length; i++)
                {
                    if (scoreCounts[i] != 0)
                    {
                        Console.WriteLine($"{ratingLabel[i]}   {scoreCounts[i]}  {totals[i] / scoreCounts[i]:F2}");
                    }
                    else
                    {
                        Console.WriteLine($"{ratingLabel[i]} had no entries");
                    }
                }

                Console.WriteLine($"Highest rating: {reviewScores[highestIndex]}");
                Console.WriteLine($"Average rating: {totalScores / count:F2}");

            }
            else
            {
                Console.WriteLine(" No reviews posted.");
            }

        }
    }


}


   