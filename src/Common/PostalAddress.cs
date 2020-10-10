namespace TaleLearnCode.ChChChChanges.Common
{
	public class PostalAddress
	{

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string StreetAddress { get; set; }

		public string State { get; set; }

		public string PostalCode { get; set; }

		public string Country { get; set; }

		public PostalAddress() { }

		public PostalAddress(string firstName, string lastName, string streetAddress, string state, string postalCode, string country)
		{
			FirstName = firstName;
			LastName = lastName;
			StreetAddress = streetAddress;
			State = state;
			PostalCode = postalCode;
			Country = country;
		}

	}

}