CREATE TABLE [dbo].[Cards] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [Question]        VARCHAR (MAX) NOT NULL,
    [Answer]        VARCHAR (MAX) NOT NULL,
    [Decks_ID]  INT           NOT NULL,    
    CONSTRAINT [PK_table_19] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_24] FOREIGN KEY ([Decks_ID]) REFERENCES [dbo].[Decks] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [fkIdx_24]
    ON [dbo].[Cards]([Decks_ID] ASC);