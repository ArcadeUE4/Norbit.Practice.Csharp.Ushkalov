
-- Создание таблицы "Проекты".
CREATE TABLE [Projects] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(255) NOT NULL, 
    [Code] NVARCHAR(50) NOT NULL,   
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Projects] PRIMARY KEY ([Id])
);

-- Создание таблицы "Задачи".
CREATE TABLE [WorkTasks] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(255) NOT NULL,
    [ProjectId] INT NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_WorkTasks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WorkTasks_Projects_ProjectId] 
    FOREIGN KEY ([ProjectId]) REFERENCES [Projects] 
    ([Id]) ON DELETE CASCADE
);

-- Созздание таблицы "Учет времени".
CREATE TABLE [TimeRecords] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Date] DATE NOT NULL,             
    [Hours] DECIMAL(4, 2) NOT NULL,   
    [Description] NVARCHAR(MAX) NULL, 
    [WorkTaskId] INT NOT NULL,
    CONSTRAINT [PK_TimeRecords] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TimeRecords_WorkTasks_WorkTaskId] 
    FOREIGN KEY ([WorkTaskId]) REFERENCES [WorkTasks] 
    ([Id]) ON DELETE CASCADE
);
-- Привязка таблицы учета времени к датам.
CREATE INDEX [IX_TimeRecords_Date] ON [TimeRecords] ([Date]);