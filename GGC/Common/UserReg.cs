using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GGC.Common
{
    public class UserReg
    {
        public string generatorName { get; set; }
        public string contactPersonName { get; set; }
        public string generatorPan { get; set; }
        public string generatorPinCode { get; set; }
        public string dtOfBirth { get; set; }
        public string generatorId { get; set; }
        public string companyAddressLandMark { get; set; }
        public string companyRegistrationNo { get; set; }
        public string generatorUserName { get; set; }
        public string applicantType { get; set; }
        public string generatorEmailId { get; set; }
        public string filUploadFlagYn { get; set; }
        public string generatorCode { get; set; }
        public string generatorPassword { get; set; }
        public string appliedForBaggase { get; set; }
        public string generatorCity { get; set; }
        public string fileIdSeq { get; set; }
        public string statusCd { get; set; }
        public string generatorAddress { get; set; }
        public string createdDt { get; set; }
        //public string contactPersonName { get; set; }
        public string appliedForSolar { get; set; }
        public string createdBy { get; set; }
        public string response { get; set; }
        public string companyPhoneNo { get; set; }
        public string generatorGst { get; set; }
        public string generatorMobileNo { get; set; }
        public SpvDetails SpvDetails { get; set; }
    }
    public class SpvDetails
    {
        public string generatorName { get; set; }
        public string generatorPan { get; set; }
        public string generatorPassword { get; set; }
        public string appliedForOtherDetails { get; set; }
        public string gender { get; set; }
        public string generatorCity { get; set; }
        public string generatorPinCode { get; set; }
        public string dtOfBirth { get; set; }
        public string statusCd { get; set; }
        public string generatorAddress { get; set; }
        public int generatorId { get; set; }
        public string createdDt { get; set; }
        public string companyAddressLandMark { get; set; }
        public string contactPersonName { get; set; }
        public string appliedForSolar { get; set; }
        public string companyRegistrationNo { get; set; }
        public string generatorUserName { get; set; }
        public string applicantType { get; set; }
        public string createdBy { get; set; }
        public string generatorEmailId { get; set; }
        public string companyPhoneNo { get; set; }
        public string generatorCode { get; set; }
        public string generatorGst { get; set; }
        public string generatorMobileNo { get; set; }
    }

}