using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon;
using CommonAPICommon.Dto;
using TriTech;

namespace CommonAPIBusinessLayer.Services.Impl
{
    public class TaxJurisdictionService : ITaxJurisdictionService
    {
        private static readonly log2sql log = new log2sql(nameof(TaxJurisdictionService));

        public JurisdictionResultDto GetTaxJurisdiction(string address, string city, string state, string zip, string effdate, string taxcodeselected)
        {
            var jurisdictiondto = new JurisdictionResultDto
            {
                AddressSuggest = string.Empty,
                CitySuggest = string.Empty,
                StateSuggest = string.Empty,
                ZipSuggest = string.Empty,
                TaxCode = string.Empty,
                TaxCodeAssignedBy = string.Empty,
                TaxCodeSuggest = string.Empty,
                TaxMatch = string.Empty
            };

            try
            {
                var tt = new AgentPortClient(AgentPortClient.EndpointConfiguration.AgentPortSoap12);

                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                var Allocate = new AllocateAddress() { CompanyID = "alfavision", Password = "tfrench" , SourceStreet = address, 
                    SourceCity = city, SourceZipCode = zip, LineOfBusiness = "C", EffectiveDate = effdate};
                var results = tt.AllocateAddress(Allocate).AllocateAddressResult;
                if (results != null)
                {
                    var matchq = results.MatchQuality;
                    var matchcode = matchq.MatchCode;
                    string citycode = results.CityCode.PadLeft(5, '0');
                    string countycode = results.CountyCode.PadLeft(5, '0');

                    //*** if the taxcode is 9998 then make it 99999 for non-taxable
                    if (citycode == "09998")
                        citycode = "99999";
                    if (countycode == "09998")
                        countycode = "99999";

                    switch (matchcode)
                    {
                        //single match
                        case 0:
                            {
                                //is it a perfect match?
                                if (DoAddressesMatch(address, city, state, zip, results))
                                {
                                    jurisdictiondto.TaxMatch = "Perfect Match";

                                    if (citycode == string.Empty)
                                        jurisdictiondto.TaxCode = countycode;
                                    else
                                        jurisdictiondto.TaxCode = citycode;

                                    jurisdictiondto.TaxCodeAssignedBy = "T";
                                }
                                else
                                {
                                    jurisdictiondto.TaxMatch = "Address Suggestion";
                                    if (citycode == string.Empty)
                                        jurisdictiondto.TaxCodeSuggest = countycode;
                                    else
                                        jurisdictiondto.TaxCodeSuggest = citycode;

                                    jurisdictiondto.AddressSuggest = results.MatchedAddress.Street;
                                    jurisdictiondto.CitySuggest = results.MatchedAddress.City;
                                    jurisdictiondto.StateSuggest = results.MatchedAddress.State;
                                    jurisdictiondto.ZipSuggest = results.MatchedAddress.ZipCode;
                                    jurisdictiondto.TaxCodeAssignedBy = string.Empty;
                                }
                                break;
                            }
                        case 1: //multiple matches
                            {
                                jurisdictiondto.TaxMatch = "Address Suggestion";
                                if (citycode == string.Empty)
                                    jurisdictiondto.TaxCodeSuggest = countycode;
                                else
                                    jurisdictiondto.TaxCodeSuggest = citycode;

                                jurisdictiondto.AddressSuggest = results.MatchedAddress.Street;
                                jurisdictiondto.CitySuggest = results.MatchedAddress.City;
                                jurisdictiondto.StateSuggest = results.MatchedAddress.State;
                                jurisdictiondto.ZipSuggest = results.MatchedAddress.ZipCode;
                                jurisdictiondto.TaxCodeAssignedBy = string.Empty;
                                break;
                            }
                        case 2: //no match
                            {
                                jurisdictiondto.TaxMatch = "No Hit";
                                jurisdictiondto.TaxCode = "0";
                                if (taxcodeselected.Length > 0)
                                {
                                    jurisdictiondto.TaxCodeAssignedBy = "M";
                                    jurisdictiondto.TaxMatch = "Perfect Match";
                                }
                                break;
                            }
                    }
                }
                else
                {
                    if (taxcodeselected.Length > 0)
                    {
                        jurisdictiondto.TaxCodeAssignedBy = "M";
                        jurisdictiondto.TaxMatch = "Perfect Match";
                    }
                }
            }
            catch (Exception ex)
            {
                jurisdictiondto.TaxMatch = "No Hit";
                jurisdictiondto.AddressSuggest = jurisdictiondto.CitySuggest = jurisdictiondto.StateSuggest = jurisdictiondto.ZipSuggest =
                jurisdictiondto.TaxCodeSuggest = jurisdictiondto.TaxCodeAssignedBy = string.Empty;
                log.Error("Error connecting to TriTech.", ex);
            }

            return jurisdictiondto;
        }

        internal bool DoAddressesMatch(string address, string city, string state, string zip, CAlloResults results)
        {
            return (address.ToUpper().Trim() == results.MatchedAddress.Street.ToUpper().Trim() &&
              city.ToUpper().Trim() == results.MatchedAddress.City.ToUpper().Trim() &&
              state.ToUpper().Trim() == results.MatchedAddress.State.ToUpper().Trim() &&
              zip.ToUpper().Trim() == results.MatchedAddress.ZipCode.ToUpper().Trim());
        }
    }
}
