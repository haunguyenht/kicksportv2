using System;
using System.Collections.Generic;
using System.Text;

namespace KickSport.Data.Common.Constant
{
    public static class Constants
    {
        public struct VALIDATION
        {
            //Categories Input and Ingredient Input
            public const int NameMinimumLength = 3;
            public const int NameMaximumLength = 20;
            public const string NameErrorMessage = "Name should be at least 3 characters long and not more than 20.";

            //Product Input
            public const int DescriptionMinimumLength = 10;
            public const int DescriptionMaximumLength = 220;
            public const int ImageMinimumLength = 14;
            public const int MinimumWeight = 250;
            public const int MaximumWeight = 800;
            public const double MinimumPrice = 0.1;
            public const string DescriptionErrorMessage = "Description should be at least 10 characters long and not more than 220.";
            public const string ImageErrorMessage = "Image URL should be at least 14 characters long.";
            public const string ImageRegex = @"^(http:|https:)\/\/.+";
            public const string ImageRegexErrorMessage = "Image URL should be valid.";
            public const string WeightErrorMessage = "Weight should be between 250 and 800 grams";
            public const string PriceErrorMessage = "Price should be a positive number.";

            //Register Input 
            public const int UsernameMinimumLength = 4;
            public const int PasswordMinimumLength = 8;
            public const string EmailErrorMessage = "Please enter valid e-mail address.";
            public const string UsernameRegex = "^[^@]*$";
            public const string UsernameRegexErrorMessage = "Username should not contain @ symbol.";
            public const string UsernameErrorMessage = "Username should be at least 4 symbols long.";
            public const string PasswordErrorMessage = "Password should be at least 8 symbols long.";

            //OrderProduct Input
            public const int MinimumQuantity = 1;
            public const string QuantityErrorMessage = "Quantity should be a positive number.";

            //Review Input
            public const int ReviewMinimumLength = 4;
            public const string ReviewErrorMessage = "Review Text should be at least 4 characters long.";
        };
    }
}
