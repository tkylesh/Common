using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIDAL.VisionAppModels
{
    public partial class Staging_Driver
    {
        private string _DOB;
        /// <summary>
        /// Does not exist in the database
        /// </summary>
        public string DOB
        {
            get
            {
                return _DOB;
            }
            set
            {
                _DOB = value;
            }
        }
    }
}
