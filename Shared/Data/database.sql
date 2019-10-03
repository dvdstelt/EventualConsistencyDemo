USE master

IF DB_ID (N'EventualConsistencyDemo') IS NOT NULL
BEGIN
	ALTER DATABASE [EventualConsistencyDemo] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

	DROP DATABASE EventualConsistencyDemo;
END
GO

CREATE DATABASE EventualConsistencyDemo
GO

USE [EventualConsistencyDemo]
GO

/****** Object:  Table [dbo].[Movies]    Script Date: 10/3/2019 13:32:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Movies](
	[Id] [int] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[UrlTitle] [nvarchar](50) NOT NULL,
	[Image] [nvarchar](50) NOT NULL,
	[Rating] [int] NOT NULL,
	[Icons] [varchar](100) NOT NULL,
	[MovieDetails] [nvarchar](100) NOT NULL,
	[ReleaseDate] [datetime] NOT NULL,
	[Showtimes] [varchar](50) NOT NULL,
	[PricePerTicket] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_Movies] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO Movies VALUES (1, 'Game of Thrones', 'Watch this last episode on the big screen! In the aftermath of the devastating attack on King&#39;s Landing, Daenerys must face the survivors.', 'gameofthrones', 'got.jpg',
							5, '16;sex;violence', '80 minutes | English, Dutch subtitles', '2019-05-19', '19:00', 0)

INSERT INTO Movies VALUES (2, 'Jay and Silent Bob Reboot', 'Jay and Silent Bob return to Hollywood to stop a reboot of &#39;Bluntman and Chronic&#39; movie from getting made.', 'jayandsilentbobreboot', 'jaysilentbobreboot.jpg',
							2, '16;alcoholdrugabuse;explicitlanguage', '105 minutes | English, Dutch subtitles', '2019-10-15', '10:00;15:00;20:00', 10)

INSERT INTO Movies VALUES (3, 'Star Wars : The Rise of Skywalker', 'The surviving Resistance faces the First Order once more in the final chapter of the Skywalker saga.', 'riseofskywalker', 'riseofskywalker.jpg',
							3, '', '152 minutes | English, Dutch subtitles', '2019-10-15', '10:00;13:00;15:00;20:00;23:00', 10)
