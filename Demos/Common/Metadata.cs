using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{

	public class Metadata
	{

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		public Metadata() { }

		public Metadata(string type, string id, string name)
		{
			Type = type;
			Id = id;
			Name = name;
		}

		public static Metadata TagFactory(string id, string name)
		{
			return new Metadata("tag", id, name);
		}

		public static Metadata TopicFactory(string id, string name)
		{
			return new Metadata("topic", id, name);
		}


	}

}