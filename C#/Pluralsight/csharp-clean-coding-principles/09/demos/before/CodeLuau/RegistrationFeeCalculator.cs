using System.Collections.Generic;

namespace CodeLuau
{
    public class RegistrationFeeCalculator
    {
        public Dictionary<Limit, int> LimitFees = new Dictionary<Limit, int>
        {
            { new Limit{ Min = null, Max =1 }, 500 },
            {  new Limit{ Min = 2, Max =3 }, 250 },
            {  new Limit{ Min = 4, Max =5 }, 100 },
            {  new Limit{ Min = 6, Max =9 }, 50 }
        };

        public int CalculateFee(int? yearsOfExperience)
        {
            foreach (var limit in LimitFees.Keys)
            {
                if (yearsOfExperience >= limit.Min && yearsOfExperience <= limit.Max)
                {
                    return LimitFees[limit];
                }
            }
            return 0;
        }
    }
}