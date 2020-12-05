using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace AoC_2020
{
    class PassportInfo
    {
        private string _inputString;

        #region Properties

        public ValidatableString BirthYear { get; set; } = new ValidatableNumberString(1920, 2002);
        public ValidatableString IssueYear { get; set; } = new ValidatableNumberString(2010, 2020);
        public ValidatableString ExpirationYear { get; set; } = new ValidatableNumberString(2020, 2030);
        public ValidatableString Height { get; set; } = new ValidatableHeightString();
        public ValidatableString HairColor { get; set; } = new ValidatableRegexString(@"#[0-9a-f]{6}");
        public ValidatableString EyeColor { get; set; } = new ValidatableCategoryString("amb", "blu", "brn", "gry", "grn", "hzl", "oth");
        public ValidatableString PassportId { get; set; } = new ValidatableRegexString(@"^\d{9}$");
        public ValidatableString CountryId { get; set; } = new ValidatableString();

        #endregion

        public bool IsValid
        {
            get
            {
                bool result = !string.IsNullOrEmpty(BirthYear.Value) &&
                      !string.IsNullOrEmpty(IssueYear.Value) &&
                      !string.IsNullOrEmpty(ExpirationYear.Value) &&
                      !string.IsNullOrEmpty(Height.Value) &&
                      !string.IsNullOrEmpty(HairColor.Value) &&
                      !string.IsNullOrEmpty(EyeColor.Value) &&
                      !string.IsNullOrEmpty(PassportId.Value);
                return result;
            }
        }

        public bool IsValid2
        {
            get
            {
                bool result = BirthYear.IsValid &&
                              IssueYear.IsValid &&
                              ExpirationYear.IsValid &&
                              Height.IsValid &&
                              HairColor.IsValid &&
                              EyeColor.IsValid &&
                              PassportId.IsValid;
                return result;
            }
        }

        public PassportInfo(string input)
        {
            _inputString = input;
            Regex passportFieldRegex = new Regex(@"(\w{3}):(.+?)(?:\s|$)");
            var matches = passportFieldRegex.Matches(input);
            foreach (Match match in matches)
            {
                string key = match.Groups[1].Value;
                string value = match.Groups[2].Value;
                switch (key)
                {
                    case "byr":
                        BirthYear.Value = value;
                        break;
                    case "iyr":
                        IssueYear.Value = value;
                        break;
                    case "eyr":
                        ExpirationYear.Value = value;
                        break;
                    case "hgt":
                        Height.Value = value;
                        break;
                    case "hcl":
                        HairColor.Value = value;
                        break;
                    case "ecl":
                        EyeColor.Value = value;
                        break;
                    case "pid":
                        PassportId.Value = value;
                        break;
                    case "cid":
                        CountryId.Value = value;
                        break;
                }
            }
        }
        public override string ToString()
        {
            return (
                   (BirthYear.IsValid ? $"byr: {BirthYear.Value} : {BirthYear.IsValid}\n" : "") +
                   (IssueYear.IsValid ? $"iyr: {IssueYear.Value} : {IssueYear.IsValid}\n" : "") +
                   (ExpirationYear.IsValid ? $"eyr: {ExpirationYear.Value} : {ExpirationYear.IsValid}\n" : "") +
                   (Height.IsValid ? $"hgt: {Height.Value} : {Height.IsValid}\n" : "") +
                   (HairColor.IsValid ? $"hcl: {HairColor.Value} : {HairColor.IsValid}\n" : "") +
                   (EyeColor.IsValid ? $"ecl: {EyeColor.Value} : {EyeColor.IsValid}\n" : "") +
                   (PassportId.IsValid ? $"pid: {PassportId.Value} : {PassportId.IsValid}\n" : "") +
                   (CountryId.IsValid ? $"cid: {CountryId.Value} : {CountryId.IsValid}\n" : "")
                   );
        }
    }

    public class ValidatableString
    {
        public virtual bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Value);
            }
        }
        public string Value { get; set; }
    }

    /// <summary>
    /// A string that is validated against a numeric range
    /// </summary>
    public class ValidatableNumberString : ValidatableString
    {
        public override bool IsValid
        {
            get
            {
                if (base.IsValid)
                {
                    if (int.TryParse(Value, out int val))
                    {
                        return MinValue <= val && val <= MaxValue;
                    }
                }
                return false;
            }
        }

        public int MinValue { get; }
        public int MaxValue { get; }

        public ValidatableNumberString(int min, int max)
        {
            MinValue = min;
            MaxValue = max;
        }
    }

    /// <summary>
    /// A string that is validated against a regular expression
    /// </summary>
    public class ValidatableRegexString : ValidatableString
    {
        public override bool IsValid
        {
            get
            {
                if (base.IsValid)
                {
                    bool result = Pattern.IsMatch(Value);
                    return result;
                }
                return false;
            }
        }
        public Regex Pattern { get; }

        public ValidatableRegexString(string pattern)
        {
            Pattern = new Regex(pattern);
        }
    }

    /// <summary>
    /// A string that is validated against a list of categories
    /// </summary>
    public class ValidatableCategoryString : ValidatableString
    {
        private string[] _categoryValues;

        public override bool IsValid
        {
            get
            {
                if (base.IsValid)
                {
                    return _categoryValues.Any(v => v == Value);
                }
                return false;
            }
        }

        public ValidatableCategoryString(params string[] categories)
        {
            _categoryValues = categories;
        }
    }

    /// <summary>
    /// A string that is validated against the ranges for height in inches and centimeters
    /// </summary>
    public class ValidatableHeightString : ValidatableString
    {

        private Regex heightRegex = new Regex(@"(\d+)(in|cm)");
        public override bool IsValid
        {
            get
            {
                if (base.IsValid)
                {
                    var match = heightRegex.Match(Value);
                    if (match.Success)
                    {
                        int height = int.Parse(match.Groups[1].Value);
                        string units = match.Groups[2].Value;
                        if (units == "in")
                        {
                            return 59 <= height && height <= 76;
                        }
                        else if (units == "cm")
                        {
                            return 150 <= height && height <= 193;
                        }
                    }
                }
                return false;
            }
        }
    }
}
