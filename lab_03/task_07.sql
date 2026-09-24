-- Хранимая процедура с курсором

CREATE OR REPLACE PROCEDURE archive_expired_certificates()
AS $main$
DECLARE
    rec RECORD;
BEGIN
    FOR rec IN 
        SELECT student_id, diagnosis, expire_date 
        FROM medical_certificates 
        WHERE expire_date < CURRENT_DATE
    LOOP
        UPDATE medical_certificates 
        SET diagnosis = rec.diagnosis || ' (истекло)'
        WHERE student_id = rec.student_id AND expire_date = rec.expire_date;
    END LOOP;
END;
$main$
LANGUAGE plpgsql;

CALL archive_expired_certificates();
SELECT * FROM medical_certificates;