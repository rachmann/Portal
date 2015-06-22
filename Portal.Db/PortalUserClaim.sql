CREATE TABLE [dbo].[PortalUserClaim]
(
	[ClaimId] INT IDENTITY(1,1) NOT NULL, 
    [UserId] INT NOT NULL, 
    [ClaimTypeId] INT NOT NULL, 
    [ClaimValue] VARCHAR(10) NOT NULL, 
    [ClaimValueType] VARCHAR(10) NOT NULL, 
    [Issuer] NVARCHAR(200) NOT NULL, 
	CONSTRAINT [PK_PortalUserClaim] PRIMARY KEY CLUSTERED ([ClaimId] ASC),
	CONSTRAINT [FK_dbo.PortalUserClaim_dbo.PortalUser_Id] FOREIGN KEY ([UserId]) REFERENCES [dbo].[PortalUser] ([Id]) ON DELETE CASCADE
)
