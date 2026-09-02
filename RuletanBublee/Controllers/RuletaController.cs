using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuletanBublee.Models;

namespace RuletanBublee.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RuletaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RuletaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("girar")]
        public async Task<IActionResult> Girar()
        {
            var random = new Random();

            // 1. Mapeo de índices exactos según tu array 'slots' en index.html:
            // Índice 0: Bebida Gratis
            // Índice 1: Sigue intentando (Piña Celeste)
            // Índice 2: Descuento 15%
            // Índice 3: Sigue intentando (Piña Naranja)
            // Índice 4: Bebida Gratis
            // Índice 5: Descuento 50%
            int[] indicesPinas = new int[] { 1, 3 };

            var premiosConfig = new[]
            {
                new { Nombre = "Bebida Gratis", Indice = 0 },
                new { Nombre = "Descuento 15%", Indice = 2 },
                new { Nombre = "Bebida Gratis", Indice = 4 },
                new { Nombre = "Descuento 50%", Indice = 5 }
            };

            // 2. Verificamos si quedan giros pendientes en la tanda actual de 6
            var totalDisponibles = await _context.GirosResultados.CountAsync(g => !g.Entregado);

            // Si no hay giros pendientes (se completó un ciclo), generamos el nuevo paquete de 6 giros
            if (totalDisponibles == 0)
            {
                var listaGiros = new List<GiroResultado>();

                // Seleccionamos un premio al azar de la configuración para entregarlo al final
                var premioElegido = premiosConfig[random.Next(premiosConfig.Length)];

                // Creamos 5 giros de "Sigue intentando"
                for (int i = 0; i < 5; i++)
                {
                    listaGiros.Add(new GiroResultado
                    {
                        Premio = "Sigue intentando",
                        IndiceVisual = indicesPinas[random.Next(indicesPinas.Length)],
                        Entregado = false
                    });
                }

                // El giro número 6 será el ganador
                listaGiros.Add(new GiroResultado
                {
                    Premio = premioElegido.Nombre,
                    IndiceVisual = premioElegido.Indice,
                    Entregado = false
                });

                _context.GirosResultados.AddRange(listaGiros);
                await _context.SaveChangesAsync();
            }

            // 3. Obtenemos el SIGUIENTE giro en orden correlativo por Id (garantiza el flujo de 6)
            var siguienteGiro = await _context.GirosResultados
.           Where(g => !g.Entregado)
.           OrderBy(g => g.Id)
.           FirstOrDefaultAsync();

            if (siguienteGiro != null)
            {
                int indiceAEnviar = siguienteGiro.IndiceVisual;

                // Solo si es fallo asignamos aleatoriamente una de las dos piñas
                if (siguienteGiro.Premio == "Sigue intentando")
                {
                    indiceAEnviar = indicesPinas[random.Next(indicesPinas.Length)];
                }

                siguienteGiro.Entregado = true;
                _context.GirosResultados.Update(siguienteGiro);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    premio = siguienteGiro.Premio,
                    indice = indiceAEnviar
                });
            }

            return BadRequest(new { success = false, message = "No hay giros disponibles." });
        }
    }


}