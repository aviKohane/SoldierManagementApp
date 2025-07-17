using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace MyCompiler
{
    abstract class Soldier
    {

        public int ID { get; set; }
        public int NumOperations { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Soldier() { }

        public abstract bool Medal();
        public virtual void Print()
        {
            Console.WriteLine($"ID : {ID}");
            Console.WriteLine($"NumOperations : {NumOperations}");
            Console.WriteLine($"FirstName : {FirstName}");
            Console.WriteLine($"LastName : {LastName}");

        }

        public abstract string SoldierType();

    }
    class PrivateSoldier : Soldier
    {
        public int[] Grades { get; set; }
        public override bool Medal()
        {
            int gradesSum = 0;
            for (int i = 0; i < Grades.Length; i++)
            {
                gradesSum += Grades[i];
            }

            int avgGrades = gradesSum / Grades.Length;
            if (NumOperations >= 10 && avgGrades >= 95)
            {
                return true;
            }
            else return false;
        }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Grades :");
            for (int i = 0; i < Grades.Length; i++)
                Console.WriteLine(Grades[i] + " ");
        }
        public override string SoldierType()
        {
            return "private";

        }
    }
    class Commander : PrivateSoldier
    {
        public bool isCombat { get; set; }

        public override bool Medal()
        {
            int gradesSum = 0;
            for (int i = 0; i < Grades.Length; i++)
            {
                gradesSum += Grades[i];
            }

            int avgGrades = gradesSum / Grades.Length;
            if (NumOperations >= 7 && avgGrades > 90 && isCombat)
            {
                return true;
            }
            else return false;
        }
        public override void Print()
        {
            base.Print(); // Affiche les infos de base du soldat
            string combat = isCombat ? "yes" : "no";
            Console.WriteLine($"Combat : {combat}");
        }
        public override string SoldierType()
        {
            return "commander";

        }
    }

    class Officer : Soldier
    {
        public int SociometricScore { get; set; }
        public override bool Medal()
        {


            if (NumOperations > 2 && SociometricScore >= 92)
            {
                return true;
            }
            else return false;
        }
        public override void Print()
        {
            base.Print();
            Console.WriteLine($"sociometric score :{SociometricScore}");
        }
        public override string SoldierType()
        {
            return "officer";

        }
    }
    class Program
    {
        static List<Soldier> soldiers = new List<Soldier>();

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("please choose an option :");
                Console.WriteLine("0 : Exit");
                Console.WriteLine("1 : Add new soldier(Private, Commander, or Officer)");
                Console.WriteLine("2 : Print soldiers eligible for medal");
                Console.WriteLine("3 : Print Officer with highest sociometric score");
                Console.WriteLine("4 : Count PrivateSoldiers who received medals");
                Console.WriteLine("5  Print names of Commanders who are not combat:");
                Console.WriteLine("6 : Print if any soldier has more than 15 operations");
                Console.WriteLine("7 : Remove soldiers and officers with 0 operations");



                int option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 0:
                        Console.WriteLine("Exit");
                        return;

                    case 1:
                        Console.WriteLine("choose a soldier type : ");
                        Console.WriteLine("enter 1 to add a private");
                        Console.WriteLine("enter 2 to add a commander");
                        Console.WriteLine("enter 3 to add a officer");

                        int type = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("enter id : ");
                        int id = Convert.ToInt32(Console.ReadLine());

                        Console.WriteLine("enter first name : ");
                        string firstName = Console.ReadLine();

                        Console.WriteLine("enter last name : ");
                        string lastName = Console.ReadLine();

                        Console.WriteLine("enter number of operations : ");
                        int numOperations = Convert.ToInt32(Console.ReadLine());



                        switch (type)
                        {
                            case 1:
                                Console.WriteLine("enter number of grades");
                                int gradesNum = Convert.ToInt32(Console.ReadLine());
                                int i = 0;
                                int[] soldierGrades = new int[gradesNum];
                                while (i < gradesNum)
                                {
                                    int grade;
                                    do
                                    {
                                        Console.WriteLine("Enter a grade (0-100): ");
                                        grade = Convert.ToInt32(Console.ReadLine());
                                    } while (grade < 0 || grade > 100); soldierGrades[i] = Convert.ToInt32(Console.ReadLine());
                                    i++;
                                }

                                soldiers.Add(new PrivateSoldier()
                                {
                                    ID = id,
                                    NumOperations = numOperations,
                                    FirstName = firstName,
                                    LastName = lastName,
                                    Grades = soldierGrades.ToArray() //deep copy


                                }); break;

                            case 2:
                                Console.WriteLine("enter number of grades");
                                gradesNum = Convert.ToInt32(Console.ReadLine());
                                i = 0;
                                string combat;
                                soldierGrades = new int[gradesNum];
                                while (i < gradesNum)
                                {
                                    Console.WriteLine("enter a grade : ");
                                    soldierGrades[i] = Convert.ToInt32(Console.ReadLine());
                                    i++;
                                }

                                do
                                {
                                    Console.WriteLine("enter 'yes' or 'no' if the soldier is combat ? ");
                                    combat = Console.ReadLine().ToLower();
                                } while (!(combat == "yes" || combat == "no"));

                                soldiers.Add(new Commander()
                                {
                                    ID = id,
                                    NumOperations = numOperations,
                                    FirstName = firstName,
                                    LastName = lastName,
                                    Grades = soldierGrades,
                                    isCombat = combat == "yes"

                                }); ;
                                break;
                            case 3:
                                Console.WriteLine("enter the sociometric score : ");
                                int sociometricScore = Convert.ToInt32(Console.ReadLine());
                                soldiers.Add(new Officer()
                                {
                                    ID = id,
                                    NumOperations = numOperations,
                                    FirstName = firstName,
                                    LastName = lastName,
                                    SociometricScore = sociometricScore
                                });
                                break;
                        }
                        break;
                    case 2:
                        //Print soldiers eligible for medal
                        int sumSoldiersWithMedal = 0;
                        Console.WriteLine("Soldiers eligible for medal:");
                        foreach (Soldier soldier in soldiers)
                        {
                            if (soldier.Medal())
                            {

                                sumSoldiersWithMedal++;
                                soldier.Print();
                                Console.WriteLine(); // ↩ saut de ligne après chaque soldat
                            }

                        }
                        if (sumSoldiersWithMedal == 0)
                        {
                            Console.WriteLine("There is no soldier eligible for medal.");
                        }
                        break;
                    case 3:
                        //Print Officer with highest sociometric score
                        List<Officer> officers = new List<Officer>();

                        Console.WriteLine("Officer with highest sociometric score :");
                        foreach (Soldier soldier in soldiers)
                        {
                            if (soldier.SoldierType() == "officer")
                            {
                                officers.Add((Officer)soldier);
                            }
                        }
                        Officer bestOfficer = officers.OrderByDescending(o => o.SociometricScore).FirstOrDefault();
                        if (bestOfficer != null)
                            bestOfficer.Print();
                        else
                            Console.WriteLine("No Officer Found");

                        break;

                    case 4:
                        //Count PrivateSoldiers who received medals
                        int sumPrivateSoldier = 0;
                        Console.WriteLine("Number of private soldiers who received a medal : ");
                        foreach (Soldier soldier in soldiers)
                        {
                            if (soldier.SoldierType() == "private" && soldier.Medal())
                            {
                                sumPrivateSoldier++;
                            }
                        }
                        Console.WriteLine(sumPrivateSoldier);
                        break;
                    case 5:
                        //Print names of Commanders who are not combat
                        List<Commander> commander = new List<Commander>();
                        Console.WriteLine("Commanders who are not combat:");
                        foreach (Soldier soldier in soldiers)
                        {
                            if (soldier.SoldierType() == "commander")
                            {
                                Commander c = (Commander)soldier; // cast
                                if (!c.isCombat)
                                {
                                    Console.WriteLine($"{c.FirstName} {c.LastName}");

                                }
                            }
                        }


                        break;

                    case 6:
                        var soldiersMoreFifteenOper = soldiers.Where(c => c.NumOperations > 15).ToList();
                        if (soldiersMoreFifteenOper.Count == 0)
                        {
                            Console.WriteLine("No soldier has more than 15 operations.");
                        }
                        else
                        {
                            foreach (var s in soldiersMoreFifteenOper)
                            {
                                Console.WriteLine($"{s.FirstName} {s.LastName}");
                            }
                        }
                        break;

                    case 7:
                        var officersZeroOperations = soldiers.Where(s => s.SoldierType() == "officer" && s.NumOperations == 0).ToList();
                        if (officersZeroOperations.Count() == 0)
                        {
                            Console.WriteLine("there is no officers with 0 operations");
                        }
                        else foreach (var c in officersZeroOperations)
                            {
                                soldiers.Remove(c);
                            }

                        break;

                    default:
                        Console.WriteLine("Invalid option. Please enter a number from 0 to 7.");
                        break;
                }
            }

        }


    }
}
