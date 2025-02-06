using CommonAPIBusinessLayer.Services.Impl;
using CommonAPIBusinessLayer.Services.Interface;
using System.ComponentModel;
using CommonAPICommon;
using CommonAPICommon.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CommonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class VehicleLookupController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly SystemConfigurationManager _configuration;
        private IVehicleLookupService _service { get; set; }
        private IRAPA2Service _rapa2Service { get; set; }

        public VehicleLookupController(ILogger<TestController> logger, SystemConfigurationManager config)
        {
            _logger = logger;
            _configuration = config;
        }

        [HttpGet("[controller]/")]   //vehicles?vin=1&98687678
        public async Task<IActionResult> GetVINResults(string vin)
        {
            var rapaheader = Request.Headers.FirstOrDefault(x => x.Key == "RAPAParms");
            string rapaparm = rapaheader.Value.First();//for the log

            if (!rapaheader.Value.Any())
                return BadRequest();

            Dictionary<string, string> RapaParmDict = new Dictionary<string, string>();
            string[] parms = rapaheader.Value.First().Split(';');
            int userapa = Convert.ToInt16(parms[0]);

            foreach (string parm in parms)
            {
                if (parm.Length > 1)
                {
                    if (!parm.Contains("=")) //Looking for ill formed RAPAParms input
                    {
                        return BadRequest();
                    }
                    string[] parmsplit = parm.Split('=');
                    RapaParmDict.Add(parmsplit[0].ToUpper(), parmsplit[1].ToUpper());
                }
            }
            string state = string.Empty;
            string policy = string.Empty;
            string quoteid = string.Empty;

            if (RapaParmDict.ContainsKey("POLICY")) { policy = RapaParmDict["POLICY"]; }
            if (RapaParmDict.ContainsKey("QUOTEID")) { quoteid = RapaParmDict["QUOTEID"]; }
            if (RapaParmDict.ContainsKey("STATE")) { state = RapaParmDict["STATE"]; }

            if (parms.Count() < 2)
                return BadRequest();

            state = parms[1];

            if (vin.Trim().Length < 4)
                return BadRequest();

            VINResultDto vinresults = new VINResultDto();

            switch (userapa)
            {
                case 1:
                    _service = new VehicleLookupRAPAService(state.ToLower()); //_container.Resolve<IVehicleLookupService>("RAPAVINLookup", state.ToLower());
                    vinresults = _service.GetVINResults(vin.ToUpper());
                    break;
                case 2:
                    _rapa2Service = new RAPA2Service();
                    //RTR sends symbolized vins with % in place of & which would break the call.  Verisk needs & for vin searches.
                    vinresults = await _rapa2Service.GetVINResults(vin.ToUpper().Replace("%", "&").Trim(), RapaParmDict, rapaparm);
                    break;
            }
            if (vinresults == null || vinresults.VinItems == null)
            {
                var message = "No Results for VIN:" + vin;
                return NotFound();
            }
            return Ok(vinresults);
        }

        [Route("selected")]
        [HttpGet]
        public async Task<IActionResult> SetSelectedRapa2Vin(int quoteid, string vin, int seq) // Updated Supp X2
        {
            //Updates the Rapa2 audit tables with the selected vehicle when a vin search returns more than one result.
            //It's for the warehouse.
            _rapa2Service = new RAPA2Service();
            _rapa2Service.SetSelectedRapa2Vin(quoteid, vin, seq);
            return Ok();
        }

        [Route("makes")]//    vehicles/makes?modelyear=2015
        [HttpGet]
        public async Task<IActionResult> GetMakes(int modelYear) // Updated Supp X2
        {
            var rapaheader = Request.Headers.FirstOrDefault(x => x.Key == "RAPAParms");

            if (!rapaheader.Value.Any())
                return BadRequest("Invalid Header Parameter");

            string[] parms = rapaheader.Value.First().Split(';');

            bool userapa = Convert.ToBoolean(parms[0]);

            string state = string.Empty;

            if (userapa)
            {
                if (parms.Count() != 2)
                    return BadRequest("Invalid Header Parameter");

                state = parms[1];
            }

            //_service = (bool)userapa ? _container.Resolve<IVehicleLookupService>("RAPAVINLookup", state.ToLower()) : _container.Resolve<IVehicleLookupService>("LegacyVINLookup");
            GetService(userapa, state);

            var makes = _service.GetMakes(modelYear);

            if (makes == null)
            {
                var message = "No Makes for Model Year:" + modelYear;
                return NotFound(message);
            }
            return Ok(makes);
        }

        [Route("models")]//    vehicles/models?modelyear=2015&makeid=5
        [HttpGet]
        public async Task<IActionResult> GetModels(int modelYear, int makeId) // Updated Supp X2
        {
            var rapaheader = Request.Headers.FirstOrDefault(x => x.Key == "RAPAParms");

            if (!rapaheader.Value.Any())
                return BadRequest("Invalid Header Parameter");

            string[] parms = rapaheader.Value.First().Split(';');

            bool userapa = Convert.ToBoolean(parms[0]);
            string state = string.Empty;

            if (userapa)
            {
                if (parms.Count() != 2)
                    return BadRequest("Invalid Header Parameter");

                state = parms[1];
            }

            //_service = (bool)userapa ? _container.Resolve<IVehicleLookupService>("RAPAVINLookup", state.ToLower()) : _container.Resolve<IVehicleLookupService>("LegacyVINLookup");
            GetService(userapa, state);

            var model = _service.GetModels(modelYear, makeId, state);

            if (model == null)
            {
                return NotFound("No Models found.");
            }
            return Ok(model);
        }

        [Route("models/{modelYear}/{make}")]//    vehicles/models?modelyear=2015&make=DODG
        [HttpGet]
        public async Task<IActionResult> GetModels(int modelYear, string make) // Updated Supp X2
        {
            var rapaheader = Request.Headers.FirstOrDefault(x => x.Key == "RAPAParms");

            if (!rapaheader.Value.Any())
                return BadRequest("Invalid Header Parameter");

            string[] parms = rapaheader.Value.First().Split(';');

            bool userapa = Convert.ToBoolean(parms[0]);
            string state = string.Empty;

            if (userapa)
            {
                if (parms.Count() != 2)
                    return BadRequest("Invalid Header Parameter");

                state = parms[1];
            }

            //_service = (bool)userapa ? _container.Resolve<IVehicleLookupService>("RAPAVINLookup", state.ToLower()) : _container.Resolve<IVehicleLookupService>("LegacyVINLookup");
            GetService(userapa, state);

            var model = _service.GetModels(modelYear, 0, state, make);

            if (model == null)
            {
                return NotFound("No Models found.");
            }
            return Ok(model);
        }

        [Route("")]//    vehicles/modelyear=2015&model=CARAVAN&make=DODG
        [HttpGet]
        public async Task<IActionResult> Get(int modelyear, string model, string make) // Updated Supp X1
        {
            var rapaheader = Request.Headers.FirstOrDefault(x => x.Key == "RAPAParms");

            if (!rapaheader.Value.Any())
                return BadRequest("Invalid Header Parameter");

            string[] parms = rapaheader.Value.First().Split(';');

            bool userapa = Convert.ToBoolean(parms[0]);
            string state = string.Empty;

            if (userapa)
            {
                if (parms.Count() != 2)
                    return BadRequest("Invalid Header Parameter");

                state = parms[1];
            }

            //_service = (bool)userapa ? _container.Resolve<IVehicleLookupService>("RAPAVINLookup", state.ToLower()) : _container.Resolve<IVehicleLookupService>("LegacyVINLookup");
            GetService(userapa, state);

            bool found = _service.ValidateVehicle(modelyear, model, make);

            if (!found)
                return NotFound();
            else
                return Ok();
        }

        [Route("matches")]//    vehicles/matches?modelyear=1990&make=DODG&model=CARAVAN
        [HttpGet]
        public async Task<IActionResult> GetMatches(int modelyear, string make, string model) // Updated Supp X2
        {
            var rapaheader = Request.Headers.FirstOrDefault(x => x.Key == "RAPAParms");

            if (!rapaheader.Value.Any())
                return BadRequest("Invalid Header Parameter");

            string[] parms = rapaheader.Value.First().Split(';');

            bool userapa = Convert.ToBoolean(parms[0]);
            string state = string.Empty;

            if (userapa)
            {
                if (parms.Count() != 2)
                    return BadRequest("Invalid Header Parameter");

                state = parms[1];
            }

            //_service = (bool)userapa ? _container.Resolve<IVehicleLookupService>("RAPAVINLookup", state.ToLower()) : _container.Resolve<IVehicleLookupService>("LegacyVINLookup");
            GetService(userapa, state);

            var hits = _service.GetMatchingMakeModels(modelyear, make, model);

            return Ok(hits);
        }

        [Route("makes2")]//    vehicles/makes/5
        [HttpGet]
        public async Task<IActionResult> GetMakeById(int makeId) // Updated Supp X1 - Always = false
        {
            //_service = _container.Resolve<IVehicleLookupService>("LegacyVINLookup");
            GetService(true, "");

            var make = _service.GetMakeById(makeId);

            if (make == null)
            {
                var message = "No Make for Make Id:" + makeId;
                return NotFound(message);
            }
            return Ok(make);
        }

        [Route("models/{vinId}")]//    vehicles/models?vinid=110997
        [HttpGet]
        public async Task<IActionResult> GetModelByVINId(int vinId) // Updated Supp X2
        {
            var rapaheader = Request.Headers.FirstOrDefault(x => x.Key == "RAPAParms");

            if (!rapaheader.Value.Any())
                return BadRequest("Invalid Header Parameter");

            string[] parms = rapaheader.Value.First().Split(';');

            bool userapa = Convert.ToBoolean(parms[0]);
            string state = string.Empty;

            if (userapa)
            {
                if (parms.Count() != 2)
                    return BadRequest("Invalid Header Parameter");

                state = parms[1];
            }

            //_service = (bool)userapa ? _container.Resolve<IVehicleLookupService>("RAPAVINLookup", state.ToLower()) : _container.Resolve<IVehicleLookupService>("LegacyVINLookup");
            GetService(userapa, state);
            var model = _service.GetModelByVINId(vinId, state);

            if (model == null)
            {
                var message = "No Models found for VIN Id " + vinId;
                return NotFound(message);
            }
            return Ok(model);
        }


        private void GetService(bool userapa, string state)
        {
            _service = (bool)userapa ? new VehicleLookupRAPAService(state.ToLower()) : new VehicleLookupService();
        }
    }
}
