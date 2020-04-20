CREATE TABLE [dbo].[Decks] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (50)   NOT NULL,
    [Subject]    VARCHAR (50)   NOT NULL,
    [User_ID] NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_Decks] PRIMARY KEY CLUSTERED ([ID] ASC)
);