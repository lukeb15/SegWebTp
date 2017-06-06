
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/05/2017 22:34:33
-- Generated from EDMX file: C:\Users\Lucas\Source\Repos\SegWebTp\HacForo\HacForo\Models\HacForo.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [master];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserThread]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ForumThreadSet] DROP CONSTRAINT [FK_UserThread];
GO
IF OBJECT_ID(N'[dbo].[FK_UserComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommentSet] DROP CONSTRAINT [FK_UserComment];
GO
IF OBJECT_ID(N'[dbo].[FK_ThreadComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CommentSet] DROP CONSTRAINT [FK_ThreadComment];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[ForumThreadSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ForumThreadSet];
GO
IF OBJECT_ID(N'[dbo].[CommentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommentSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Password] nvarchar(150)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [UserName] nvarchar(150)  NOT NULL,
    [PasswordSalt] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [ProfilePictureLink] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ForumThreadSet'
CREATE TABLE [dbo].[ForumThreadSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(150)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [UserId] int  NOT NULL,
    [ImageLink] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CommentSet'
CREATE TABLE [dbo].[CommentSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [UserId] int  NOT NULL,
    [ThreadId] int  NOT NULL
);
GO

-- Creating table 'UserThreadPointsSet'
CREATE TABLE [dbo].[UserThreadPointsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Points] int  NOT NULL,
    [UserId] int  NOT NULL,
    [ThreadId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ForumThreadSet'
ALTER TABLE [dbo].[ForumThreadSet]
ADD CONSTRAINT [PK_ForumThreadSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CommentSet'
ALTER TABLE [dbo].[CommentSet]
ADD CONSTRAINT [PK_CommentSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserThreadPointsSet'
ALTER TABLE [dbo].[UserThreadPointsSet]
ADD CONSTRAINT [PK_UserThreadPointsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'ForumThreadSet'
ALTER TABLE [dbo].[ForumThreadSet]
ADD CONSTRAINT [FK_UserThread]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserThread'
CREATE INDEX [IX_FK_UserThread]
ON [dbo].[ForumThreadSet]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'CommentSet'
ALTER TABLE [dbo].[CommentSet]
ADD CONSTRAINT [FK_UserComment]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserComment'
CREATE INDEX [IX_FK_UserComment]
ON [dbo].[CommentSet]
    ([UserId]);
GO

-- Creating foreign key on [ThreadId] in table 'CommentSet'
ALTER TABLE [dbo].[CommentSet]
ADD CONSTRAINT [FK_ThreadComment]
    FOREIGN KEY ([ThreadId])
    REFERENCES [dbo].[ForumThreadSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ThreadComment'
CREATE INDEX [IX_FK_ThreadComment]
ON [dbo].[CommentSet]
    ([ThreadId]);
GO

-- Creating foreign key on [UserId] in table 'UserThreadPointsSet'
ALTER TABLE [dbo].[UserThreadPointsSet]
ADD CONSTRAINT [FK_UserUserThreadPoints]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserThreadPoints'
CREATE INDEX [IX_FK_UserUserThreadPoints]
ON [dbo].[UserThreadPointsSet]
    ([UserId]);
GO

-- Creating foreign key on [ThreadId] in table 'UserThreadPointsSet'
ALTER TABLE [dbo].[UserThreadPointsSet]
ADD CONSTRAINT [FK_ForumThreadUserThreadPoints]
    FOREIGN KEY ([ThreadId])
    REFERENCES [dbo].[ForumThreadSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ForumThreadUserThreadPoints'
CREATE INDEX [IX_FK_ForumThreadUserThreadPoints]
ON [dbo].[UserThreadPointsSet]
    ([ThreadId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------