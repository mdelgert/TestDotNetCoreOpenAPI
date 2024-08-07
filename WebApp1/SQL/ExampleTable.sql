CREATE TABLE Notes (
    NoteID INT IDENTITY(1,1) PRIMARY KEY,  -- Unique identifier for each note, auto-incrementing
    Title VARCHAR(255) NOT NULL,           -- Title of the note
    Content TEXT,                          -- Content of the note
    CreatedAt DATETIME2 DEFAULT GETDATE(), -- Timestamp for when the note was created
    UpdatedAt DATETIME2 DEFAULT GETDATE(), -- Timestamp for the last update
    AuthorID INT,                          -- ID of the author (if notes are linked to users)
    Tags VARCHAR(255)                      -- Optional tags for categorization
);

