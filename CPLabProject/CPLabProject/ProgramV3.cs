using System;
using System.IO;

namespace CPLabProject
{
    internal class Program
    {
        static string[] mcqs;
        static string[] inputs;
        static int correct;    //Number of correct answers
        static int incorrect;  //Number of incorrect answers

        static void Main(string[] args)
        {
            About();
            while (true)
            {
                char ch = DisplayMenuGetKey();
                if (ch == '1')//Start MCQs
                {
                    StartMCQs();
                }
                else if (ch == '2')//Display History
                {
                    DisplayHistory();
                }
                else if (ch == '3')//Delete History
                {
                    DeleteHistory();
                }
                else if (ch == '4')//Delete History
                {
                    About();
                }
                else if (ch == '0')//Exit
                {
                    break;
                }
            }//End of while loop
        }

        static void StartMCQs()
        {
            LoadMCQsFromFile();
            inputs = new string[mcqs.Length / 6];//Inputs will be as many as questions
            string str;
            char ch;
            ConsoleKeyInfo keyInfo;
            ConsoleKey key;
            int n = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("Question Number {0}/{1}", (n / 6 + 1), mcqs.Length / 6);
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("{0}", mcqs[n]);
                Console.WriteLine("[A] {0}", mcqs[n + 1]);
                Console.WriteLine("[B] {0}", mcqs[n + 2]);
                Console.WriteLine("[C] {0}", mcqs[n + 3]);
                Console.WriteLine("[D] {0}", mcqs[n + 4]);
                Console.WriteLine("[0] Exit from the MCQs");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("[<-Previous]                          {0}                               [Next->]", inputs[n / 6]);
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.Write("Enter your choice ...");
                do
                {
                    keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;
                    ch = char.ToUpper(keyInfo.KeyChar);
                } while (!(ch >= 'A' && ch <= 'D' || ch == '0' || (key == ConsoleKey.LeftArrow) || (key == ConsoleKey.RightArrow)));

                str = char.ToString(ch);
                int max = mcqs.Length / 6;//TotalMCQs
                if (str == "0")
                {
                    break;
                }
                else if (key == ConsoleKey.LeftArrow)//Previous Record
                {
                    if (n != 0)
                    {
                        n -= 6;
                        continue;
                    }

                }
                else if (key == ConsoleKey.RightArrow)//Next Record
                {
                    if (n != mcqs.Length - 6)
                    {
                        n += 6;
                        continue;
                    }
                }
                else//input is A, B, C or D
                {
                    inputs[n / 6] = str;
                }

            } while (n < mcqs.Length);//End of while loop
            CalculateResult();
        }
        static char DisplayMenuGetKey()
        {
            char ch;//user input
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("*** MAIN-MENU ***");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("[1] Start MCQs");
            Console.WriteLine("[2] Display History");
            Console.WriteLine("[3] Delete History");
            Console.WriteLine("[4] About this software");
            Console.WriteLine("[0] Exit");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.Write("Enter your choice ...");
            do
            {
                ch = Console.ReadKey(intercept: true).KeyChar;

            } while (!(ch >= '0' && ch <= '4'));
            return ch;
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
                Console.Beep();
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
            for (int k = 0; k < mcqs.Length; k += 6)
            {
                if (mcqs[k + 5] == inputs[k / 6])
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

            DateTime now = DateTime.Now;
            string str = String.Format("\n{0}\n{1}\n{2}\n{3}\n{4}", correct, incorrect, correct + incorrect,
                (float)correct / (mcqs.Length / 6) * 100, now.ToString("dd-MMM-yyyy hh:mm:ss tt"));

            Console.WriteLine("Updating history ...");
            AddToHistory(str);
            Console.Write("Press any key to continue ...");
            Console.ReadKey();
        }

        

        static void LoadMCQsFromFile()
        {
            try
            {
                mcqs = File.ReadAllLines("mcqs.txt");
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
