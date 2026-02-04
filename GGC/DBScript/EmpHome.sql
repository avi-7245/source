
-- if (rollid <= 10)
-- {
   SELECT  
		*
		, date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT 
	FROM MSKVY_applicantdetails a 
	WHERE 
		IsPaymentDone='Y' 
		AND WF_STATUS_CD_C BETWEEN 6 AND 13 
	ORDER BY a.CREATED_DT DESC;
-- }
-- else
-- {

   SELECT 
		* 
		, date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT 
	FROM MSKVY_applicantdetails a
	WHERE 
		IsPaymentDone='Y' 
		AND ZONE='XYZ' -- Session["EmpZone"]
		AND WF_STATUS_CD_C BETWEEN 6 AND 13 
	ORDER BY a.CREATED_DT DESC;
-- }


SELECT ma.IsPaymentDone,ma.WF_STATUS_CD_C,ma.isAppApproved, ma.* FROM mskvy_applicantdetails ma;