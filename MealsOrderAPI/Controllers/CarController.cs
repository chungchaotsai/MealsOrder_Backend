using Microsoft.AspNetCore.Mvc;

namespace MealsOrderAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllCars()
        {
            var cars = new List<Car>();
            cars.Add(new Car()
            {
                Brand = "BMW",
                Model = "A7"
            });
            cars.Add(new Car()
            {
                Brand = "Benz",
                Model = "B9"
            });

            return Ok(cars);
        }


    }

    public class Car
    {
        public string Brand { get; set; }
        public string Model { get; set; }

    }
}


