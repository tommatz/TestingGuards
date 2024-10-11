using CommunityToolkit.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }

    public Person(string? firstName, string lastName, int age, string email)
    {
        Guard.IsNotNullOrWhiteSpace(firstName, "Someone named you null? Thats sad. Just illustrating custom error messages");
        Guard.HasSizeLessThan(firstName, 25);
        Guard.IsNotNullOrWhiteSpace(lastName, nameof(lastName));
        Guard.HasSizeLessThan(lastName, 25);
        Guard.IsLessThan(age, 40, "Booooo youre old, must be under 40.");
        CustomGuard.IsValidEmail(email, "Email is not valid.");

        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Email = email;
    }
}
class Program
{
    static void Main()
    {
        Person goodPerson = new Person("John", "Doe", 30, "john.doe@example.com");
        //Person personNamedNull = new Person(null, "Doe", 30, "john.doe@example.com");
        //Person personNameTooLong = new Person("JohnAReallyLongNameThatShouldNotBeHereWhyWouldYouBeNamedThis", "Doe", 30, "john.doe@example.com");
        //Person personTooOld = new Person("John", "Doe", 50, "john.doe@example.com");
        //Person malformedEmailPerson = new Person("John", "Doe", 30, "john.doe#example.com");

    }
}


//I could not figure out how to extend the guard to add extra functionality on top. This is a completely custom class. We would likely have to do the same in situations like this
public static class CustomGuard
{
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

    public static void IsValidEmail(string email, string message = null)
    {
        Guard.IsNotNullOrWhiteSpace(email, message ?? "Email cannot be null or whitespace.");

        if (!EmailRegex.IsMatch(email))
        {
            throw new ArgumentException(message ?? $"{nameof(email)} is not a valid email address.", nameof(email));
        }
    }
}