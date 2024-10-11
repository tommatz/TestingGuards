using Ardalis.GuardClauses;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public class Person
{
    public string FirstName { get; }
    public string LastName { get; }
    public int Age { get; }
    public string Email { get; }

    public Person(string firstName, string lastName, int age, string email)
    {
        Guard.Against.NullOrWhiteSpace(firstName, "Someone named you null? That's sad.");
        Guard.Against.MaxStringLength(firstName, 25, nameof(firstName));
        Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName), "Last name is required.");
        Guard.Against.MaxStringLength(lastName, 25, nameof(lastName));
        Guard.Against.OutOfRange(age, nameof(age), 0, 39, "Booooo you're old, must be under 40.");
        Guard.Against.IsValidEmail(email, "Email is not valid.");

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


//This library supports proper extenstion of the guard clauses. Very easy to use as well
namespace Ardalis.GuardClauses
{
    public static class MaxStringLengthGuard
    {
        public static void MaxStringLength(this IGuardClause guardClause,
            string input,
            int maxLength,
            [CallerArgumentExpression("input")] string? parameterName = null)
        {
            if (input?.Length >= maxLength)
                throw new ArgumentException($"{parameterName} cannot be longer than {maxLength} characters", parameterName);
        }
    }

    public static class IsValidEmailGuard
    {
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        public static void IsValidEmail(this IGuardClause guardClause,
            string input,
            [CallerArgumentExpression("input")] string? parameterName = null)
        {
            if (!EmailRegex.IsMatch(input))
            {
                throw new ArgumentException($"{parameterName} must be a valid email format", parameterName);
            }
        }
    }
}