-- Подставляемая табличная функция
-- Предоставляет таблицу всех медицинских сертификатов данного студента

CREATE OR REPLACE FUNCTION get_students_medical_certificates(target_id INT)
RETURNS TABLE (
    certificate_id INT,
    student_diagnosis VARCHAR(100),
    certificate_issue_date DATE,
    certificate_expire_date DATE,
    student_recommended_zone INT[]
) AS $main$
BEGIN
    RETURN QUERY
    SELECT id, diagnosis, issue_date, expire_date, recommended_zone FROM medical_certificates WHERE student_id = target_id;
END;
$main$
LANGUAGE plpgsql;

SELECT student_diagnosis FROM get_students_medical_certificates(67); 