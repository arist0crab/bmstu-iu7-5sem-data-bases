-- Триггер AFTER

CREATE TABLE IF NOT EXISTS seatings_history (
    id SERIAL PRIMARY KEY,
    assignment_id INT,
    student_id INT,
    old_seat_id INT,
    new_seat_id INT,
    action_type VARCHAR(20),
    changed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION log_seating_changes()
RETURNS TRIGGER
AS $main$
BEGIN
    IF (TG_OP = 'UPDATE') THEN
        INSERT INTO seatings_history (assignment_id, student_id, old_seat_id, new_seat_id, action_type)
        VALUES (OLD.id, OLD.student_id, OLD.seat_id, NEW.seat_id, 'UPDATE');
        RETURN NEW;
    ELSIF (TG_OP = 'DELETE') THEN
        INSERT INTO seatings_history (assignment_id, student_id, old_seat_id, new_seat_id, action_type)
        VALUES (OLD.id, OLD.student_id, OLD.seat_id, NULL, 'DELETE');
        RETURN OLD;
    END IF;
    RETURN NULL;
END;
$main$
LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trg_seatings_after_change
AFTER UPDATE OR DELETE ON seatings_assignments
FOR EACH ROW
EXECUTE FUNCTION log_seating_changes();