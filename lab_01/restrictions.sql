-- primary keys

ALTER TABLE teachers ADD CONSTRAINT pk_teachers PRIMARY KEY (id);
ALTER TABLE students ADD CONSTRAINT pk_students PRIMARY KEY (id);
ALTER TABLE medical_certificates ADD CONSTRAINT pk_medical_certificates PRIMARY KEY (id);
ALTER TABLE absents ADD CONSTRAINT pk_absents PRIMARY KEY (id);
ALTER TABLE class_groups ADD CONSTRAINT pk_class_group PRIMARY KEY (id);
ALTER TABLE cabinets ADD CONSTRAINT pk_cabinets PRIMARY KEY (id);
ALTER TABLE desks ADD CONSTRAINT pk_desks PRIMARY KEY (id);
ALTER TABLE seats ADD CONSTRAINT pk_seats PRIMARY KEY (id);
ALTER TABLE seatings_assignments ADD CONSTRAINT pk_seatings_assignments PRIMARY KEY (id);

-- foreign keys

ALTER TABLE teachers ADD CONSTRAINT fk_mentor_teacher FOREIGN KEY (mentor_teacher_id) REFERENCES teachers(id);

ALTER TABLE students ADD CONSTRAINT fk_class_group_student FOREIGN KEY (class_group_id) REFERENCES class_groups(id);

ALTER TABLE medical_certificates ADD CONSTRAINT fk_student_medical_certificate FOREIGN KEY (student_id) REFERENCES students(id);

ALTER TABLE absents ADD CONSTRAINT fk_student_absent FOREIGN KEY (student_id) REFERENCES students(id);

ALTER TABLE class_groups ADD CONSTRAINT fk_teacher_class_group FOREIGN KEY (teacher_id) REFERENCES teachers(id);
ALTER TABLE class_groups ADD CONSTRAINT fk_cabinet_class_group FOREIGN KEY (cabinet_id) REFERENCES cabinets(id);

ALTER TABLE cabinets ADD CONSTRAINT fk_teacher_cabinet FOREIGN KEY (teacher_id) REFERENCES teachers(id);

ALTER TABLE desks ADD CONSTRAINT fk_cabinet_desk FOREIGN KEY (cabinet_id) REFERENCES cabinets(id);

ALTER TABLE seats ADD CONSTRAINT fk_desk_seat FOREIGN KEY (desk_id) REFERENCES desks(id);

ALTER TABLE seatings_assignments ADD CONSTRAINT fk_student_seating_assignment FOREIGN KEY (student_id) REFERENCES students(id);
ALTER TABLE seatings_assignments ADD CONSTRAINT fk_seat_seating_assignment FOREIGN KEY (seat_id) REFERENCES seats(id);

-- not null

ALTER TABLE teachers ALTER COLUMN first_name SET NOT NULL;
ALTER TABLE teachers ALTER COLUMN last_name SET NOT NULL;
ALTER TABLE teachers ALTER COLUMN birth_date SET NOT NULL;

ALTER TABLE students ALTER COLUMN class_group_id SET NOT NULL;
ALTER TABLE students ALTER COLUMN first_name SET NOT NULL;
ALTER TABLE students ALTER COLUMN last_name SET NOT NULL;
ALTER TABLE students ALTER COLUMN birth_date SET NOT NULL;
ALTER TABLE students ALTER COLUMN sex SET NOT NULL;
ALTER TABLE students ALTER COLUMN is_active SET NOT NULL;

ALTER TABLE medical_certificates ALTER COLUMN student_id SET NOT NULL;
ALTER TABLE medical_certificates ALTER COLUMN diagnosis SET NOT NULL;
ALTER TABLE medical_certificates ALTER COLUMN issue_date SET NOT NULL;

ALTER TABLE absents ALTER COLUMN student_id SET NOT NULL;
ALTER TABLE absents ALTER COLUMN absent_date SET NOT NULL;
ALTER TABLE absents ALTER COLUMN reason SET NOT NULL;
ALTER TABLE absents ALTER COLUMN status SET NOT NULL;

ALTER TABLE class_groups ALTER COLUMN grade SET NOT NULL;
ALTER TABLE class_groups ALTER COLUMN internal_id SET NOT NULL;
ALTER TABLE class_groups ALTER COLUMN letter_id SET NOT NULL;

ALTER TABLE cabinets ALTER COLUMN rows_count SET NOT NULL;
ALTER TABLE cabinets ALTER COLUMN cols_count SET NOT NULL;

ALTER TABLE desks ALTER COLUMN cabinet_id SET NOT NULL;
ALTER TABLE desks ALTER COLUMN row_index SET NOT NULL;
ALTER TABLE desks ALTER COLUMN col_index SET NOT NULL;
ALTER TABLE desks ALTER COLUMN seats_count SET NOT NULL;

ALTER TABLE seats ALTER COLUMN desk_id SET NOT NULL;
ALTER TABLE seats ALTER COLUMN seat_index SET NOT NULL;

ALTER TABLE seatings_assignments ALTER COLUMN student_id SET NOT NULL;
ALTER TABLE seatings_assignments ALTER COLUMN seat_id SET NOT NULL;
ALTER TABLE seatings_assignments ALTER COLUMN start_date SET NOT NULL;

-- check

ALTER TABLE students ADD CONSTRAINT chk_students_sex CHECK (sex IN ('M', 'F'));

ALTER TABLE medical_certificates ADD CONSTRAINT chk_certificates_dates CHECK (expire_date IS NULL OR expire_date >= issue_date);

ALTER TABLE class_groups ADD CONSTRAINT chk_class_group_grade CHECK (grade BETWEEN 1 AND 11);

ALTER TABLE cabinets ADD CONSTRAINT chk_cabinet_rows CHECK (rows_count > 0);
ALTER TABLE cabinets ADD CONSTRAINT chk_cabinet_cols CHECK (cols_count > 0);

ALTER TABLE desks ADD CONSTRAINT chk_desk_row_index CHECK (row_index >= 0);
ALTER TABLE desks ADD CONSTRAINT chk_desk_col_index CHECK (col_index >= 0);
ALTER TABLE desks ADD CONSTRAINT chk_desk_seats_quantity CHECK (seats_count > 0);

ALTER TABLE seats ADD CONSTRAINT chk_seat_index CHECK (seat_index >= 0);

ALTER TABLE seatings_assignments ADD CONSTRAINT chk_seatings_dates CHECK (end_date IS NULL OR end_date >= start_date);

-- default

ALTER TABLE students ALTER COLUMN is_active SET DEFAULT TRUE;