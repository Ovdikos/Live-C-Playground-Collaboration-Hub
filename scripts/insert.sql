-- Password is -> P@ssw0rd1
INSERT INTO Users (Id, Username, Email, PasswordHash, CreatedAt, AvatarFileName, IsAdmin, IsBlocked, BlockedByAdminEmail)
VALUES
    ('00000000-0000-0000-0000-000000000001', 'admin', 'admin@example.com', '$2a$11$txwLo8g6Mzy1Bm9gOLSWSufOjnatugFiRF3m.EZr3Nu5.6ecjwf5G', SYSUTCDATETIME(), NULL, 1, 0, NULL),
    ('00000000-0000-0000-0000-000000000002', 'user1', 'user1@example.com', '$2a$11$txwLo8g6Mzy1Bm9gOLSWSufOjnatugFiRF3m.EZr3Nu5.6ecjwf5G', SYSUTCDATETIME(), NULL, 0, 0, NULL);

INSERT INTO CodeSnippets (Id, Title, Content, OwnerId, CreatedAt, UpdatedAt, IsPublic)
VALUES
    ('00000000-0000-0000-0000-000000000011', 'HelloWorld', 'Console.WriteLine("Hello");', '00000000-0000-0000-0000-000000000002', SYSUTCDATETIME(), NULL, 1);

INSERT INTO CollabSessions (Id, Name, OwnerId, CodeSnippetId, CreatedAt, ExpiresAt, EditedAt, IsActive, JoinCode)
VALUES
    ('00000000-0000-0000-0000-000000000101', 'Session1', '00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000011', SYSUTCDATETIME(), NULL, NULL, 1, 'join1234');

INSERT INTO CollabParticipants (Id, SessionId, UserId, JoinedAt)
VALUES
    ('00000000-0000-0000-0000-000000000201', '00000000-0000-0000-0000-000000000101', '00000000-0000-0000-0000-000000000002', SYSUTCDATETIME());

INSERT INTO SessionEditHistories (Id, SessionId, EditedByUserId, EditedAt, Changes)
VALUES
    ('00000000-0000-0000-0000-000000000301', '00000000-0000-0000-0000-000000000101', '00000000-0000-0000-0000-000000000002', SYSUTCDATETIME(), 'Initial creation');
