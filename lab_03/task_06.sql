-- Рекурсивная хранимая процедура или хранимая процедура с рекурсивным ОТВ

CREATE OR REPLACE PROCEDURE get_subordinate_teachers(
    p_mentor_id INT
)
AS $main$
BEGIN
    DROP TABLE IF EXISTS temp_subordinates;
    CREATE TEMP TABLE temp_subordinates AS
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

CALL get_subordinate_teachers(1);
SELECT * FROM temp_subordinates;