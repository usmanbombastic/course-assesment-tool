using System;
using System.Data;
using System.IO;


namespace CPLabProject
{
    internal class Program
    {
        static string[] mcqs;
        static string[] inputs;
        static int correct;    //Number of correct answers
        static int incorrect;  //Number of incorrect answers
        static char DisplayMenuGetKey()
        {
            Console.WriteLine("*** MAIN-MENU ***");
            Console.WriteLine("[1] Start MCQs");
            Console.WriteLine("[2] Display History");
            Console.WriteLine("[3] Delete History");
            Console.WriteLine("[0] Exit");
            Console.Write("Enter your choice ...");
            ConsoleKeyInfo keyInfo = Console.ReadKey();//intercept: true
            return keyInfo.KeyChar;
        }

        static void Main(string[] args)
        {
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
                else if (ch == '0')//Exit
                {
                    break;
                }
            }//End of while loop
        }

        static void StartMCQs()
        {
            LoadMCQsFromFile();
            inputs=new string[mcqs.Length/6];//Inputs will be as many as questions
            string str;
            for(int n=0;n<mcqs.Length;n+=6)
            {
                Console.WriteLine("Question Number {0}/{1}",(n/6+1), mcqs.Length/6);
                Console.WriteLine("{0}", mcqs[n]);
                Console.WriteLine("[A] {0}", mcqs[n+1]);
                Console.WriteLine("[B] {0}", mcqs[n+2]);
                Console.WriteLine("[C] {0}", mcqs[n+3]);
                Console.WriteLine("[D] {0}", mcqs[n+4]);
                Console.WriteLine("[0] Exit from the MCQs");
                Console.Write("Enter your choice ...");
                str = Console.ReadKey(intercept: true).KeyChar.ToString();                
                if(str=="0")
                {
                    break;
                }
                inputs[n / 6] = str;
            }//End of while loop
            CalculateResult();
        }

        static void CalculateResult()
        {
            correct=0;
            incorrect = 0;
            for (int k = 0; k < mcqs.Length; k+=6)
            {
                if (mcqs[k + 5] == inputs[k/6])
                    correct++;
                else
                    incorrect++;
            }
            DisplaySummary();
        }

        static void DisplaySummary()
        {
            Console.WriteLine("No. of questions         = {0}", mcqs.Length / 6);
            Console.WriteLine("No. of correct answers   = {0}", correct);
            Console.WriteLine("No. of incorrect answers = {0}", incorrect);
            Console.WriteLine("Percentage =             = {0,3}%", (float)correct/(mcqs.Length / 6)*100);

        } 

        static void LoadMCQsFromFile()
        {
            try
            {
                mcqs = File.ReadAllLines("mcqs.txt");
                Console.WriteLine("\n{0} MCQs loaded successfully ...", mcqs.Length/6);//divided by 4 check
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void DisplayHistory()
        {
            try
            {
                string[] records=File.ReadAllLines("history.txt");

                Console.WriteLine("\n{0,7}\t\t{1,9}\t\t{3,5}\t\t{4,4}", "CORRECT", "INCORRECT", "TOTAL", "%AGE", "WHEN");
                for(int k = 0; k < records.Length; k+=5)
                {
                    Console.WriteLine("{0,7}\t\t{1,9}\t\t{3,5}%\t\t{4,4}", 
                        records[k+0], records[k + 1], records[k + 2], 
                        records[k + 3], records[k + 4]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            
        }
    }
}
