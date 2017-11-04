using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using CarShareApi.Models.Repositories;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Tests.Fakes
{
    public class FakeRegistrationRepository : IRegistrationRepository
    {
        public FakeRegistrationRepository()
        {
            if (MemoryCache.Default.Contains("Registrations"))
                Registrations =
                    MemoryCache.Default["Registrations"] as List<Registration>;

            if (Registrations == null)
            {
                Registrations = new List<Registration>
                {
                    new Registration
                    {
                        AccountID = 1,
                        DriversLicenceID = "F1234",
                        DriversLicenceState = "NSW",
                        AddressLine1 = "22 Kent St",
                        AddressLine2 = "",
                        Suburb = "Sydney",
                        DateOfBirth = new DateTime(1970, 1, 1),
                        PhoneNumber = "0290008000",
                        State = "NSW",
                        Postcode = "2000"
                    },
                    new Registration
                    {
                        AccountID = 2,
                        DriversLicenceID = "F1234",
                        DriversLicenceState = "NSW",
                        AddressLine1 = "22 Kent St",
                        AddressLine2 = "",
                        Suburb = "Sydney",
                        DateOfBirth = new DateTime(1970, 1, 1),
                        PhoneNumber = "0290008000",
                        State = "NSW",
                        Postcode = "2000"
                    },
                    new Registration
                    {
                        AccountID = 3,
                        DriversLicenceID = "F1234",
                        DriversLicenceState = "NSW",
                        AddressLine1 = "22 Kent St",
                        AddressLine2 = "",
                        Suburb = "Sydney",
                        DateOfBirth = new DateTime(1970, 1, 1),
                        PhoneNumber = "0290008000",
                        State = "NSW",
                        Postcode = "2000"
                    },
                    new Registration
                    {
                        AccountID = 4,
                        DriversLicenceID = "F1234",
                        DriversLicenceState = "NSW",
                        AddressLine1 = "22 Kent St",
                        AddressLine2 = "",
                        Suburb = "Sydney",
                        DateOfBirth = new DateTime(1970, 1, 1),
                        PhoneNumber = "0290008000",
                        State = "NSW",
                        Postcode = "2000"
                    }
                };
                MemoryCache.Default.Add("Registrations", Registrations,
                    DateTime.Now.AddDays(1));
            }
        }

        public List<Registration> Registrations { get; set; }

        public Registration Add(Registration item)
        {
            Registrations.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            Registrations.RemoveAll(x => x.AccountID == id);
        }

        public Registration Find(int id)
        {
            return Registrations.FirstOrDefault(x => x.AccountID == id);
        }

        public List<Registration> FindAll()
        {
            return Registrations;
        }

        public IQueryable<Registration> Query()
        {
            return Registrations.AsQueryable();
        }


        public Registration Update(Registration item)
        {
            Registrations.RemoveAll(x => x.AccountID == item.AccountID);
            Registrations.Add(item);
            return item;
        }

        public void Dispose()
        {
            //nah
        }
    }
}