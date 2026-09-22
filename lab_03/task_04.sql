-- Рекурсивная функция или функция с рекурсивным ОТВ

CREATE OR REPLACE FUNCTION get_teacher_mentorship_chain(
    target_teacher_id INT
)
RETURNS TABLE (
    id INT,
    name TEXT,
    m_id INT,
    m_name TEXT
)
AS $main$
BEGIN
    RETURN QUERY
    WITH RECURSIVE teacher_mentors (teacher_id, teacher_name, mentor_id, mentor_name) AS (
        SELECT 
            T.id,
            (T.first_name || ' ' || T.last_name)::TEXT,
            T.mentor_teacher_id,
            (SELECT M.first_name || ' ' || M.last_name FROM teachers AS M WHERE M.id = T.mentor_teacher_id)::TEXT
        FROM teachers T
        WHERE T.id = target_teacher_id 

        UNION ALL

        SELECT 
            T.id, 
            (T.first_name || ' ' || T.last_name)::TEXT,
            T.mentor_teacher_id, 
            (SELECT (M.first_name || ' ' || M.last_name)::TEXT FROM teachers AS M WHERE M.id = T.mentor_teacher_id)        FROM teachers AS T 
        JOIN teacher_mentors AS TM ON T.id = TM.mentor_id
    )
    SELECT 
        teacher_id, 
        teacher_name, 
        mentor_id, 
        mentor_name 
    FROM teacher_mentors;
END;
$main$
LANGUAGE plpgsql;

SELECT * FROM get_teacher_mentorship_chain(67);