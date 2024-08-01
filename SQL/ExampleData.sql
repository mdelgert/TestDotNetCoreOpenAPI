-- Insert a note with basic information
INSERT INTO Notes (Title, Content, AuthorID, Tags)
VALUES ('First Note', 'This is the content of the first note.', 1, 'personal,ideas');

-- Insert another note with different content and tags
INSERT INTO Notes (Title, Content, AuthorID, Tags)
VALUES ('Second Note', 'Here is some more content for the second note.', 2, 'work,important');

-- Insert a note with minimal fields (no tags)
INSERT INTO Notes (Title, Content, AuthorID)
VALUES ('Third Note', 'A note without tags.', 3);

-- Insert a note without specifying AuthorID and Tags (assuming these are optional)
INSERT INTO Notes (Title, Content)
VALUES ('Anonymous Note', 'A note with no author and no tags.');
