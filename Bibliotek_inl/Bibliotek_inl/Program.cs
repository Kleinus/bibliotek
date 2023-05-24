namespace Bibliotek_inl
{
    internal class Program
    {
        public static void Main(string[] args)
        {   
            Console.WriteLine("Hej, du kommer nu att få 2 alternativ välj en av dom"); //detta kommer upp när man startar 
            Console.WriteLine("Om du skulle vilja skapa ett konto, tryck 1");
            Console.WriteLine("Vill du logga in, tryck 2\n");
            Console.WriteLine("Välj en av dom: ");
            string number = Console.ReadLine();


            if (number == "1")
            {
                // om man väljer 1 så tar den dig till registreringen
                Console.WriteLine("");
                RegisterUser();
            }
           
            else if (number == "2")
            {
                //om man väljer 2 tar den dig till in loggningen 
                Console.WriteLine("");
                LogInPage();
            }

        }


        
        static void RegisterUser()
        {
            //här är registreringen
            Console.WriteLine("Ge ditt för och efternamn sen personnummer och lösenord för att registrera dig");

            Console.Write("Ditt Förnamn: ");
            string firstName = Console.ReadLine();

            Console.Write("Ditt Efternamn: ");
            string lastName = Console.ReadLine();


            Console.Write("Ditt personnummer:");
            string numbers = Console.ReadLine();

            Console.Write("Lösenord:");
            string password = Console.ReadLine();
            // all infnormation skickas vidare till databasen
            if (!IsUserInfoIncomplete(firstName, lastName, numbers, password))
            {
                if (!IsUserRegistered(firstName, lastName, numbers))
                {
                    
                    string line = firstName + " " + lastName + " " + numbers + " " + password;
                    string dataForPn = numbers;
                    string dataForPassword = password;
                    File.AppendAllText(@"C:\Users\alexander.klein\Desktop\Bibliotek_inl\Bibliotek_inl\users.txt", line + Environment.NewLine);
                    File.AppendAllText(@"C:\Users\alexander.klein\Desktop\Bibliotek_inl\Bibliotek_inl\numbers.txt", dataForPn + Environment.NewLine);
                    File.AppendAllText(@"C:\Users\alexander.klein\Desktop\Bibliotek_inl\Bibliotek_inl\password.txt", dataForPassword + Environment.NewLine);



                    Console.WriteLine("grattis! du är nu registrerad");
                }
                else
                {
                    Console.WriteLine("du har redan ett konto. fel personnummer.");
                }
            }
            else
            {
                Console.WriteLine("Fel, ange de rätta uppgifterna.");
            }

            Console.WriteLine("påväg till innlogningsidan");

            Thread.Sleep(1000);

            LogInPage();  
        }

           
        static bool IsUserRegistered(string firstName, string lastName, string numbers)
        {
            string[] users = File.ReadAllLines(@"C:\Users\alexander.klein\Desktop\Bibliotek_inl\Bibliotek_inl\users.txt");
            foreach (string user in users)
            {
                string[] parts = user.Split(' ');
                if (parts[0] == firstName && parts[1] == lastName && parts[2] == numbers)
                {
                    return true;
                }
            }
            return false;
        }


        static bool IsUserInfoIncomplete(string firstName, string lastName, string numbers, string password)
        {
            return string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                   string.IsNullOrWhiteSpace(numbers) || string.IsNullOrWhiteSpace(password);
        }

        static bool Authenticate(string numbers, string password)
        {
            string[] numbersFromDb = System.IO.File.ReadAllLines(@"C:\Users\alexander.klein\Desktop\Bibliotek_inl\Bibliotek_inl\numbers.txt");
            string[] passwordsFromDb = System.IO.File.ReadAllLines(@"C:\Users\alexander.klein\Desktop\Bibliotek_inl\Bibliotek_inl\password.txt");

            if (numbersFromDb.Length != passwordsFromDb.Length)
            {
                return false;
            }

            for (int i = 0; i < numbersFromDb.Length; i++)
            {
                string numberFromDb = numbersFromDb[i];
                string passFromDb = passwordsFromDb[i];

                if (numbers == numberFromDb && password == passFromDb)
                {
                    return true;
                }
            }

            return false;
        }

        static void LogInPage()  
        {

            bool wrongpassword = false;

            string numbers = "";

            string password = "";

            while (!Authenticate(numbers, password))
            {
                Console.Clear();

                if (wrongpassword)
                {
                    Console.WriteLine("lösenordet är fel");
                }
                else
                {
                    Console.WriteLine("hej");
                }

                Console.WriteLine("För att logga in, ange personnummer och lösenord.");
                Console.WriteLine("");

                Console.Write("Personnummer: ");
                numbers = Console.ReadLine();

                Console.Write("Lösenord: ");
                password = Console.ReadLine();

                Console.WriteLine("");

                wrongpassword = true;
            }

            ProfilSida(numbers);
        }
        
        static void ProfilSida(string numbers)
        {
            Console.Clear();
            Console.WriteLine("ProfilSida\n");
            Console.WriteLine("Nu har du loggat inloggad");

            Console.WriteLine("Byta lösenord  1");

            Console.WriteLine("Söka böcker  2");

            Console.WriteLine("Logga ut  3");

            Console.Write("Välj ett alternativ: ");

            int option = int.Parse(Console.ReadLine());

            if (option == 1)
            {
                ChangePassword(numbers);
            }
            if (option == 2)
            { // här tar den dig till böckerna
                BookSearchProgram();
               
            }
            else if (option == 3)
            {
                Console.WriteLine("Nu är du utloggad.");
                Console.ReadKey();
            }
        }
        // Här ändrar man lösenordet
        static void ChangePassword(string numbers) 
        {
            Console.Clear();
            Console.WriteLine("Ändra lösenord");

            string newPassword;

            do
            {
                Console.Write("Nytt lösenord (minst 4 tecken): ");
                newPassword = Console.ReadLine();

                if (newPassword.Length < 4)
                {
                    Console.WriteLine("Lösenordet måste vara minst 4 tecken långt.");
                }
            } while (newPassword.Length < 4);

            string[] numbersFromDb = File.ReadAllLines(@"C:\Users\alexander.klein\desktop\Bibliotek_inl\Bibliotek_inl\numbers.txt");
            string[] passwordsFromDb = File.ReadAllLines(@"C:\Users\alexander.klein\desktop\Bibliotek_inl\Bibliotek_inl\password.txt");
            string[] usersFromDb = File.ReadAllLines(@"C:\Users\alexander.klein\desktop\Bibliotek_inl\Bibliotek_inl\users.txt");

            for (int i = 0; i < numbersFromDb.Length; i++)
            {
                if (numbersFromDb[i] == numbers)
                {
                    passwordsFromDb[i] = newPassword;
                    usersFromDb[i] = numbers + "," + newPassword;
                    File.WriteAllLines(@"C:\Users\alexander.klein\desktop\Bibliotek_inl\Bibliotek_inl\password.txt", passwordsFromDb);
                    File.WriteAllLines(@"C:\Users\alexander.klein\desktop\Bibliotek_inl\Bibliotek_inl\users.txt", usersFromDb);
                    Console.WriteLine("Ditt lösenord har nu byts.");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine("Fel");
            Console.ReadKey();
        }

        static void BookSearchProgram()
        {
            // böckerna som man kan söka på 
            List<Book> books = new List<Book>();
            books.Add(new Book("Lockdown: Star Wars Legends", "Joe Schreiber", "Del Rey", 2016));

            books.Add(new Book("Boken om Pippi Långstrump", "Astrid Lindgren", "Adlibris", 2015));

            books.Add(new Book("Kampen om järntronen", "Georg R.R Martin","Bokus" ,2011));

            books.Add(new Book("Breaking bad: The official book", "David Thomson", "Amazon", 2007));
    
            //det är namnen på böckerna som kommer upp när man trycker på 2
            Console.WriteLine("sök på antingen en författare eller en bok");
            Console.WriteLine("Namn på böcker är");
            Console.WriteLine("Lockdown: Star Wars Legends");
            Console.WriteLine("Boken om Pippi Långstrump");
            Console.WriteLine("Kampen om järntronen");
            Console.WriteLine("Breaking bad: The official book");
            string searchTerm = Console.ReadLine();

            List<Book> results = new List<Book>();
            foreach (Book book in books)
            {
                if (book.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    book.Author.ToLower().Contains(searchTerm.ToLower()) ||
                    book.Publisher.ToLower().Contains(searchTerm.ToLower()) ||
                    book.Year.ToString().Contains(searchTerm))
                {
                    results.Add(book);
                }
            }

            Console.WriteLine($"Hitta {results.Count} matchning: '{searchTerm}':");
            foreach (Book result in results)
            {
                Console.WriteLine($"Titel: {result.Title}, Författare: {result.Author}, Publiceraren: {result.Publisher}, Publicerad: {result.Year}");
            }

            Console.ReadLine();
              }
         class Book
        {
            public string Title { get; set; } 
            public string Author { get; set; }
            public string Publisher { get; set; }
            public int Year { get; set; }

            public Book(string title, string author, string publisher, int year) 
            {
                Title = title;
                Author = author;
                Publisher = publisher;
                Year = year;
            }
        } 
    }
} 