-- Users
INSERT INTO Users (Id, Username, Email, PasswordHash, CreatedAt, AvatarUrl, IsAdmin, IsBlocked)
VALUES
    (NEWID(), 'admin', 'admin@example.com', 'hashedpassword', GETUTCDATE(), NULL, 1, 0),
    (NEWID(), 'testuser', 'test@example.com', 'hashedpassword', GETUTCDATE(), NULL, 0, 0);

-- CodeSnippets
INSERT INTO CodeSnippets (Id, Title, Content, OwnerId, CreatedAt, UpdatedAt, IsPublic)
VALUES
    (NEWID(), 'Sample Snippet', 'Console.WriteLine("Hello, World!");', (SELECT TOP 1 Id FROM Users WHERE Username = 'admin'), GETUTCDATE(), NULL, 1);

-- CollabSessions
INSERT INTO CollabSessions (Id, Name, OwnerId, CodeSnippetId, CreatedAt, ExpiresAt, EditedAt, IsActive)
VALUES
    (NEWID(), 'Session 1', (SELECT TOP 1 Id FROM Users WHERE Username = 'admin'), (SELECT TOP 1 Id FROM CodeSnippets), GETUTCDATE(), NULL, NULL, 1);

-- CollabParticipants
INSERT INTO CollabParticipants (Id, SessionId, UserId, JoinedAt)
VALUES
    (NEWID(), (SELECT TOP 1 Id FROM CollabSessions), (SELECT TOP 1 Id FROM Users WHERE Username = 'testuser'), GETUTCDATE());

-- SessionEditHistories
INSERT INTO SessionEditHistories (Id, SessionId, EditedByUserId, EditedAt, Changes)
VALUES
    (NEWID(), (SELECT TOP 1 Id FROM CollabSessions), (SELECT TOP 1 Id FROM Users WHERE Username = 'admin'), GETUTCDATE(), 'Session created');
