USE lwdb;
GO

PRINT 'fn_CalculateAge';
SELECT TOP (5)
    id, 
    first_name, 
    last_name, 
    birth_date, 
    dbo.fn_CalculateAge(birth_date, GETDATE()) AS age
FROM dbo.students;
GO

PRINT 'AggregateDiagnoses';
SELECT 
    student_id, 
    dbo.AggregateDiagnoses(diagnosis) AS concatenated_diagnoses
FROM dbo.medical_certificates
GROUP BY student_id;
GO

PRINT 'fn_GenerateSeatingAssignments';
SELECT TOP 10 * 
FROM dbo.fn_GenerateSeatingAssignments(1);
GO

PRINT 'sp_IsolateInfectedGroup';
EXEC dbo.sp_IsolateInfectedGroup 
    @studentId = 1, 
    @diagnosis = N'Flu', 
    @quarantineDurationDays = 14, 
    @startDate = '2026-10-01';

SELECT TOP 10 * FROM dbo.absents WHERE status = 'QUARANTINE';
GO

PRINT 'TR_MedicalCertificates_OnInsert';
INSERT INTO dbo.medical_certificates (student_id, diagnosis, issue_date, expire_date)
VALUES (1, N'COVID-19', CAST(GETDATE() AS DATE), DATEADD(day, 14, CAST(GETDATE() AS DATE)));

SELECT TOP 5 * FROM dbo.seatings_assignments WHERE student_id = 1;
GO

PRINT 'SeatCoordinate';
DECLARE @seat dbo.SeatCoordinate;
SET @seat = CAST('2-5-1' AS dbo.SeatCoordinate);
SELECT @seat.ToString() AS seat_str, @seat.Row AS row_idx, @seat.Col AS col_idx, @seat.Seat AS seat_idx;
GO