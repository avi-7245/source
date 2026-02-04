
-- 20800100000136
-- PWND20234750472
-- SELECT * FROM 


-- SELECT IsPaymentDone, WF_STATUS_CD_C,a.* FROM mskvy_applicantdetails a;


-- SELECT *, date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT 
-- FROM MSKVY_applicantdetails a 
-- WHERE 
-- 	IsPaymentDone='Y' 
-- 	AND WF_STATUS_CD_C BETWEEN 6 AND 13 
-- ORDER BY a.CREATED_DT DESC;

-- 1 
-- APPLICATION_NO
-- select * FROM mskvy_applicantdetails LIMIT 1;

-- 0 - empDet
-- SELECT * FROM empmaster e WHERE role_id=53 and isactive='Y' order BY 1;

-- UPDATE mskvy_applicantdetails SET IsPaymentDone='Y', WF_STATUS_CD_C ='7' 


-- select * from proposalapproval;

SELECT ap.PROJECT_DISTRICT,ap.* from mskvy_applicantdetails ap ;
SELECT * FROM zone_district WHERE district='THANE';
SELECT z.zone_name FROM zone_circle_district_map z WHERE dist_name = 'THANE' LIMIT 1;

SELECT e.ROLE_ID,e.* FROM empmaster e 
-- WHERE role_id=53 and isactive='Y' order BY 1;