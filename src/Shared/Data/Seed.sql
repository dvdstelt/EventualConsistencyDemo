-- Seed data for the movie-ticket demo.
-- Ids are fixed and INSERT OR IGNORE is used so this script is idempotent and
-- safe to run from either endpoint on startup. The schema itself is created by
-- EF Core (MoviesDbContext) from the entity model; this script only seeds data.

INSERT OR IGNORE INTO "Movies"
    ("Id", "UrlTitle", "Title", "Description", "Image", "Rating", "Icons", "MovieDetails", "ReleaseDate", "PricePerTicket", "PopularityScore", "TicketType", "Showtimes")
VALUES
    ('11111111-1111-1111-1111-111111111111', 'gameofthrones', 'Game of Thrones',
     'Watch this last episode on the big screen! In the aftermath of the devastating attack on King&#39;s Landing, Daenerys must face the survivors.',
     'gameofthrones.jpg', 5, '["16","sex","violence"]', '80 minutes | English, Dutch subtitles',
     '2019-05-19 00:00:00', 0, 1000, 'DrawingTicket', '["19:00"]'),

    ('22222222-2222-2222-2222-222222222222', 'deadpool', 'Deadpool & Wolverine',
     'Deadpool is offered a place in the Marvel Cinematic Universe by the Time Variance Authority, but instead recruits a variant of Wolverine to save his universe from extinction.',
     'deadpool.jpg', 4, '["16","violence","explicitlanguage"]', '127 minutes | English, Dutch subtitles',
     '2024-07-24 00:00:00', 10, 200, 'RegularTicket', '["10:00","15:00","20:00"]'),

    ('33333333-3333-3333-3333-333333333333', 'dune', 'Dune',
     'Feature adaptation of Frank Herbert''s science fiction novel, about the son of a noble family entrusted with the protection of the most valuable asset and most vital element in the galaxy.',
     'dune.jpg', 5, '["12","violence","fear"]', '156 minutes | English, Dutch subtitles',
     '2021-09-16 00:00:00', 20, 500, 'RegularTicket', '["10:00","13:00","15:00","20:00","23:00"]');

INSERT OR IGNORE INTO "Reviews"
    ("Id", "MovieIdentifier", "ReviewedAt", "Description", "PostedByEmail")
VALUES
    ('a1111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111', '2019-05-19 23:30:12',
     'A riddle: A father and his son fight in the Battle of the Bastards. The father is killed in battle and the son is brought to the Warden of the North to be knighted. The Warden of the North looks at the boy and says: "I can''t knight him, he''s my son!" How can this be?', NULL),

    ('a2222222-2222-2222-2222-222222222222', '11111111-1111-1111-1111-111111111111', '2019-05-19 23:35:36',
     'I want more Arya! She''s the best!', NULL),

    ('b1111111-1111-1111-1111-111111111111', '22222222-2222-2222-2222-222222222222', '2024-07-25 12:12:12',
     'Ryan Reynolds is hilarious again! Might just be the best Deadpool yet!', NULL),

    ('b2222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222222', '2024-07-26 13:45:18',
     'Loved it! Don''t forget to wait for the past-credits scene! Never laughed so hard.', NULL),

    ('c1111111-1111-1111-1111-111111111111', '33333333-3333-3333-3333-333333333333', '2021-10-08 08:14:56',
     'Great visuals, screenplay adaptation is too close to the books though.', NULL),

    ('c2222222-2222-2222-2222-222222222222', '33333333-3333-3333-3333-333333333333', '2021-10-09 13:37:00',
     'Can''t wait to go back to Mordor!', NULL);
