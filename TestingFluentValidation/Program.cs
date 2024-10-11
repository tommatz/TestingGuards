using FluentValidation;

using TestingFluentValidation;

public class Person
{
    public string FirstName { get; }
    public string LastName { get; }
    public string AccountKey { get; }

    public int Age { get; }
    public string Email { get; }

    public Person(string firstName, string lastName, int age, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Email = email;

        ValidationWrapper.Validate(this, new PersonValidator());
    }

    public Person(string firstName, string lastName, string accountKey, int age, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Email = email;
        AccountKey = accountKey;

        ValidationWrapper.Validate(this, new PersonValidator(), "WithAccountKey");
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
        Person goodPersonWithAccountKey = new Person("John", "Doe", Guid.NewGuid().ToString(), 30, "john.doe@example.com");
        //Person badPersonWithNullAccountKey = new Person("John", "Doe", null, 30, "john.doe@example.com");
    }
}

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(person => person.FirstName)
            .NotNull().WithMessage("Someone named you null? That's sad.")
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(25).WithMessage("First name must not exceed 25 characters.");

        RuleFor(person => person.LastName)
            .NotNull().WithMessage("Last name is required.")
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(25).WithMessage("Last name must not exceed 25 characters.");

        RuleFor(person => person.Age)
            .InclusiveBetween(0, 39).WithMessage("Booooo you're old, must be under 40.");

        RuleFor(person => person.Email)
            .EmailAddress().WithMessage("Email is not valid."); //wild that this exists out of the box

        RuleSet("WithAccountKey", () => //this allows us to support 1 validator for multiple Ctors! Yay
        {
            RuleFor(x => x.AccountKey)
                .NotNull().WithMessage("AccountKey cannot be null!")
                .NotEmpty().WithMessage("AccountKey is required.")
                .MaximumLength(40).WithMessage("AccountKey must not exceed 40 characters.");
        });

    }
}