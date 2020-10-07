using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{

	public class Speaker : Metadata
	{

		private string _firstName;
		private string _lastName;

		[JsonPropertyName("firstName")]
		public string FirstName
		{
			get
			{
				return _firstName;
			}
			set
			{
				_firstName = value;
				SetName();
			}
		}

		[JsonPropertyName("lastName")]
		public string LastName
		{
			get
			{
				return _lastName;
			}
			set
			{
				_lastName = value;
				SetName();
			}
		}

		public Speaker() : base() { }
		
		public Speaker(string id, string firstName, string lastName) : base("speaker", id, $"{firstName} {lastName}")
		{
			_firstName = firstName;
			_lastName = lastName;
		}

		private void SetName()
		{
			Name = $"{_firstName} {_lastName}";
		}


		// TODO: Create the metadata container/data

	}

}