using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Operational.Forms.PassangerAssistanceForm;
using Ground_Handlng.DataObjects.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    [DisplayName("Operational")]
    public class PassangerAssistanceFormController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PassangerAssistanceForm _passangerAssistanceForm;
        public PassangerAssistanceFormController(ApplicationDbContext context)
        {
            _context = context;
            _passangerAssistanceForm = new PassangerAssistanceForm(_context);
        }
        [DisplayName("Operational")]
        public async Task<ActionResult> Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(_passangerAssistanceForm.GetList().Result.Cast<PassangerAssistanceForm>());
        }

        public async Task<PartialViewResult> Detail(long id)
        {
            if (id != 0)
            {
                _passangerAssistanceForm.Id = id;
                var passangerAssistance = _passangerAssistanceForm.Refresh().Result as PassangerAssistanceForm;
                return PartialView(passangerAssistance);
            }
            return PartialView();
        }
        public PartialViewResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(PassangerAssistanceForm model, IFormCollection formCollection)
        {
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.NOT_OK,
                ErrorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage != "" ? e.ErrorMessage : e.Exception.Message).ToList()
            });
        }
        [HttpGet]
        public PartialViewResult Seed()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Seed(PassangerAssistanceForm model, IFormCollection formCollection)
        {
            List<Ambulance> ambulances = new List<Ambulance>()
            {
                new Ambulance (){ AmbulanceCompanyContact="911",Address="Addis Ababa"},
                new Ambulance (){ AmbulanceCompanyContact="652",Address="Dubai"}
            };
            List<FrequentTravelerMedicalCard> frequentTravelerMedicalCards = new List<FrequentTravelerMedicalCard>()
            {
                new FrequentTravelerMedicalCard (){ FrequentTravelerCardNumber="FREMEC01",IssuedBy="Abebe",ExpireDate = DateTime.Now},
                new FrequentTravelerMedicalCard (){ FrequentTravelerCardNumber="FREMEC02",IssuedBy="Wende",ExpireDate = DateTime.Now},
            };
            List<IntendedEscorts> intendedEscorts = new List<IntendedEscorts>()
            {
                new IntendedEscorts () {Age=24,LanguageSpoken="Amharic",MedicalQualification = false ,PNR="PGERSE",Title="Mr."},
                new IntendedEscorts () {Age=25,LanguageSpoken="English",MedicalQualification = true ,PNR="PGEOSP",Title="Ms."}
            };
            List<MeetAndAssist> meetAndAssists = new List<MeetAndAssist>()
            {
                new MeetAndAssist () { Contact= "0936118197", Name="Wende Buzu"},
                new MeetAndAssist () { Contact= "0926287677", Name="Surafel Fekadu"}
            };
            List<OtherGroundArrangment> otherGroundArrangments = new List<OtherGroundArrangment>()
            {
                new OtherGroundArrangment(){ArrivalAirport="ADD",DepatureAirport="DXB",TransitAirport="DIR",Remark="Test Remark 1"},
                new OtherGroundArrangment(){ArrivalAirport="DXB",DepatureAirport="ADD",TransitAirport="DIR",Remark="Test Remark 1"},
            };
            List<ProposedIternary> proposedIternaries = new List<ProposedIternary>()
            {
                new ProposedIternary () {Airlines="ET",Class="Bussiness",FlightNumber="ET123",FlightDate=DateTime.Now,Destination="ADD",Orgin="DXB" },
                new ProposedIternary () {Airlines="ET",Class="Bussiness",FlightNumber="ET3545",FlightDate=DateTime.Now,Destination="DXB",Orgin="ADD" },
            };
            List<SpecialInflightNeed> specialInflightNeeds = new List<SpecialInflightNeed>()
            {
                new SpecialInflightNeed () { ArrangingCompanyName="Ethiopian Airlines",ArrivalAirport="Dubai International Airport",Equipment=Equipment.Incubator,TypeOfArrangment=TypeOfArrangment.SpecialMeal },
                new SpecialInflightNeed () { ArrangingCompanyName="Ethiopian Airlines",ArrivalAirport="Bole Addis Ababa International Airport",Equipment=Equipment.Respirator,TypeOfArrangment=TypeOfArrangment.Specail_Seat }
            };
            List<WheelChairNeeded> wheelChairNeededs = new List<WheelChairNeeded>()
            {
                new WheelChairNeeded () { CollapsaibleWCOB=false,OwnWheelChair= true,WheelChairCatagory=WheelChairCatagory.WCHC,WheelChairType=WheelChairType.WCBW },
                new WheelChairNeeded () { CollapsaibleWCOB=false,OwnWheelChair= false,WheelChairCatagory=WheelChairCatagory.WCHS,WheelChairType=WheelChairType.WCMP }
            };
            //_context.Ambulance.AddRange(ambulances);
            //_context.FrequentTravelerMedicalCard.AddRange(frequentTravelerMedicalCards);
            //_context.IntendedEscorts.AddRange(intendedEscorts);
            //_context.MeetAndAssist.AddRange(meetAndAssists);
            //_context.OtherGroundArrangment.AddRange(otherGroundArrangments);
            //_context.ProposedIternary.AddRange(proposedIternaries);
            //_context.SpecialInflightNeed.AddRange(specialInflightNeeds);
            //_context.WheelChairNeeded.AddRange(wheelChairNeededs);

                _context.PassangerAssistanceForm.Add(
                    new PassangerAssistanceForm
                    {
                        FullName = "Wende Buzuayew",
                        Title = "MR.",
                        NatureOfDisablity ="Test Disablity",
                        PNR = "KSHUED",
                        StreacherNeeded = true,
                        CreatedBy = "Seed",
                        Ambulance= new Ambulance() { AmbulanceCompanyContact = "911", Address = "Addis Ababa" },
                        IntendedEscorts = new IntendedEscorts() { Age = 24, LanguageSpoken = "Amharic", MedicalQualification = false, PNR = "PGERSE", Title = "Mr." },
                        FrequentTravelerMedicalCard = new FrequentTravelerMedicalCard() { FrequentTravelerCardNumber = "FREMEC01", IssuedBy = "Abebe", ExpireDate = DateTime.Now },
                        MeetAndAssist = new MeetAndAssist() { Contact = "0936118197", Name = "Wende Buzu" },
                        OtherGroundArrangment = new OtherGroundArrangment() { ArrivalAirport = "ADD", DepatureAirport = "DXB", TransitAirport = "DIR", Remark = "Test Remark 1" },
                        ProposedIternary = new ProposedIternary() { Airlines = "ET", Class = "Bussiness", FlightNumber = "ET123", FlightDate = DateTime.Now, Destination = "ADD", Orgin = "DXB" },
                        SpecialInflightNeed = new SpecialInflightNeed() { ArrangingCompanyName = "Ethiopian Airlines", ArrivalAirport = "Dubai International Airport", Equipment = Equipment.Incubator, TypeOfArrangment = TypeOfArrangment.SpecialMeal },
                        WheelChairNeededs = new WheelChairNeeded() { CollapsaibleWCOB = false, OwnWheelChair = true, WheelChairCatagory = WheelChairCatagory.WCHC, WheelChairType = WheelChairType.WCBW },

                    });
            _context.SaveChanges();
            //List<PassangerAssistanceForm> passangerAssistanceForms = new List<PassangerAssistanceForm>()
            //{
            //    new PassangerAssistanceForm() { Title="Mr.", FullName="Abebe Test",PNR="HKSWIA",StreacherNeeded=false,NatureOfDisablity="Test Dis",}
            //};
            return new JsonResult(new DatabaseOperationResponse
            {
                Status = OperationStatus.SUCCESS,
                Message = "Success"
            });
        }
    }
}
