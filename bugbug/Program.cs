using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bugbug
{
    class Program
    {
        static List<Data> BugCond = new List<Data>();
        static List<int> Time = new List<int>();

        static void Main(string[] args)
        {
            char[] delimiter = { ' ' };
            int noPatch = 0, noBug = 0;
            bool isNumber = true;
            do
            {
                isNumber = true;
                Console.Write("Input Bug and Patch Number (a b):");
                string input1 = Console.ReadLine();
                string[] splitData1 = input1.Split(delimiter,StringSplitOptions.RemoveEmptyEntries);

                isNumber = int.TryParse(splitData1[0], out noPatch);
                if (!isNumber)
                {
                    break;
                }
                else
                {
                    isNumber = int.TryParse(splitData1[1], out noBug);
                    if (!isNumber)
                    {
                        break;
                    }
                    else
                    {
                        if (noPatch != 0)
                        {
                            Console.WriteLine("Insert Time PatchCondition PatchFix");
                            //read value of patch by no patch and no bug
                            for (int i = 0; i < noPatch; i++)
                            {
                                string input2 = Console.ReadLine();
                                Data cal1 = new Data();
                                string[] splitData2 = input2.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                                cal1.Time = int.Parse(splitData2[0]);
                                cal1.PatchCond = splitData2[1];
                                cal1.Debug = splitData2[2];
                                BugCond.Add(cal1);
                            }

                            if (BugCond.Count > 0)
                            {
                                string Initial = string.Empty;
                                for (int i = 0; i < noBug; i++)
                                {
                                    Initial = Initial + "+";
                                }

                                Time = new List<int>();

                                //call for CalculateData
                                FindPath(0, string.Empty, Initial);

                                if (Time.Count > 0)
                                {
                                    Console.WriteLine("This bug use minimum time is " + Time.Min());
                                }
                                else
                                {
                                    Console.WriteLine("Cannot fix this bug.");
                                }
                            }
                        }
                    }
                }
                if (!isNumber)
                {
                    Console.WriteLine("There is not number.");
                }

            }while((noPatch != 0 && noBug != 0) || !isNumber);

            Console.ReadKey();
        }

        static void FindPath(int sumtime, string PreCondition, string initial)
        {
            for (int i = 0; i < BugCond.Count; i++)
            {
                Data check = BugCond[i];
                if (string.Compare(PreCondition,check.PatchCond) != 0)
                {
                    //checking => precondition is ok or not?
                    if (CheckingPreCondition(initial, check.PatchCond))
                    {
                        //do fix bug
                        string debug = fixBug(initial, check.Debug);

                        //checkig is fix full
                        if (isFixFullPatch(debug))
                        {
                            //add value of time to something
                            Time.Add(check.Time + sumtime);
                        }
                        else
                        {
                            if(sumtime <1000)
                            //call this function again
                            FindPath(check.Time + sumtime, check.PatchCond, debug);
                        }
                    }

                }
            }
        }

        static bool CheckingPreCondition(string initial, string PreCondition)
        {
            int m = 0;
            for (int i = 0; i < initial.Length; i++)
            {
                if (initial[i] == PreCondition[i] || PreCondition[i] == '0')
                {
                    m++; 
                }
            }
            if (m == initial.Length)
                return true;
            else  return false;
        }

        static string fixBug(string initial, string FixPatch)
        {
            string fbug = string.Empty;
            for (int i = 0; i < initial.Length; i++)
            {
                if (FixPatch[i] != '0')
                {
                    fbug = fbug + FixPatch[i];
                }
                else
                {
                    fbug = fbug + initial[i];
                }
            }
            return fbug;
        }

        static bool isFixFullPatch(string input)
        {
            int k = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '-')
                {
                    k++;
                }
            }
            if (k == input.Length)
                return true;
            else
                return false;
        }
    }

    public class Data
    {
        public int Time { get; set; }
        public string PatchCond { get; set; }
        public string Debug { get; set; }
    }

}
