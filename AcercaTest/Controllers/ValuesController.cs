using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using AcercaTest.Helper;
using AcercaTest.Models;
using AcercaTest.Service;
using Newtonsoft.Json.Linq;


namespace AcercaTest.Controllers
{
    /// <summary>
    /// Api para obtener y modificar un listado de vehiculos.
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]    
    [RoutePrefix("api/Values")]    
    public class ValuesController : ApiController
    {
        // GET api/values

        /// <summary>
        /// Llamada para obtener un listado de todos los vechiculos
        /// </summary>
        /// <returns>Json con listdo de VehicleModel</returns>
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {

            
                //Obtenemos todos los registros
                using(VehicleService Service = new VehicleService())
                {
                    List<VehicleModel> ListaVehiculos = await Service.Get();
                    ListaVehiculos = ListaVehiculos.OrderBy(x => x.NumeroPedido).ToList();

                    return Json(ListaVehiculos);

                }
            }
            catch (Exception ex)
            {

                return new HttpActionResult(HttpStatusCode.InternalServerError, ex.Message);
            }


        }

        
        /// <summary>
        /// Llamada para obtener un listado de vehiculos, ordenado, paginado y con busqueda.
        /// </summary>
        /// <param name="orden">Orden del listado, por defecto por Numero de pedido </param>
        /// <param name="tipo_orden">tipo de orden, por defecto Ascendente (ASC/DESC)</param>
        /// <param name="pagina">Numero de pagina, por defecto 1</param>
        /// <param name="registros_por_pagina">Registros por pagina, por defecto 10</param>
        /// <param name="buscar">Texto a buscar en Matricula, Modelo o Bastidor, por defecto NULL</param>
        /// <returns>Json con listado de vehiculos ordenado, paginado y con busqueda</returns>
        [Route("{orden?}/{tipo_orden?}/{pagina?}/{registros_por_pagina?}/{buscar?}")]
        public async Task<IHttpActionResult> GetPaged(string orden = "NumeroPedido",
                                                    string tipo_orden = "ASC",
                                                    int pagina = 1,
                                                    int registros_por_pagina = 10,
                                                    string buscar = null)
        {
            try
            {
                //Obtenemos todos los registros
                using (VehicleService Service = new VehicleService())
                {
                    List<VehicleModel> Vehiculos = await Service.Get();
                    PaginadorGenerico<VehicleModel> PaginadorVehiculos;

                    //Buscamos si pertoca
                    if (!string.IsNullOrEmpty(buscar))
                    {
                        foreach (var item in buscar.Split(new char[] { ' ' },
                                 StringSplitOptions.RemoveEmptyEntries))
                        {
                            Vehiculos = Vehiculos.Where(x => x.Matricula.ToUpper().Contains(item.ToUpper()) ||
                                                          x.Modelo.ToUpper().Contains(item.ToUpper()) ||
                                                          x.Bastidor.ToUpper().Contains(item.ToUpper())).ToList();

                        }
                    }

                    //Ordenamos
                    switch (orden)
                    {
                        case "Id":
                            if (tipo_orden.ToLower() == "desc")
                                Vehiculos = Vehiculos.OrderByDescending(x => x.Id).ToList();
                            else if (tipo_orden.ToLower() == "asc")
                                Vehiculos = Vehiculos.OrderBy(x => x.Id).ToList();
                            break;
                        case "NumeroPedido":
                            if (tipo_orden.ToLower() == "desc")
                                Vehiculos = Vehiculos.OrderByDescending(x => x.NumeroPedido).ToList();
                            else if (tipo_orden.ToLower() == "asc")
                                Vehiculos = Vehiculos.OrderBy(x => x.NumeroPedido).ToList();
                            break;
                        case "Bastidor":
                            if (tipo_orden.ToLower() == "desc")
                                Vehiculos = Vehiculos.OrderByDescending(x => x.Bastidor).ToList();
                            else if (tipo_orden.ToLower() == "asc")
                                Vehiculos = Vehiculos.OrderBy(x => x.Bastidor).ToList();
                            break;
                        case "Modelo":
                            if (tipo_orden.ToLower() == "desc")
                                Vehiculos = Vehiculos.OrderByDescending(x => x.Modelo).ToList();
                            else if (tipo_orden.ToLower() == "asc")
                                Vehiculos = Vehiculos.OrderBy(x => x.Modelo).ToList();
                            break;
                        case "Matricula":
                            if (tipo_orden.ToLower() == "desc")
                                Vehiculos = Vehiculos.OrderByDescending(x => x.Matricula).ToList();
                            else if (tipo_orden.ToLower() == "asc")
                                Vehiculos = Vehiculos.OrderBy(x => x.Matricula).ToList();
                            break;
                        case "FechaEntrega":
                            if (tipo_orden.ToLower() == "desc")
                                Vehiculos = Vehiculos.OrderByDescending(x => x.FechaEntrega).ToList();
                            else if (tipo_orden.ToLower() == "asc")
                                Vehiculos = Vehiculos.OrderBy(x => x.FechaEntrega).ToList();
                            break;

                        default:
                            if (tipo_orden.ToLower() == "desc")
                                Vehiculos = Vehiculos.OrderByDescending(x => x.Id).ToList();
                            else if (tipo_orden.ToLower() == "asc")
                                Vehiculos = Vehiculos.OrderBy(x => x.Id).ToList();
                            break;
                    }


                    //Paginamos
                    int _TotalRegistros = 0;
                    int _TotalPaginas = 0;

                    _TotalRegistros = Vehiculos.Count();

                    Vehiculos = Vehiculos.Skip((pagina - 1) * registros_por_pagina)
                                                     .Take(registros_por_pagina)
                                                     .ToList();

                    _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / registros_por_pagina);


                    PaginadorVehiculos = new PaginadorGenerico<VehicleModel>()
                    {
                        RegistrosPorPagina = registros_por_pagina,
                        TotalRegistros = _TotalRegistros,
                        TotalPaginas = _TotalPaginas,
                        PaginaActual = pagina,
                        BusquedaActual = buscar,
                        OrdenActual = orden,
                        TipoOrdenActual = tipo_orden,
                        Resultado = Vehiculos
                    };

                    return Json(PaginadorVehiculos);
                }
            }
            catch (Exception ex)
            {

                return new HttpActionResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Llamada para eliminar un vehiculo
        /// </summary>
        /// <param name="Id">Id del vehiculo</param>
        /// <returns>Confirmación de la acción</returns>
        [Route("{Id}")]
        public async Task<IHttpActionResult> Delete(int Id)
        {
            try
            {
                if(Id <= 0)
                {
                    
                    return BadRequest("El Id de vehiculo debe ser mayor que 0");
                }

                using (VehicleService Service = new VehicleService())
                {
                    if (await Service.Delete(Id))
                        return Ok();
                    else
                        return BadRequest("No se encontro el Id de vehiculo");

                }
            }
            catch (Exception ex)
            {

               return new HttpActionResult(HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }

        /// <summary>
        /// LLamada para añadir un vehiculo al listado
        /// </summary>
        /// <param name="vehicle">Objeto tipo VehicleModel</param>
        /// <returns>Confirmación de la acción</returns>
        [HttpPost]
        [Route("Post")]
        public async Task<IHttpActionResult> Post(VehicleModel vehicle)
        {
            try
            {
                if (!vehicle.Validate()) return BadRequest("Las propiedades del vehiculo no son correctas");
                using (VehicleService Service = new VehicleService())
                {
                    if (await Service.Add(vehicle))
                        return Created<VehicleModel>("",vehicle);
                    else
                        return BadRequest();
                }


            }
            catch (Exception ex)
            {
                return new HttpActionResult(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Llamada para actualizar los datos de un vehiculo
        /// </summary>
        /// <param name="vehicle">Objeto tipo VehicleModel</param>
        /// <returns>Confirmación de la acción</returns>
        [HttpPut]
        [Route("Put")]
        public async Task<IHttpActionResult> Put(VehicleModel vehicle)
        {
            try
            {

                if (!vehicle.Validate()) return BadRequest("Las propiedades del vehiculo no son correctas");

                using(VehicleService Service = new VehicleService())
                {
                    if (await Service.Update(vehicle))
                        return Ok();
                    else
                        return BadRequest("No existe el vehiculo con el Id indicado");
                }
            }
            catch (Exception ex)
            {
                return new HttpActionResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
