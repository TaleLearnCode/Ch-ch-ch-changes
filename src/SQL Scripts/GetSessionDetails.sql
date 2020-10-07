SELECT Session.Id,
       SessionPeriod.StartDateTime,
       SessionPeriod.EndDateTime,
       Session.Title,
       Topic.Name AS Topic,
       Technology.Name AS Tag,
       CONCAT(Contact.FirstName, ' ', Contact.LastName) AS Speaker
  FROM Session
 INNER JOIN SessionPeriod ON SessionPeriod.Id = Session.SessionPeriodId
 INNER JOIN SessionTopic ON SessionTopic.SessionId = Session.Id
 INNER JOIN Topic ON Topic.Id = SessionTopic.TopicId
 INNER JOIN SessionTechnology ON SessionTechnology.SessionId = Session.Id
 INNER JOIN Technology ON Technology.Id = SessionTechnology.TechnologyId
 INNER JOIN SessionSpeaker ON SessionSpeaker.SessionId = Session.Id
 INNER JOIN Speaker ON Speaker.AspNetUserId = SessionSpeaker.SpeakerId
 INNER JOIN Contact ON Contact.Id = Speaker.ContactId
 WHERE Session.EventId = 10
   AND Session.SessionStatusId = 6