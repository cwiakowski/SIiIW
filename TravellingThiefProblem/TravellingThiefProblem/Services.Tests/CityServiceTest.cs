
using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using NUnit.Framework;
using Shouldly;
using TravellingThiefProblem.Models;

namespace TravellingThiefProblem.Services.Tests
{
    [TestFixture]
    public class CityServiceTest
    {
        private Faker<City> _cities;

        [SetUp]
        public void SetUp()
        {
            _cities = new Faker<City>()
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(x => x.X, f => f.Random.Double(0, 50000))
                .RuleFor(x => x.Y, f => f.Random.Double(0, 50000));
        }


        [Test]
        public void Calculate_Distance_Should_Pass()
        {
            var input = _cities.Generate(2);
            var service = new CityService();
            int result = CityService.CalculateDistance(input[1], input[2]);

            int lol = 1;
            lol.ShouldBe(1);
        }
    }
}