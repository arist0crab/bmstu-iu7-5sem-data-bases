USE lwdb;
GO

BULK INSERT dbo.teachers
FROM '/tmp/teachers.csv'
WITH (
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    KEEPIDENTITY,
    KEEPNULLS
);

BULK INSERT dbo.cabinets
FROM '/tmp/cabinets.csv'
WITH (
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    KEEPIDENTITY,
    KEEPNULLS
);

BULK INSERT dbo.class_groups
FROM '/tmp/class_groups.csv'
WITH (
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    KEEPIDENTITY,
    KEEPNULLS
);

BULK INSERT dbo.desks
FROM '/tmp/desks.csv'
WITH (
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    KEEPIDENTITY,
    KEEPNULLS
);

BULK INSERT dbo.seats
FROM '/tmp/seats.csv'
WITH (
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    KEEPIDENTITY,
    KEEPNULLS
);

BULK INSERT dbo.students
FROM '/tmp/students.csv'
WITH (
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    KEEPIDENTITY,
    KEEPNULLS
);

BULK INSERT dbo.absents
FROM '/tmp/absents.csv'
WITH (
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    KEEPIDENTITY,
    KEEPNULLS
);

BULK INSERT dbo.medical_certificates
FROM '/tmp/medical_certificates.csv'
WITH (
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIELDQUOTE = '"',
    KEEPIDENTITY,
    KEEPNULLS
);

BULK INSERT dbo.seatings_assignments
FROM '/tmp/seatings_assignments.csv'
WITH (
    FIRSTROW = 2,
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    KEEPIDENTITY,
    KEEPNULLS
);
GO