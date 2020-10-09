using System.Text.Json.Serialization;

namespace TaleLearnCode.ChChChChanges.Common
{
	public class PresentationByTag : Presentation
	{

		[JsonPropertyName("tagId")]
		public string TagId { get; set; }

		[JsonPropertyName("tagName")]
		public string TagName { get; set; }

		[JsonPropertyName("presentationId")]
		public string PresentationId { get; set; }

		public PresentationByTag() : base() { }

		public PresentationByTag(Presentation presentation, Metadata tag)
		{
			Id = $"{presentation.Id}|{tag.Id}";
			EventId = presentation.EventId;
			EventName = presentation.EventName;
			Title = presentation.Title;
			Speakers = presentation.Speakers;
			StartDateTime = presentation.StartDateTime;
			EndDateTime = presentation.EndDateTime;
			Tags = presentation.Tags;
			Topics = presentation.Topics;

			TagId = tag.Id;
			TagName = tag.Name;
			PresentationId = presentation.Id;
		}

	}
}