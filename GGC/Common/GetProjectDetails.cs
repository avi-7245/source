using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GGC.Common
{
    //public class ClusterDetails
    //{
    //    public string SPV_name { get; set; }
    //    public string Cluster_ID { get; set; }
        
    //    public string SVP_mobile_no { get; set; }
    //    public string SVP_Email_ID { get; set; }
    //    public string Cluster_Capacity { get; set; }
    //    //public string districtCode { get; set; }
    //    public string Cluster_Name { get; set; }
    //    public List<Substation_List> substation_List { get; set; }
        
        
    //}

    //public class Customers
    //{
    //    public List<Customer> customer { get; set; }
       
        
    //}
    //public class Customer
    //{

    //    public string landRequired { get; set; }
    //    public string ssNo { get; set; }
    //    public string ssName { get; set; }
    //    public string ssSolarCapacity { get; set; }
    //    public string subProjectCode33 { get; set; }

    //}
    //public class Substation_List
    //{

        
    //    public List<Customers> customers { get; set; }

    //}
    public class GetProjectDetails
    {
        public string generatorID { get; set; }
        public string districtCode { get; set; }
        public string talukaCode { get; set; }
        public string villageCensusCode { get; set; }
        public string districtName { get; set; }
        public string projectCapacityMw { get; set; }
        public string gutNo { get; set; }
        public string projectTypeCode { get; set; }
        public string talukaName { get; set; }
        public string statusCd { get; set; }
        public string villageName { get; set; }
        public string createdDt { get; set; }
        public string projectHolderCode { get; set; }
        public string projectCode { get; set; }
        public string projectTypeName { get; set; }
        public string createdBy { get; set; }
        public string response { get; set; }


        

        public List<ClusterDetails> clusterDetails { get; set; }
    }
}