INSERT INTO Users (Id, UserName, Email, PasswordHash, CreatedAt, AvatarUrl)
VALUES
    ('b1aefd99-9c29-4a25-bd2d-93d167a20b00', 'alice', 'alice@mail.com', 'hashedpass1', GETUTCDATE(), NULL),
    ('c2ed3f7a-57e7-4d1c-92b4-fc4bb1006e11', 'bob', 'bob@mail.com', 'hashedpass2', GETUTCDATE(), NULL);

INSERT INTO CodeSnippets (Id, Title, Content, OwnerId, CreatedAt, UpdatedAt, IsPublic)
VALUES
    ('55f2f16e-8a52-49e2-9d4e-44457b3f2c11', 'Hello World', 'Console.WriteLine("Hello World!");', 'b1aefd99-9c29-4a25-bd2d-93d167a20b00', GETUTCDATE(), GETUTCDATE(), 1),
    ('d1ea1ed0-3e7d-4b7f-a8c1-45b42b39a3e7', 'Fibonacci', 'int F(int n) => n <= 1 ? 1 : F(n-1) + F(n-2);', 'c2ed3f7a-57e7-4d1c-92b4-fc4bb1006e11', GETUTCDATE(), GETUTCDATE(), 0);
