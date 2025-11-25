using System;
// Project Description:
// This is a simple console application to manage human resource data
// for an education center (Education Center System).
// - Store Person objects and derived classes (Teacher, Admin, Student)
// - Allows adding, viewing, filtering by role, editing and deleting records.
// - Focus on simple data entry and console display.
// Note: the main parts of the code remain the same, only comments are added to explain
// the functionality and processing flow without changing the logic.
namespace EducationCentreSystem
{
// 1. BASE CLASS: Person (Generic Person)
// Description: The `Person` class is the base class for all types of people in the system.
// - Contains basic information: name, phone, email, role.
// - Provides methods for inputting, displaying, and editing basic information.
// - Subclasses (Teacher, Admin, Student) inherit and extend this functionality.
    public class Person
    {
// Data field (private) stores the internal state of the object
        private string name;
        private string telephone;
        private string email;
        private string role;

// --- PROPERTIES --- (public properties to access data fields)     
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
// Note: the Name property can be read/written directly using getter/setter
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }
// Note: Telephone does not have direct validation in the setter; checks
// `Role` is set in the constructor or by a subclass; the setter is protected      // are performed in InputInfo/EditInfo (e.g., must be 10 digits)

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
// Email is also evaluated at the input level but the format is not validated here
        public string Role
        {
            
            get { return role; }
            protected set { role = value; } 
        }

// Constructor takes a `role` parameter. Set default values
// for name, phone and email if user does not enter any.
        public Person(string role)
        {
            this.Role = role;
            name = "Unknown";
            telephone = "0000000000";
            email = "N/A";
        }
// --- DISPLAY INFORMATION ---
        public virtual void DisplayInfo()
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Role:  " + Role);
            Console.WriteLine("Name:  " + Name);
            Console.WriteLine("Phone: " + Telephone);
            Console.WriteLine("Email: " + Email);
        }
// --- ENTER NEW INFORMATION (Validate required fields) ---
        public virtual void InputInfo()
        {
// 1. Check name (required): loop makes sure user enters value
            while (true)
            {
                Console.Write("Enter Name (Required): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    Name = input;
                    break;
                }
                Console.WriteLine(">>> Error: Name cannot be empty.");
            }

// 2. Check telephone (required): must be exactly 10 digits and all numbers
            while (true)
            {
                Console.Write("Enter Telephone (10 digits, Required): ");
                string input = Console.ReadLine();
// Check logic: Not empty + Length 10 + All digits
                bool isNumeric = !string.IsNullOrEmpty(input) && input.All(char.IsDigit);

                if (input.Length == 10 && isNumeric)
                {
                    Telephone = input;
                    break; 
                }
// Display error if conditions are not met
                Console.WriteLine(">>> Error: Phone must be exactly 10 digits.");
            }

// 3. Check Email (required): simply not empty
            while (true)
            {
                Console.Write("Enter Email (Required): ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    Email = input;
                    break;
                }
                Console.WriteLine(">>> Error: Email cannot be empty.");
            }
        }

// --- EDIT INFORMATION (Smart Edit)
// Suggestion: user can press Enter to keep current value
        public virtual void EditInfo()
        {
            Console.WriteLine("--- Editing Common Info (Press Enter to keep current) ---");
            
// Edit name: if input is empty -> keep current
            Console.Write("Name (" + Name + "): ");
            string inputName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(inputName)) Name = inputName;

// Edit telephone: loop ensures input is exactly 10 digits or Enter to keep current
            while (true)
            {
                Console.Write("Telephone (" + Telephone + "): ");
                string inputPhone = Console.ReadLine();
                
                if (string.IsNullOrEmpty(inputPhone)) break; // Enter -> keep current value

                if (inputPhone.Length == 10 && inputPhone.All(char.IsDigit))
                {
                    Telephone = inputPhone;
                    break;
                }
                Console.WriteLine(">>> Error: Must be 10 digits.");
            }

// Edit email: Enter to keep current, otherwise update
            Console.Write("Email (" + Email + "): ");
            string inputEmail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(inputEmail)) Email = inputEmail;
        }
    }

// Note: The Teacher class supports simple input/output and editing operations.
// Data validation (e.g. salary >= 0, telephone 10 digits) is handled
// locally in the respective methods to keep the object state valid.

// ==============================================================================================
// 2. INHERITANCE CLASS: Teacher
// Description: The `Teacher` class extends `Person` with:
// - a `Salary` property and a `Subjects` list
// - implementations of display, input and editing specifically for teachers.   
    public class Teacher : Person
    {
// Salary of the teacher
        private decimal salary;
        public decimal Salary 
        { 
            get { return salary; }
            set { if (value >= 0) salary = value; }
        }

// List of subjects (up to 2 in the current UI) that the teacher teaches
        public List<string> Subjects { get; set; }

// Constructor: sets Role = "Teacher" and initializes the subjects list
        public Teacher() : base("Teacher")
        {
            Subjects = new List<string>();
        }

// Display full information (salary and 2 subjects) for Teacher
        public override void DisplayInfo()
        {
            base.DisplayInfo();
// Display "N/A" if subject is empty (no subject)
            string sub1 = (Subjects.Count > 0 && !string.IsNullOrEmpty(Subjects[0])) ? Subjects[0] : "N/A";
            string sub2 = (Subjects.Count > 1 && !string.IsNullOrEmpty(Subjects[1])) ? Subjects[1] : "N/A";

            Console.WriteLine("Salary: " + Salary.ToString("C")); 
            Console.WriteLine("Subject 1: " + sub1);
            Console.WriteLine("Subject 2: " + sub2);
            Console.WriteLine("--------------------------------------------------");
        }

// Input additional information specific to Teacher:
// - Salary is optional (empty input -> 0)
// - Subjects: up to 2; empty input -> store empty string
        public override void InputInfo()
        {
            base.InputInfo(); // Call mandatory checks for Name/Phone/Email

            // OPTIONAL: Salary (If user presses Enter, default = 0)
            Console.Write("Enter Salary (Optional, Enter to skip): ");
            string inputSal = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(inputSal))
            {
                Salary = 0; // Default
            }
            else
            {
                if (decimal.TryParse(inputSal, out decimal val) && val >= 0) Salary = val;
                else 
                {
                    Console.WriteLine(">>> Invalid input. Set to 0.");
                    Salary = 0;
                }
            }

// OPTIONAL: Subjects: keep up to 2 subjects; an empty string indicates no subject
            Subjects.Clear();
            for (int i = 1; i <= 2; i++)
            {
                Console.Write("Enter Subject " + i + " (Optional): ");
                string sub = Console.ReadLine();
                Subjects.Add(string.IsNullOrWhiteSpace(sub) ? "" : sub);
            }
        }

// Edit information for Teacher (inherits general edit method)
// Adds ability to edit salary and subjects.
        public override void EditInfo()
        {
            base.EditInfo(); 

// Edit salary: if empty -> keep current; update only if valid
            Console.Write("Salary (" + Salary + "): ");
            string inputSal = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(inputSal))
            {
                if (decimal.TryParse(inputSal, out decimal val) && val >= 0) Salary = val;
            }

// Edit subjects according to the current Subjects list
            Console.WriteLine("--- Edit Subjects (Press Enter to keep) ---");
            for (int i = 0; i < Subjects.Count; i++)
            {
                string current = string.IsNullOrEmpty(Subjects[i]) ? "N/A" : Subjects[i];
                Console.Write("Subject " + (i + 1) + " (" + current + "): ");
                string inputSub = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(inputSub))
                {
                    Subjects[i] = inputSub;
                }
            }
        }
    }

// ===================================================================================
// 3. INHERITANCE CLASS: Admin
// Description: Class `Admin` extends `Person` to store information related to employees
// administrative: salary, employee type (full-time or part-time) and working hours.
// Provides separate input and editing methods as required.
    public class Admin : Person
    {
        private decimal salary;
        public decimal Salary
        {
            get { return salary; }
            set { if (value >= 0) salary = value; }
        }

        public bool IsFullTime { get; set; }
        public int WorkingHours { get; set; }
// Constructor for Admin: assign Role = "Admin" and inherit Person state
        public Admin() : base("Admin") { }

// Display Admin information: extends Person's DisplayInfo
// and adds salary, employment type, and working hours
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("Salary: " + Salary.ToString("C"));
            Console.WriteLine("Type: " + (IsFullTime ? "Full-time" : "Part-time"));
            Console.WriteLine("Working Hours: " + WorkingHours);
            Console.WriteLine("--------------------------------------------------");
        }

// Input specific information for Admin: salary (optional), full-time flag,
// and working hours (optional). Non-mandatory fields have default values.
        public override void InputInfo()
        {
            base.InputInfo(); // Call mandatory checks

// Salary: if user does not enter, default = 0
            Console.Write("Enter Salary (Optional): ");
            string inputSal = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(inputSal)) Salary = 0;
            else
            {
                if (decimal.TryParse(inputSal, out decimal val) && val >= 0) Salary = val;
                else { Console.WriteLine(">>> Invalid. Set to 0."); Salary = 0; }
            }

// Check if full-time employee (yes/no) - default is No
            Console.Write("Is Full-time? (yes/no, Optional): ");
            string ft = Console.ReadLine();
            IsFullTime = (!string.IsNullOrWhiteSpace(ft) && (ft.ToLower() == "yes" || ft.ToLower() == "y"));

// WorkingHours: enter working hours; empty -> 0
            Console.Write("Enter Working Hours (Optional): ");
            string inputWh = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(inputWh)) WorkingHours = 0;
            else
            {
                if (int.TryParse(inputWh, out int val)) WorkingHours = val;
                else { Console.WriteLine(">>> Invalid. Set to 0."); WorkingHours = 0; }
            }
        }

// Edit Admin information: allow editing salary and working hours
// (press Enter to keep current values)
        public override void EditInfo()
        {
            base.EditInfo();

// Edit Salary: keep current if input is empty
            Console.Write("Salary (" + Salary + "): ");
            string inputSal = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(inputSal) && decimal.TryParse(inputSal, out decimal s) && s >= 0) Salary = s;

            // Edit WorkingHours: enter valid integer
            Console.Write("Working Hours (" + WorkingHours + "): ");
            string inputWh = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(inputWh) && int.TryParse(inputWh, out int w)) WorkingHours = w;
        }
    }

// ===================================================================================
// 4. SUCCESSOR CLASS: Student
// Description: The `Student` class inherits from `Person` and adds a list of subjects (max 3)
// - Methods: DisplayInfo, InputInfo, EditInfo; similar to Teacher but for students.
    public class Student : Person
    {
        public List<string> Subjects { get; set; }

// Constructor: set Role = "Student" and initialize empty subject list
        public Student() : base("Student")
        {
            Subjects = new List<string>();
        }

// Display Student information, loop through Subjects and print each (N/A if empty)
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            for (int i = 0; i < Subjects.Count; i++)
            {
                string subName = string.IsNullOrEmpty(Subjects[i]) ? "N/A" : Subjects[i];
                Console.WriteLine("Subject " + (i + 1) + ": " + subName);
            }
            Console.WriteLine("--------------------------------------------------");
        }
// Input Student information: inherit mandatory checks then input up to 3 subjects
        public override void InputInfo()
        {
            base.InputInfo();
            Subjects.Clear();
            for (int i = 1; i <= 3; i++)
            {
                Console.Write("Enter Subject " + i + " (Optional): ");
                string sub = Console.ReadLine();
                Subjects.Add(string.IsNullOrWhiteSpace(sub) ? "" : sub);
            }
        }

// Edit Student's subject list; keep current if Enter
        public override void EditInfo()
        {
            base.EditInfo();
            Console.WriteLine("--- Edit Subjects (Press Enter to keep) ---");
            for (int i = 0; i < Subjects.Count; i++)
            {
                string current = string.IsNullOrEmpty(Subjects[i]) ? "N/A" : Subjects[i];
                Console.Write("Subject " + (i + 1) + " (" + current + "): ");
                string inputSub = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(inputSub))
                {
                    Subjects[i] = inputSub;
                }
            }
        }
    }

// ===================================================================================
// 5. MAIN CLASS: Program
// Description: Contains the main loop of the application (console menu) and CRUD functions
// for the `peopleList` list. Main operations:
// - AddNewData: add new Teacher/Admin/Student
// - ViewAllData: print the entire list
// - ViewByGroup: filter by role
// - EditData: find and edit records
// - DeleteData: find and delete records
// Some notes: search by full name (case-insensitive)
    class Program
    {
// Global list storing Person objects (and derived classes);
// Using List<Person> for easy add/remove/iterate.
        static List<Person> peopleList = new List<Person>();

// Main menu loop: print menu and wait for user choice
        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("===========================================");
                Console.WriteLine("        DESKTOP INFORMATION SYSTEM         ");
                Console.WriteLine("===========================================");
                Console.WriteLine("1. Add New Data");
                Console.WriteLine("2. View All Existing Data");
                Console.WriteLine("3. View Data by User Group");
                Console.WriteLine("4. Edit Existing Data");
                Console.WriteLine("5. Delete Existing Data");
                Console.WriteLine("6. Exit");
                Console.WriteLine("===========================================");
                Console.Write("Please select an option (1-6): ");

                // Read user selection from the console (press Enter after typing)
                string choice = Console.ReadLine();

                // Handle the selection from the main menu using a switch-case; each case calls the corresponding function
                switch (choice)
                {
                    case "1": AddNewData(); break;
                    case "2": ViewAllData(); break;
                    case "3": ViewByGroup(); break;
                    case "4": EditData(); break;
                    case "5": DeleteData(); break;
                    case "6": running = false; break;
                    default:
                        Console.WriteLine("Invalid option. Press Enter to try again."); 
                        Console.ReadLine(); 
                        break;
                }
            }
        }

// AddNewData: display submenu to select role then create corresponding object, 
// call InputInfo() for user to enter required/optional information.
        static void AddNewData()
        {
            Console.WriteLine("\n--- Add New Record ---");
            Console.WriteLine("Select Role: 1. Teacher | 2. Admin | 3. Student");
            Console.Write("Choice: ");
// Store the selected user type (1,2,3). If invalid -> display message and return.
            string typeChoice = Console.ReadLine();

            // Initialize the newPerson variable according to the selection; the object
            // will be created and then InputInfo() will be called for detailed input.
            Person newPerson = null;

            switch (typeChoice)
            {
                case "1": newPerson = new Teacher(); break;
                case "2": newPerson = new Admin(); break;
                case "3": newPerson = new Student(); break;
                default: 
                    Console.WriteLine("Invalid role. Press Enter."); 
                    Console.ReadLine(); 
                    return;
            }

            // If the user selected a valid option and newPerson was instantiated, call InputInfo()
            // and add the newPerson to the list.
            if (newPerson != null)
            {
                newPerson.InputInfo(); 
                peopleList.Add(newPerson);
                Console.WriteLine("\nRecord added successfully! Press Enter to return...");
                Console.ReadLine();
            }
        }

        // ViewAllData: list all existing records; if none, display a message
        static void ViewAllData()
        {
            Console.WriteLine("\n--- All Records ---");
            if (peopleList.Count == 0) Console.WriteLine("No records found.");
            else
            {
                foreach (var person in peopleList) person.DisplayInfo();
            }
            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
        }

        // ViewByGroup: ask the user for a role (Teacher/Admin/Student)
        // and print all records that match the role (case-insensitive comparison)
        static void ViewByGroup()
        {
            Console.WriteLine("\n--- View by Role ---");
            Console.Write("Enter role (Teacher / Admin / Student): ");
            string roleFilter = Console.ReadLine();
            
            // `found` is used to check whether any matching records were found
            bool found = false;
            foreach (var person in peopleList)
            {
                if (person.Role != null && roleFilter != null && 
                    person.Role.ToLower() == roleFilter.ToLower())
                {
                    person.DisplayInfo();
                    found = true;
                }
            }
            if (!found) Console.WriteLine("No records found for this role.");
            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
        }

        // EditData: find a record by full name using FindPerson(), display current details
        // and call EditInfo() for updates. Use Enter to keep existing values.
        static void EditData()
        {
            Console.WriteLine("\n--- Edit Record ---");
            Person p = FindPerson();

            if (p != null)
            {
                Console.WriteLine("\n[ Current Details ]");
                // Display current details before editing
                p.DisplayInfo();
                
                Console.WriteLine("\n>>> UPDATING MODE (Press Enter to keep existing values) <<<");
                p.EditInfo();
                
                // After editing is complete, display a success message
                Console.WriteLine("\nRecord updated successfully!");
            }
            Console.WriteLine("Press Enter to return...");
            Console.ReadLine();
        }

        // DeleteData: find a record by name and ask for confirmation before removing
        // DeleteData: remove the record only after confirmation
        static void DeleteData()
        {
            Console.WriteLine("\n--- Delete Record ---");
            Person p = FindPerson();
            if (p != null)
            {
                // Ask the user to confirm the deletion to avoid accidental removal
                Console.Write("Are you sure you want to delete " + p.Name + "? (y/n): ");
                string confirm = Console.ReadLine();
                if (confirm.ToLower() == "y")
                {
                    peopleList.Remove(p);
                    Console.WriteLine("Record deleted.");
                }
                else Console.WriteLine("Cancelled.");
            }
            Console.WriteLine("Press Enter to return...");
            Console.ReadLine();
        }

        // FindPerson: helper to find a Person by full name (case-insensitive).
        // Returns `null` if not found.
        // Note: if the search name is empty or whitespace -> returns null.
        static Person FindPerson()
        {
            Console.Write("Enter the full name of the person: ");
            string searchName = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(searchName)) return null;

            // Iterate through the list and compare names (convert to lowercase) to find a match
            foreach (var person in peopleList)
            {
                if (person.Name.ToLower() == searchName.ToLower())
                {
                    return person;
                }
            }
            
            Console.WriteLine("Person not found.");
            return null;
        }
    }
}