DELETE FROM dbo.Errors
DBCC CHECKIDENT ('dbo.Errors',RESEED, 0)

DELETE FROM dbo.ApplicationUsers
DBCC CHECKIDENT ('dbo.ApplicationUsers',RESEED, 0)