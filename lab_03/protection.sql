-- Реализовать функцию, получающую статистику о классах у которых за последние
-- 3 месяца наибольшее количество действующих медицинских сертификатов
-- Функция возвращает топ-3 класса

CREATE OR REPLACE FUNCTION get_top_3_classes_with_medical_certificates()
RETURNS TABLE (
    class_group_id INT,
    class_group_name TEXT,
    class_group_students_quantity INT,
    class_group_all_medical_sertificates_quantity INT,
    class_group_last_3_month_medical_sertificates_quantity INT
)
AS $main$
BEGIN

    RETURN QUERY
    SELECT 
        CG.id,
        (CG.grade || ' ' || CG.letter_id)::TEXT,
        COUNT(S.id)::INT,
        COUNT(MC.id)::INT,
        COUNT(
            CASE 
                WHEN 
                    MC.expire_date >= CURRENT_DATE 
                    AND MC.issue_date >= CURRENT_DATE - INTERVAL '3 month' 
                THEN MC.id 
            END
        )::INT AS last_3_month_medical_sertificates_quantity
    FROM class_groups AS CG 
    LEFT JOIN students AS S ON CG.id = S.class_group_id
    LEFT JOIN medical_certificates AS MC ON S.id = MC.student_id
    GROUP BY CG.id
    ORDER BY last_3_month_medical_sertificates_quantity DESC
    LIMIT 3;

END;
$main$
LANGUAGE plpgsql;

SELECT * FROM get_top_3_classes_with_medical_certificates();