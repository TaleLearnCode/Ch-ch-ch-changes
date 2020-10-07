using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{

	public class Speaker : Metadata
	{

		[JsonPropertyName("firstName")]
		public string FirstName { get; set; }

		[JsonPropertyName("lastName")]
		public string LastName { get; set; }

		// TODO: Create the constructor
		// TODO: Create the metadata container/data

	}

}