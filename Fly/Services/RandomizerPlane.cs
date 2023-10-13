using Fly.Models.Entities;

namespace Fly.Services
{
    public class RandomizerPlane
    {
        private readonly Random _random;
        private List<string> _listOfOwners;
        private List<string> _listOfTypes;

        public RandomizerPlane()
        {
            _random = new Random();
            _listOfOwners = new List<string>() { "SAS", "RYANAIR", "SOMETHING", "AIRCENTReL", "AirFrance" };
            _listOfTypes = new List<string>() { "BOING", "AIRBUS" };
        }

        public Airplane AirplaneRandom()
        {
            Airplane airplane = new Airplane
            {
                Owner = RandomOwner(),
                Type = RandomType(),
                MaxSeats = _random.Next(1,300),
                MaxVolumeCargo = _random.NextDouble()*100+1,
                MaxWeightCargo = _random.NextDouble()*100+1,
            };
            return airplane;
        }

        public string RandomOwner()
        {
            return _listOfOwners[_random.Next(0, 4)];
        }

        public string RandomType()
        {
            return _listOfTypes[_random.Next(0, 1)];
        }
    }


}
