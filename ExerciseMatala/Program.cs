using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCompiler
{
    abstract class Soldier
    {
        public int ID { get; set; }
        public int NumOperations { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public abstract bool Medal();
        public abstract string SoldierType();

        public virtual void Print()
        {
            Console.WriteLine($"ID: {ID}");
            Console.WriteLine($"First Name: {FirstName}");
            Console.WriteLine($"Last Name: {LastName}");
            Console.WriteLine($"Num Operations: {NumOperations}");
        }
    }

    class PrivateSoldier : Soldier
    {
        public int[] Grades { get; set; }

        public override bool Medal()
        {
            return NumOperations >= 10 && Grades.Average() >= 95;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine("Grades: " + string.Join(", ", Grades));
        }

        public override string SoldierType() => "private";
    }

    class Commander : PrivateSoldier
    {
        public bool isCombat { get; set; }

        public override bool Medal()
        {
            return NumOperations >= 7 && Grades.Average() > 90 && isCombat;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Combat: {(isCombat ? "yes" : "no")}");
        }

        public override string SoldierType() => "commander";
    }

    class Officer : Soldier
    {
        public int SociometricScore { get; set; }

        public override bool Medal()
        {
            return NumOperations > 2 && SociometricScore >= 92;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Sociometric Score: {SociometricScore}");
        }

        public override string SoldierType() => "officer";
    }

    class Program
    {
        static List<Soldier> soldiers = new List<Soldier>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("0 - Exit");
                Console.WriteLine("1 - Add new soldier");
                Console.WriteLine("2 - Print soldiers eligible for medal");
                Console.WriteLine("3 - Print officer with highest sociometric score");
                Console.WriteLine("4 - Count private soldiers with medals");
                Console.WriteLine("5 - Print commanders who are not combat");
                Console.WriteLine("6 - Print soldiers with more than 15 operations");
                Console.WriteLine("7 - Remove soldiers with 0 operations");

                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    case 0: return;
                    case 1: AddSoldier(); break;
                    case 2: PrintSoldiersWithMedals(); break;
                    case 3: PrintBestOfficer(); break;
                    case 4: CountPrivateSoldiersWithMedals(); break;
                    case 5: PrintNonCombatCommanders(); break;
                    case 6: PrintSoldiersWithManyOperations(); break;
                    case 7: RemoveInactiveSoldiers(); break;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        static void AddSoldier()
        {
            int type;
            do
            {
                Console.WriteLine("Enter soldier type (1-Private, 2-Commander, 3-Officer):");
                type = Convert.ToInt32(Console.ReadLine());
            } while (type < 1 || type > 3);

            int id;
            do
            {
                Console.Write("ID (positive and 9 digits max): ");
            } while (!int.TryParse(Console.ReadLine(), out id) || id <= 0);

            Console.Write("First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();
            int numOperations;
            do
            {
                Console.Write("Number of operations (>= 0): ");
            } while (!int.TryParse(Console.ReadLine(), out numOperations) || numOperations < 0);

            if (type == 1)
            {
                soldiers.Add(new PrivateSoldier
                {
                    ID = id,
                    FirstName = firstName,
                    LastName = lastName,
                    NumOperations = numOperations,
                    Grades = ReadGrades()
                });
            }
            else if (type == 2)
            {
                string combatInput;
                do
                {
                    Console.Write("Is combat? (yes/no): ");
                    combatInput = Console.ReadLine().Trim().ToLower();
                } while (combatInput != "yes" && combatInput != "no");

                bool isCombat = combatInput == "yes";
                soldiers.Add(new Commander
                {
                    ID = id,
                    FirstName = firstName,
                    LastName = lastName,
                    NumOperations = numOperations,
                    isCombat = isCombat,
                    Grades = ReadGrades()
                });
            }
            else if (type == 3)
            {
                int score;
                do
                {
                    Console.Write("Sociometric Score (0-100): ");
                } while (!int.TryParse(Console.ReadLine(), out score) || score < 0 || score > 100);
                soldiers.Add(new Officer
                {
                    ID = id,
                    FirstName = firstName,
                    LastName = lastName,
                    NumOperations = numOperations,
                    SociometricScore = score
                });
            }
        }

        static int[] ReadGrades()
        {
            int gradesNum;
            string input;

            // Lecture sécurisée du nombre de notes
            do
            {
                Console.Write("Number of grades: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out gradesNum) || gradesNum <= 0);

            int[] grades = new int[gradesNum];

            // Lecture sécurisée de chaque note
            for (int i = 0; i < gradesNum; i++)
            {
                int grade;
                string gradeInput;

                do
                {
                    Console.Write($"Grade #{i + 1} (0-100): ");
                    gradeInput = Console.ReadLine();
                } while (!int.TryParse(gradeInput, out grade) || grade < 0 || grade > 100);

                grades[i] = grade;
            }

            return grades;
        }


        static void PrintSoldiersWithMedals()
        {
            var list = soldiers.Where(s => s.Medal()).ToList();
            if (list.Count == 0)
            {
                Console.WriteLine("No soldiers eligible for medal.");
                return;
            }

            Console.WriteLine("Soldiers eligible for medal:");
            foreach (var s in list)
            {
                s.Print();
                Console.WriteLine();
            }
        }

        static void PrintBestOfficer()
        {
            var best = soldiers.OfType<Officer>().OrderByDescending(o => o.SociometricScore).FirstOrDefault();
            if (best != null)
                best.Print();
            else
                Console.WriteLine("No officer found.");
        }

        static void CountPrivateSoldiersWithMedals()
        {
            int count = soldiers.OfType<PrivateSoldier>()
                .Where(p => p.SoldierType() == "private" && p.Medal())
                .Count();
            Console.WriteLine($"Private soldiers with medals: {count}");
        }

        static void PrintNonCombatCommanders()
        {
            var list = soldiers.OfType<Commander>().Where(c => !c.isCombat).ToList();
            if (list.Count == 0)
            {
                Console.WriteLine("No non-combat commanders.");
                return;
            }

            Console.WriteLine("Non-combat commanders:");
            foreach (var c in list)
                Console.WriteLine($"{c.FirstName} {c.LastName}");
        }

        static void PrintSoldiersWithManyOperations()
        {
            var list = soldiers.Where(s => s.NumOperations > 15).ToList();
            if (list.Count == 0)
            {
                Console.WriteLine("No soldier has more than 15 operations.");
                return;
            }

            Console.WriteLine("Soldiers with more than 15 operations:");
            foreach (var s in list)
                Console.WriteLine($"{s.FirstName} {s.LastName}");
        }

        static void RemoveInactiveSoldiers()
        {
            if (!soldiers.Any())
            {
                Console.WriteLine("There are no soldiers in the system.");
                return;
            }

            int removedCount = soldiers.RemoveAll(s => s.NumOperations == 0);

            if (removedCount == 0)
            {
                Console.WriteLine("There are no inactive soldiers (with 0 operations).");
            }
            else
            {
                Console.WriteLine($"{removedCount} inactive soldier(s) removed.");
            }
        }

    }
}
