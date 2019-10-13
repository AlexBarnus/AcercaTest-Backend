using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AcercaTest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AcercaTest.Service
{
    public class VehicleService: IDisposable            
    {
        /// <summary>
        /// Metodo que obtiene los vehiculos del archivo Json
        /// </summary>
        /// <returns></returns>
       public async Task<List<VehicleModel>> Get()
        {
            try
            {
              
                List<VehicleModel> Vehicles = JsonConvert.DeserializeObject<List<VehicleModel>>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Data/vehicles.json"));
                return Vehicles;

              
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Metodo para borrar un vehiculo del archivo Json
        /// </summary>
        /// <param name="id">Id del vehiculo</param>
        /// <returns>Resultado de la operacion</returns>
        public async Task<Boolean> Delete(int id)
        {
            try
            {

                List<VehicleModel> Vehicles = JsonConvert.DeserializeObject<List<VehicleModel>>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Data/vehicles.json"));
                if (Vehicles.Where(x => x.Id == id).Count() == 0) return false;

                Vehicles = Vehicles.Where(x => x.Id != id).ToList();

               
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Data/vehicles.json", JToken.FromObject(Vehicles).ToString());
                    return true;
              

                

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Metodo para añadir un vehiculo a la lista
        /// </summary>
        /// <param name="Vehicle">Objeto tipo VehiceModel</param>
        /// <returns>Confirmación del resultado</returns>
        public async Task<Boolean> Add(VehicleModel Vehicle)
        {
            try
            {
                

                List<VehicleModel> ListVehicles = await Get();
                               

                if (ListVehicles.Count > 0)
                {
                    ListVehicles = ListVehicles.OrderBy(x => x.Id).ToList();
                    Vehicle.Id = ListVehicles.Last().Id + 1;
                }
                else
                    Vehicle.Id = 1;
                

                ListVehicles.Add(Vehicle);

                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Data/vehicles.json", JToken.FromObject(ListVehicles).ToString());

                return true;


            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Metodo para actualizar los datos de un vehiculo
        /// </summary>
        /// <param name="_vehicle">Objeto tipo VehicleModel</param>
        /// <returns>Confirmación del resultado</returns>
        public async Task<Boolean> Update(VehicleModel _vehicle)
        {
            try
            {
                           
                List<VehicleModel> ListVehicles = await Get();

                int index = ListVehicles.IndexOf(ListVehicles.Find(x => x.Id == _vehicle.Id));
                if (index != -1)
                {
                    ListVehicles[index] = _vehicle;
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Data/vehicles.json", JToken.FromObject(ListVehicles).ToString());
                    return true;

                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: elimine el estado administrado (objetos administrados).
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~VehicleService()
        // {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}