using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon.Dto;
using System.Configuration;
using com.qas.proweb;

namespace CommonAPIBusinessLayer.Services.Impl
{
    //Validates the Garaging or Mailing address with the QAS Software.  If the validation
    //returns a verification-level of Verified, the cleaned and formatted address is
    //captured from QAS and the appropriate controls are updated before a database save.

    //FIELD_ROUTE values: current search state, how we arrived at the final page

    public class AddressVerificationService : IAddressVerificationService
    {

        //Common configuration keys (reading from config.web)
        private readonly string KEY_SERVER_URL = "com.qas.proweb.ServerURL";
        private readonly string KEY_LAYOUT = "com.qas.proweb.Layout";

        public AddressDto VerifyAddress(string[] addressToVerify)
        {
            var resultdto = new AddressDto
            {
                AddressValid = false,
                AddressLine1 = string.Empty,
                AddressLine2 = string.Empty,
                AddressLine3 = string.Empty,
                City = string.Empty,
                State = string.Empty,
                Zip = string.Empty
            };

            string dataid = "USA";

            SearchResult searchresult = null;
            string url = ConfigurationManager.AppSettings[KEY_SERVER_URL].ToString();

            var resultaddress = new string[7];
            var inputaddress = new string[7];

            addressToVerify.CopyTo(inputaddress, 0);

            inputaddress = inputaddress.Select(t => t.Replace("#", "")).ToArray();

            var searchservice = new QuickAddress(url);

            CanSearch cansearch;

            var layout = ConfigurationManager.AppSettings[KEY_LAYOUT].ToString();
            searchservice.Engine = QuickAddress.EngineTypes.Verification;
            searchservice.Flatten = true;

            cansearch = searchservice.CanSearch(dataid, layout);
            if (cansearch.IsOk)
            {
                searchresult = searchservice.Search(dataid, inputaddress, PromptSet.Types.Default, layout);
                if (searchresult.Address != null)
                {
                    //we need only the first 6 elements of the return
                    resultaddress = searchresult.Address.AddressLines.Select(a => a.Line).Take(6).ToArray<string>();
                }

                switch (searchresult.VerifyLevel)
                {
                    case SearchResult.VerificationLevels.StreetPartial:
                    case SearchResult.VerificationLevels.PremisesPartial:
                    case SearchResult.VerificationLevels.Multiple:
                        resultdto.QASVerifyLevel = searchresult.VerifyLevel.ToString();
                        if (searchresult.Picklist.Items != null && searchresult.Picklist.Items.Length > 0)
                        {
                            var picklist = new PicklistDto();
                            IList<PicklistItemDto> list = new List<PicklistItemDto>();
                            foreach (var item in searchresult.Picklist.Items)
                            {
                                var plItem = new PicklistItemDto();
                                plItem.PartialAddress = item.PartialAddress;
                                plItem.PostalCode = item.Postcode;
                                plItem.Text = item.Text;
                                list.Add(plItem);
                            }
                            picklist.Items = list;
                            resultdto.PickList = picklist;
                        }
                        break;
                    case SearchResult.VerificationLevels.None:
                        resultdto.QASVerifyLevel = searchresult.VerifyLevel.ToString();
                        break;
                    case SearchResult.VerificationLevels.Verified:
                    case SearchResult.VerificationLevels.InteractionRequired:
                        resultdto.AddressValid = searchresult.VerifyLevel == SearchResult.VerificationLevels.Verified;
                        resultdto.AddressLine1 = resultaddress[0];
                        resultdto.AddressLine2 = resultaddress[1];
                        resultdto.AddressLine3 = resultaddress[2];
                        resultdto.City = resultaddress[3];
                        resultdto.State = resultaddress[4];
                        resultdto.Zip = resultaddress[5];
                        resultdto.QASVerifyLevel = searchresult.VerifyLevel.ToString();
                        break;
                    default:
                        break;
                }

                //if (searchresult.VerifyLevel == SearchResult.VerificationLevels.Verified)
                //{

                //}
                //else
                //{
                //  if (addressToVerify == resultaddress)
                //  {
                //    resultdto.AddressValid = true;
                //    resultdto.AddressLine1 = addressToVerify[0];
                //    resultdto.AddressLine2 = addressToVerify[1];
                //    resultdto.AddressLine3 = addressToVerify[2];
                //    resultdto.City = addressToVerify[3];
                //    resultdto.State = addressToVerify[4];
                //    resultdto.Zip = addressToVerify[5];
                //  }
                //  else
                //    resultdto.AddressValid = false;
                //}
            }
            return resultdto;
        }
    }
}
