USE lwdb;
GO

EXEC sp_configure 'show advanced options', 1; 
RECONFIGURE;
GO
EXEC sp_configure 'clr enabled', 1; 
RECONFIGURE;
GO

ALTER DATABASE lwdb SET TRUSTWORTHY ON;
GO

IF EXISTS (SELECT * FROM sys.types WHERE name = 'SeatCoordinate')
    DROP TYPE dbo.SeatCoordinate;
GO
IF EXISTS (SELECT * FROM sys.assemblies WHERE name = 'Clr6Assembly')
    DROP ASSEMBLY Clr6Assembly;
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_MedicalCertificates_OnInsert')
    DROP TRIGGER dbo.TR_MedicalCertificates_OnInsert;
GO
IF EXISTS (SELECT * FROM sys.assemblies WHERE name = 'Clr5Assembly')
    DROP ASSEMBLY Clr5Assembly;
GO

IF EXISTS (SELECT * FROM sys.objects WHERE name = 'sp_IsolateInfectedGroup' AND type = 'PC')
    DROP PROCEDURE dbo.sp_IsolateInfectedGroup;
GO
IF EXISTS (SELECT * FROM sys.assemblies WHERE name = 'Clr4Assembly')
    DROP ASSEMBLY Clr4Assembly;
GO

IF EXISTS (SELECT * FROM sys.objects WHERE name = 'fn_GenerateSeatingAssignments' AND type = 'FT')
    DROP FUNCTION dbo.fn_GenerateSeatingAssignments;
GO
IF EXISTS (SELECT * FROM sys.assemblies WHERE name = 'Clr3Assembly')
    DROP ASSEMBLY Clr3Assembly;
GO

IF EXISTS (SELECT * FROM sys.objects WHERE name = 'AggregateDiagnoses' AND type = 'AF')
    DROP AGGREGATE dbo.AggregateDiagnoses;
GO
IF EXISTS (SELECT * FROM sys.assemblies WHERE name = 'Clr2Assembly')
    DROP ASSEMBLY Clr2Assembly;
GO

IF EXISTS (SELECT * FROM sys.objects WHERE name = 'fn_CalculateAge' AND type = 'FS')
    DROP FUNCTION dbo.fn_CalculateAge;
GO
IF EXISTS (SELECT * FROM sys.assemblies WHERE name = 'Clr1Assembly')
    DROP ASSEMBLY Clr1Assembly;
GO

CREATE ASSEMBLY Clr1Assembly
FROM '/tmp/clr1.dll'
WITH PERMISSION_SET = SAFE;
GO

CREATE FUNCTION dbo.fn_CalculateAge(@birthDate DATETIME, @targetDate DATETIME)
RETURNS INT
AS EXTERNAL NAME Clr1Assembly.AgeCalculation.CalculateAge;
GO

CREATE ASSEMBLY Clr2Assembly
FROM '/tmp/clr2.dll'
WITH PERMISSION_SET = SAFE;
GO

CREATE AGGREGATE dbo.AggregateDiagnoses(@value NVARCHAR(MAX))
RETURNS NVARCHAR(MAX)
EXTERNAL NAME Clr2Assembly.DiagnosesAggregation;
GO

CREATE ASSEMBLY Clr3Assembly
FROM '/tmp/clr3.dll'
WITH PERMISSION_SET = SAFE;
GO

CREATE FUNCTION dbo.fn_GenerateSeatingAssignments(@classGroupId INT)
RETURNS TABLE (
    student_id INT,
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    seat_id INT
)
AS EXTERNAL NAME Clr3Assembly.SeatingAssignmentGenerator.InitMethod;
GO

CREATE ASSEMBLY Clr4Assembly
FROM '/tmp/clr4.dll'
WITH PERMISSION_SET = SAFE;
GO

CREATE PROCEDURE dbo.sp_IsolateInfectedGroup
    @studentId INT,
    @diagnosis NVARCHAR(100),
    @quarantineDurationDays INT,
    @startDate DATETIME = NULL
AS EXTERNAL NAME Clr4Assembly.QuarantineSimulation.IsolateInfectedGroup;
GO

CREATE ASSEMBLY Clr5Assembly
FROM '/tmp/clr5.dll'
WITH PERMISSION_SET = SAFE;
GO

CREATE TRIGGER TR_MedicalCertificates_OnInsert
ON dbo.medical_certificates
FOR INSERT
AS EXTERNAL NAME Clr5Assembly.MedicalCertificateTriggers.OnMedicalCertificateAdded;
GO

CREATE ASSEMBLY Clr6Assembly
FROM '/tmp/clr6.dll'
WITH PERMISSION_SET = SAFE;
GO

CREATE TYPE dbo.SeatCoordinate
EXTERNAL NAME Clr6Assembly.SeatCoordinate;
GO