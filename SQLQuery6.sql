CREATE TABLE dbo.Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL,
    DisplayOrder INT NOT NULL
);