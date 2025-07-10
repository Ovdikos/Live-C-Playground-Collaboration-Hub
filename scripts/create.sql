CREATE TABLE Users (
                       Id UNIQUEIDENTIFIER PRIMARY KEY,
                       Username NVARCHAR(64) NOT NULL,
                       Email NVARCHAR(128) NOT NULL UNIQUE,
                       PasswordHash NVARCHAR(256) NOT NULL,
                       CreatedAt DATETIME2 NOT NULL,
                       AvatarUrl NVARCHAR(256),
                       IsAdmin BIT NOT NULL DEFAULT 0,
                       IsBlocked BIT NOT NULL DEFAULT 0
);

CREATE TABLE CodeSnippets (
                              Id UNIQUEIDENTIFIER PRIMARY KEY,
                              Title NVARCHAR(128) NOT NULL,
                              Content NVARCHAR(MAX) NOT NULL,
                              OwnerId UNIQUEIDENTIFIER NOT NULL,
                              CreatedAt DATETIME2 NOT NULL,
                              UpdatedAt DATETIME2,
                              IsPublic BIT NOT NULL DEFAULT 0,
                              CONSTRAINT FK_CodeSnippets_Users_OwnerId FOREIGN KEY (OwnerId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE TABLE CollabSessions (
                                Id UNIQUEIDENTIFIER PRIMARY KEY,
                                Name NVARCHAR(128) NOT NULL,
                                OwnerId UNIQUEIDENTIFIER NOT NULL,
                                CodeSnippetId UNIQUEIDENTIFIER NOT NULL,
                                CreatedAt DATETIME2 NOT NULL,
                                ExpiresAt DATETIME2,
                                EditedAt DATETIME2,
                                IsActive BIT NOT NULL DEFAULT 1,
                                CONSTRAINT FK_CollabSessions_Users_OwnerId FOREIGN KEY (OwnerId) REFERENCES Users(Id) ON DELETE RESTRICT,
                                CONSTRAINT FK_CollabSessions_CodeSnippets_CodeSnippetId FOREIGN KEY (CodeSnippetId) REFERENCES CodeSnippets(Id) ON DELETE CASCADE
);

CREATE TABLE CollabParticipants (
                                    Id UNIQUEIDENTIFIER PRIMARY KEY,
                                    SessionId UNIQUEIDENTIFIER NOT NULL,
                                    UserId UNIQUEIDENTIFIER NOT NULL,
                                    JoinedAt DATETIME2 NOT NULL,
                                    CONSTRAINT FK_CollabParticipants_CollabSessions_SessionId FOREIGN KEY (SessionId) REFERENCES CollabSessions(Id) ON DELETE CASCADE,
                                    CONSTRAINT FK_CollabParticipants_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE RESTRICT
);

CREATE TABLE SessionEditHistories (
                                      Id UNIQUEIDENTIFIER PRIMARY KEY,
                                      SessionId UNIQUEIDENTIFIER NOT NULL,
                                      EditedByUserId UNIQUEIDENTIFIER NOT NULL,
                                      EditedAt DATETIME2 NOT NULL,
                                      Changes NVARCHAR(MAX) NOT NULL,
                                      CONSTRAINT FK_SessionEditHistories_CollabSessions_SessionId FOREIGN KEY (SessionId) REFERENCES CollabSessions(Id) ON DELETE RESTRICT,
                                      CONSTRAINT FK_SessionEditHistories_Users_EditedByUserId FOREIGN KEY (EditedByUserId) REFERENCES Users(Id) ON DELETE RESTRICT
);
