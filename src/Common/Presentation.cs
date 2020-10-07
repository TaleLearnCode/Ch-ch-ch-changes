using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{
	public class Presentation
	{

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("eventId")]
		public string EventId { get; set; }

		[JsonPropertyName("eventName")]
		public string EventName { get; set; }

		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("speakers")]
		public List<Speaker> Speakers { get; set; } = new List<Speaker>();

		[JsonPropertyName("startDateTime")]
		public DateTime StartDateTime { get; set; }

		[JsonPropertyName("endDateTime")]
		public DateTime EndDateTime { get; set; }

		[JsonPropertyName("tags")]
		public List<Metadata> Tags { get; } = new List<Metadata>();

		[JsonPropertyName("topics")]
		public List<Metadata> Topics { get; } = new List<Metadata>();

		public Presentation() { }

		public Presentation(string[] fields)
		{
			Id = fields[PresentationFields.Id];
			EventId = fields[PresentationFields.EventId];
			EventName = fields[PresentationFields.EventName];
			StartDateTime = Convert.ToDateTime(fields[PresentationFields.StartDateTime]);
			EndDateTime = Convert.ToDateTime(fields[PresentationFields.EndDateTime]);
			Title = fields[PresentationFields.Title];

			Tags.Add(
				Metadata.TagFactory(
					fields[PresentationFields.TagId],
					fields[PresentationFields.TagName]));
			Topics.Add(
				Metadata.TopicFactory(
					fields[PresentationFields.TopicId],
					fields[PresentationFields.TopicName]));
			Speakers.Add(
				new Speaker(fields[PresentationFields.SpeakerId],
				fields[PresentationFields.SpeakerFirstName],
				fields[PresentationFields.SpeakerLastName]));

		}

	}

}