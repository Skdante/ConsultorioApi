using ConsultorioApi.Core;
using ConsultorioApi.DataAccess;
using ConsultorioApi.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace ConsultorioApi.Test
{
    [TestClass]
    public class CompaniaTest
    {
        [TestMethod]
        public void CompaniaInsert_success()
        {
            // Preparación
            Exception expectedExcepcion = null;
            var companiaInsert = new CompaniaInsert();
            var mock = new Mock<ICompaniaReporitorio>();
            StatusProcess status = new StatusProcess();
            var statusExpected = new StatusProcess() { Estatus = true };
            var statusMock = Task.Run(() => new StatusProcessDB() { Estatus = true });
            mock.Setup(x => x.SetCompania(companiaInsert)).Returns(statusMock);
            var compania = new Compania(mock.Object);

            // Prueba
            try
            {
                status = compania.CompaniaInsert(companiaInsert).Result;
            }
            catch (Exception ex)
            {
                expectedExcepcion = ex;
            }

            // verificación
            Assert.AreEqual(statusExpected.Estatus, status.Estatus);
        }
    }
}
