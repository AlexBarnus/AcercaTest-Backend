using NUnit.Framework;
using AcercaTest.Service;
using AcercaTest.Models;
using System.Collections.Generic;

namespace Tests
{
    public class Tests
    {
        VehicleModel vehicle = new VehicleModel();
        [SetUp]
        public void Setup()
        {
            vehicle.NumeroPedido = 22;
            vehicle.Bastidor = "VSSS2Z6KZ1R149943";
            vehicle.Matricula = "1245SDF";
            vehicle.Modelo = "Chevrolet Impala";
            vehicle.FechaEntrega = new System.DateTime();
        }

        [Test]
        public void AddTest()
        {
            using (VehicleService service = new VehicleService())
            {
                bool Ok = service.Add(vehicle).Result;
                Assert.IsTrue(Ok);

            }

        }

        [Test]
        public void GetTest()
        {
            using(VehicleService service = new VehicleService())
            {
                List<VehicleModel> ListVehicle = service.Get().Result;
                Assert.IsNotNull(ListVehicle);
                Assert.Greater(ListVehicle.Count, 0);
            }
        }

        [Test]
        public void UpdateTest()
        {
            using (VehicleService service = new VehicleService())
            {
                List<VehicleModel> ListVehicle = service.Get().Result;
                ListVehicle[0].Matricula = "4567MDK";
                bool Ok = service.Update(ListVehicle[0]).Result;
                Assert.IsTrue(Ok);
            }
        }



        [Test]
        public void DeleteTest()
        {
            using (VehicleService service = new VehicleService())
            {
                List<VehicleModel> ListVehicle = service.Get().Result;

                bool Ok = service.Delete(ListVehicle[0].Id).Result;
                Assert.IsTrue(Ok);
            }
        }
    }
}