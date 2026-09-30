USE lwdb;
GO

IF OBJECT_ID('dbo.seatings_assignments', 'U') IS NOT NULL DROP TABLE dbo.seatings_assignments;
IF OBJECT_ID('dbo.medical_certificate_zones', 'U') IS NOT NULL DROP TABLE dbo.medical_certificate_zones;
IF OBJECT_ID('dbo.medical_certificates', 'U') IS NOT NULL DROP TABLE dbo.medical_certificates;
IF OBJECT_ID('dbo.absents', 'U') IS NOT NULL DROP TABLE dbo.absents;
IF OBJECT_ID('dbo.students', 'U') IS NOT NULL DROP TABLE dbo.students;
IF OBJECT_ID('dbo.class_groups', 'U') IS NOT NULL DROP TABLE dbo.class_groups;
IF OBJECT_ID('dbo.teachers', 'U') IS NOT NULL DROP TABLE dbo.teachers;
IF OBJECT_ID('dbo.seats', 'U') IS NOT NULL DROP TABLE dbo.seats;
IF OBJECT_ID('dbo.desks', 'U') IS NOT NULL DROP TABLE dbo.desks;
IF OBJECT_ID('dbo.cabinets', 'U') IS NOT NULL DROP TABLE dbo.cabinets;
GO

CREATE TABLE dbo.teachers (
    id INT IDENTITY(1,1),
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    birth_date DATE,
    mentor_teacher_id INT
);

CREATE TABLE dbo.students (
    id INT IDENTITY(1,1),
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    sex CHAR(1),
    birth_date DATE,
    class_group_id INT,
    is_active BIT
);

CREATE TABLE dbo.medical_certificates (
    id INT IDENTITY(1,1),
    student_id INT,
    diagnosis NVARCHAR(100),
    issue_date DATE,
    expire_date DATE
);

CREATE TABLE dbo.medical_certificate_zones (
    id INT IDENTITY(1,1),
    medical_certificate_id INT,
    zone INT
);

CREATE TABLE dbo.absents (
    id INT IDENTITY(1,1),
    student_id INT,
    absent_date DATE,
    reason NVARCHAR(100),
    status NVARCHAR(100)
);

CREATE TABLE dbo.class_groups (
    id INT IDENTITY(1,1),
    grade INT,
    internal_id INT,
    letter_id CHAR(1),
    teacher_id INT,
    cabinet_id INT
);

CREATE TABLE dbo.cabinets (
    id INT IDENTITY(1,1),
    number INT,
    rows_count INT,
    cols_count INT
);

CREATE TABLE dbo.desks (
    id INT IDENTITY(1,1),
    cabinet_id INT,
    row_index INT,
    col_index INT,
    seats_count INT
);

CREATE TABLE dbo.seats (
    id INT IDENTITY(1,1),
    desk_id INT,
    seat_index INT
);

CREATE TABLE dbo.seatings_assignments (
    id INT IDENTITY(1,1),
    student_id INT,
    seat_id INT,
    start_date DATE,
    end_date DATE
);
GO