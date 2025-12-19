using System;
using System.IO;
using System.Linq;

namespace CPLabProject
{
    internal class Program
    {
        static string[] mcqs;
        static string[] inputs;
        static int NoOfMCQs = 0;
        static int correct;    //Number of correct answers
        static int incorrect;  //Number of incorrect answers

        static void Main(string[] args)
        {
            if (args.Length != 0)//We have command line argument
            {
                LoadMCQsFromFile(args[0]); //User provided file name
            }
            else//We do not have command line argument
            {
                LoadMCQsFromFile("Default.txt");//Default file
            }

            About();
            ConsoleKeyInfo keyInfo;
            while (true)
            {
                keyInfo = DisplayMenuGetKey();
                if (keyInfo.KeyChar == '1')//Start MCQs
                {
                    StartMCQs();
                }
                else if (keyInfo.KeyChar == '2')//Display History
                {
                    DisplayHistory();
                }
                else if (keyInfo.KeyChar == '3')//Delete History
                {
                    DeleteHistory();
                }
                else if (keyInfo.KeyChar == '4')//Delete History
                {
                    About();
                }
                else if (keyInfo.Key == ConsoleKey.Escape)//Exit
                {
                    break;
                }
            }//End of while loop
        }
        static ConsoleKeyInfo DisplayMenuGetKey()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("*** MAIN-MENU ***");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("[1] Start MCQs");
            Console.WriteLine("[2] Display History");
            Console.WriteLine("[3] Delete History");
            Console.WriteLine("[4] About this software");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("[Esc] Exit from Program");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.Write("Enter your choice ...");

            char ch;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(intercept: true);
                ch = char.ToUpper(keyInfo.KeyChar);
            } while (!(ch >= '1' && ch <= '4' || (keyInfo.Key == ConsoleKey.Escape)));

            return keyInfo;
        }
        static void StartMCQs()
        {
            inputs = new string[mcqs.Length];//Inputs will be as many as mcqs array
            string str;
            int[] RandomIndexes = GenerateRandomIndexArray(NoOfMCQs);

            int n = 0;
            int r;
            do
            {
                r = RandomIndexes[n / 6];//Get next random index
                Console.Clear();
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("Question Number {0}/{1}", (n / 6 + 1), NoOfMCQs);
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("{0}", mcqs[r * 6]);//Question
                Console.WriteLine("[A] {0}", mcqs[r * 6 + 1]);//Option-A
                Console.WriteLine("[B] {0}", mcqs[r * 6 + 2]);//Option-B
                Console.WriteLine("[C] {0}", mcqs[r * 6 + 3]);//Option-C
                Console.WriteLine("[D] {0}", mcqs[r * 6 + 4]);//Option-D
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("[Esc] Exit from the MCQs");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("[<-Previous]                          {0}                               [Next->]", inputs[r * 6 + 5]);
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.Write("Enter your choice ...");

                char ch;
                ConsoleKeyInfo keyInfo;
                do
                {
                    keyInfo = Console.ReadKey(intercept: true);
                    ch = char.ToUpper(keyInfo.KeyChar);
                } while (!(ch >= 'A' && ch <= 'D' || (keyInfo.Key == ConsoleKey.Escape) || (keyInfo.Key == ConsoleKey.LeftArrow) || (keyInfo.Key == ConsoleKey.RightArrow)));

                str = char.ToString(ch);
                int max = mcqs.Length / 6;//TotalMCQs
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)//Previous Record
                {
                    if (n != 0)
                    {
                        n -= 6;
                        continue;
                    }

                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)//Next Record
                {
                    if (n != mcqs.Length - 6)
                    {
                        n += 6;
                        continue;
                    }
                }
                else//input is A, B, C or D
                {
                    inputs[r * 6 + 5] = str;//Answer save
                }
            } while (n < mcqs.Length);   //End of for loop
            CalculateResult();
        }
        static int[] GenerateRandomIndexArray(int max)
        {
            Random rnd = new Random();
            int[] arr = new int[max];
            int r;
            int n = 0;
            for (int i = 0; i < max; i++)//Fill -1 to all element
            {                       //Otherwise 0 will be exists
                arr[i] = -1;
            }
            while (n < max)
            {
                r = rnd.Next(max);//0 to max
                if (arr.Contains(r))
                {
                    continue;
                }
                arr[n] = r;
                n++;
            }
            return arr;
        }


        private static void DeleteHistory()
        {
            try
            {
                File.Delete("history.txt");
                Console.WriteLine("History deleted !");
                Console.Write("Press any key to continue ...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Write("Press any key to continue ...");
                Console.ReadKey();
            }
        }
        private static void About()
        {
            Console.Clear();
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("                          COURSE ASSESSMENT TOOL");
            Console.WriteLine("                     Computer Programming Lab - Project");
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("CREDITS:");
            Console.WriteLine("\t02-131252-053  Maryam Altaf");
            Console.WriteLine("\t02-131252-080  M.Khizr Ur Rehman");
            Console.WriteLine("\t02-131252-094  Ubaid Rasool");
            Console.WriteLine("\t02-131252-106  Amna Ramzan");
            Console.WriteLine("COURSE INSTRUCTORs:");
            Console.WriteLine("\tEngr. Muhammad Faisal");
            Console.WriteLine("\tEngr. Hamza");
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("ABOUT");
            Console.WriteLine("The Course Assessment Tool is a console-based software that is developed in C# using structured programming techniques. It automates the process of conducting multiple-choice question (MCQ) assessments for any academic or training course. The system reads questions and answers from a text file, randomizes questions to ensure fairness, and interacts with the user through a simple console interface");
            Console.WriteLine("********************************************************************************");
            Console.Write("Press any key to continue ...");
            Console.ReadKey();
        }

        static void CalculateResult()
        {
            correct = 0;
            incorrect = 0;
            for (int k = 5; k < mcqs.Length; k += 6)
            {
                if (mcqs[k] == inputs[k])
                    correct++;
                else
                    incorrect++;
            }
            DisplaySummary();
        }

        static void DisplaySummary()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("***Performance Summary***");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("No. of questions         = {0}", mcqs.Length / 6);
            Console.WriteLine("No. of correct answers   = {0}", correct);
            Console.WriteLine("No. of incorrect answers = {0}", incorrect);
            Console.WriteLine("Percentage =             = {0}%", (float)correct / (mcqs.Length / 6) * 100);
            Console.WriteLine("--------------------------------------------------------------------------------");

            //Getting current time with specific format
            DateTime now = DateTime.Now;
            string str = String.Format("\n{0}\n{1}\n{2}\n{3}\n{4}", correct, incorrect, correct + incorrect,
                (float)correct / (mcqs.Length / 6) * 100, now.ToString("dd-MMM-yyyy hh:mm:ss tt"));

            Console.WriteLine("Updating history ...");
            AddToHistory(str);
            Console.Write("Press any key to continue ...");
            Console.ReadKey();
        }

        static void LoadMCQsFromFile(string FileName)
        {
            try
            {
                mcqs = File.ReadAllLines(FileName);
                NoOfMCQs = mcqs.Length / 6;//6 lines per MCQs
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Write("Press any key to continue ...");
                Console.ReadKey();
            }
        }
        static void DisplayHistory()
        {
            try
            {
                string[] records = File.ReadAllLines("history.txt");
                Console.Clear();
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("*** History ***");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("{0,7}\t\t{1,9}\t\t{2,5}\t\t{3,4}\t{4}",
                    "CORRECT", "INCORRECT", "TOTAL", "%AGE", "WHEN");
                for (int k = 0; k < records.Length; k += 5)
                {
                    Console.WriteLine("{0,7}\t\t{1,9}\t\t{2,5}\t\t{3,4}%\t{4}",
                        records[k + 0], records[k + 1], records[k + 2],
                        records[k + 3], records[k + 4]);
                }
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.Write("Press any key to continue ...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Write("Press any key to continue ...");
                Console.ReadKey();
            }
        }
        static void AddToHistory(string str)
        {
            try
            {
                File.AppendAllText("history.txt", str);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Write("Press any key to continue ...");
                Console.ReadKey();
            }
        }
    }
}