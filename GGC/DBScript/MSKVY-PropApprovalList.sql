SELECT 
	DISTINCT a.APPLICATION_NO
	, a.MEDAProjectID 
	, a.PROMOTOR_NAME
	, a.PROJECT_LOC
	, a.NATURE_OF_APP
	, a.PROJECT_TYPE
	, a.PROJECT_CAPACITY_MW
	, a.app_status
	, a.APP_STATUS_DT
	, concat(a.PROJECT_LOC,' ', a.PROJECT_TALUKA,' ', a.PROJECT_DISTRICT) as Location,Quantum_power_injected_MW 
FROM MSKVY_applicantdetails  a, proposalapproval b 
WHERE 
	a.APPLICATION_NO=b.APPLICATION_NO 
	and b.roleid='53' -- logged in emp 
	and WF_STATUS_CD_C < 15;
	
SELECT ma.WF_STATUS_CD_C,ma.* FROM 	mskvy_applicantdetails ma;
SELECT * FROM 	proposalapproval pa;

-- application no : 8820700088
-- 11776 / Test@123
-- DeemGC19_Feb_2410_38_18.pdf

-- UPDATE mskvy_upload_doc_spd 
-- SET 
-- 	FieName='DeemGC19_Feb_2410_38_18.pdf' -- filename
-- WH RE 
-- 	Application_ No='8820700088' -- seleccted app
-- 	and FileType=3

-- Save File location
-- GGC\Files\MSKVY\8820700088

SELECT * from mskvy_upload_doc_spd a ORDER BY a.CreateDT DESC
-- strQuery = "insert into proposalapprovaltxn(APPLICATION_NO , isAppr_Rej_Ret,remark,Aprove_Reject_Return_by, roleid , createDT ,createBy) values ('" + strAppID + "','Y','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "', '" + rollid + "', now() , '" + Session["SAPID"].ToString() + "')";