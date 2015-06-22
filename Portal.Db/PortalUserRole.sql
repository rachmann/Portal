CREATE TABLE [dbo].[PortalUserRole]
(
	[RoleId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    CONSTRAINT [PK_PortalUserRole] PRIMARY KEY CLUSTERED ([RoleId], [UserId]) 
)
