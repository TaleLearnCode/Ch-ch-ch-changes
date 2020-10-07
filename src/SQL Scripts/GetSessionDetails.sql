DECLARE @EventId INT = 10

SELECT Session.EventId,
       EventDetail.Name,
       Session.Id,
       SessionPeriod.StartDateTime,
       SessionPeriod.EndDateTime,
       Session.Title,
       CONCAT('Topic-', Topic.Id) AS TopicId,
     Topic.Name AS TopicName,
       CONCAT('Tag-', Technology.Id) AS TagId,
     Technology.Name AS TagName,
       CONCAT(Contact.FirstName, ' ', Contact.LastName) AS Speaker
  FROM Session
 INNER JOIN EventDetail ON EventDetail.Id = Session.EventId
 INNER JOIN SessionPeriod ON SessionPeriod.Id = Session.SessionPeriodId
 INNER JOIN SessionTopic ON SessionTopic.SessionId = Session.Id
 INNER JOIN Topic ON Topic.Id = SessionTopic.TopicId
 INNER JOIN SessionTechnology ON SessionTechnology.SessionId = Session.Id
 INNER JOIN Technology ON Technology.Id = SessionTechnology.TechnologyId
 INNER JOIN SessionSpeaker ON SessionSpeaker.SessionId = Session.Id
 INNER JOIN Speaker ON Speaker.AspNetUserId = SessionSpeaker.SpeakerId
 INNER JOIN Contact ON Contact.Id = Speaker.ContactId
 WHERE Session.EventId = @EventId
   AND Session.SessionStatusId = 6