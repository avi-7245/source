using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace GGC.Common
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class ClusterDetails
    //{
    //    public string SPV_name { get; set; }
    //    public string Cluster_ID { get; set; }
    //    public string SVP_mobile_no { get; set; }
    //    public double Cluster_Capacity { get; set; }
    //    public List<Customers> Substation_List { get; set; }
    //    public string SVP_Email_ID { get; set; }
    //    public string District { get; set; }
    //    public string Cluster_Name { get; set; }
    //}
//    public class Type
//{
//    private readonly ICollection<Customer1> _attributes;

//    public Type()
//    {
//        _attributes = new Collection<Customer1>();
//    }

//    public void AddAttributes(IEnumerable<Customer1> attrs)
//    {
//        foreach (Customer1 ta in attrs)
//        {
//            _attributes.Add(ta);
//        }
//    }

//    public string Name { get; set; }
//    public IEnumerable<Customer1> Attributes
//    {
//        get { return _attributes; }

//        set 
//        {
//            foreach (Customer1 ta in value)
//            {
//                _attributes.Add(ta);
//            }
//        }
//    }
//}

    //public class Types
    //{
    //    ICollection<Type> _items;

    //    public Types()
    //    {
    //        _items = new Collection<Type>();
    //    }

    //    public void AddItems(IEnumerable<Type> tps)
    //    {
    //        foreach (Type t in tps)
    //        {
    //            _items.Add(t);
    //        }
    //    }

    //    public IEnumerable<Type> Items
    //    {
    //        get { return _items; }

    //        set
    //        {
    //            foreach (Type t in value)
    //            {
    //                _items.Add(t);
    //            }
    //        }
    //    }
    //}
    //public class Customers
    //{
    //    public List<Customer> customer { get; set; }
    //    //public string customers { get; set; }
    //    //public string custs { get; set; }

    //}
    //public class Customer
    //{
    //    public double landRequired { get; set; }
    //    public string ssNo { get; set; }
    //    public double ssSolarCapacity { get; set; }
    //    public string distName { get; set; }
    //    public string solarCap11 { get; set; }
    //    public string solarCap22 { get; set; }
    //    public string solarCap33 { get; set; }
    //    public string subProjectCode11 { get; set; }
    //    public string subProjectCode22 { get; set; }
    //    public string subProjectCode33 { get; set; }
    //    public string ssName { get; set; }
    //}


    public class GetMSKVYDetails
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        
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
            public List<ClusterDetails> ClusterDetails { get; set; }
            public string response { get; set; }
            public string projectName { get; set; }
            //public List<Customers> customers { get; set; }
            //public List<Customer> customer { get; set; }
        


        
    }
}