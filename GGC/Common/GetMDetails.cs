using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GGC.Common
{
    public class GetMDetails
    {
        public string districtCode { get; set; }
        public string districtName { get; set; }
        public double projectCapacityMw { get; set; }
        public string projectTypeCode { get; set; }
        public string talukaName { get; set; }
        public string hybTable { get; set; }
        public string statusCd { get; set; }
        public string villageName { get; set; }
        public string createdDt { get; set; }
        public string projectHolderCode { get; set; }
        public string projectCode { get; set; }
        public string projectTypeName { get; set; }
        public string createdBy { get; set; }
        public ClusterDetails ClusterDetails { get; set; }
        public string response { get; set; }
        public string projectName { get; set; }
    }
    public class LandDetail
    {
        public string landTalukaName { get; set; }
        public string landSurveyNo { get; set; }
        public string landDistrictName { get; set; }
        public string landSubSurveyNo { get; set; }
        public string landPanchayatName { get; set; }
        public double landArea { get; set; }
        public string landDistanceFromSs { get; set; }
        public string landVillageName { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ClusterDetails
    {
        public string SPV_name { get; set; }
        public string Cluster_ID { get; set; }
        public string SVP_mobile_no { get; set; }
        public double Cluster_Capacity { get; set; }
        public SubstationList Substation_List { get; set; }
        public string SVP_Email_ID { get; set; }
        public string District { get; set; }
        public string Cluster_Name { get; set; }
    }

    public class Customer
    {
        public double landRequired { get; set; }
        public string ssNo { get; set; }
        public double ssSolarCapacity { get; set; }
        public string distName { get; set; }
        public string solarCap11 { get; set; }
        public string solarCap22 { get; set; }
        public string solarCap33 { get; set; }
        public string subProjectCode11 { get; set; }
        public string subProjectCode22 { get; set; }
        public string subProjectCode33 { get; set; }
        public string ssName { get; set; }
        public List<LandDetail> landDetails { get; set; }
    }

    public class Root
    {
        
    }

    public class SubstationList
    {
        public List<Customer> customers { get; set; }
    }


}