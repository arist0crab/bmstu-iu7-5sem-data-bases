-- Триггер INSTEAD OF

CREATE OR REPLACE VIEW v_student_details AS
SELECT 
    S.id AS student_id,
    S.first_name,
    S.last_name,
    S.sex,
    S.birth_date,
    S.is_active,
    CG.grade,
    CG.letter_id
FROM students S
LEFT JOIN class_groups CG ON S.class_group_id = CG.id;

CREATE OR REPLACE FUNCTION insert_student_via_view()
RETURNS TRIGGER
AS $main$
DECLARE
    v_class_group_id INT;
BEGIN
    SELECT id INTO v_class_group_id
    FROM class_groups
    WHERE grade = NEW.grade AND letter_id = NEW.letter_id
    LIMIT 1;

    IF v_class_group_id IS NULL THEN
        RAISE EXCEPTION 'Класс %-% не найден', NEW.grade, NEW.letter_id;
    END IF;

    INSERT INTO students (first_name, last_name, sex, birth_date, class_group_id, is_active)
    VALUES (NEW.first_name, NEW.last_name, NEW.sex, NEW.birth_date, v_class_group_id, COALESCE(NEW.is_active, TRUE));

    RETURN NEW;
END;
$main$
LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trg_v_student_details_insert
INSTEAD OF INSERT ON v_student_details
FOR EACH ROW
EXECUTE FUNCTION insert_student_via_view();