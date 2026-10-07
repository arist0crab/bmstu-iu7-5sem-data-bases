USE lwdb;
GO

IF EXISTS (SELECT * FROM sys.objects WHERE name = 'fn_GetUninfectedStudents' AND type = 'FT')
    DROP FUNCTION dbo.fn_GetUninfectedStudents;
GO

IF EXISTS (SELECT * FROM sys.assemblies WHERE name = 'ProtectionAssembly')
    DROP ASSEMBLY ProtectionAssembly;
GO

CREATE ASSEMBLY ProtectionAssembly
FROM '/tmp/protection.dll'
WITH PERMISSION_SET = SAFE;
GO

CREATE FUNCTION dbo.fn_GetUninfectedStudents(@studentId INT)
RETURNS TABLE (
    student_id INT,
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    row_index INT,
    col_index INT,
    seat_id INT
)
AS EXTERNAL NAME ProtectionAssembly.UninfectedStudentsDetector.InitMethod;
GO