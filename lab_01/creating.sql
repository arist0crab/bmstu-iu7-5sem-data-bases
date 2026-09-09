CREATE TABLE "public"."teachers" (
    id SERIAL,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    birth_date DATE,
    mentor_teacher_id INT
);

CREATE TABLE "public"."students" (
    id SERIAL,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    sex CHAR(1),
    birth_date DATE,
    class_group_id INT,
    is_active BOOLEAN
);

CREATE TABLE "public"."medical_certificates" (
    id SERIAL,
    student_id INT,
    diagnosis VARCHAR(100),
    issue_date DATE,
    expire_date DATE,
    recommended_zone INT[]
);

CREATE TABLE "public"."absents" (
    id SERIAL,  
    student_id INT,
    absent_date DATE,
    reason VARCHAR(100),
    status VARCHAR(100)
);

CREATE TABLE "public"."class_groups" (
    id SERIAL,
    grade INT,
    internal_id INT,
    letter_id CHAR(1),
    teacher_id INT,
    cabinet_id INT
);

CREATE TABLE "public"."cabinets" (
    id  SERIAL,
    rows_count INT,
    cols_count INT
);

CREATE TABLE "public"."desks" (
    id SERIAL,
    cabinet_id INT,
    row_index INT,
    col_index INT,
    seats_count INT
);

CREATE TABLE "public"."seats" (
    id SERIAL,
    desk_id INT,
    seat_index INT
);

CREATE TABLE "public"."seatings_assignments" (
    id SERIAL,
    student_id INT,
    seat_id INT,
    start_date DATE,
    end_date DATE
);
