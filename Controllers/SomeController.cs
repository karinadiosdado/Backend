using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SomeController : ControllerBase
    {
        [HttpGet("sync")]// todas al mismo tiempo
        public IActionResult GetSync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            Thread.Sleep(1000);
            Console.WriteLine("Conexion a base de datos terminada");

            Thread.Sleep(1000);
            Console.WriteLine("envio de mail terminada");

            Console.WriteLine("Todo finalizado");

            stopwatch.Stop();

            return Ok(stopwatch.Elapsed);
        }

        // diferentes tareas
        [HttpGet("async")]// todas son independientes


        public async Task<IActionResult> GetAsync()
        {
            var task1 = new Task<int>(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Conexion a base de datos terminada");
                return 1;
            });

            var task2 = new Task<int>(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("envio de mail terminada");
                return 2;
            });
            Stopwatch stopwatch = Stopwatch.StartNew();

            stopwatch.Start();

            task1.Start();
            Console.WriteLine("Hago otra cosa");
            var result1 = await task1;
            Console.WriteLine("todo a terminado");
            stopwatch.Stop();
            // return Ok(result1);
            return Ok(stopwatch.Elapsed);

        }
    }
}