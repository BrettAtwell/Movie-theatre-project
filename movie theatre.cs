// Group Project - Movie Theater 

// Authors: Brett Atwell, Christopher Marshall, Gitanjali Denton 

// Description: Add modules to modify the movie characteristics and the screen/theater data. Also, adding an option to pre-order food for the movie. 



using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

public enum MPAARating

{

    G, PG, PG13, R, NC17 // These will be the ratings given for each movie. 

}



public class Screen

{

    public int ScreenNumber { get; set; } // Where the screen is relative to the theater. 

    public int SeatingCapacity { get; set; } // How many people the theater can seat. 

    public int CurrentCapacity { get; set; } // How many people are currently in the theater for a movie. 



    public Screen(int screenNumber, int seatingCapacity)

    {

        ScreenNumber = screenNumber;

        SeatingCapacity = seatingCapacity;

        CurrentCapacity = 0; // Starting at zero becuase zero people are at the theater right now. 

    }



    public bool IsAvailable(int ticketCount)

    {

        return CurrentCapacity + ticketCount <= SeatingCapacity; // Zero + the number of tickets purchased which is the seating capacity. 

    }



    public void ReserveSeats(int ticketCount)

    {

        if (IsAvailable(ticketCount)) // Calling the IsAvailable method to check if there are enough seats available. 

        {

            CurrentCapacity += ticketCount;

        }

    }

}



public class Movie

{ // Creating variables... 

    public string Title { get; set; } // The title of the movie. 

    public decimal Price { get; set; } // The price of the movie. 

    public MPAARating Rating { get; set; } // The rating of the movie. 

    public Screen ScreenInfo { get; set; } // About the screen the movie is playing on. 

    public List<TimeSpan> Showtimes { get; set; } // A listing of show times for the movie. 



    public Movie(string title, decimal price, MPAARating rating, Screen screen, List<TimeSpan> showtimes) // public Movie declares a public constructor for the Movie class. 

    {

        Title = title; // Assigning values of the constructor parameters to properties of the Movie class. 

        Price = price;

        Rating = rating;

        ScreenInfo = screen;

        Showtimes = showtimes;

    }



    public bool CanUserAttend(User user)

    {

        return user.Age >= 17 || Rating != MPAARating.R; // This is the case in which someone who is >=17 can watch a R film. 

    }

}



public class User // 'public' indicates that this class can be accessed from other classes. 

{

    public string Name { get; set; } // Both properties are declared and generates a getter and setter. 

    public int Age { get; set; }



    public User(string name, int age)

    {

        Name = name; // This initializes the Name and Age properties. 

        Age = age;

    }

}



public class SnackItem

{

    public string Name { get; set; } // The name, category and price properties are declared and generates a getter and setter. 

    public string Category { get; set; }

    public decimal Price { get; set; }



    public SnackItem(string name, string category, decimal price)

    {

        Name = name; // This initializes the Name, Category and Price properties. 

        Category = category;

        Price = price;

    }

}



public class TicketOrder

{

    public int OrderNumber { get; set; } // Represents the order number. 

    public string MovieTitle { get; set; } // Represents the title of the movie. 

    public int ScreenNumber { get; set; } // Represents the screen number where the movie is shown. 

    public TimeSpan Showtime { get; set; } // Represents the time at which the movie is scheduled. 

    public decimal UnitPrice { get; set; } // Represents the unit price of a movie ticket. 

    public int TicketCount { get; set; } // Represents the number of tickets in the order. 

    public List<SnackItem> Snacks { get; set; } //  Represents a list of snack options. 

    public decimal Subtotal => UnitPrice * TicketCount + Snacks.Sum(snack => snack.Price); // Calculating the subtotal of the order.  



    public TicketOrder(int orderNumber, Movie movie, TimeSpan showtime, int ticketCount, List<SnackItem> snacks)

    {

        OrderNumber = orderNumber; // This initializes all the properties. 

        MovieTitle = movie.Title;

        ScreenNumber = movie.ScreenInfo.ScreenNumber;

        Showtime = showtime;

        UnitPrice = movie.Price;

        TicketCount = ticketCount;

        Snacks = snacks;

    }

}

public class Program

{

    static int nextOrderNumber = 1;

    static List<TicketOrder> ticketOrders = new List<TicketOrder>();

    static List<Movie> movies;

    static List<SnackItem> availableSnacks = new List<SnackItem>

    {

        new SnackItem("Popcorn", "Snack", 4.50m), // Assigning popcorn as a snack. 

        new SnackItem("Soda", "Drink", 3.00m), // Assigning soda as a drink. 

        new SnackItem("Chocolate Bar", "Candy", 2.50m) // Assigning a chocolate bar as candy. 

    };



    static void DisplayMovies()

    {

        Console.WriteLine("Available Movies:"); // This will display to the user the movies available for them to watch. 

        for (int i = 0; i < movies.Count; i++) // Movies count will go up by 1. 

        {

            Console.WriteLine($"{i + 1}. {movies[i].Title}");

        }

    }



    static void DisplayShowtimes(Movie movie)

    {

        Console.WriteLine($"Selected Movie: {movie.Title}"); // The movie the user selected. 

        Console.WriteLine("Available Showtimes:"); // The available showtimes for that movie. 

        foreach (var time in movie.Showtimes)

        {

            Console.WriteLine(time.ToString(@"hh\:mm"));

        }

        Console.WriteLine("Select a showtime by entering the time (hh:mm):"); // A foreach loop for entering the time. 

    }



    static int GetUserInput(string message)

    {

        Console.WriteLine(message); //  Displaying the message to the user. 

        return int.Parse(Console.ReadLine()); // This converts the user input into an integer. 

    }



    static bool GetYesOrNo(string message)

    {

        Console.WriteLine(message); //  Displaying the message to the user. 

        return Console.ReadLine().Trim().ToLower() == "yes"; // Checks if the user input is equal to the string 'yes'. 

    }



    static List<SnackItem> SelectSnacks()

    {

        List<SnackItem> selectedSnacks = new List<SnackItem>();

        if (GetYesOrNo("Would you like to pre-buy snacks? (yes/no)")) // The program will ask this to the user, the user will type yes or no. 

        {

            bool continueSnackSelection = true;

            while (continueSnackSelection) // An if/while loop for available snacks is iterated over. 

            {

                for (int j = 0; j < availableSnacks.Count; j++) // Adding the available snacks count by 1. 

                {

                    Console.WriteLine($"{j + 1}. {availableSnacks[j].Name} - {availableSnacks[j].Price:C}");

                }



                int snackSelection = GetUserInput("Select a snack by number:"); // The output or the user to select a snack based on the number on the output. 

                selectedSnacks.Add(availableSnacks[snackSelection - 1]);

                continueSnackSelection = GetYesOrNo("Do you want to select more snacks? (yes/no)"); // Asking the user if they would like to select more snacks, the user will type yes or no. 

            }

        }

        return selectedSnacks;

    }



    static void ProcessOrder(User user, Movie selectedMovie, TimeSpan selectedTime, int ticketCount, List<SnackItem> selectedSnacks)

    {

        if (selectedMovie.ScreenInfo.IsAvailable(ticketCount) && selectedMovie.CanUserAttend(user))

        {

            selectedMovie.ScreenInfo.ReserveSeats(ticketCount);

            var order = new TicketOrder(nextOrderNumber++, selectedMovie, selectedTime, ticketCount, selectedSnacks);

            ticketOrders.Add(order);

            Console.WriteLine($"Tickets and snacks for '{selectedMovie.Title}' at {selectedTime} added to your order."); // Using an if/else loop because if the snacks are available, this is the message the user gets from the program. 

        }

        else

        {

            Console.WriteLine("Unable to add tickets to your order. Show may be sold out or you may not meet the age requirement."); // Using an if/else loop because 'else' if the snacks are unavailable, this is the message the user will get from the program. 

        }

    }



    static void PrintOrderSummary()

    {

        Console.WriteLine("\nOrder Summary:"); // This will display the order summary to the user. 

        decimal grandTotal = 0; // Converting the granTotal to a decimal. 



        foreach (var order in ticketOrders)

        {

            Console.WriteLine($"Order Number: {order.OrderNumber}, Movie: {order.MovieTitle}, Screen: {order.ScreenNumber}, Time: {order.Showtime}, Price: {order.UnitPrice}, Tickets: {order.TicketCount}, Subtotal: {order.Subtotal:C}"); // What will display to the user. 

            foreach (var snack in order.Snacks)

            {

                Console.WriteLine($"   Snack: {snack.Name}, Price: {snack.Price:C}"); // Foreach loop for snack and price. 

            }

            grandTotal += order.Subtotal; // This is the grand total cost of everything the user has purchased. 

        }



        Console.WriteLine($"Total number of movies selected: {ticketOrders.Count}"); // Display the total number of movies selected. 

        Console.WriteLine($"Grand Total: {grandTotal:C}"); // Displaying the grandTotal to the user. 

    }



    static List<Movie> LoadMoviesFromFile(string filePath)

    {

        var movies = new List<Movie>();

        try // A try/catch of the external file. 

        {

            using (var reader = new StreamReader(@"C:\Users\brett\Downloads\School stuff\MIS 366\MovieTheatre_Group_Project\bin\Debug\movie.txt"))

            {

                string line;

                while ((line = reader.ReadLine()) != null)

                {

                    var fields = line.Split(',');

                    var title = fields[0];

                    var price = decimal.Parse(fields[1]);

                    var rating = (MPAARating)Enum.Parse(typeof(MPAARating), fields[2]);

                    var screenNumber = int.Parse(fields[3]);

                    var seatingCapacity = int.Parse(fields[4]);

                    var screen = new Screen(screenNumber, seatingCapacity);

                    var showtimes = new List<TimeSpan>();

                    foreach (var time in fields[5].Split('|'))

                    {

                        showtimes.Add(TimeSpan.Parse(time));

                    }



                    movies.Add(new Movie(title, price, rating, screen, showtimes));

                }

            }

        }

        catch (Exception ex) // Using exception handling. 

        {

            Console.WriteLine($"Error reading file: {ex.Message}"); // This is what is displayed to the user if there wasn an error reading a file. 

        }

        return movies;

    }



    static void DisplayMainMenu()

    {

        Console.WriteLine("Main Menu:"); // This displays the Main menu options to the user upon logging on. 

        Console.WriteLine("1. Movie Selection"); // This option lets the user select a movie. 

        Console.WriteLine("2. Movie Management"); // This option deals on movie management. 

        Console.WriteLine("3. Exit"); // This option exits the program. 

    }



    static void RunMovieSelectionPath()

    {

        Console.WriteLine("Enter your name:"); // This is what the code will display first to the user. The input will be a string since it is a name. 

        string userName = Console.ReadLine();



        Console.WriteLine("Enter your age:"); // This is the next output to the user that will be an int because it is a number. 

        int userAge;

        while (!int.TryParse(Console.ReadLine(), out userAge))

        {

            Console.WriteLine("Please enter a valid age."); // If the user does not put in a number (int). 

        }



        var user = new User(userName, userAge);

        bool continuePurchasing = true;



        while (continuePurchasing)

        {

            DisplayMovies();

            int movieSelection = GetUserInput("Select a movie by number:"); // Displayed to the user and the intput will be an int. 

            var selectedMovie = movies[movieSelection - 1];



            DisplayShowtimes(selectedMovie);

            TimeSpan selectedTime = TimeSpan.Parse(Console.ReadLine());



            int ticketCount = GetUserInput("Enter number of tickets:"); // The user will enter the number of tickets they wish to purchase. 

            List<SnackItem> selectedSnacks = SelectSnacks();



            ProcessOrder(user, selectedMovie, selectedTime, ticketCount, selectedSnacks); // All the data we have from the user so far. 



            continuePurchasing = GetYesOrNo("Do you want to purchase tickets for another movie? (yes/no)"); // Displayed to the user if they want to watch another movie as well during their trip. 

        }



        PrintOrderSummary();

        string receiptFilePath = ("C:/Users/brett/Downloads/School stuff/MIS 366/MovieTheatre_Group_Project/bin/Debug/receipt.txt"); // Brett's actual file path (external file). 

        CreateReceiptFile(receiptFilePath, ticketOrders);



    }



    static void AddNewMovie() // When adding a new movie to the program. 

    {

        Console.WriteLine("Enter movie title:"); // Enter the title of the movie you are adding. 

        string title = Console.ReadLine();



        Console.WriteLine("Enter ticket price:"); // Enter the price of the movie. 

        decimal price = decimal.Parse(Console.ReadLine()); // Decimal input because it is a price. 



        Console.WriteLine("Enter MPAA rating (G, PG, PG13, R, NC17):"); // Enter the rating of the movie. 

        MPAARating rating = (MPAARating)Enum.Parse(typeof(MPAARating), Console.ReadLine());



        Console.WriteLine("Enter screen number:"); // Enter the screen number of the movie in the theater. 

        int screenNumber = int.Parse(Console.ReadLine()); // Integer input because the screen number will be a number.  



        Console.WriteLine("Enter seating capacity:"); // Enter the seating capacity of the theater. 

        int seatingCapacity = int.Parse(Console.ReadLine()); // Integer input because the seating capacity will be a number. 



        Console.WriteLine("Enter showtimes (separated by '|'):"); // Enter possible showtimes of the movie. 

        var showtimes = Console.ReadLine().Split('|').Select(TimeSpan.Parse).ToList();



        var screen = new Screen(screenNumber, seatingCapacity); // Everything about the movie playing in the theater and where. 

        var newMovie = new Movie(title, price, rating, screen, showtimes); // Everything about the movie. 

        movies.Add(newMovie);



        Console.WriteLine("New movie added successfully."); // The output the program will give when your movie is added correctly. 

    }



    static void UpdateExistingMovie()

    {

        DisplayMovies();

        int movieIndex = GetUserInput("Select the number of the movie to update:") - 1; // When updating the movie you added. 



        if (movieIndex < 0 || movieIndex >= movies.Count)

        {

            Console.WriteLine("Invalid movie selection."); // An if statement if the user does not input an int. 

            return;

        }



        Movie movieToUpdate = movies[movieIndex];

        Console.WriteLine($"Updating movie: {movieToUpdate.Title}"); // If the input was valid and an int, this will be displayed. 



        // Updating the price of the movie. 

        Console.WriteLine($"Current price: {movieToUpdate.Price}");

        Console.WriteLine("Enter new price:"); // The new price the user wants. 

        movieToUpdate.Price = decimal.Parse(Console.ReadLine());  // Decimal input because it is a price. 



        Console.WriteLine("Movie updated successfully.");

    }



    static void RemoveMovie()

    {

        DisplayMovies();

        int movieIndex = GetUserInput("Select the number of the movie to remove:") - 1; // When trying to update by removing a movie from the list. 



        if (movieIndex < 0 || movieIndex >= movies.Count)

        {

            Console.WriteLine("Invalid movie selection."); // If the input was not an int, this will be displayed. 

            return;

        }



        Movie movieToRemove = movies[movieIndex];

        Console.WriteLine($"Removing movie: {movieToRemove.Title}"); // How the program removes the movie. 



        // Confirmation before removal. 

        if (GetYesOrNo("Are you sure you want to remove this movie? (yes/no)")) // Confirmation displayed to the user. 

        {

            movies.RemoveAt(movieIndex);

            Console.WriteLine("Movie removed successfully."); // an if/else statement if the movie was able to be removed. 

        }

        else

        {

            Console.WriteLine("Movie removal cancelled."); // an if/else statement if the movie was unable to be removed because the user did not use an int. 

        }

    }



    static void CreateReceiptFile(string filePath, List<TicketOrder> orders)

    {

        try

        {

            using (StreamWriter writer = new StreamWriter(filePath))

            {

                writer.WriteLine("Customer Order Receipt");

                writer.WriteLine("--------------------------------------------------"); // This is how the order receipt will look like at the end to the user. 

                foreach (var order in orders)

                {

                    writer.WriteLine($"Order Number: {order.OrderNumber}"); // The order for each thing the program asks the user. 

                    writer.WriteLine($"Movie Title: {order.MovieTitle}");

                    writer.WriteLine($"Screen: {order.ScreenNumber}");

                    writer.WriteLine($"Showtime: {order.Showtime}");

                    writer.WriteLine($"Price per Ticket: {order.UnitPrice:C}");

                    writer.WriteLine($"Number of Tickets: {order.TicketCount}");

                    writer.WriteLine($"Subtotal: {order.Subtotal:C}");

                    writer.WriteLine("Snacks:");

                    foreach (var snack in order.Snacks)

                    {

                        writer.WriteLine($"   {snack.Name} - {snack.Price:C}");

                    }

                    writer.WriteLine("--------------------------------------------------");

                }

                writer.WriteLine($"Grand Total: {orders.Sum(o => o.Subtotal):C}"); // Displays the grand total to the user. 

            }

            Console.WriteLine($"Receipt file created at {filePath}"); // If the file is read successfully. 

        }

        catch (Exception ex)

        {

            Console.WriteLine($"Error writing to file: {ex.Message}"); // A try/catch if there is an error writing to Brett's file. 

        }

    }





    static void Main()

    {



        Console.WriteLine("Welcome to Willie the Wildcat Cinema!"); // The beggining welcome for the code. 

        Console.WriteLine("Experience the magic of movies and more!\n");

        movies = LoadMoviesFromFile("movies.csv");  // Loading movies outside the while loop. 



        while (true)

        {

            DisplayMainMenu();

            int choice = GetUserInput("Choose an option:"); // This will be a number because it is an int. If it does not work then the code will read invalid choice. 

            switch (choice)

            {

                case 1:

                    RunMovieSelectionPath();

                    break; // Because it is a switch, we have to break for every statement. 

                case 2:

                    RunMovieManagementPath();

                    break;

                case 3:

                    Console.WriteLine("Exiting the program...");

                    return; // Exit the application. 

                default:

                    Console.WriteLine("Invalid choice. Please try again."); // Default code for the switch. 

                    break;

            }



            Console.WriteLine("Enter your name:"); // The first question of the program, the user will enter their name. This will be a string because it is a name. 

            string userName = Console.ReadLine();



            Console.WriteLine("Enter your age:"); // The user will enter their age, this is an int because it is a number. 

            int userAge;

            while (!int.TryParse(Console.ReadLine(), out userAge))

            {

                Console.WriteLine("Please enter a valid age."); // If the user does not input a valid int. 

            }



            var user = new User(userName, userAge);

            bool continuePurchasing = true;



            while (continuePurchasing)

            {

                DisplayMovies(); // List of available movies for the user to choose from. 

                int movieSelection = GetUserInput("Select a movie by number:"); // Assumed to take a prompt as an argument and return the users input as an int. 

                var selectedMovie = movies[movieSelection - 1];



                DisplayShowtimes(selectedMovie); // Available showtimes for the selected movie. 

                TimeSpan selectedTime = TimeSpan.Parse(Console.ReadLine()); // 'Parse' assumes that the user enters a valid time format. 



                int ticketCount = GetUserInput("Enter number of tickets:");

                List<SnackItem> selectedSnacks = SelectSnacks(); // User chooses snacks for their order.  



                ProcessOrder(user, selectedMovie, selectedTime, ticketCount, selectedSnacks);



                continuePurchasing = GetYesOrNo("Do you want to purchase tickets for another movie? (yes/no)"); // Output asks wether the user wants to purchase tickets to another movie. 

            }



            PrintOrderSummary(); // Print function for the order summary. 

        }

    }

    static void RunMovieManagementPath()

    {

        bool isRunning = true; // If isRunning is true then this code while loop will run. 

        while (isRunning)

        {

            Console.WriteLine("\n--- Movie Management Menu ---"); // The different things the user can ask for in the code if they want to add a movie or cancel etc. 

            Console.WriteLine("1. Add New Movie"); // The user will add a new movie. 

            Console.WriteLine("2. Update Existing Movie");

            Console.WriteLine("3. Remove Movie"); // The user will remove a movie from the list. 

            Console.WriteLine("4. Return to Main Menu"); // The code will return to the main menu. 



            int choice = GetUserInput("Enter your choice:"); // The choice will be a number so it is represented as an int.  

            switch (choice) // Switch functions require breaks at the end of each case. 

            {

                case 1:

                    AddNewMovie();

                    break;

                case 2:

                    UpdateExistingMovie();

                    break;

                case 3:

                    RemoveMovie();

                    break;

                case 4:

                    isRunning = false; // If isRunning is false then the code will break. 

                    break;

            }

        }

    }

}







