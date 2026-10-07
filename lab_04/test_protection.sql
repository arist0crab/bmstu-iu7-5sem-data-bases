USE lwdb;
GO

SELECT 
    student_id,
    first_name,
    last_name,
    row_index,
    col_index,
    seat_id
FROM dbo.fn_GetUninfectedStudents(1);
GO