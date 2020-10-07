using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{
	public class Presentation
	{

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("speakers")]
		public List<string> Speakers { get; set; }

		[JsonPropertyName("startDateTime")]
		public DateTime StartDateTime { get; set; }

		[JsonPropertyName("endDateTime")]
		public DateTime EndDateTime { get; set; }

		[JsonPropertyName("tags")]
		public List<string> Tags { get; set; }

		[JsonPropertyName("topics")]
		public List<string> Topics { get; set; }

		public Presentation() { }

		public Presentation(string[] fields)
		{
			Id = fields[PresentationFields.Id];
			StartDateTime = Convert.ToDateTime(fields[PresentationFields.StartDateTime]);
			EndDateTime = Convert.ToDateTime(fields[PresentationFields.EndDateTime]);
			Title = fields[PresentationFields.Title];
			Topics = new List<string>() { fields[PresentationFields.Topic] };
			Tags = new List<string>() { fields[PresentationFields.Tag] };
			Speakers = new List<string>() { fields[PresentationFields.Speaker] };
		}

	}

}