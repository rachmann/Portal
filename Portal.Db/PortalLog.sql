CREATE TABLE [dbo].[PortalLog]
(
    [Id] INT NOT NULL PRIMARY KEY, 
    [Category] NVARCHAR(50) NOT NULL, 
    [Created] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [Text] NVARCHAR(2000) NOT NULL, 
    [Message] NVARCHAR(2000) NULL, 
    [User] NVARCHAR(50) NULL, 
    [IP] NVARCHAR(20) NULL, 
    [Browser] NVARCHAR(50) NULL, 
    [HttpMethod] NVARCHAR(20) NULL, 
    [ContentType] NVARCHAR(20) NULL, 
    [ContentEncoding] NVARCHAR(20) NULL

)
