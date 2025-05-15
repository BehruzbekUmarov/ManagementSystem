using InnerSystem.Identity.Enums;
using Microsoft.AspNetCore.Identity;

namespace InnerSystem.Identity.Models;

public class User : IdentityUser<Guid>
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public decimal Salary { get; set; }
	public GenderEnum? Gender { get; set; } = GenderEnum.Male;
	public DateTime BirthDate { get; set; }
	public Branch Branch { get; set; } = Branch.Bosh_Ofis;
	public DateTime CreateDate { get; set; } = DateTime.Now;
	public int GivenPoint { get; set; } = 0;
	public bool IsActive { get; set; } = true;

    public User(string firstName, string lastName, string email)
    {
		FirstName = firstName;
		LastName = lastName;
		Email = email;
		UserName = email;
    }

    public User(string firstName, string lastName, decimal salary, GenderEnum gender, DateTime birthDate, Branch branch, string email)
    {
        FirstName = firstName;
		LastName = lastName;
		Salary = salary;
		Gender = gender;
		BirthDate = birthDate;
		Branch = branch;
		Email = email;
    }

    public User()
    {
    }

	public void AddPoints(int points)
	{
		if (!IsActive)
			throw new InvalidOperationException("Cannot add points to an inactive employee.");

		if (points <= 0)
			throw new ArgumentException("Points must be greater than zero.");

		GivenPoint += points;
	}

	public void Activate()
	{
		IsActive = true;
	}

	public void Deactivate()
	{
		IsActive = false;
	}
}
