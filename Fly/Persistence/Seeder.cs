using Fly.Models.Entities;
using Fly.Services;

namespace Fly.Persistence
{
    public static class Seeder
    {
        public static void Seed(this AirplaneDbContext airplaneDb)
        {
            try
            {
                if (!airplaneDb.Airplanes.Any())
                {
                    List<Airplane> airplanes = new List<Airplane>();
                    RandomizerPlane randomizerPlane = new RandomizerPlane();

                    for (int i = 0; i < 10; i++)
                    {
                        airplanes.Add(
                            randomizerPlane.AirplaneRandom()
                            );
                    }

                    airplaneDb.Airplanes.AddRange(airplanes);
                    airplaneDb.SaveChanges();
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
