-- Хранимая процедура с параметрами

CREATE OR REPLACE PROCEDURE change_class_teacher(
    p_class_id INT,
    p_new_teacher_id INT
)
AS $main$
BEGIN

    UPDATE class_groups
    SET teacher_id = p_new_teacher_id
    WHERE id = p_class_id;

END;
$main$
LANGUAGE plpgsql;

CALL change_class_teacher(52, 67);