using FluentValidation;
using System;
using System.Net.WebSockets;

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

        //In order to presever our current setup where Ctors throw, we'd need this.
        //Seems like typical usecase is to validate after constructing an object, not in the ctor.
        var validator = new PersonValidator();
        var result = validator.Validate(this);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }

    public Person(string firstName, string lastName, string accountKey, int age, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Email = email;
        AccountKey = accountKey;

        //In order to presever our current setup where Ctors throw, we'd need this.
        //Seems like typical usecase is to validate after constructing an object, not in the ctor.
        var validator = new PersonValidator();
        var result = validator.Validate(this, options => options.IncludeRuleSets("WithAccountKey"));
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
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

//Each model would need a validator. Sort of unclear to me how this would work for models with multiple constructors where some are missing parameters. Maybe itd just work, I dont know.
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

        RuleSet("WithAccountKey", () =>
        {
            RuleFor(x => x.AccountKey)
                .NotNull().WithMessage("AccountKey cannot be null!")
                .NotEmpty().WithMessage("AccountKey is required.")
                .MaximumLength(40).WithMessage("AccountKey must not exceed 40 characters.");
        });

    }
}