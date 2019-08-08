using System.Collections.Generic;

namespace Pieshop.ViewServices
{
    public class ProfileOptionsService
    {
        public List<string> ListCountries()
        {
            // keeping this simple
            return new List<string>() { "Scotland", "England", "Wales", "Ireland", "Northen Ireland" };
        }

    }
}
