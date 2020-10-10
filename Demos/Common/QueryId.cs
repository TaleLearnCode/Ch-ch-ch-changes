using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{
	public class QueryId
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}

}