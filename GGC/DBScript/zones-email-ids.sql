-- Create msetcl_zone table with primary key
CREATE TABLE msetcl_zone (
    id INT PRIMARY KEY,
    zone_name VARCHAR(255)
);

-- Create corresponding_msedcl_zone table with primary key and foreign key constraint
CREATE TABLE corresponding_msedcl_zone (
    id INT AUTO_INCREMENT PRIMARY KEY,
    zone_name VARCHAR(255),
	zone_email VARCHAR(255),
    msetcl_zone_id INT,
    FOREIGN KEY (msetcl_zone_id) REFERENCES msetcl_zone(id)
);


-- Insert data into msetcl_zone table
INSERT INTO msetcl_zone (id, zone_name) VALUES
(1, 'Karad'),
(2, 'Pune'),
(3, 'Aurangabad'),
(4, 'Nagpur'),
(5, 'Amravati'),
(6, 'Vashi'),
(7, 'Nashik');

-- Insert data into corresponding_msedcl_zone table
INSERT INTO corresponding_msedcl_zone (zone_name, zone_email, msetcl_zone_id) VALUES
('Kolhapur','cekolhapur@mahadiscom.in', 1),
('Ratnagiri','cekokan@mahadiscom.in', 1),
('Pune','cepune@mahadiscom.in', 2),
('Baramati','cebaramati@mahadiscom.in', 2),
('Aurangabad','ceaurangabad@mahadiscom.in', 3),
('Nanded','cenanded@mahadiscom.in', 3),
('Latur','celatur@mahadiscom.in', 3),
('Nagpur','cenagpur@gmail.com', 4),
('Gondia','cegondia@mahadiscom.in', 4),
('Chandrapur','cechandrapur@mahadiscom.in', 4),
('Amravati','ceamravati@mahadiscom.in', 5),
('Akola','ceakola@mahadiscom.in', 5),
('Bhandup','cebhandup@mahadiscom.in', 6),
('Kalyan','cekalyan@mahadiscom.in', 6),
('Vasai','cevashi@mahadiscom.in', 6),
('Nashik','cenashik@mahadiscom.in', 7),
('Jalgaon','cejalgaon@mahadiscom.in', 7);