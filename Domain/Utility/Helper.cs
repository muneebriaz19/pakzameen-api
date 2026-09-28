using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Utility
{
    public class Helper
    {
        public static string GenerateOTP(int digits)
        {
           
            string otp = string.Empty;

            for (int i = 0; i < digits; i++)
            {
                int index = new Random().Next(0, 10);
                otp += index;
            }

            return otp;
        }
    }
}
