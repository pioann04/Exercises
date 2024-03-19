using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeLuau
{
    /// <summary>
    /// Represents a single speaker
    /// </summary>
    public class Speaker
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? YearsExperience { get; set; }
        public bool HasBlog { get; set; }
        public string BlogURL { get; set; }
        public WebBrowser Browser { get; set; }
        public List<string> Certifications { get; set; }
        public string Employer { get; set; }
        public int RegistrationFee { get; set; }
        public List<Session> Sessions { get; set; }

        private List<string> Employers = new List<string>() { "Pluralsight", "Microsoft", "Google" };
        private List<string> Domains = new List<string>() { "aol.com", "prodigy.com", "compuserve.com" };
        private List<string> Technics = new List<string>() { "Cobol", "Punch Cards", "Commodore", "VBScript" };
        private int MaxAlloedYearsExp { get; set; } = 10;

        public RegisterResponse Register(IRepository repository)
        {
            int? speakerId = null;

            var reqResponse = ValidateData();

            if (reqResponse is not null) return new RegisterResponse(reqResponse);

            Sessions.Where(s => !Technics.Any(t => s.Title.Contains(t) || s.Description.Contains(t)))
                .ToList()
                .ForEach(s => s.Approved = true);

            if (Sessions.All(s => s.Approved == false))
            {
                return new RegisterResponse(RegisterError.NoSessionsApproved);
            }

            RegistrationFee = new RegistrationFeeCalculator().CalculateFee(YearsExperience);

            try
            {
                speakerId = repository.SaveSpeaker(this);
            }
            catch (Exception e)
            {
            }

            return new RegisterResponse((int)speakerId);
        }

        private RegisterError? ValidateData()
        {
            if (string.IsNullOrWhiteSpace(FirstName)) return RegisterError.FirstNameRequired;
            if (string.IsNullOrWhiteSpace(LastName)) return RegisterError.LastNameRequired;
            if (string.IsNullOrWhiteSpace(Email)) return RegisterError.EmailRequired;
            if (!SpeakersMeetStandards()) return RegisterError.SpeakerDoesNotMeetStandards;
            if (Sessions.Count() == 0) return RegisterError.NoSessionsProvided;
            return null;
        }

        private bool SpeakersMeetStandards()
        {
            var basicStandards = YearsExperience > MaxAlloedYearsExp || HasBlog || Certifications.Count() > 3 || Employers.Contains(Employer);
            return basicStandards || DomailEmailIsValid();
        }

        private bool DomailEmailIsValid()
        {
            string emailDomain = Email.Split('@').Last();
            var browserSupported = Browser.Name != WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion >= 9;
            return !Domains.Contains(emailDomain) && browserSupported;
        }
    }
}