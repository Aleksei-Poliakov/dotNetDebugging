using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Demo2_pdb_makes_a_difference
{
    class Demo2_pdb
    {
        static void Main()
        {
            while (true)
            {
                Console.Write("Enter employee name -> ");
                var name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }
                if (name == "exit")
                {
                    return;
                }
                var employee = employees[name];
                Console.WriteLine(cals[employee.WorkType].CalculateIncome(employee.Salary));
                Console.WriteLine("Press enter to continue");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static readonly Dictionary<WorkType, IncomeCalculator> cals = new Dictionary<WorkType, IncomeCalculator>
                                                              {
                                                                  {WorkType.Easy, new IncomeCalculator(15)},
                                                                  {WorkType.Medium, new IncomeCalculator(13)},
                                                                  {WorkType.Hard, new IncomeCalculator(11)}
                                                              };

        private static readonly Dictionary<string, Employee> employees = new Dictionary<string, Employee>
                                           {
                                               { "Steve1",  new Employee("Steve1", 1, 1000)   },
                                               { "Steve2",  new Employee("Steve2", 2, 2000)   },
                                               { "Steve3",  new Employee("Steve3", 3, 3000)   },
                                               { "Steve4",  new Employee("Steve4", 1, 4000)   },
                                               { "Steve5",  new Employee("Steve5", 2, 5000)   },
                                               { "Steve6",  new Employee("Steve6", 3, 6000)   },
                                               { "Steve7",  new Employee("Steve7", 1, 7000)   },
                                               { "Steve8",  new Employee("Steve8", 2, 8000)   },
                                               { "Steve9",  new Employee("Steve9", 3, 9000)   },
                                               { "Steve10", new Employee("Steve10", 2, 10000) },
                                               { "Steve11", new Employee("Steve11", 3, 11000) },
                                               { "Steve12", new Employee("Steve12", 1, 12000) },
                                               { "Steve13", new Employee("Steve13", 2, 13000) },
                                               { "Steve14", new Employee("Steve14", 3, 14000) },
                                               { "Steve15", new Employee("Steve15", 1, 15000) },
                                               { "Steve16", new Employee("Steve16", 2, 16000) },
                                               { "Steve17", new Employee("Steve17", 3, 17000) },
                                               { "Steve18", new Employee("Steve18", 1, 18000) },
                                               { "Steve19", new Employee("Steve19", 2, 19000) },
                                               { "Steve20", new Employee("Steve20", 3, 20000) },
                                           }; 
    }

    public enum WorkType
    {
        Hard,  // 1
        Easy,  // 2
        Medium // 3
    }

    public class Employee
    {
        public string Name { get; private set; }
        public WorkType WorkType { get; private set; }
        public int Salary { get; private set; }

        public Employee(string name, int workType, int salary)
        {
            Name = name;
            WorkType = (WorkType)workType;
            Salary = salary;
        }
    }

    public class IncomeCalculator
    {
        private int Tax { get; set; }

        public IncomeCalculator(int tax)
        {
            Tax = tax;
        }

        public int CalculateIncome(int payroll)
        {
            return payroll*Tax/1000;
        }
    }
}
