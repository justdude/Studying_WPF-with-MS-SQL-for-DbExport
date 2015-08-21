CREATE TABLE [dbo].[Conf_Dyn_Values] (
    [Id]       NVARCHAR (50) NOT NULL,
    [TableId]  NVARCHAR (50) NOT NULL,
    [CollId]   NVARCHAR (50) NOT NULL,
    [RowNumb]  INT           NOT NULL,
    [Float]    FLOAT (53)    NULL,
    [String]   NTEXT         NULL,
    [DateTime] DATETIME2 (7) NULL,
    [Bool]     BIT           DEFAULT ((0)) NULL,
    [Int]      INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

