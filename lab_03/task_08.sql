CREATE OR REPLACE PROCEDURE get_seating_system_tables_info()
AS $main$
BEGIN
    DROP TABLE IF EXISTS temp_seating_metadata;
    CREATE TEMP TABLE temp_seating_metadata AS
    SELECT 
        table_name, 
        column_name, 
        data_type,
        ordinal_position
    FROM information_schema.columns
    WHERE table_schema = 'public' AND table_name IN ('desks', 'seats', 'seatings_assignments')
    ORDER BY table_name, ordinal_position;
END;
$main$
LANGUAGE plpgsql;

CALL get_seating_system_tables_info();
SELECT * FROM temp_seating_metadata;