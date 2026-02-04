using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GGC.WebService
{
    public class GetStatusDTO
    {
        public string projID { get; set; }
        public string subProjID { get; set; }
        public string status { get; set; }
        public string statusDT { get; set; }
        public string APPID { get; set; }
        public string Contact_Person { get; set; }
        public string Contact_Person_No { get; set; }
        public string Contact_Person_MobNo { get; set; }
        public string Contact_Person_Email { get; set; }
        public string TotalLandRequired { get; set; }
        public string ProjecType { get; set; }
        public string ProjCapacityMW { get; set; }
        public string GenerationVoltage { get; set; }
        public string PointOfInjection { get; set; }
        public string InjectionVoltage { get; set; }
        public string GENERATOR_NAME { get; set; }
        public string TYPE_OF_GENERATION { get; set; }
        public string NAME_OF_SUBSTATION { get; set; }
        public string secKey { get; set; }
        public string entity { get; set; }
        public string userName { get; set; }
        public string statusId { get; set; }
        public string OLD_GC_APPROVED_FLAG { get; set; }
        public string OLD_GC_APPLICATION_DATE { get; set; }
        public string OLD_GC_APPROVED_DATE { get; set; }
        public string validity { get; set; }
        public string FIRST_EXTENSION_DATE { get; set; }
        public string SECOND_EXTENSION_DATE { get; set; }
        public string DISTANCE_FROM_PLANT { get; set; }
        public string VOLT_LEVEL_SUBSTATION { get; set; }
        public int PAYMENT_TYPE_ID { get; set; }
        public string PAYMENT_TYPE { get; set; }
        public string Txn_Date { get; set; }
        public string Txn_No { get; set; }
        public string Txn_Amount { get; set; }

        public string NameTransLicensee { get; set; }
       
        public string LandInPossession { get; set; } = string.Empty;
        public string TotalPrivateLand { get; set; } = string.Empty;
        public string VoltLevelSubstation { get; set; }
        public string SaleOfPower { get; set; }
        public string InterState { get; set; }
        public string TotalRequiredLand { get; set; }
        public string TotalForestLand { get; set; }
        public string StatusForestLand { get; set; }
        public string BirdSanctuaryEtc { get; set; }
        public string PpaPowerToBeInjected { get; set; }
        public string AgreementWithTrader { get; set; } = string.Empty;
        public string StatusFuelLinkage { get; set; }
        public string StatusOfWaterSupply { get; set; }

        public string NAME_TRANS_LICENSEE { get; set; }
        public string POINT_OF_INJECTION { get; set; }
      
        public string GENERATION_VOLTAGE { get; set; } = "0.0"; // default value
        public string LAND_IN_POSSESSION { get; set; } = "";
        public string TOTAL_PRIVATE_LAND { get; set; } = "";

       
        public string INTER_STATE { get; set; }
        public string TOTAL_REQUIRED_LAND { get; set; }

        public string TOTAL_FOREST_LAND { get; set; }
        public string STATUS_FOREST_LAND { get; set; }
        public string BIRD_SANCTURY_ETC { get; set; }

        public string PPA_POWER_TOBE_INJECTED { get; set; }
        public string AGGREEMENT_WITH_TRADER { get; set; } = ""; // default value
        public string STATUS_FUEL_LINKAGE { get; set; }
        public string STATUS_OF_WATER_SUPPLY { get; set; }

        public int WF_STATUS_CD_C { get; set; }

        public string MEDAProjectID { get; set; }
    }

}