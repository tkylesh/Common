using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPIDAL.DataAccess;

namespace CommonAPIBusinessLayer.Services
{
    public class PrefillWorker
    {
        public int GetRejectionType(bool _addressChanged, int raterID, bool _vinMerge)
        {
            // rejectionType 4 = "included with different address"
            // rejectionType 5 = "duplicate from comparative rater"
            // rejectionType 6 = "vin merge"
            int rejectionType = 4;

            if (raterID > 0)
            {
                if (_addressChanged.Equals(true))
                {
                    rejectionType = 4;
                }
                if (_addressChanged.Equals(false))
                {
                    rejectionType = 5;
                }
                if (_vinMerge == true)
                {
                    rejectionType = 6;
                }
            }
            return rejectionType;
        }

        public int GetRaterID(int quoteID)
        {
            var prefillDataAccess = new PrefillDataAccess();
            var stagingPolicy = prefillDataAccess.GetStagingPolicy(quoteID);
            var raterID = Convert.ToInt16(stagingPolicy.RaterID);
            return raterID;
        }

        public bool GetAddressChange(int quoteID, int current_isoMasterID)
        {
            var prefillDataAccess = new PrefillDataAccess();
            var _addressChanged = false;
            var isoMaster = prefillDataAccess.GetIsoMaster(quoteID);

            if (isoMaster.Count > 0)
            {
                foreach (var subj in isoMaster)
                {
                    if (subj.ISOMasterID != current_isoMasterID)
                    {
                        _addressChanged = true;
                    }
                }
            }
            return _addressChanged;
        }
    }
}
