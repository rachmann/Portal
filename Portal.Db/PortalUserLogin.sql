CREATE TABLE [dbo].[PortalUserLogin]
(
	[UserId] INT NOT NULL , 
    [LoginProvider] NVARCHAR(500) NOT NULL, 
    [ProviderKey] NVARCHAR(500) NOT NULL, 
    CONSTRAINT [PK_PortalUserLogin] PRIMARY KEY CLUSTERED ([UserId], [ProviderKey], [LoginProvider] ASC) ,
	CONSTRAINT [FK_dbo.PortalUserLogin_dbo.PortalUser_Id] FOREIGN KEY ([UserId]) REFERENCES [dbo].[PortalUser] ([Id]) ON DELETE CASCADE

)
