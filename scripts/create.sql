CREATE TABLE Users (
                       Id            UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                       Username      NVARCHAR(64)      NOT NULL UNIQUE,
                       Email         NVARCHAR(128)     NOT NULL UNIQUE,
                       PasswordHash  NVARCHAR(MAX)     NOT NULL,
                       CreatedAt     DATETIME2         NOT NULL DEFAULT SYSUTCDATETIME(),
                       AvatarFileName NVARCHAR(260)    NULL,
                       IsAdmin       BIT               NOT NULL DEFAULT 0,
                       IsBlocked     BIT               NOT NULL DEFAULT 0,
                       BlockedByAdminEmail NVARCHAR(128) NULL
);

CREATE TABLE CodeSnippets (
                              Id           UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                              Title        NVARCHAR(128)     NOT NULL,
                              Content      NVARCHAR(MAX)     NOT NULL,
                              OwnerId      UNIQUEIDENTIFIER  NOT NULL,
                              CreatedAt    DATETIME2         NOT NULL DEFAULT SYSUTCDATETIME(),
                              UpdatedAt    DATETIME2         NULL,
                              IsPublic     BIT               NOT NULL DEFAULT 0,
                              CONSTRAINT FK_CodeSnippets_Users FOREIGN KEY (OwnerId)
                                  REFERENCES Users (Id)
                                  ON DELETE CASCADE
);

CREATE TABLE CollabSessions (
                                Id            UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                                Name          NVARCHAR(128)     NOT NULL,
                                OwnerId       UNIQUEIDENTIFIER  NOT NULL,
                                CodeSnippetId UNIQUEIDENTIFIER  NOT NULL,
                                CreatedAt     DATETIME2         NOT NULL DEFAULT SYSUTCDATETIME(),
                                ExpiresAt     DATETIME2         NULL,
                                EditedAt      DATETIME2         NULL,
                                IsActive      BIT               NOT NULL DEFAULT 1,
                                JoinCode      NVARCHAR(64)      NOT NULL,
                                CONSTRAINT FK_CollabSessions_Owner FOREIGN KEY (OwnerId)
                                    REFERENCES Users (Id)
                                    ON DELETE NO ACTION,
                                CONSTRAINT FK_CollabSessions_Snippet FOREIGN KEY (CodeSnippetId)
                                    REFERENCES CodeSnippets (Id)
                                    ON DELETE CASCADE
);

CREATE TABLE ColladParticipants (
                                    Id         UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                                    SessionId  UNIQUEIDENTIFIER  NOT NULL,
                                    UserId     UNIQUEIDENTIFIER  NOT NULL,
                                    JoinedAt   DATETIME2         NOT NULL DEFAULT SYSUTCDATETIME(),
                                    CONSTRAINT FK_Participants_Session FOREIGN KEY (SessionId)
                                        REFERENCES CollabSessions (Id)
                                        ON DELETE CASCADE,
                                    CONSTRAINT FK_Participants_User FOREIGN KEY (UserId)
                                        REFERENCES Users (Id)
                                        ON DELETE NO ACTION
);

CREATE TABLE SessionEditHistories (
                                      Id               UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                                      SessionId        UNIQUEIDENTIFIER  NOT NULL,
                                      EditedByUserId   UNIQUEIDENTIFIER  NOT NULL,
                                      EditedAt         DATETIME2         NULL,
                                      Changes          NVARCHAR(MAX)     NOT NULL,
                                      CONSTRAINT FK_History_Session FOREIGN KEY (SessionId)
                                          REFERENCES CollabSessions (Id)
                                          ON DELETE CASCADE,
                                      CONSTRAINT FK_History_EditedBy FOREIGN KEY (EditedByUserId)
                                          REFERENCES Users (Id)
                                          ON DELETE NO ACTION
);
