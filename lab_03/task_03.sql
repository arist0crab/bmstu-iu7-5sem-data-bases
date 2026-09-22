-- Многооператорная табличная функция

CREATE OR REPLACE FUNCTION get_class_health_and_attendance_report(
    target_class_group_id INT
)
RETURNS TABLE (
    student_full_name TEXT,           -- полное имя и фамилия
    total_absences INT,               -- общее кол-во пропусков
    unexcused_absenses INT,           -- кол-во пропусков по неуважительной причине
    risk_category TEXT                -- риск отчисления
)
AS $main$
BEGIN

    DROP TABLE IF EXISTS result;
    CREATE TEMP TABLE IF NOT EXISTS result(
        student_id INT,
        full_name TEXT,
        absences_count INT,
        unexcused_count INT,
        risk TEXT
    );

    INSERT INTO result (student_id, full_name, absences_count, unexcused_count)
    SELECT 
        S.id

        , (S.first_name || ' ' || S.last_name)

        , (
            SELECT COUNT(*)::INT
            FROM absents AS A
            WHERE S.id = A.student_id
        )

        , (
            SELECT COUNT(*)::INT 
            FROM absents AS A
            WHERE S.id = A.student_id AND A.reason <> 'Уважительная'
        )
    FROM students AS S WHERE S.class_group_id = target_class_group_id;

    UPDATE result 
    SET risk = CASE 
        WHEN absences_count = 0 THEN 'Отсутствует'
        WHEN unexcused_count <= 3 THEN 'Низкий'
        WHEN unexcused_count <= 7 THEN 'Средний'
        ELSE 'Высокий'
    END;

    RETURN QUERY
    SELECT 
        full_name,
        absences_count,
        unexcused_count,
        risk 
    FROM result;

END;
$main$
LANGUAGE plpgsql;


SELECT * FROM get_class_health_and_attendance_report(66);
