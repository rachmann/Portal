-- Disable all the constraints in database

USE [aspnet-Portal-20150304012050]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.PortalUserClaim_dbo.PortalUser_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[PortalUserClaim]'))
ALTER TABLE [dbo].[PortalUserClaim] DROP CONSTRAINT [FK_dbo.PortalUserClaim_dbo.PortalUser_Id]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.PortalUserLogin_dbo.PortalUser_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[PortalUserLogin]'))
ALTER TABLE [dbo].[PortalUserLogin] DROP CONSTRAINT [FK_dbo.PortalUserLogin_dbo.PortalUser_Id]
GO

-- truncate the users table
TRUNCATE TABLE [dbo].[PortalUser];
GO

-- Enable all the constraints in database
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.PortalUserLogin_dbo.PortalUser_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[PortalUserLogin]'))
ALTER TABLE [dbo].[PortalUserLogin]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PortalUserLogin_dbo.PortalUser_Id] FOREIGN KEY([UserId])
REFERENCES [dbo].[PortalUser] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.PortalUserLogin_dbo.PortalUser_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[PortalUserLogin]'))
ALTER TABLE [dbo].[PortalUserLogin] CHECK CONSTRAINT [FK_dbo.PortalUserLogin_dbo.PortalUser_Id]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.PortalUserClaim_dbo.PortalUser_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[PortalUserClaim]'))
ALTER TABLE [dbo].[PortalUserClaim]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PortalUserClaim_dbo.PortalUser_Id] FOREIGN KEY([UserId])
REFERENCES [dbo].[PortalUser] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.PortalUserClaim_dbo.PortalUser_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[PortalUserClaim]'))
ALTER TABLE [dbo].[PortalUserClaim] CHECK CONSTRAINT [FK_dbo.PortalUserClaim_dbo.PortalUser_Id]
GO
