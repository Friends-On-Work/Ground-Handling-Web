using System.ComponentModel;

namespace Ground_Handlng.DataObjects.Models.Others
{
    public enum RecordStatus { Active = 1, Inactive, Deleted }
    public enum EmployeeSubgroup { Management = 1, NonManagement }
    public enum Quarter { Q1 = 1, Q2, Q3, Q4 }

    public enum Measure 
    {
        [DisplayName("hour(s)")]
        hours = 1,
        [DisplayName("day(s)")]
        days,
        [DisplayName("month(s)")]
        months,
        [DisplayName("quarter(s)")]
        quarters
    }
    public enum TimelineType
    {
        Duration = 1,
        Interval
    }
    public enum TypeOfArrangment
    {
        SpecialMeal = 1,
        Extra_seat,
        Leg_Rest,
        Specail_Seat
    }
    public enum Equipment
    {
        Respirator = 1,
        Incubator,
        Oxygen
    }

    public enum WheelChairType
    {
        WCBD = 1,
        WCBW,
        WCMP
    }

    public enum WheelChairCatagory
    {
        WCHR =1,
        WCHS,
        WCHC
    }

    public enum PregnancyCertificateType
    {
        Certificate1 = 1,
        Certificate2
    }

}
