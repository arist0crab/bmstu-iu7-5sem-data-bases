-- Рекурсивная хранимая процедура или хранимая процедура с рекурсивным ОТВ

CREATE OR REPLACE PROCEDURE get_subordinate_teachers(
    p_mentor_id INT
)
AS $main$
BEGIN
    CREATE TEMP TABLE temp_subordinates ON COMMIT DROP AS
    WITH RECURSIVE subordinates AS (
        SELECT 
            id, 
            first_name, 
            last_name, 
            1 AS level
        FROM teachers
        WHERE mentor_teacher_id = p_mentor_id

        UNION ALL

        SELECT 
            T.id, 
            T.first_name, 
            T.last_name, 
            S.level + 1
        FROM teachers T
        JOIN subordinates S ON T.mentor_teacher_id = S.id
    )
    SELECT * FROM subordinates;
END;
$main$
LANGUAGE plpgsql;