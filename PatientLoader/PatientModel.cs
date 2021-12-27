using System;

namespace PatientLoader
{
    /// <summary>
    /// Model representing a patient.
    /// </summary>

    public class PatientModel
    {
        /// <summary>
        /// The patient ID.
        /// </summary>

        public int PatientId { get; set; }

        /// <summary>
        /// The patient's first name.
        /// </summary>

        public string FirstName { get; set; }

        /// <summary>
        /// The patient's last name.
        /// </summary>

        public string LastName { get; set; }

        /// <summary>
        /// The patient's gender.
        /// </summary>

        public string Gender { get; set; }

        /// <summary>
        /// The patient's date of birth.
        /// </summary>

        public DateTime? DOB { get; set; }

        /// <summary>
        /// The patient's first address line.
        /// </summary>

        public string Address1 { get; set; }

        /// <summary>
        /// The patient's city.
        /// </summary>

        public string City { get; set; }

        /// <summary>
        /// The patient's state.
        /// </summary>

        public string State { get; set; }

        /// <summary>
        /// The patient's zip or postal code.
        /// </summary>

        public string ZipCode { get; set; }

        /// <summary>
        /// The patient's country.
        /// </summary>

        public string Country { get; set; }

        /// <summary>
        /// The patient's primary phone number.
        /// </summary>

        public string PhonePrimary { get; set; }

        /// <summary>
        /// The patient's email address.
        /// </summary>

        public string Email { get; set; }       // Join Patient.ContactInformationId to ContactInformation.ContactInformationId

        /// <summary>
        /// The date the patient was modified into the system.
        /// </summary>
        public string ReferralCategory { get; set; }

        public string MarketingSource { get; set; }

        /// <summary>
        /// The date the patient was added into the system.
        /// </summary>

        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// The date the patient was added into the system.
        /// </summary>

        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// The status of the patient.

        /// </summary>
        public string Status { get; set; }

    }
}
