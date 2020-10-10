SELECT 'topic' AS Type,
       CONCAT('Topic-', Id) AS Id,
       Name,
     '' AS FirstName,
     '' AS LastName
  FROM Topic
UNION
SELECT 'tag' AS Type,
       CONCAT('Tag-', Id) AS Id,
       Name,
     '' AS FirstName,
     '' AS LastName
  FROM Technology
UNION
SELECT DISTINCT
       'speaker' AS Type,
     CONCAT('Speaker-', Contact.Id) AS Id,
       CONCAT(Contact.FirstName, ' ', Contact.LastName) AS Name,
       Contact.FirstName,
     Contact.LastName
  FROM Session
 INNER JOIN SessionSpeaker ON SessionSpeaker.SessionId = Session.Id
 INNER JOIN Speaker ON Speaker.AspNetUserId = SessionSpeaker.SpeakerId
 INNER JOIN Contact ON Contact.Id = Speaker.ContactId
 WHERE FirstName IS NOT NULL
   AND LastName IS NOT NULL