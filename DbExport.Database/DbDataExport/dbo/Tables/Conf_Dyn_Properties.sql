CREATE TABLE [dbo].[Conf_Dyn_Properties] (
    [Id]       NVARCHAR (50) NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    [CollType] NCHAR (10)    NOT NULL,
    [TableId]  NCHAR (10)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

