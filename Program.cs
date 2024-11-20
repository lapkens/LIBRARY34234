using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int YearPublished { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class Library
{
    private const string BookFilePath = "books.json";
    private const string UserFilePath = "users.json";
    private List<Book> books = new List<Book>();
    private List<User> users = new List<User>();

    public Library()
    {
        LoadBooks();
        LoadUsers();
    }

    public void AddBook(Book book)
    {
        books.Add(book);
        SaveBooks();
    }

    public void AddUser(User user)
    {
        users.Add(user);
        SaveUsers();
    }

    public void RemoveBook(int id)
    {
        books.RemoveAll(b => b.Id == id);
        SaveBooks();
    }

    public List<Book> GetBooks()
    {
        return books;
    }

    public List<User> GetUsers()
    {
        return users;
    }

    private void LoadBooks()
    {
        if (File.Exists(BookFilePath))
        {
            var jsonData = File.ReadAllText(BookFilePath);
            books = JsonSerializer.Deserialize<List<Book>>(jsonData) ?? new List<Book>();
        }
    }

    private void LoadUsers()
    {
        if (File.Exists(UserFilePath))
        {
            var jsonData = File.ReadAllText(UserFilePath);
            users = JsonSerializer.Deserialize<List<User>>(jsonData) ?? new List<User>();
        }
    }

    private void SaveBooks()
    {
        var jsonData = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(BookFilePath, jsonData);
    }

    private void SaveUsers()
    {
        var jsonData = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(UserFilePath, jsonData);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();

        while (true)
        {
            Console.WriteLine("Выберите действие: 1. Добавить книгу 2. Удалить книгу 3. Просмотреть книги 4. Добавить пользователя 5. Просмотреть пользователей 6. Выход");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddBook(library);
                    break;
                case "2":
                    RemoveBook(library);
                    break;
                case "3":
                    ShowBooks(library);
                    break;
                case "4":
                    AddUser(library);
                    break;
                case "5":
                    ShowUsers(library);
                    break;
                case "6":
                    return;
            }
        }
    }

    static void AddBook(Library library)
    {
        var book = new Book
        {
            Id = new Random().Next(1, 1000),
            Title = Prompt("Введите название книги: "),
            Author = Prompt("Введите имя автора: "),
            YearPublished = Int32.Parse(Prompt("Введите год публикации: "))
        };
        library.AddBook(book);
        Console.WriteLine($"Книга '{book.Title}' добавлена.");
    }

    static void AddUser(Library library)
    {
        var user = new User
        {
            Id = new Random().Next(1, 1000),
            Name = Prompt("Введите имя пользователя: "),
            Email = Prompt("Введите почту пользователя: ")
        };
        library.AddUser(user);
        Console.WriteLine($"Пользователь '{user.Name}' добавлен.");
    }

    static void RemoveBook(Library library)
    {
        Console.Write("Введите ID книги для удаления: ");

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            library.RemoveBook(id);
            Console.WriteLine($"Книга с ID '{id}' удалена.");
        }
        else
        {
            Console.WriteLine("Неверный ID.");
        }
    }

    static void ShowBooks(Library library)
    {
        var books = library.GetBooks();
        if (books.Count == 0)
        {
            Console.WriteLine("Нет книг в библиотеке.");
            return;
        }

        foreach (var book in books)
        {
            Console.WriteLine($"{book.Id}: {book.Title} - {book.Author} ({book.YearPublished})");
        }
    }

    static void ShowUsers(Library library)
    {
        var users = library.GetUsers();
        if (users.Count == 0)
        {
            Console.WriteLine("Нет пользователей в системе.");
            return;
        }

        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}: {user.Name} - {user.Email}");
        }
    }

    static string Prompt(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }
}
